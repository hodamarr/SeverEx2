using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebChatServer.Models
{
    public class User
    {
        [Key]

        public string Name { get; set; }
        public string Nick { get; set; }


        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Server { get; set; }

        [JsonIgnore]
        public List<Contact> Chats { get; set; }

        public User(string name, string nick, string password, string server)
        {
            Name = name;
            Nick = nick;
            Password = password;
            Server = server;
            Chats = new List<Contact>();
        }
    }
}
