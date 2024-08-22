using responseexample.Models;
using System.Data.SqlClient;

namespace responseexample.DAL
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            User ?user = null;

            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT Id, Name FROM Users WHERE Id = @UserId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    await conn.OpenAsync();
                    SqlDataReader reader = await cmd.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                        }
                    }

                    reader.Close();
                }
            }
            catch (Exception)
            {
                throw; // Re-throw the exception to be handled at higher levels if needed
            }

            return user;
        }

        public void DeleteUser(int userId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "DELETE FROM Users WHERE Id = @UserId";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception)
            {
                throw; // Re-throw the exception to be handled at higher levels if needed
            }
        }

        public async Task<bool> CreateUserAsync(User user)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO Users (Name) VALUES (@Name)";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Name", user.Name);

                    await conn.OpenAsync();
                    int result = await cmd.ExecuteNonQueryAsync();

                    return result > 0;
                }
            }
            catch (Exception)
            {
                throw; // Re-throw the exception to be handled at higher levels if needed
            }
        }

        public HttpResponseMessage GetHttpResponseMessage()
        {
            try
            {
                return new HttpResponseMessage(System.Net.HttpStatusCode.OK)
                {
                    Content = new StringContent("This is a custom HTTP response message.")
                };
            }
            catch (Exception)
            {
                throw; // Re-throw the exception to be handled at higher levels if needed
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var command = new SqlCommand("SELECT * FROM Users", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        // Method to get users starting with a specific letter (needed for V2)
        public async Task<IEnumerable<User>> GetUsersStartingWithAsync(char letter)
        {
            var users = new List<User>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Users WHERE Name LIKE @Letter + '%'";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Letter", letter.ToString());

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var user = new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }
    }
}
