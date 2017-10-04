using SimpleChat.UserData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Requests
{
    public class SignIn
    {
        public AuthData User { get; set; }
        

        public SignIn()
        { }

        public SignIn(string Login, string Password)
        {
            User = new AuthData(Login, Password);
        }
    }
}
