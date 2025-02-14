using System.Text.Json.Serialization;
using Newtonsoft.Json;
using TecCore.Models;
using TecCore.Utilities;

namespace TecCore;

public interface IConfigService
{
    public static string ConfigPath { get; }
    public static string ConfigFile { get; }
    public static string LogPath { get; }
    public static string LogFile { get; }
    public TecConfig Config { get; set; }
    public void UpdateKeyValue(string key, string value);
}


public class ConfigService : IConfigService
{
    private static string BasePath { get; set; } = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Gitec");
    public TecConfig Config { get; set; } = new TecConfig();
    public static string ConfigPath { get; set; } = Path.Combine(BasePath, "Config");
    public static string ConfigFile { get; set; } = Path.Combine(ConfigPath, "config.json");
    public static string LogPath { get; set; } = Path.Combine(BasePath, "Logs");
    public static string LogFile { get; set; } = Path.Combine(LogPath, "events.log");
    
    public ConfigService()
    {
        Init();
        LoadConfig();
        Console.WriteLine("ConfigService initialized");
        Console.WriteLine($"Base Path: {BasePath}");
    }
 
    private void Init()
    {
        FileDirectoryHelper.CreateDirectory(BasePath);
        FileDirectoryHelper.CreateDirectory(ConfigPath);
        FileDirectoryHelper.CreateFile(ConfigFile, JsonConvert.SerializeObject(Config));
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