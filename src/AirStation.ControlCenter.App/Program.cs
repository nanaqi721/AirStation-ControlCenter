using AirStation.ControlCenter.App.Configuration;
using System.Diagnostics;
using System.Text.Json;

namespace AirStation.ControlCenter.App
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            try
            {
                using Mutex mutex = new Mutex(false, @"Local\AirStatioon.ControlCenter.App");
                if (!TryAcquireMutex(mutex))
                {
                    MessageBox.Show(
                        // 提示框内容
                        "程序已经运行，请切换已打开的窗口",
                        // 提示框标题
                        "提示",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return;
                }

                try
                {
                    RunApplication();
                }
                finally
                {
                    mutex.ReleaseMutex(); 
                }

            }
            catch (Exception ex) when (
                ex is IOException or UnauthorizedAccessException)
            {
                MessageBox.Show(
                    $"程序启动失败：\n{ex.Message}",
                    "启动失败",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                // 退出状态码
                Environment.ExitCode = 1;
            }
        }

        /// <summary>
        /// 立即尝试取得单实例互斥锁，判断当前实例是否可以继续启动。
        /// </summary>
        /// <param name="mutex">已创建或打开且未调用 Dispose 的命名互斥锁对象。</param>
        /// <returns>
        /// 正常取得锁，或接管旧实例遗弃的锁时，返回 <see langword="true"/>；
        /// 锁由其他线程持有时，返回 <see langword="false"/>。
        /// </returns>
        /// <remarks>
        /// 内部使用零等待时间申请锁；捕获 <see cref="AbandonedMutexException"/> 时，当前线程已经取得所有权。
        /// 每次返回 <see langword="true"/> 后，调用方必须在同一线程调用一次 <see cref="Mutex.ReleaseMutex"/>；
        /// 本方法只申请所有权，不负责释放。
        /// </remarks>
        /// <exception cref="ObjectDisposedException">互斥锁对象已被释放，无法继续申请所有权。</exception>
        private static bool TryAcquireMutex(Mutex mutex)
        {
            try
            {
                // WaitOne() 阻塞等待
                // WaitOne(0) 立即尝试取得锁；成功返回 true，被其他进程占用则返回 false
                return mutex.WaitOne(0);
            }
            catch (AbandonedMutexException)
            {
                // 旧的实例异常退出，当前线程已经获取这个互斥锁了
                return true;
            }
        }

        /// <summary>
        /// 加载并校验应用配置，使用站名设置主窗体标题，然后运行 WinForms 消息循环。
        /// </summary>
        /// <remarks>
        /// 调用前应在 STA 主线程完成 ApplicationConfiguration.Initialize，并由调用方持有单实例互斥锁。
        /// 从 <see cref="AppContext.BaseDirectory"/> 查找 appsettings.json；
        /// 配置读取、解析或校验失败时，显示错误提示，将 <see cref="Environment.ExitCode"/> 设为 1 并返回，不创建主窗体。
        /// 正常情况下，消息循环结束后返回；互斥锁的释放由调用方负责。
        /// </remarks>
        /// <exception cref="InvalidOperationException">当前线程已有消息循环，无法再次启动 WinForms 消息循环。</exception>
        public static void RunApplication()
        {
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
                    MessageBoxIcon.Error);

                Environment.ExitCode = 1;
                return;
            }
            MainForm mainForm = new MainForm();
            mainForm.Text = settings.StationName;
            System.Windows.Forms.Application.Run(mainForm);
        }
    }
}