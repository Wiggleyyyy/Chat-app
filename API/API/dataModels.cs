using System.ComponentModel.DataAnnotations;

namespace API
{
    public class Users
    {
        public int user_id { get; set; }
        [Required]
        public string first_name { get; set; }
        [Required]
        public string last_name { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public DateTime date_of_birth { get; set; }
        [Required]
        public string username { get; set; }
        [Required]
        public string email { get; set; }
        [Required]
        public string hashed_password { get; set; }
        [Required]
        public DateTime created_at { get; set; }
        [Required]
        public string user_tag { get; set; }
    }

    public class Friendships
    {
        public int friendship_id { get; set; }
        [Required]
        public int user_1_id { get; set; }
        [Required]
        public int user_2_id { get; set; }
        [Required]
        public int status_id { get; set; }
        [Required]
        public DateOnly created_at { get; set; }
    }

    public class Chats
    {
        public int chat_id { get; set; }
        [Required]
        public int from_user_id { get; set; }
        [Required]
        public int to_user_id { get; set; }
        [Required]
        public string text { get; set; }
        [Required]
        public bool pinned { get; set; }
        [Required]
        public DateTime created_at { get; set; }
    }

    public class ChatReturnData
    {
        public List<Chats> chats_from_user_1 = new List<Chats>();
        public List<Chats> chats_from_user_2 = new List<Chats>();
        public int user_id_1 { get; set; }
        public int user_id_2 { get; set; }
    }
}