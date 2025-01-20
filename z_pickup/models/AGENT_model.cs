namespace gcp_api.z_pickup.models;

using System.Text.Json.Serialization;

public class AgentModel {
    [JsonPropertyName("userId")]
    required public string UserId { get; set; }
    [JsonPropertyName("name")]
    required public string Name { get; set; }
    [JsonPropertyName("zoneCode")]
    required public string ZoneCode { get; set; }
    [JsonPropertyName("updatedOn")]
    required public DateTime UpdatedOn { get; set; }
}
