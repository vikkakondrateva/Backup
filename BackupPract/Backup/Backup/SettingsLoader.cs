using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backup
{
    public class SettingsLoader
    {
        public static SettingsClass Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                var defaults = new SettingsClass
                {
                    Source = "D:/Уник/3 курс/SourceFolder",
                    Target = "D:/Уник/3 курс/DefaultBackup",
                    Level = "Info"
                };
                File.WriteAllText(filePath, JsonSerializer.Serialize(defaults));
                return defaults;
            }
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<SettingsClass>(json);
        }
    }
}
