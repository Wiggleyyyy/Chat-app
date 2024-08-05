using System.ComponentModel.DataAnnotations;

namespace API
{
    public class Users
    {
        public int user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string country { get; set; }
        public DateTime date_of_birth { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string private_encrypted_password { get; set; }
        public string public_encrypted_password { get; set; }
        public string public_key { get; set; }
        public DateTime created_at { get; set; }
        public string user_tag { get; set; }
    }

    public class Friendships
    {
        public int friendship_id { get; set; }
        public int user_1_id { get; set; }
        public int user_2_id { get; set; }
        public int status_id { get; set; }
        public DateOnly created_at { get; set; }
    }

    public class Chats
    {
        public int chat_id { get; set; }
        public int user_id { get; set; }
        public string text { get; set; }
        public bool pinned { get; set; }
        public DateTime created_at { get; set; }
    }
}