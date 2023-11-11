using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AmootSearch.Entities
{
    public class DatabaseInitializer
    {
        public static string ConnectionString{ get; private set; }

        public static void Initialize()
        {
            ConnectionString = $"Server=localhost;" +
                               $"Database=Search;" +
                               $"User Id=search;" +
                               $"Password=123465;" +
                               $"MultipleActiveResultSets=True;" +
                               $"Encrypt=False;" +
                               $"Integrated Security=False";

            if (!CheckDatabaseExist())
            {
                string query = $"CREATE DATABASE Search;";

                using var conn = new SqlConnection(ConnectionString);
                var command = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
            }

            if (!CheckDatabaseExist())
            {
                string query = $"CREATE DATABASE Search;";

                using var conn = new SqlConnection(ConnectionString);
                var command = new SqlCommand(query, conn);
                conn.Open();
                command.ExecuteNonQuery();
            }

            SetUpDatabase();
        }

        public static bool CheckDatabaseExist()
        {
            var tmpConn = new SqlConnection(ConnectionString);

            var sqlCreateDBQuery = string.Format($"SELECT database_id FROM sys.databases WHERE Name = 'Search'");

            using (tmpConn)
            {
                using SqlCommand sqlCmd = new SqlCommand(sqlCreateDBQuery, tmpConn);

                tmpConn.Open();

                object resultObj = sqlCmd.ExecuteScalar();

                int databaseID = 0;

                if (resultObj != null)
                {
                    int.TryParse(resultObj.ToString(), out databaseID);
                }
                tmpConn.Close();

                return (databaseID > 0);
            }
        }

        public static void SetUpDatabase()
        {
            using var context = new DataBaseContext(ConnectionString);
            try
            {
                context.Database.Migrate();

                context.Database.EnsureCreated();//do not use EnsureCreate before migration
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
