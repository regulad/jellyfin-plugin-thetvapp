using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Chat;

namespace Jellyfin.Plugin.TheTVApp;

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// The TV App video decryption utility.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Initialization is implicit by the plugin loader, no opportunity to teardown")]
public class VideoDecryption
{
    private readonly ILogger logger;

    private readonly JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    private readonly SemaphoreSlim cacheLock = new SemaphoreSlim(1, 1);
    private readonly Dictionary<string, LlmReturn> llmReturnCache = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="VideoDecryption"/> class.
    /// </summary>
    /// <param name="logger">The logger the decryptor will use when outputting log messages.</param>
    public VideoDecryption(ILogger logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Decrypts an encrypted video URL using a custom Vigenère-like cipher.
    /// </summary>
    /// <param name="encryptedText">The encrypted text to decrypt.</param>
    /// <param name="rawObfuscatedJavascript">The raw obfuscated JavaScript containing the decryption key.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The decrypted URL.</returns>
    /// <exception cref="ArgumentException">Thrown when encryption key cannot be generated, or if the encryptedText is null.</exception>
    public async Task<Uri?> DecryptVideoUrl(string encryptedText, string rawObfuscatedJavascript, CancellationToken cancellationToken)
    {
        string key = await GenerateDecryptionKey(rawObfuscatedJavascript, cancellationToken).ConfigureAwait(false);

        if (string.IsNullOrEmpty(key))
        {
            throw new ArgumentException("Failed to generate decryption key");
        }

        if (string.IsNullOrEmpty(encryptedText))
        {
            return null;
        }

        StringBuilder decrypted = new StringBuilder();
        int keyIndex = 0;
        int keyLength = key.Length;

        // Process each character in the encrypted text
        foreach (char currentChar in encryptedText)
        {
            if (char.IsLetter(currentChar))
            {
                char keyChar = key[keyIndex % keyLength];
                int shift = keyChar % 26;
                int charCode = currentChar;

                if (char.IsUpper(currentChar))
                {
                    int baseChar = 'A';
                    int newCharCode = baseChar + ((charCode - baseChar - shift + 26) % 26);
                    decrypted.Append((char)newCharCode);
                }
                else
                {
                    // lowercase
                    int baseChar = 'a';
                    int newCharCode = baseChar + ((charCode - baseChar - shift + 26) % 26);
                    decrypted.Append((char)newCharCode);
                }

                keyIndex++;
            }
            else
            {
                // Non-letter characters pass through unchanged
                decrypted.Append(currentChar);
            }
        }

        // Convert from Base64 to get the final URL
        byte[] base64Bytes = Convert.FromBase64String(decrypted.ToString());
        string decodedString = Encoding.UTF8.GetString(base64Bytes);
        return new Uri(decodedString);
    }

    /// <summary>
    /// Generates the decryption key using the same approach as the original JavaScript.
    /// </summary>
    /// <returns>The combined decryption key.</returns>
    private async Task<string> GenerateDecryptionKey(string rawObfuscatedJavascript, CancellationToken cancellationToken)
    {
        LlmReturn? llmReturn;

        await this.cacheLock.WaitAsync(cancellationToken).ConfigureAwait(true);
        try
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(rawObfuscatedJavascript)))
            {
                byte[] javascriptSha256 = await SHA256.HashDataAsync(stream, cancellationToken).ConfigureAwait(false);
                string javascriptSha256String = Convert.ToBase64String(javascriptSha256); // honestly just used because its easier han trying to use a dict with an array (no hashkey)

                if (!llmReturnCache.TryGetValue(javascriptSha256String, out llmReturn))
                {
                    llmReturn = await this.ParseObfuscatedJavascript(rawObfuscatedJavascript, cancellationToken).ConfigureAwait(false);

                    if (llmReturn != null)
                    {
                        logger.LogInformation("Successfully deobfuscated the JavaScript ({00}) and extracted the encryption key components {0}, {1}, {2}", javascriptSha256String, llmReturn.Part1, llmReturn.Part2, llmReturn.Part3);
                        llmReturnCache[javascriptSha256String] = llmReturn;
                    }
                }
            }
        }
        finally
        {
            this.cacheLock.Release();
        }

        if (llmReturn == null || llmReturn.Part1 == null || llmReturn.Part2 == null || llmReturn.Part3 == null)
        {
            logger.LogError("Could not determine the encryption key! Cannot possibly stream!");
            throw new ArithmeticException("Could not determine the encryption key!");
        }

        // Part 1: Character sequence
        string part1 = new string(llmReturn.Part1!.Select(i => (char)i).ToArray());

        // Part 2: Base64 decoded value
        string part2 = Encoding.UTF8.GetString(Convert.FromBase64String(llmReturn.Part2!));

        // Part 3: Static character sequence
        string part3 = new string(llmReturn.Part3!.Select(i => (char)i).ToArray());

        return part1 + part2 + part3;
    }

    // using an llm to extract important elements from deobfuscated code sounds overkill but is suprisingly not
    private async Task<LlmReturn?> ParseObfuscatedJavascript(string rawObfuscatedJavascript, CancellationToken cancellationToken)
    {
        // first step: load the prompt & completion schema
        var assembly = Assembly.GetExecutingAssembly();

        string prompt;
        using (Stream stream = assembly.GetManifestResourceStream("Jellyfin.Plugin.TheTVApp.Llm.prompt.md")!)
        using (StreamReader reader = new StreamReader(stream))
        {
            prompt = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        }

        string jsonSchema;
        using (Stream stream = assembly.GetManifestResourceStream("Jellyfin.Plugin.TheTVApp.Llm.jsonSchema.json")!)
        using (StreamReader reader = new StreamReader(stream))
        {
            jsonSchema = await reader.ReadToEndAsync(cancellationToken).ConfigureAwait(false);
        }

        // second step: dispatch a chat request to the OpenAI API
        var apiKey = Plugin.Instance!.Configuration.OpenAiApiKey;

        if (string.IsNullOrEmpty(apiKey))
        {
            logger.LogError("OpenAI API key is not set! Cannot determine the encryption key.");
            throw new InvalidOperationException("OpenAI API key is not set");
        }

        var client = new OpenAIClient(apiKey);
        var chatClient = client.GetChatClient("gpt-4o");

        var chatPrompt = new List<ChatMessage>()
        {
            new SystemChatMessage(prompt),
            new UserChatMessage(rawObfuscatedJavascript)
        };

        var chatCompletionOptions = new ChatCompletionOptions()
        {
            ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                jsonSchemaFormatName: "encryption_key_raw_data",
                jsonSchema: BinaryData.FromBytes(Encoding.UTF8.GetBytes(jsonSchema)),
                jsonSchemaIsStrict: true)
        };

        var completion = (await chatClient.CompleteChatAsync(chatPrompt, chatCompletionOptions, cancellationToken).ConfigureAwait(false)).Value;

        // third step: parse the JSON response
        var jsonString = completion.Content[0].Text;

        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
        {
            return await JsonSerializer.DeserializeAsync<LlmReturn>(stream, jsonSerializerOptions, cancellationToken).ConfigureAwait(false);
        }
    }

    private sealed class LlmReturn
    {
        public int[]? Part1 { get; set; }

        public string? Part2 { get; set; }

        public int[]? Part3 { get; set; }
    }
}
