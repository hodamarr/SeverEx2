using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebChatServer.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public bool Sent { get; set; }

        [JsonIgnore]
        public int ContactId { get; set; }

        public Message(string content, bool sent, int contactId)
        {
            Content = content;
            Sent = sent;
            Created = DateTime.Now;
            ContactId = contactId;
        }
    }
}
