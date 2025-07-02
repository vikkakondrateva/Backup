using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backup
{
    internal class Logs
    {
        private static string logFilePath;

        public static void Initialize(string backupFolder)
        {
            logFilePath = Path.Combine(backupFolder, $"backup_log_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");
        }

        public static void Log(string message, string level, string configuredLevel)
        {
            // Проверяем, нужно ли логировать это сообщение
            if (ShouldLog(level, configuredLevel))
            {
                string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] [{level}] {message}";
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
                Console.WriteLine(logEntry); // Дублируем в консоль
            }
        }

        private static bool ShouldLog(string messageLevel, string configuredLevel)
        {
            var levels = new Dictionary<string, int>
            {
                {"Error", 3},
                {"Info", 2},
                {"Debug", 1}
            };

            return levels[messageLevel] >= levels[configuredLevel];
        }

    }
}
