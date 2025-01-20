namespace gcp_api.z_pickup.models;

using System.Text.Json.Serialization;

public class PlateModel {
    [JsonPropertyName("code")]
    required public string Code { get; set; }
    [JsonPropertyName("vehicleCode")]
    required public string VehicleCode { get; set; }
    [JsonPropertyName("updatedOn")]
    required public DateTime UpdatedOn { get; set; }
}
