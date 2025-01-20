// yoo there... all here are global variables in the api, so later in case 
// you need to switch things you dont have to deep dive to code. enjoy!
using System.Text.Json;

namespace gcp_api;

//todo================== SETUP ====================================
//todo          paste the .editorconfig to have flutter style on brackets {}    

public class GLOBAL {
    //!========= BASE YEAR (year can be encoded up to 60 only) ===========
    // means if base year 2020, then api can handle only to year 2080
    public const int BASE_YEAR = 2020;
    public const string DB_format = ".json";

    //!========= data directories ========================================
    public const string versionDirectory = "VERSION.json";
    //?--------------------------------------------------------------------------------
    public const string pickupAuthDirectory = "z_pickup/app_data/AUTH_CENTRAL.json";
    public const string pickupAgentDirectory = "z_pickup/app_data/agents.json";
    public const string pickupDriverDirectory = "z_pickup/app_data/drivers.json";
    public const string pickupFactoryDirectory = "z_pickup/app_data/factories.json";
    public const string pickupPlateDirectory = "z_pickup/app_data/plates.json";
    public const string pickupVehicleDirectory = "z_pickup/app_data/vehicles.json";
    public const string pickupZoneDirectory = "z_pickup/app_data/zones.json";
    public const string pickupJsonDirectory = "z_pickup/app_data/jsons";
    public const string pickupPhotoDirectory = "z_pickup/app_data/photos";

    //!========= base JSON structure names, copy to flutter ==============
    public const string jsonType = "type";
    public const string jsonMessage = "message";
    public const string jsonDevMessage = "devMessage";
    public const string jsonData = "data";

    //!========= formatter ==============
    public static readonly JsonSerializerOptions formatter = new() { WriteIndented = true };
}