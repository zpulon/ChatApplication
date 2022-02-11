using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketPlugins.Model
{
   public class WebSocketDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public WebSocketDbContext(DbContextOptions<WebSocketDbContext> options)
            : base(options) { }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<OS_User> OS_Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<OS_User>(b =>
            {
                b.ToTable("OS_User");
            });
        }
      }
}
