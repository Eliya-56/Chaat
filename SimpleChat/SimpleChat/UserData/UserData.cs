using SimpleChat.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleChat.UserData
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public virtual AuthData Auth { get; set; }
        public DateTime TimeOfRegistration { get; set; }

        public virtual ICollection<MessagesHistory> OutMessages{ get; set; }
        public virtual ICollection<MessagesHistory> InMessages { get; set; }
    }
}
