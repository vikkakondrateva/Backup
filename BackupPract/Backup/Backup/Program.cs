
namespace Backup
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Загружаем данные из json
                var settings = SettingsLoader.Load("D:\\Уник\\3 курс\\BackupPract\\Backup\\Backup\\settings.json");
                Console.WriteLine($"Source: {settings.Source}");
                Console.WriteLine($"Target: {settings.Target}");

                // Получаем текущую дату и время. Создаем имя папки
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string backupFolder = Path.Combine(settings.Target, $"backup_{timestamp}");
                Console.WriteLine($"backupFolder: {backupFolder}\n");



                //создпние папки
                try
                {
                    Directory.CreateDirectory(backupFolder);
                    Logs.Initialize(backupFolder);
                    Logs.Log($"Создана папка для бэкапа: {backupFolder}", "Info", settings.Level);

                    Logs.Log("Запуск программы", "Info", settings.Level);
                    Logs.Log($"Настройки: Source={settings.Source}, Target={settings.Target}", "Debug", settings.Level);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Logs.Log($"Ошибка доступа при создании папки: {ex.Message}", "Error", settings.Level);
                    return;
                }
                catch (Exception ex) when (ex is IOException || ex is ArgumentException)
                {
                    Logs.Log($"Ошибка при создании папки: {ex.Message}", "Error", settings.Level);
                    return;
                }

                try
                {
                    //throw new UnauthorizedAccessException($"test");
                    //throw new DirectoryNotFoundException($"test");
                    foreach (string file in Directory.GetFiles(settings.Source, "*", SearchOption.AllDirectories))
                    {
                       
                        try
                        {
                            //throw new ArgumentException($"test name");

                            string relativePath = file.Substring(settings.Source.Length + 1);
                            string newPath = Path.Combine(backupFolder, relativePath);

                            try
                            {
                                Directory.CreateDirectory(Path.GetDirectoryName(newPath));
                                File.Copy(file, newPath, overwrite: true);
                                Logs.Log($"Скопировано: {file} -> {newPath}", "Debug", settings.Level);
                                //Console.WriteLine($"Copied: {file} -> {newPath}");
                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                Logs.Log($"Нет доступа: {file} ({ex.Message})", "Error", settings.Level);
                            }
                            catch (IOException ex)
                            {
                                Logs.Log($"Ошибка ввода-вывода: {file} ({ex.Message})", "Error", settings.Level);
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            Logs.Log($"Неверный путь: {file} ({ex.Message})", "Error", settings.Level);

                        }


                    }


                }
                catch (UnauthorizedAccessException ex)
                {
                    Logs.Log($"Доступ к каталогу запрещен: {ex.Message}", "Error", settings.Level);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Logs.Log($"Каталог не найден: {settings.Source}: {ex.Message}", "Error", settings.Level);
                }

                Logs.Log("Резервное копирование завершено", "Info", settings.Level);
            }
            catch (Exception ex)
            {
                Logs.Log($"Критическая ошибка: {ex.Message}", "Error", "Error");
            }
        }
            
    }
}
