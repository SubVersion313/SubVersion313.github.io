using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace El_Harrifa.Services
{
    public class BackupService : BackgroundService
    {
        private readonly ILogger<BackupService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _backupPath;
        private readonly string _connectionString;
        private readonly int _daysToKeep;

        public BackupService(
            ILogger<BackupService> logger,
            IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _backupPath = _configuration["BackupSettings:Path"] ?? "Backups";
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _daysToKeep = _configuration.GetValue<int>("BackupSettings:DaysToKeep", 7);

            // Ensure backup directory exists
            Directory.CreateDirectory(_backupPath);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting scheduled database backup");
                    var backupFile = await DatabaseBackup.CreateBackupAsync(_connectionString, _backupPath);
                    _logger.LogInformation("Database backup completed: {BackupFile}", backupFile);

                    // Cleanup old backups
                    DatabaseBackup.CleanupOldBackups(_backupPath, _daysToKeep);
                    _logger.LogInformation("Old backups cleanup completed");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while creating database backup");
                }

                // Wait for 24 hours before next backup
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
} 