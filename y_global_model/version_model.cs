namespace gcp_api.y_global_model;

using System;
using System.Text.Json.Serialization;

public class VersionModel
{
    [JsonPropertyName("app_code")]
    public int AppCode { get; set; }

    [JsonPropertyName("app_name")]
    public string AppName { get; set; } = string.Empty; // Default empty string

    [JsonPropertyName("deprecated_version")]
    public string DeprecatedVersion { get; set; } = string.Empty; // Default empty string

    [JsonPropertyName("minimum_version")]
    public string MinimumVersion { get; set; } = string.Empty; // Default empty string

    [JsonPropertyName("newest_version")]
    public string NewestVersion { get; set; } = string.Empty; // Default empty string

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty; // Default empty string

    [JsonPropertyName("updatedOn")]
    public DateTime UpdatedOn { get; set; }
}