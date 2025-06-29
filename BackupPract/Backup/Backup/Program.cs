
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
                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"No access to create backup directory: {ex.Message}");
                    return;
                }
                catch (Exception ex) when (ex is IOException || ex is ArgumentException)
                {
                    Console.WriteLine($"Failed to create backup directory: {ex.Message}");
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
                                Console.WriteLine($"Copied: {file} -> {newPath}");
                            }
                            catch (UnauthorizedAccessException ex)
                            {
                                Console.WriteLine($"No access to create/copy file: {file} ({ex.Message})");
                            }
                            catch (IOException ex)
                            {
                                Console.WriteLine($"IO error with file: {file} ({ex.Message})");
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            Console.WriteLine($"Invalid name of file: {file} ({ex.Message})\n");

                        }


                    }


                }
                catch (UnauthorizedAccessException ex)
                {
                    Console.WriteLine($"No access to directory: {settings.Source} ({ex.Message})");
                }
                catch (DirectoryNotFoundException ex)
                {
                    Console.WriteLine($"Directory not found: {settings.Source} ({ex.Message})");
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fatal error: {ex.Message}");
            }
        }
            
    }
}
