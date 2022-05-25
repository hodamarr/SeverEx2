using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebChatServer.Models
{
    public class Contact
    {

        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Server { get; set; }

        [JsonIgnore]
        public List<Message>? Msgs { get; set; }

        //[JsonIgnore]
        //public string UserId { get; set; }

        public string? Last {get; set; }

        public DateTime? Lastdate { get; set; }

        public Contact(string id, string name, string server)
        {
            Id = id;
            Name = name;
            Server = server;
            Msgs = new List<Message>();
            Last = null;
            Lastdate = null;
            //ADD CONNECTED USER ID
            //UserId = "me";
        }
    }
}
