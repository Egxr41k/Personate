using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Personate.Settings;
public class SettingsService
{
    private readonly string pathToLast = "";
    private readonly string pathToDefault = "";

    public UserSettingsDTO Settings { get; private set; }
    public SettingsService()
    {
        Settings = new(); //GetFromFile(pathToLast);
        Settings.Taskbar = CreateDefault();
    }

    //public void Actualize()
    //{
    //    Settings = GetFromFile(pathToLast);
    //}

    //private UserSettingsDTO GetFromFile(string pathToSettings)
    //{
    //    if (!File.Exists(pathToSettings)) return GetDefault();
    //    string jsonString = File.ReadAllText(pathToSettings);

    //    UserSettingsDTO? settings = JsonSerializer.Deserialize<UserSettingsDTO>(jsonString, new JsonSerializerOptions()
    //    {
    //        PropertyNameCaseInsensitive = true
    //    });

    //    return settings;// ?? CreateDefault();
    //}

    private UserSettingsDTO GetDefault()
    {
        Settings = new()
        {
            Wallpaper = "",
            Cursor = "",
            Taskbar = CreateDefault()
        }; 
        return Settings;
        //return GetFromFile(pathToDefault);
    }

    public void Save()
    {
        string modifiedJson = JsonSerializer.Serialize(Settings);

        File.WriteAllText(pathToLast, modifiedJson);
    }

    private TaskbarSettingsDTO CreateDefault()
    {
        TaskbarSettingsDTO Settings = new TaskbarSettingsDTO();
        Settings.TaskbarStyle = 0;
        Settings.PrimaryTaskbarOffset = 0;
        Settings.SecondaryTaskbarOffset = 0;
        Settings.CenterPrimaryOnly = 0;
        Settings.CenterSecondaryOnly = 0;
        Settings.AnimationStyle = "cubiceaseinout";
        Settings.AnimationSpeed = 300;
        Settings.LoopRefreshRate = 400;
        Settings.CenterInBetween = 0;
        Settings.DontCenterTaskbar = 0;
        Settings.FixToolbarsOnTrayChange = 1;
        Settings.OnBatteryAnimationStyle = "cubiceaseinout";
        Settings.OnBatteryLoopRefreshRate = 400;
        Settings.RevertZeroBeyondTray = 1;
        Settings.TaskbarRounding = 0;
        Settings.TaskbarSegments = 0;

        return Settings;
    }
}
