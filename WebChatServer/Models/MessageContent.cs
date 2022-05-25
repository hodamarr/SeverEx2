using System.ComponentModel.DataAnnotations;

namespace WebChatServer.Models
{
    public class MessageContent
    {
        [Key]
        public string Content { get; set; }
    }
}
