
using System.Text.Json;

namespace AirStation.ControlCenter.App.Configuration;

internal static class AppSettingsLoader
{
    public static AppSettings Load(string filePath)
    {
        string json = File.ReadAllText(filePath);

        AppSettings? settings = JsonSerializer.Deserialize<AppSettings>(json);

        if (settings is null)
        {
            throw new InvalidDataException("配置内容不能为 null。");
        }

        if (string.IsNullOrWhiteSpace(settings.StationName))
        {
            throw new InvalidDataException("StationName 必须填写，且不能全是空白");
        }
        return settings;
    }
}
