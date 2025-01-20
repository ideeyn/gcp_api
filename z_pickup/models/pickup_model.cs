namespace gcp_api.z_pickup.models;
using System.Text.Json.Serialization;

public class PickupModel {
    [JsonPropertyName("head")]
    required public PickupHeader Head { get; set; }

    [JsonPropertyName("zones")]
    required public List<PickupZone> Zones { get; set; }
}

//!======================== HEADER MODEL =================================

public class PickupHeader {
    [JsonPropertyName("isDeleted")]
    required public bool IsDeleted { get; set; } = false;

    [JsonPropertyName("id")]
    required public string Id { get; set; }

    [JsonPropertyName("latitude")]
    required public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    required public double Longitude { get; set; }

    [JsonPropertyName("factory")]
    required public string Factory { get; set; }

    [JsonPropertyName("driver")]
    required public string Driver { get; set; }

    [JsonPropertyName("vehicle")]
    required public string Vehicle { get; set; }

    [JsonPropertyName("plate")]
    required public string Plate { get; set; }

    [JsonPropertyName("departure")]
    required public DateTime Departure { get; set; }

    [JsonPropertyName("duration")]
    required public int Duration { get; set; }

    [JsonPropertyName("note")]
    required public string Note { get; set; } = "";
}

//!======================== ZONE MODEL =================================

public class PickupZone {

    [JsonPropertyName("headerId")]
    required public string HeaderId { get; set; }

    [JsonPropertyName("index")]
    required public int Index { get; set; }

    [JsonPropertyName("zone")]
    required public string Zone { get; set; }

    [JsonPropertyName("agent")]
    required public string Agent { get; set; }

    [JsonPropertyName("quality")]
    required public string Quality { get; set; }

    [JsonPropertyName("isSeparated")]
    required public bool IsSeparated { get; set; }

    [JsonPropertyName("bunch")]
    public int? Bunch { get; set; }

    [JsonPropertyName("weight")]
    required public double Weight { get; set; }

    [JsonPropertyName("photos")]
    required public string Photos { get; set; } = "";
}