#if SS_WINDOWS
using System.Windows.Forms;
#elif SS_LINUX
using System.Diagnostics;
#endif

namespace StardustSandbox.OS
{
    internal static class SSMessageBox
    {
#if SS_LINUX
        private static void StartProcess(string fileName, string arguments)
        {
            using Process process = new()
            {
                StartInfo = new()
                {
                    FileName = fileName,
                    Arguments = arguments,
                }
            };
            _ = process.Start();
        }
#endif

        internal static void ShowError(string title, string message)
        {
#if SS_WINDOWS
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
#elif SS_LINUX
            StartProcess("zenity", $"--error --title=\"{title}\" --text=\"{message.Replace("\"", "\\\"")}\"");
#endif
        }
    }
}
