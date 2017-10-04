using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleChat.Requests
{
    public enum SignType
    {
        Up,
        In
    }

    public class SigningResponse
    {
        public bool SigningResult { get; set; }
        public string ErrorMessage { get; set; }
        public SignType Type { get; set; }

        public SigningResponse()
        { }

        public SigningResponse(bool SignInResult, string ErrorMessage, SignType type)
        {
            this.SigningResult = SignInResult;
            this.Type = type;
            this.ErrorMessage = ErrorMessage;
        }
    }
}
