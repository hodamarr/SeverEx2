using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebChatServer;

namespace WebChatServer.Data
{
    public class WebChatServerContext : DbContext
    {
        public WebChatServerContext (DbContextOptions<WebChatServerContext> options)
            : base(options)
        {
        }

        public DbSet<WebChatServer.Contact>? Contact { get; set; }
    }
}
