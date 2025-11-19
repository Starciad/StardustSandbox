using StardustSandbox.Constants;

using System;
using System.IO;

namespace StardustSandbox.IO
{
    internal static class SSFile
    {
        internal static string WriteException(Exception exception)
        {
            string logFileName = string.Concat(GameConstants.TITLE, "log", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), ".txt").ToLower();
            string logFilePath = Path.Combine(SSDirectory.Logs, logFileName);

            using StringWriter stringWriter = new();

            stringWriter.WriteLine(exception.ToString());
            File.WriteAllText(logFilePath, stringWriter.ToString());

            return logFilePath;
        }
    }
}
