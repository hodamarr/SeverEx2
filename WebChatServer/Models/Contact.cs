using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebChatServer.Models
{
    public class Contact
    {

        [Key]
        [JsonIgnore]
        public int number {get; set; }
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Server { get; set; }

        public string? Last { get; set; }

        public DateTime? Lastdate { get; set; }

        [JsonIgnore]
        public List<Message>? Msgs { get; set; }

        [JsonIgnore]
        public string UserName { get; set; }


        public Contact(string id, string name, string server, User user2)
        {
            Id = id;
            Name = name;
            Server = server;
            Msgs = new List<Message>();
            Last = null;
            Lastdate = null;
            UserName = user2.Name;
        }
        public Contact(string id, string name, string server)
        {
            Id = id;
            Name = name;
            Server = server;
            Msgs = new List<Message>();
            Last = null;
            Lastdate = null;
            UserName = "";
        }
    }
}
