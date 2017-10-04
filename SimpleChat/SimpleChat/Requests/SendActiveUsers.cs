using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Requests
{
    public class SendActiveUsers
    {
        public int Num { get; set; }
        public string[] UsersNames { get; set; }

        public SendActiveUsers()
        {}

        public SendActiveUsers(string[] UsersNames)
        {
            Num = UsersNames.Length;
            this.UsersNames = UsersNames;
        }
    }
}
