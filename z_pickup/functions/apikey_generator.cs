namespace gcp_api.z_pickup.functions;

using System.Security.Cryptography;

public class Apikey_Generator {
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static string GenerateKey(string jsonString) {
        string apiKey;
        int length = 32;

        //! Check if the file already contains this new generated key
        do {
            // Generate a new API key
            char[] apiKeyChars = new char[length];
            using (RandomNumberGenerator generator = RandomNumberGenerator.Create()) {
                byte[] randomBytes = new byte[length];
                generator.GetBytes(randomBytes);
                for (int i = 0; i < length; i++) {
                    apiKeyChars[i] = chars[randomBytes[i] % chars.Length];
                }
            }
            apiKey = new string(apiKeyChars);
        } while (jsonString.Contains(apiKey)); // Repeat until a unique key is found

        return apiKey;
    }
}