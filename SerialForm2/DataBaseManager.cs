using SerialForm2;
using System;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;

namespace SerialComm
{
    public class DatabaseManager
    {
        public string CreateFolder(string prefix)
        {
            string year = DateTime.Now.ToString("yyyy");
            string month = DateTime.Now.ToString("MM");
            string day = DateTime.Now.ToString("dd");
            string hour = DateTime.Now.ToString("HH");

            string folderPath = Path.Combine(Variable.baseFolderPath, $"SVMU_{year}", $"SVMU_{year}{month}", $"SVMU_{year}{month}{day}");
            string dbFilePath = Path.Combine(folderPath, $"SVMU_{year}{month}{day}_{hour}_{prefix}.db");

            Directory.CreateDirectory(folderPath);

            return dbFilePath;
        }

        public async Task AddDBTableAsync(string dbFilePath)
        {
            string connectionString = $"Data Source={dbFilePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS DataLog (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Prefix TEXT NOT NULL,
                    Data TEXT NOT NULL,
                    Timestamp TEXT NOT NULL
                );";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task AddDataToDBAsync(string dbFilePath, string prefix, string data)
        {
            string connectionString = $"Data Source={dbFilePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync();
                string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string insertQuery = @"
                INSERT INTO DataLog (Prefix, Data, Timestamp) 
                VALUES (@Prefix, @Data, @Timestamp);";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Prefix", prefix);
                    command.Parameters.AddWithValue("@Data", data);
                    command.Parameters.AddWithValue("@Timestamp", currentTime);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}