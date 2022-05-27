using System.ComponentModel.DataAnnotations;

namespace WebChatServer.Models
{
    public class AddContactData
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Server { get; set; }

        public AddContactData(string id, string name, string server)
        {
            Id = id;
            Name = name;
            Server = server;
        }
    }
}
