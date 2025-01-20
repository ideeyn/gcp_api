namespace gcp_api.z_pickup.functions;

using System.Text.Json;
using gcp_api.z_pickup.models;

public class Photo_delete {
    public static List<string> Delete(string dataString) {
        // deserialize it to model
        PickupModel? prevData;
        try { prevData = JsonSerializer.Deserialize<PickupModel>(dataString); } //
        catch { return ["Error while deserializing the data", ""]; }

        List<string> prevPhotos = [.. prevData!.Zones.SelectMany(zone => zone.Photos.Split("||"))];
        List<string> errors = [];
        foreach (var prevFilename in prevPhotos) {
            if (prevFilename == "") continue;
            string prevPath = Path.Combine(Directory.GetCurrentDirectory(), GLOBAL.pickupPhotoDirectory, prevFilename);
            try { File.Delete(prevPath); } // 
            catch (Exception ex) { errors.Add($"error while deleting {prevFilename}: {ex.Message}"); }
        }

        return ["", string.Join(". ", errors)];
    }
}