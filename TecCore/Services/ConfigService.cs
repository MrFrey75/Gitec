using Newtonsoft.Json;
using TecCore.Models;
using TecCore.Utilities;

namespace TecCore.Services;

public interface IConfigService
{
    public static string ConfigPath { get; }
    public static string ConfigFile { get; }
    public static string LogPath { get; }
    public static string LogFile { get; }
    public TecConfig Config { get; set; }
    public void UpdateKeyValue(string key, string value);
    public void Init();
}


public class ConfigService : IConfigService
{
    private static string BasePath { get; set; } = GetBasePath();
    public TecConfig Config { get; set; } = new TecConfig();
    public static string ConfigPath { get; set; } = Path.Combine(BasePath, "Config");
    public static string ConfigFile { get; set; } = Path.Combine(ConfigPath, "config.json");
    public static string LogPath { get; set; } = Path.Combine(BasePath, "Logs");
    public static string LogFile { get; set; } = Path.Combine(LogPath, "events.log");
    public static string DatabasePath { get; set; } = Path.Combine(BasePath, "Data");
    public static string DatabaseFile { get; set; } = Path.Combine(DatabasePath, "TecCore.db");
    
    public ConfigService()
    {
        Init();
        LoadConfig();
        Console.WriteLine("ConfigService initialized");
        Console.WriteLine($"Base Path: {BasePath}");
    }
    
    private static string GetBasePath()
    {
        return Environment.OSVersion.Platform switch
        {
            // is application running on windows or linux
            PlatformID.Win32NT => Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gitec"),
            PlatformID.Unix => Path.Combine("/var/lib/", "Gitec"),
            _ => BasePath
        };
    }

    private TecConfig GetConfig()
    {
        return Config;
    }

    public void Init()
    {
        FileDirectoryHelper.CreateDirectory(BasePath);
        FileDirectoryHelper.CreateDirectory(ConfigPath);
        FileDirectoryHelper.CreateFile(ConfigFile, JsonConvert.SerializeObject(GetConfig()));
        FileDirectoryHelper.CreateDirectory(LogPath);
        FileDirectoryHelper.CreateFile(LogFile);
    }
   
    private void SaveConfig()
    {
        File.WriteAllText(ConfigFile, JsonConvert.SerializeObject(Config));
    }
    
    public new void UpdateKeyValue(string key, string value)
    {
        var config = Config;
        var type = config.GetType();
        var property = type.GetProperty(key);
        if (property == null) return;
        property.SetValue(config, value);
        SaveConfig();
    }
    
    private void LoadConfig()
    {
        Config = JsonConvert.DeserializeObject<TecConfig>(File.ReadAllText(ConfigFile)) ?? new TecConfig();
    }
    
}