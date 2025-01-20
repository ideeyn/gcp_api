namespace gcp_api.y_global_model;
using System.Text.Json.Serialization;

public class BaseJSON
{
    [JsonPropertyName(GLOBAL.jsonType)]
    public string? Type { get; set; }

    [JsonPropertyName(GLOBAL.jsonMessage)]
    public string? Message { get; set; }

    [JsonPropertyName(GLOBAL.jsonDevMessage)]
    public string DevMessage { get; set; } = string.Empty; // default empty string ""

    [JsonPropertyName(GLOBAL.jsonData)]
    public List<object> Data { get; set; } = []; // default empty []
}

//!==================== SOME REPEATEDs ========================================

public class RESPONSE
{
    public static readonly BaseJSON NOT_FOUND = new()
    {
        Type = "NOT_FOUND",
        Message = "the file is not accessible for you"
    };
    public static readonly BaseJSON BAD_REQUEST = new()
    {
        Type = "BAD_REQUEST",
        Message = "there some connection problem while processing the file"
    };
    public static readonly BaseJSON INVALID_SESSION = new()
    {
        Type = "INVALID_SESSION",
        Message = "your login token is expired, please do login again."
    };
    public static readonly BaseJSON INVALID_CREDENTIAL = new()
    {
        Type = "INVALID_CREDENTIAL",
        Message = "wrong credential. please try again."
    };
    public static readonly BaseJSON INVALID_INPUT = new()
    {
        Type = "INVALID_INPUT",
        Message = "something missing, check your data again"
    };
    public static readonly BaseJSON DUPLICATE_DATA = new()
    {
        Type = "DUPLICATE_DATA",
        Message = "your input is corrupted by your own device. re-create new input please"
    };
    public static readonly BaseJSON EDIT_NOT_EXIST = new()
    {
        Type = "EDIT_NOT_EXIST",
        Message = "the data you requested to edit is not edit-able. create new input instead"
    };
    public static readonly BaseJSON SUCCESS = new()
    {
        Type = "SUCCESS",
        Message = "operation done successfully"
    };
}