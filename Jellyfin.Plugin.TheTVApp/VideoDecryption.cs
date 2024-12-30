namespace Jellyfin.Plugin.TheTVApp;

using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// The TV App video decryption utility.
/// </summary>
public static class VideoDecryption
{
    /// <summary>
    /// Decrypts an encrypted video URL using a custom Vigenère-like cipher.
    /// </summary>
    /// <param name="encryptedText">The encrypted text to decrypt.</param>
    /// <returns>The decrypted URL.</returns>
    /// <exception cref="ArgumentException">Thrown when encryption key cannot be generated, or if the encryptedText is null.</exception>
    public static Uri? DecryptVideoUrl(string encryptedText)
    {
        string key = GenerateDecryptionKey();

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
    /// Returns a unique identifier for the string that is deterministic and consistent.
    /// </summary>
    /// <param name="str">The string to generate an ID for.</param>
    /// <returns>The ID string.</returns>
    public static string IdOfString(this string str)
    {
        ArgumentNullException.ThrowIfNull(str);
        return unchecked((uint)str.GetHashCode(StringComparison.Ordinal)).ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Generates the decryption key using the same approach as the original JavaScript.
    /// </summary>
    /// <returns>The combined decryption key.</returns>
    private static string GenerateDecryptionKey()
    {
        // Part 1: Character sequence
        string part1 = new string(new[]
        {
            (char)99, // 'c'
            (char)51, // '3'
            (char)56, // '8'
            (char)75, // 'K'
            (char)82, // 'R'
        });

        // Part 2: Base64 decoded value
        string part2 = Encoding.UTF8.GetString(Convert.FromBase64String("SHpRc20="));

        // Part 3: Static character sequence
        string part3 = new string(new[]
        {
            (char)79, // 'O'
            (char)84, // 'T'
            (char)106, // 'j'
            (char)85, // 'U'
            (char)73, // 'I'
            (char)118, // 'v'
        });

        return part1 + part2 + part3;
    }
}
