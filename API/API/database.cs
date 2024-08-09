using Microsoft.AspNetCore.Http.HttpResults;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace API
{
    public class database
    {
        private readonly string connectionString = "Data Source=localhost;Initial Catalog=Chat_app;User ID=ConnectionUser;Password=AppConnection!;Integrated Security=True";
        public SqlConnection connection;

        //Aes aes = Aes.Create();
        //byte[] ciphertext;

        public Users signIn(string username, string hashed_password)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                string email = "";
                if (!username.StartsWith("@") && !username.Contains("@"))
                {
                    username = "@" + username;
                }
                else if (username.Contains("@"))
                {
                    email = username;
                }

                // Check for user
                string query = "SELECT * FROM Users WHERE email = @email AND hashed_password = @password OR user_tag = @user_tag AND hashed_password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", hashed_password);
                command.Parameters.AddWithValue("@user_tag", username);
                SqlDataReader reader = command.ExecuteReader();

                Users user = new Users();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        user.user_id = reader.GetInt32(reader.GetOrdinal("user_id"));
                        user.user_tag = reader.GetString(reader.GetOrdinal("user_tag"));
                        user.first_name = reader.GetString(reader.GetOrdinal("first_name"));
                        user.last_name = reader.GetString(reader.GetOrdinal("last_name"));
                        user.country = reader.GetString(reader.GetOrdinal("country"));
                        user.date_of_birth = reader.GetDateTime(reader.GetOrdinal("date_of_birth")); //Maybe error
                        user.username = reader.GetString(reader.GetOrdinal("username"));
                        user.email = reader.GetString(reader.GetOrdinal("email"));
                        user.hashed_password = "Hidden";
                        user.created_at = reader.GetDateTime(reader.GetOrdinal("created_at")); // Maybe error

                        reader.Close();
                        break;
                    }
                }

                connection.Close();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't login", ex.Message);

                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }

                return null;
            }
        }

        public bool signUp(Users user)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                Console.WriteLine("created connection");

                //Check for user
                string query = "SELECT COUNT(*) FROM Users WHERE user_tag = @user_tag OR email = @email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_tag", user.user_tag);
                command.Parameters.AddWithValue("@email", user.email);
                int existingUsersCount = (int)command.ExecuteScalar();

                // Returns false if user exists
                if (existingUsersCount > 0)
                {
                    Console.WriteLine("user exists");
                    return false;
                }

                //Create user
                query = "INSERT INTO Users (first_name, last_name, country, date_of_birth, username, email, hashed_password, created_at, user_tag) VALUES(@first_name, @last_name, @country, @date_of_birth, @username, @email, @hashed_password, @created_at, @user_tag)";
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@first_name", user.first_name);
                command.Parameters.AddWithValue("@last_name", user.last_name);
                command.Parameters.AddWithValue("@country", user.country);
                command.Parameters.AddWithValue("@date_of_birth", user.date_of_birth);
                command.Parameters.AddWithValue("@username", user.username);
                command.Parameters.AddWithValue("@email", user.email);
                command.Parameters.AddWithValue("@hashed_password", user.hashed_password);
                command.Parameters.AddWithValue("@created_at", user.created_at);
                if (!user.user_tag.StartsWith("@"))
                {
                    user.user_tag = "@" + user.user_tag;
                }
                command.Parameters.AddWithValue("@user_tag", user.user_tag);

                int rowsAffected = command.ExecuteNonQuery();

                connection.Close();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("created account");

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error creating account", error);

                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }

                return false;
            }
        }
        #region encryptionCode

        //private static string encrypt(string input, string key)
        //{
        //    using (Aes aesAlg = Aes.Create())
        //    {
        //        try
        //        {
        //            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
        //            aesAlg.Key = keyBytes;
        //            //aesAlg.Key = GetAesKey(key, 32);
        //        }
        //        catch (Exception error)
        //        {
        //            Console.WriteLine(error);
        //        }
        //        aesAlg.GenerateIV();

        //        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        {
        //            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

        //            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //                {
        //                    swEncrypt.Write(input);
        //                }
        //            }

        //            byte[] encrypted = msEncrypt.ToArray();
        //            return Convert.ToBase64String(encrypted);
        //        }
        //    }
        //}

        //public string decrypt(byte[] decInput, byte[] key)
        //{
        //    ICryptoTransform decryptor = aes.CreateDecryptor(key, aes.IV);
        //    byte[] plaintextBytes = decryptor.TransformFinalBlock(decInput, 0, ciphertext.Length);
        //    string plaintext = Encoding.UTF8.GetString(plaintextBytes);

        //    return plaintext;
        //}

        #endregion

        public bool acceptFriendRequest(string self_user_tag, string other_user_tag)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                //Get user IDs
                int user1_ID = 0;
                int user2_ID = 0;

                string query = $"SELECT * FROM Users WHERE user_tag = {self_user_tag}";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user1_ID = reader.GetInt32(reader.GetOrdinal("user_id"));
                    reader.Close();
                    break;
                }

                query = $"SELECT * FROM Users WHERE user_tag = {other_user_tag}";
                command = new SqlCommand(query, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user2_ID = reader.GetInt32(reader.GetOrdinal("user_id"));
                    reader.Close();
                    break;
                }

                Friendships friendShip = new Friendships();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }
                return false;
            }
        }

        public bool sendFriendRequst(string self_user_tag, string other_user_tag)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                Friendships friendShip = new Friendships();

                //Get user IDs first
                int user1_ID = 0;
                int user2_ID = 0;

                string query = $"SELECT * FROM Users WHERE user_tag = {self_user_tag}";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user1_ID = reader.GetInt32(reader.GetOrdinal("user_id"));
                    reader.Close();
                    break;
                }

                query = $"SELECT * FROM Users WHERE user_tag = {other_user_tag}";
                command = new SqlCommand(query, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user2_ID = reader.GetInt32(reader.GetOrdinal("user_id"));
                    reader.Close();
                    break;
                }

                friendShip.user_1_id = user1_ID;
                friendShip.user_2_id = user2_ID;
                friendShip.status_id = 2;
                friendShip.created_at = DateTime.Now;

                query = "INSER INTO Friendships(user_1_id, user_2_id, status_id, created_at) VALUES(@user_1_id, @user_2_id, @status_id, @created_at)";
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@user_1_id", friendShip.user_1_id);
                command.Parameters.AddWithValue("@user_2_id", friendShip.user_2_id);
                command.Parameters.AddWithValue("@status_id", friendShip.status_id);
                command.Parameters.AddWithValue("@created_at", friendShip.created_at);
                int rowsAffected = (int)command.ExecuteNonQuery();

                connection.Close();

                if (rowsAffected > 0)
                {
                    //return true if inserted
                    return true;
                }
                else
                {
                    //return false if not inserted
                    return false;
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }

                return false;
            }
        }

        public bool checkForFriendRequest(string self_user_tag, string other_user_tag)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();

                //Get user IDs first
                int user1_ID = 0;
                int user2_ID = 0;

                string query = $"SELECT * FROM Users WHERE user_tag = {self_user_tag}";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user1_ID = reader.GetInt32(reader.GetOrdinal("user_id"));
                    reader.Close();
                    break;
                }

                query = $"SELECT * FROM Users WHERE user_tag = {other_user_tag}";
                command = new SqlCommand(query, connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    user2_ID = reader.GetInt32(reader.GetOrdinal("user_id"));
                    reader.Close();
                    break;
                }

                // Check for existing friendrequest
                query = "SELECT COUNT(*) FROM Friendships WHERE user_1_id = @user_1_id AND user_2_id = @user_2_id OR user_1_id = @user_2_id AND user_2_id = @user_1_id";
                command = new SqlCommand(query, connection);
                int existingFriendRequestsCount = (int)command.ExecuteScalar();

                connection.Close();

                if (existingFriendRequestsCount > 0)
                {
                    //return true if exists
                    return true;
                }
                else
                {
                    //return false if exists
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }

                return false;
            }
        }

        //public bool sendMessage(Chats chat)
        //{
        //    try
        //    {
        //        connection = new SqlConnection(connectionString);
        //        connection.Open();
        //        Console.WriteLine("connection created");

        //        string query = "INSERT INTO Chats (chat, pinned, from_user_id, to_user_id, created_at) VALUES (@chat, @pinned, @from_user_id, @to_user_id, @created_at)";
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@chat", chat.text);
        //        command.Parameters.AddWithValue("@pinned", chat.pinned);
        //        command.Parameters.AddWithValue("@from_user_id", chat.from_user_id);
        //        command.Parameters.AddWithValue("@to_user_id", chat.to_user_id);
        //        command.Parameters.AddWithValue("@created_at", chat.created_at);
        //        int affected = command.ExecuteNonQuery();


        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Couldnt create database connection");
        //        if (connection.State != System.Data.ConnectionState.Closed)
        //        {
        //            connection.Close();
        //        }
        //        return false;
        //    }
        //}

        //public List<Chats> getMessagesBetweenUsers(int userid_1, int userid_2)
        //{

        //}
    }
}