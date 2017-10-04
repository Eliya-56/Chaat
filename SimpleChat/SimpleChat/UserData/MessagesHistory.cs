using SimpleChat.UserData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Requests
{
    public class MessagesHistory
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SendTime { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }

        public int ReciverId { get; set; }
        public virtual User Reciver { get; set; }
    }
}
