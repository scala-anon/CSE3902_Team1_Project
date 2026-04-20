using System;
using System.IO;

namespace HollowKnight.Shared
{
    public static class DebugLogger
    {
        private static readonly string LogFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "debug.log");
        private static readonly object _lock = new object();

        static DebugLogger()
        {
            if (GameConstants.DEBUG_LOG_ENABLED != 1) return;

            try
            {
                File.WriteAllText(LogFilePath, $"[DebugLogger] Session started at {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}\n");
            }
            catch (Exception) { }
        }

        public static void Log(string category, string message)
        {
            if (GameConstants.DEBUG_LOG_ENABLED != 1) return;

            string line = $"[{DateTime.Now:HH:mm:ss.fff}][{category}] {message}\n";
            try
            {
                lock (_lock)
                {
                    File.AppendAllText(LogFilePath, line);
                }
            }
            catch (Exception) { }
        }

        public static void LogCollision(string msg)      => Log("COLLISION", msg);
        public static void LogAStar(string msg)          => Log("ASTAR", msg);
        public static void LogObject(string msg)         => Log("OBJECT", msg);
        public static void LogRoomTransition(string msg) => Log("ROOM", msg);
        public static void LogGeneral(string msg)        => Log("GENERAL", msg);
    }
}
