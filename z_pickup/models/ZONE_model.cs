namespace gcp_api.z_pickup.models;

using System.Text.Json.Serialization;

public class ZoneModel {
    [JsonPropertyName("code")]
    required public string Code { get; set; }
    [JsonPropertyName("factoryCode")]
    required public string FactoryCode { get; set; }
    [JsonPropertyName("updatedOn")]
    required public DateTime UpdatedOn { get; set; }
}
