namespace gcp_api.z_pickup.functions;

using System.Text.Json;
using gcp_api.z_pickup.models;

public class Apikey_Validator {
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    public static async Task<string> Validate(string userId, string apiKey) {
        // read previous auth datas from AUTH_CENTRAL.json as string
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupAuthDirectory);
        if (!File.Exists(filePath)) return "failed finding auth central db file in server";
        string jsonString = await File.ReadAllTextAsync(filePath);

        // deserialize json string to model
        List<UserModel>? data;
        try { data = JsonSerializer.Deserialize<List<UserModel>>(jsonString); } //
        catch (JsonException ex) { return $"failed deserializing auth central db: {ex.Message}"; }

        UserModel? matchedUser = data!.FirstOrDefault(user => user.UserId == userId);
        if (matchedUser == null) return "there's no matching user id";
        if (!matchedUser.ApiKey.Contains(apiKey)) return "invalid apiKey";

        return "";
    }
}