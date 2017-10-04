using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Requests
{
    class SignUpResponse
    {
        public bool SignUpResult { get; set; }

        public SignUpResponse()
        {}

        public SignUpResponse(bool SignUpResult)
        {
            this.SignUpResult = SignUpResult;
        }
    }
}
