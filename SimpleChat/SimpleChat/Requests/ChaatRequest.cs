using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Requests
{
    public enum ChaatRequest
    {
        Message,
        SignUp,
        SignIn,
        SigningResponse,
        SendActiveUsers,
        GiveMeActiveUsers,
    }
}
