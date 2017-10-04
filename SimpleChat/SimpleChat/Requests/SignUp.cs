using SimpleChat.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Requests
{
    public class SignUp
    {
        public AuthData User { get; set; }


        public SignUp()
        { }

        public SignUp(string Login, string Password)
        {
            User = new AuthData(Login, Password);
        }
    }
}
