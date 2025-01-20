namespace gcp_api.z_pickup.models;

using System.Text.Json.Serialization;

public class DriverModel {
    [JsonPropertyName("userId")]
    required public string UserId { get; set; }
    [JsonPropertyName("name")]
    required public string Name { get; set; }
    [JsonPropertyName("factoryCode")]
    required public string FactoryCode { get; set; }
    [JsonPropertyName("updatedOn")]
    required public DateTime UpdatedOn { get; set; }
}
