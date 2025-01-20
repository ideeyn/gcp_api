namespace gcp_api.z_pickup.models;

using System.Text.Json.Serialization;

public class FactoryModel {
    [JsonPropertyName("code")]
    required public string Code { get; set; }
    [JsonPropertyName("name")]
    required public string Name { get; set; }
    [JsonPropertyName("vehicles")]
    required public string Vehicles { get; set; }
    [JsonPropertyName("updatedOn")]
    required public DateTime UpdatedOn { get; set; }
}
