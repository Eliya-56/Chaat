using SimpleChat.Requests;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleChat.UserData
{
    class UserDataContext : DbContext
    {
        public UserDataContext(string connectionString) : base(connectionString)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<MessagesHistory> Messages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessagesHistory>().HasRequired(x => x.Sender).WithMany(x => x.OutMessages).HasForeignKey(x => x.SenderId).WillCascadeOnDelete(false);
            modelBuilder.Entity<MessagesHistory>().HasRequired(x => x.Reciver).WithMany(x => x.InMessages).HasForeignKey(x => x.ReciverId).WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}
