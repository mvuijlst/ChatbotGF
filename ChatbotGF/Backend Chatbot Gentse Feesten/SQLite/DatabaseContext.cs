using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Chatbot_GF.Model;

namespace Chatbot_GF.SQLite
{

        public class DatabaseContext : DbContext
        {
            public DbSet<User> Posts { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite("Data Source=chatbot.db");
            }
        }
 }
