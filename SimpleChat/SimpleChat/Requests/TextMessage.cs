using SimpleChat.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Requests
{
    public class TextMessage
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Target { get; set; }
        public DateTime TimeOfSend { get; set; }

        public TextMessage()
        {}

        public TextMessage(string Name, string Text, string Target)
        {
            this.Name = Name;
            this.Text = Text;
            this.Target = Target;
            TimeOfSend = DateTime.Now;
        }
    }
}
