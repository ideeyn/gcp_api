namespace gcp_api.z_pickup.controllers;

using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using gcp_api.y_global_model;
using gcp_api.z_pickup.models;
using gcp_api.z_pickup.functions;

[ApiController]
[Route("pickup")]
public class DriverController : ControllerBase {
    [HttpGet("driver/{userId}")]
    public async Task<IActionResult> Get(string userId, string apiKey) {
        // declaring model that will store data temporary
        var model = new BaseJSON { };

        //! auth validation using apikey
        string devError = await Apikey_Validator.Validate(userId, apiKey);
        if (devError != "") {
            model.Type = RESPONSE.INVALID_SESSION.Type;
            model.Message = RESPONSE.INVALID_SESSION.Message;
            model.DevMessage = devError;
            return Ok(model);
        }

        // declaring final file path
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupDriverDirectory);

        // GUARD_CLAUSE in case the file doesn't exist
        if (!System.IO.File.Exists(filePath)) {
            model.Type = RESPONSE.NOT_FOUND.Type;
            model.Message = RESPONSE.NOT_FOUND.Message;
            model.DevMessage = $"file driver not found in server db";
            return Ok(model);
        }

        // Read the file content as a string, then deserialize it to model
        var jsonString = await System.IO.File.ReadAllTextAsync(filePath);

        List<DriverModel>? data;
        try {
            data = JsonSerializer.Deserialize<List<DriverModel>>(jsonString);
        } catch (JsonException ex) {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = $"Error while deserializing the data: {ex.Message}";
            return Ok(model); // Handle deserialization error
        }

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.Data = [.. data!];
        return Ok(model); // Return the model as JSON
    }
}