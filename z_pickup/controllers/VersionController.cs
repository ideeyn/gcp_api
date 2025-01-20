namespace gcp_api.z_pickup.controllers;
// this file should be independent, so copy-able through projects

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using gcp_api.y_global_model;

[ApiController]
[Route("pickup")]
public class VersionController : ControllerBase {
    [HttpGet("version")]
    public async Task<IActionResult> Get() {
        // declaring model that will store data temporary
        var model = new BaseJSON { };

        // declaring final file path, GUARD_CLAUSE in case the file doesn't exist
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.versionDirectory);
        if (!System.IO.File.Exists(filePath)) {
            model.Type = RESPONSE.NOT_FOUND.Type;
            model.Message = RESPONSE.NOT_FOUND.Message;
            model.DevMessage = $"file version not found in server db";
            return Ok(model);
        }

        // Read the file content as a string, then deserialize it to model
        List<VersionModel>? allData;
        var jsonString = await System.IO.File.ReadAllTextAsync(filePath);
        try {
            allData = JsonSerializer.Deserialize<List<VersionModel>>(jsonString);
        } catch (JsonException ex) {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = $"Error while deserializing the data: {ex.Message}";
            return Ok(model); // Handle deserialization error
        }

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.Data.Add(allData!.FirstOrDefault(d => d.AppName == "pickup_app")!);
        return Ok(model); // Return the model as JSON
    }
}
