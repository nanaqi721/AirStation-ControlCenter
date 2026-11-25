using AirStation.ControlCenter.App.Configuration;
using System.Text.Json;

namespace AirStation.ControlCenter.App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            string filePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            AppSettings settings;
            try
            {
                settings = AppSettingsLoader.Load(filePath);
            }
            catch (Exception ex) when (
                ex is IOException or UnauthorizedAccessException
                or JsonException or InvalidDataException)
            {
                MessageBox.Show(
                    $"无法加载配置文件：\n{filePath}\n\n{ex.Message}",
                    "启动失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error );

                Environment.ExitCode = 1;
                return;
            }
            MainForm mainForm = new MainForm();
            mainForm.Text = settings.StationName;
            System.Windows.Forms.Application.Run(mainForm);
        }
    }
}