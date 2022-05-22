using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace WebChatServer
{
    public class Contact
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Server { get; set; }

        public List<Message> Msgs { get; set; }
    }
}
