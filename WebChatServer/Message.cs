using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace WebChatServer
{
    public class Message
    {
       public int Id { get; set; }
        public DateTime Time { get; set; }
        public bool Self { get; set; }
        public string Content { get; set; }
    }
}
