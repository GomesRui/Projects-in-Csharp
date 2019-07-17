using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.ValidatePhoto.Providers
{
    public class DatabaseConnection
    {
        private readonly SqlConnectionStringBuilder builder;

        public DatabaseConnection(string dataSource, string databaseString, string oracleUser, string oraclePass)
        {
                builder = new SqlConnectionStringBuilder();
                builder.DataSource = dataSource;
                builder.InitialCatalog = databaseString;
                builder.UserID = oracleUser;
                builder.Password = oraclePass;
        }
        public string GetDataFromDatabase(SqlConnection sqlConnection, int group, string username)
        {
            try
            {
                //SQL statement: select cr.password from credentials cr inner join groups gr on cr.groupID = gr.ID where cr.username = 'GomesRui' and gr.name LIKE '%Security%';
                sqlConnection.Open();
                StringBuilder sb = new StringBuilder();
                sb.Append("SELECT cr.password");
                sb.Append(" FROM Credentials cr");
                sb.Append(" INNER JOIN Groups gr on cr.groupID = gr.ID");
                sb.Append(" WHERE cr.username = ");
                sb.Append(" '" + username + "'");
                sb.Append(" AND gr.id =");
                sb.Append(group);
                sb.Append(";");
                String sql = sb.ToString();

                using (SqlCommand command = new SqlCommand(sql, sqlConnection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                return (reader.GetString(0));
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw e;
            }
            return "No rows found";
        }

        public async Task<bool> PasswordAuthentication(int group, string username, string password)
        {

                try
                {

                    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                    {
                        Task<string> sqlData = Task.Run(() => GetDataFromDatabase(connection, group, username));
                        await sqlData;

                        if (sqlData.Result == password)
                        {
                            Console.WriteLine("\nPassword Authentication passed!");
                            API.attempts = 0;
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("\nPassword Authentication failed!");
                            API.attempts--;
                        }

                    }
                }
                catch (Exception)
                {
                    throw new Exception("Invalid Database Connection string!");
                }



            return false;

        }
    }
}
