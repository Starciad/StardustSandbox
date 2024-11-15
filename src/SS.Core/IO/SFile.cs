using StardustSandbox.Core.Constants;

using System;
using System.IO;

namespace StardustSandbox.Core.IO
{
    public static class SFile
    {
        public static string WriteException(Exception exception)
        {
            string logFileName = string.Concat(SGameConstants.TITLE, "log", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), ".txt").ToLower();
            string logFilePath = Path.Combine(SDirectory.Logs, logFileName);

            using StringWriter stringWriter = new();

            stringWriter.WriteLine(exception.ToString());
            File.WriteAllText(logFilePath, stringWriter.ToString());

            return logFilePath;
        }
    }
}
