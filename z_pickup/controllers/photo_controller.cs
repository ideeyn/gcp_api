namespace gcp_api.z_pickup.controllers;

using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using gcp_api.y_global_model;
using gcp_api.z_pickup.functions;

[ApiController]
[Route("pickup")]
public class PhotoController : ControllerBase {
    [HttpPost("photo/{userId}")]
    public async Task<IActionResult> Post([FromForm] IFormFile photo, string userId, string apiKey) {
        // declaring model that will store data temporarily
        var model = new BaseJSON { };

        //! apiKey validation, photo not empty validation, validate pickup id is exist
        string devError = await Apikey_Validator.Validate(userId, apiKey);
        if (devError != "") {
            model.Type = RESPONSE.INVALID_SESSION.Type;
            model.Message = RESPONSE.INVALID_SESSION.Message;
            model.DevMessage = devError;
            return Ok(model);
        }

        // GUARD_CLAUSE direct abort in case request is empty
        if (photo == null || photo.Length == 0) {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = "No photo uploaded.";
            return Ok(model);
        }

        // GUARD_CLAUSE in case pickup with same id NOT exist. so post action failed.
        string pickupId = photo.FileName.Split("-").First();
        string pickupFilename = pickupId + GLOBAL.DB_format;
        string pickupPath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupJsonDirectory, pickupFilename);
        if (!System.IO.File.Exists(pickupPath)) {
            model.Type = RESPONSE.EDIT_NOT_EXIST.Type;
            model.Message = RESPONSE.EDIT_NOT_EXIST.Message;
            model.DevMessage = $"violation on primary_key {pickupId}. editing failed because it's original doesn't exist in db";
            return Ok(model);
        }

        //!============== post photo to DB =================
        // Define final file name and path
        string finalFilename = photo.FileName;
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupPhotoDirectory, finalFilename);

        // Save the uploaded file
        try {
            using var stream = new FileStream(filePath, FileMode.Create);
            await photo.CopyToAsync(stream);
        } catch (Exception ex) {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = $"Error while saving the photo: {ex.Message}";
            return Ok(model);
        }

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.DevMessage = $"success uploading file {finalFilename}";
        return Ok(model);
    }
}