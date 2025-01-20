namespace gcp_api.z_pickup.controllers;

using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using gcp_api.y_global_model;
using gcp_api.z_pickup.models;
using gcp_api.z_pickup.functions;

[ApiController]
[Route("pickup")]
public class LoginController : ControllerBase {
    [HttpPost("login")]
    public async Task<IActionResult> Post([FromForm] string user, [FromForm] string password) {
        // declaring model that will store data temporary
        var model = new BaseJSON { };

        if (user == "") model.DevMessage = "username can't be empty: error because username is not filled";
        if (password == "") model.DevMessage = "password can't be empty: error because password is empty";
        if (model.DevMessage != "") {
            model.Type = RESPONSE.INVALID_INPUT.Type;
            model.Message = model.DevMessage.Split(":").First();
            return Ok(model);
        }

        // read previous auth datas from AUTH_CENTRAL.json as string
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupAuthDirectory);
        if (!System.IO.File.Exists(filePath)) {
            model.Type = RESPONSE.NOT_FOUND.Type;
            model.Message = "your connection not stable, retry";
            model.DevMessage = "failed finding auth central db file in server";
            return Ok(model);
        }
        string jsonString = await System.IO.File.ReadAllTextAsync(filePath);

        // deserialize json string to model
        List<UserModel>? data;
        try {
            data = JsonSerializer.Deserialize<List<UserModel>>(jsonString);
        } catch (JsonException ex) {
            model.Type = RESPONSE.BAD_REQUEST.Type;
            model.Message = RESPONSE.BAD_REQUEST.Message;
            model.DevMessage = $"failed deserializing auth central db: {ex.Message}";
            return Ok(model); // Handle deserialization error
        }

        int dataIndex = data!.FindIndex((d) =>
            (d.UserId == user && d.Password == password) ||
            (d.AD_username == user && d.AD_password == password));

        if (dataIndex == -1) {
            model.Type = RESPONSE.INVALID_CREDENTIAL.Type;
            model.Message = RESPONSE.INVALID_CREDENTIAL.Message;
            model.DevMessage = "failed to match login with both normal_login and ad_login";
            return Ok(model);
        }

        string finalApiKey = Apikey_Generator.GenerateKey(jsonString);

        //! creating final edited data
        UserModel finalData = data[dataIndex];
        List<string> listApikey = finalData.ApiKey == "" ? [] : [.. finalData.ApiKey.Split("||")];
        listApikey.Add(finalApiKey);
        if (listApikey.Count > 5) listApikey = [.. listApikey.Skip(Math.Max(0, listApikey.Count - 5))];
        finalData.ApiKey = string.Join("||", listApikey);

        //! push the data to json db
        data[dataIndex] = finalData;
        // Serialize the model back to JSON
        // var finalJsonDB = JsonSerializer.Serialize(data); //! compact version, for performance
        var finalJsonDB = JsonSerializer.Serialize(data, GLOBAL.formatter);
        // Write the JSON content to the file (creates the file if it doesn't exist)
        await System.IO.File.WriteAllTextAsync(filePath, finalJsonDB);

        model.Type = RESPONSE.SUCCESS.Type;
        model.Message = RESPONSE.SUCCESS.Message;
        model.Data.Add(new UserModel_JSON {
            UserId = finalData.UserId,
            Username = finalData.Username,
            ApiKey = finalApiKey
        });

        return Ok(model);
    }
}