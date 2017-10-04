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
    public class AuthData
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public AuthData()
        { }

        public AuthData(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
        }
    }
}
