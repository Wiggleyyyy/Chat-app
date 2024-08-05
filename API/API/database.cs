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

        Aes aes = Aes.Create();
        byte[] ciphertext;

        public bool signIn(Users user)
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();


                // Check for user
                string query = "SELECT * FROM Users WHERE email = @email AND password = @password OR user_tag = @user_tag AND password = @password";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@email", user.email);
                user.public_encrypted_password = encrypt(user.private_encrypted_password, user.public_key).ToString();
                command.Parameters.AddWithValue("@password", user.public_encrypted_password);
                command.Parameters.AddWithValue("@user_tag", user.user_tag);
                SqlDataReader reader = command.ExecuteReader();
                bool hasRows = reader.HasRows;

                connection.Close();

                return hasRows;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't login", ex.Message);

                if (connection.State != System.Data.ConnectionState.Closed)
                {
                    connection.Close();
                }

                return false;
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
                query = "INSERT INTO Users (first_name, last_name, country, date_of_birth, username, email, encrypted_password, public_key, created_at, user_tag) VALUES(@first_name, @last_name, @country, @date_of_birth, @username, @email, @encrypted_password, @public_key, @created_at, @user_tag)";
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@first_name", user.first_name);
                command.Parameters.AddWithValue("@last_name", user.last_name);
                command.Parameters.AddWithValue("@country", user.country);
                command.Parameters.AddWithValue("@date_of_birth", user.date_of_birth);
                command.Parameters.AddWithValue("@username", user.username);
                command.Parameters.AddWithValue("@email", user.email);
                // ENCRYPT PASSWORD HERE AND USE BELOW - MAYBE FIX
                user.public_encrypted_password = encrypt(user.private_encrypted_password, user.public_key);
                command.Parameters.AddWithValue("@encrypted_password", user.public_encrypted_password);
                command.Parameters.AddWithValue("@public_key", user.public_key);
                command.Parameters.AddWithValue("@created_at", user.created_at);
                if (!user.user_tag.StartsWith("@"))
                {
                    user.user_tag = "@" + user.user_tag;
                }
                command.Parameters.AddWithValue("@user_tag", user.user_tag);

                Console.WriteLine("TEST");
                int rowsAffected = command.ExecuteNonQuery();

                Console.WriteLine("ran command again");

                connection.Close();

                if (rowsAffected > 0)
                {
                    Console.WriteLine("created account");

                    // Created
                    return true;
                }
                else
                {
                    // Couldnt create
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

        private static string encrypt(string input, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key.Substring(0, 32));
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);

                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                    }

                    byte[] encrypted = msEncrypt.ToArray();
                    return Convert.ToBase64String(encrypted);
                }
            }
        }

        public string decrypt(byte[] decInput, byte[] key)
        {
            ICryptoTransform decryptor = aes.CreateDecryptor(key, aes.IV);
            byte[] plaintextBytes = decryptor.TransformFinalBlock(decInput, 0, ciphertext.Length);
            string plaintext = Encoding.UTF8.GetString(plaintextBytes);

            return plaintext;
        }
    }
}
