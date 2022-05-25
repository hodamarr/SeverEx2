using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebChatServer.Models;

namespace WebChatServer.Data
{
    public class WebChatServerContext : DbContext
    {
        public WebChatServerContext (DbContextOptions<WebChatServerContext> options)
            : base(options)
        {
        }

        public DbSet<WebChatServer.Models.Contact>? Contact { get; set; }

        public DbSet<WebChatServer.Models.Message>? Message { get; set; }

        public DbSet<WebChatServer.Models.User>? User { get; set; }

    }
}
