namespace gcp_api.z_pickup.functions;
using gcp_api.z_pickup.models;

public class PickupValidation {
    public static string ValidateInput(PickupModel data) {
        string error = "";

        if (data.Head.Id.Length != 10) error = "Invalid Id: should be exactly 10 chars";
        if (data.Zones.Any((z) => z.HeaderId != data.Head.Id)) error = "Invalid Zone Id: violation on Foreign Key, should be the same as header Id";
        if (data.Head.Latitude < -90 || data.Head.Latitude > 90) error = "Invalid Latitude: must be -90 > latitude > 90";
        if (data.Head.Longitude < -180 || data.Head.Longitude > 180) error = "Invalid Longitude:  must be -180 > latitude > 180";
        if (data.Head.Factory.Length > 20) error = "Too long Factory name: maximum 20 chars";
        if (data.Head.Factory.Length == 0) error = "Factory can't be empty: it's required";
        if (data.Head.Driver.Length > 20) error = "Too long Driver name: maximum 20 chars";
        if (data.Head.Driver.Length == 0) error = "Driver can't be empty: it's required";
        if (data.Head.Vehicle.Length > 20) error = "Too long Vehicle name: maximum 20 chars";
        if (data.Head.Vehicle.Length == 0) error = "Vehicle can't be empty: it's required";
        if (data.Head.Plate.Length > 20) error = "Too long Plate Number: maximum 20 chars";
        if (data.Head.Plate.Length == 0) error = "Plate can't be empty: it's required";
        if (data.Head.Departure == default) error = "Invalid Departure: must be valid date";
        if (data.Head.Duration < 1) error = "Invalid Duration: duration should be at least 1";
        if (data.Head.Note.Length > 200) error = "Too long Note: maximum 200 chars";

        // note. if no error, then empty string "" will passed. 
        // and it will pass guard_clause in controller
        return error;
    }
}