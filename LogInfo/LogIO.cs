using System.IO;

namespace LogInfo
{
    public static class LogIO
    {
        public static string path = "FirstLog.log";
        public delegate void Logging(string text, Log log);
        public static void WriteLog(string path, Log log)
        {
            try
            {
                File.AppendAllText(path, log.ToString() + "\n");
            }
            catch { }
        }
    }
}
