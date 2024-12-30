/**
 * Video Decryption and Player Setup
 * This code handles encrypted video URLs and sets up a JWPlayer instance
 */

// Main decryption key generation
function generateDecryptionKey() {
    // Creates a key from character codes and base64 values
    const part1 = String.fromCharCode(99) +
        String.fromCharCode(51) +
        String.fromCharCode(56) +
        String.fromCharCode(75) +
        String.fromCharCode(82);

    const part2 = atob("SHpRc20=");  // Base64 decoded value

    // Generated static string
    const part3 = String.fromCharCode(79, 84, 106, 85, 73, 118);

    return [part1, part2, part3].join("");
}

/**
 * Decrypts text using a Vigenère-like cipher
 * @param {string} encryptedText - The text to decrypt
 * @returns {string} Decrypted text in base64 format
 */
function decryptText(encryptedText) {
    const key = generateDecryptionKey();

    if (key.length === 0) {
        throw new Error("No encrypted text found.");
    }

    let decrypted = "";
    let keyIndex = 0;
    const keyLength = key.length;

    // Process each character
    for (let i = 0; i < encryptedText.length; i++) {
        const currentChar = encryptedText[i];

        // Only decrypt letters
        if (/[a-zA-Z]/.test(currentChar)) {
            const keyChar = key[keyIndex % keyLength];
            const shift = keyChar.charCodeAt(0) % 26;
            const charCode = currentChar.charCodeAt(0);

            // Handle uppercase letters
            if (currentChar >= "A" && currentChar <= "Z") {
                const base = "A".charCodeAt(0);
                const newCharCode = base + ((charCode - base - shift + 26) % 26);
                decrypted += String.fromCharCode(newCharCode);
            }
            // Handle lowercase letters
            else if (currentChar >= "a" && currentChar <= "z") {
                const base = "a".charCodeAt(0);
                const newCharCode = base + ((charCode - base - shift + 26) % 26);
                decrypted += String.fromCharCode(newCharCode);
            }
            keyIndex++;
        } else {
            // Non-letter characters pass through unchanged
            decrypted += currentChar;
        }
    }

    // Return decoded base64 string
    return atob(decrypted);
}

/**
 * Sets up the video player with the decrypted video URL
 * @param {string} videoUrl - The decrypted video URL
 */
function setupPlayer(videoUrl) {
    const player = jwplayer("my-jwplayer");
    const loadingElement = document.getElementById("loadVideoBtn");

    if (loadingElement) {
        loadingElement.style.display = "none";
    }

    // Configure JWPlayer
    player.setup({
        file: videoUrl,
        width: "100%",
        aspectratio: "16:9",
        mute: false,
        autostart: true,
        cast: {},
        airplay: true,
        sharing: {
            sites: ["facebook", "twitter", "email", "reddit", "pinterest"]
        }
    });

    // Reload page when complete
    player.on("complete", function() {
        location.reload();
    });
}

// Initialize when DOM is ready
document.addEventListener("DOMContentLoaded", function() {
    const decryptButton = document.getElementById("loadVideoBtn");

    if (decryptButton) {
        decryptButton.addEventListener("click", function() {
            const encryptedElement = document.getElementById("encrypted-text");

            if (!encryptedElement) {
                alert("Encrypted text element not found.");
                return;
            }

            const encryptedText = encryptedElement.getAttribute("data");

            if (encryptedText) {
                try {
                    const decryptedUrl = decryptText(encryptedText);
                    setupPlayer(decryptedUrl);
                } catch (error) {
                    alert("Error: " + error.message);
                }
            } else {
                alert("No encrypted text found.");
            }
        });
    }
});
