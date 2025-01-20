namespace gcp_api.z_pickup.models;

using System.Text.Json.Serialization;

public class UserModel {
    [JsonPropertyName("userId")]
    required public string UserId { get; set; }
    [JsonPropertyName("username")]
    required public string Username { get; set; }
    [JsonPropertyName("password")]
    required public string Password { get; set; }
    required public string AD_username { get; set; }
    required public string AD_password { get; set; }
    [JsonPropertyName("apiKey")]
    required public string ApiKey { get; set; }
}

public class UserModel_JSON {
    [JsonPropertyName("userId")]
    required public string UserId { get; set; }
    [JsonPropertyName("username")]
    required public string Username { get; set; }
    [JsonPropertyName("apiKey")]
    required public string ApiKey { get; set; }
}