using SerialForm2;
using System;
using System.Data.SQLite;
using System.IO;

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

            // Ensure the directory exists
            Directory.CreateDirectory(folderPath);

            return dbFilePath;
        }

        public void AddDBTable(string dbFilePath)
        {
            string connectionString = $"Data Source={dbFilePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS DataLog (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Prefix TEXT NOT NULL,
                    Data TEXT NOT NULL,
                    Timestamp TEXT NOT NULL
                );";

                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddDataToDB(string dbFilePath, string prefix, string data)
        {
            string connectionString = $"Data Source={dbFilePath};Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string insertQuery = @"
                INSERT INTO DataLog (Prefix, Data, Timestamp) 
                VALUES (@Prefix, @Data, @Timestamp);";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Prefix", prefix);
                    command.Parameters.AddWithValue("@Data", data);
                    command.Parameters.AddWithValue("@Timestamp", currentTime);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
