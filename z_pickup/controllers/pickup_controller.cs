namespace gcp_api.z_pickup.controllers;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using gcp_api.y_global_model;
using gcp_api.z_pickup.models;
using gcp_api.z_pickup.functions;

[ApiController]
[Route("pickup")]
public class PickupDeleteController : ControllerBase {
    [HttpGet("pickup/delete/{id}")]
    public async Task<IActionResult> Get(string id, string userId, string apiKey) {
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

        // Define final file path
        string finalFilename = id + GLOBAL.DB_format;
        string prevPath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupJsonDirectory, finalFilename);

        //!============== check if pickup id exist, and delete all prev photos =================
        if (!System.IO.File.Exists(prevPath)) {
            model.Type = RESPONSE.EDIT_NOT_EXIST.Type;
            model.Message = RESPONSE.EDIT_NOT_EXIST.Message;
            model.DevMessage = $"violation on primary_key {id}. editing failed because it's original doesn't exist in db";
            return Ok(model);
        }

        // deserialize it to model
        PickupModel? prevData;
        var pickupString = await System.IO.File.ReadAllTextAsync(prevPath);
        try { prevData = JsonSerializer.Deserialize<PickupModel>(pickupString); } //
        catch {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = "Error while deserializing the data";
            return Ok(model); // Handle deserialization error
        }

        // edit the "isDeleted" boolean only
        prevData!.Head.IsDeleted = true;

        // Serialize the model back to JSON, then re-save it to previous same path
        var jsonContent = JsonSerializer.Serialize(prevData);
        await System.IO.File.WriteAllTextAsync(prevPath, jsonContent);

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.Data.Add(prevData!);
        return Ok(model); // Return the model as JSON
    }
}
public class PickupController : ControllerBase {
    [HttpGet("pickup/{id}")]
    public async Task<IActionResult> Get(string id, string userId, string apiKey) {
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
        string finalFilename = id + GLOBAL.DB_format;
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupJsonDirectory, finalFilename);

        // GUARD_CLAUSE in case the file doesn't exist
        if (!System.IO.File.Exists(filePath)) {
            model.Type = RESPONSE.NOT_FOUND.Type;
            model.Message = RESPONSE.NOT_FOUND.Message;
            model.DevMessage = $"file {finalFilename} not found in server";
            return Ok(model);
        }

        // Read the file content as a string, then deserialize it to model
        var jsonString = await System.IO.File.ReadAllTextAsync(filePath);

        PickupModel? data;
        try {
            data = JsonSerializer.Deserialize<PickupModel>(jsonString);
        } catch {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = "Error while deserializing the data";
            return Ok(model); // Handle deserialization error
        }

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.Data.Add(data!);
        return Ok(model); // Return the model as JSON
    }

    //!#################################################################################

    [HttpPost("pickup")]
    public async Task<IActionResult> Post([FromBody] JsonElement json, string userId, string apiKey) {
        // declaring model that will store data temporary
        var model = new BaseJSON { };
        PickupModel? body;

        //! auth validation using apikey
        string devError = await Apikey_Validator.Validate(userId, apiKey);
        if (devError != "") {
            model.Type = RESPONSE.INVALID_SESSION.Type;
            model.Message = RESPONSE.INVALID_SESSION.Message;
            model.DevMessage = devError;
            return Ok(model);
        }

        try {
            body = JsonSerializer.Deserialize<PickupModel>(json.GetRawText());
        } catch {
            model.Type = RESPONSE.INVALID_INPUT.Type;
            model.Message = RESPONSE.INVALID_INPUT.Message;
            model.DevMessage = "invalid input: invalid json structure";
            return BadRequest(model);
        }

        //! INPUT VALIDATION
        string validationError = PickupValidation.ValidateInput(body!);
        if (validationError != "") {
            model.Type = RESPONSE.INVALID_INPUT.Type;
            model.Message = validationError.Split(":").First();
            model.DevMessage = validationError;
            return Ok(model);
        }

        // Serialize the model back to JSON
        var jsonContent = JsonSerializer.Serialize(body);

        // Define final file path
        string finalFilename = body!.Head.Id + GLOBAL.DB_format;
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupJsonDirectory, finalFilename);

        // GUARD_CLAUSE in case file with same id already exist
        if (System.IO.File.Exists(filePath)) {
            model.Type = RESPONSE.DUPLICATE_DATA.Type;
            model.Message = RESPONSE.DUPLICATE_DATA.Message;
            model.DevMessage = $"violation on primary_key {body!.Head.Id}. its already exist in db";
            return Ok(model);
        }

        // Write the JSON content to the file (creates the file if it doesn't exist)
        await System.IO.File.WriteAllTextAsync(filePath, jsonContent);

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.DevMessage = $"success inserting {finalFilename} to DB";
        return Ok(model);
    }

    //!#################################################################################

    [HttpPut("pickup")]
    public async Task<IActionResult> Put([FromBody] JsonElement json, string userId, string apiKey) {
        // declaring model that will store data temporary
        var model = new BaseJSON { };
        PickupModel? body;

        //! auth validation using apikey
        string devError = await Apikey_Validator.Validate(userId, apiKey);
        if (devError != "") {
            model.Type = RESPONSE.INVALID_SESSION.Type;
            model.Message = RESPONSE.INVALID_SESSION.Message;
            model.DevMessage = devError;
            return Ok(model);
        }

        try {
            body = JsonSerializer.Deserialize<PickupModel>(json.GetRawText());
        } catch {
            model.Type = RESPONSE.INVALID_INPUT.Type;
            model.Message = RESPONSE.INVALID_INPUT.Message;
            model.DevMessage = "invalid input: invalid json structure";
            return Ok(model);
        }

        //! INPUT VALIDATION
        string validationError = PickupValidation.ValidateInput(body!);
        if (validationError != "") {
            model.Type = RESPONSE.INVALID_INPUT.Type;
            model.Message = validationError.Split(":").First();
            model.DevMessage = validationError;
            return Ok(model);
        }

        // Serialize the model back to JSON
        var jsonContent = JsonSerializer.Serialize(body);

        // Define final file path
        string finalFilename = body!.Head.Id + GLOBAL.DB_format;
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupJsonDirectory, finalFilename);

        //!============== check if pickup id exist, and delete all prev photos =================
        if (!System.IO.File.Exists(filePath)) {
            model.Type = RESPONSE.EDIT_NOT_EXIST.Type;
            model.Message = RESPONSE.EDIT_NOT_EXIST.Message;
            model.DevMessage = $"violation on primary_key {body!.Head.Id}. editing failed because it's original doesn't exist in db";
            return Ok(model);
        }

        var pickupString = await System.IO.File.ReadAllTextAsync(filePath);
        List<string> errorPhoto = Photo_delete.Delete(pickupString);
        if (errorPhoto[0] != "") {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = errorPhoto[0];
            return Ok(model); // Handle deserialization error
        }

        //! Write the JSON content to the file (creates the file if it doesn't exist)
        await System.IO.File.WriteAllTextAsync(filePath, jsonContent);

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.DevMessage = $"success updating {finalFilename} in DB. " + errorPhoto[1];
        return Ok(model);
    }

    //!#################################################################################

    [HttpDelete("pickup/{id}")]
    public async Task<IActionResult> Delete(string id, string userId, string apiKey) {
        // Declaring model that will store data temporarily
        var model = new BaseJSON { };

        //! auth validation using apikey
        string devError = await Apikey_Validator.Validate(userId, apiKey);
        if (devError != "") {
            model.Type = RESPONSE.INVALID_SESSION.Type;
            model.Message = RESPONSE.INVALID_SESSION.Message;
            model.DevMessage = devError;
            return Ok(model);
        }

        // Define final file path
        string finalFilename = id + GLOBAL.DB_format;
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupJsonDirectory, finalFilename);

        // GUARD_CLAUSE in case file with the same id does not exist
        if (!System.IO.File.Exists(filePath)) {
            model.Type = RESPONSE.NOT_FOUND.Type;
            model.Message = RESPONSE.NOT_FOUND.Message;
            model.DevMessage = "Deletion failed because the original doesn't exist in db.";
            return Ok(model);
        }

        //! delete the photos first
        var pickupString = await System.IO.File.ReadAllTextAsync(filePath);
        List<string> errorPhoto = Photo_delete.Delete(pickupString);
        if (errorPhoto[0] != "") {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = errorPhoto[0];
            return Ok(model); // Handle deserialization error
        }

        // then after, safely delete the file
        System.IO.File.Delete(filePath);

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.DevMessage = $"success deleting {finalFilename} from DB. " + errorPhoto[1];
        return Ok(model);
    }
}