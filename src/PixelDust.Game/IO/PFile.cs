using PixelDust.Game.Constants;

using System;
using System.IO;

namespace PixelDust.Game.IO
{
    public static class PFile
    {
        public static string WriteException(Exception exception)
        {
            string logFileName = string.Concat(PGameConstants.TITLE, "log", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), ".txt").ToLower();
            string logFilePath = Path.Combine(PDirectory.Logs, logFileName);

            using StringWriter stringWriter = new();

            stringWriter.WriteLine(exception.ToString());
            File.WriteAllText(logFilePath, stringWriter.ToString());

            return logFilePath;
        }
    }
}
