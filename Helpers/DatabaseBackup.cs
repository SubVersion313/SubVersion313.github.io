using System.Diagnostics;

namespace El_Harrifa.Helpers
{
    public static class DatabaseBackup
    {
        public static async Task<string> CreateBackupAsync(string connectionString, string backupPath)
        {
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFile = Path.Combine(backupPath, $"backup_{timestamp}.sql");

            // Extract connection details
            var builder = new MySqlConnectionStringBuilder(connectionString);
            var server = builder.Server;
            var database = builder.Database;
            var userId = builder.UserID;
            var password = builder.Password;

            // Create mysqldump command
            var startInfo = new ProcessStartInfo
            {
                FileName = "mysqldump",
                Arguments = $"--host={server} --user={userId} --password={password} {database}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                using (var process = Process.Start(startInfo))
                {
                    using (var outputFile = File.Create(backupFile))
                    {
                        await process.StandardOutput.BaseStream.CopyToAsync(outputFile);
                    }

                    var error = await process.StandardError.ReadToEndAsync();
                    if (!string.IsNullOrEmpty(error))
                    {
                        throw new Exception($"Backup error: {error}");
                    }

                    await process.WaitForExitAsync();
                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"Backup failed with exit code {process.ExitCode}");
                    }
                }

                return backupFile;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create database backup: {ex.Message}", ex);
            }
        }

        public static void CleanupOldBackups(string backupPath, int daysToKeep = 7)
        {
            var cutoffDate = DateTime.Now.AddDays(-daysToKeep);
            var backupFiles = Directory.GetFiles(backupPath, "backup_*.sql");

            foreach (var file in backupFiles)
            {
                var fileInfo = new FileInfo(file);
                if (fileInfo.CreationTime < cutoffDate)
                {
                    try
                    {
                        fileInfo.Delete();
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue with other files
                        Debug.WriteLine($"Failed to delete old backup {file}: {ex.Message}");
                    }
                }
            }
        }
    }
} 