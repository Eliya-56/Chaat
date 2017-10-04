using SimpleChat.Requests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleChat
{
    public class Client
    {
        TcpClient client;
        IPEndPoint serverAddress;
        ChaatStream stream;
        public bool Connected { get; set; } = false;
        public bool Signing { get; set; }
        public List<string> ActiveUsers { get; set; } = new List<string>();


        public event Action NoConnecting;
        public event Action<bool, string, SignType> SigningResult;
        public event Action<string,string, DateTime> MessageRecived;
        public event Action UsersUpdated;
        public event Action Disconnected;

        public Client(IPAddress serverIP, int port)
        {
            serverAddress = new IPEndPoint(serverIP, port);
            client = new TcpClient();
        }

        public bool Connect()
        {
            Thread connecting = new Thread(() => { try { client.Connect(serverAddress); } catch { client.Close(); client = null; } });
            connecting.Start();
            Thread.Sleep(2000);
            if (client.GetStream() is null)
            {
                client = new TcpClient();
                return false;
            }
            //client.Connect(serverAddress);
            var stream = client.GetStream();
            this.stream = new ChaatStream(stream);
            Thread readingStream = new Thread(Read);
            readingStream.Start();
            Connected = true;
            return true;
        }

        private void Read()
        {

            while (true)
            {
                try
                {
                    var message = stream.Read();
                    var RequestType = message.Request;
                    switch (RequestType)
                    {
                        case ChaatRequest.Message:
                            ReadMessage((TextMessage)message.Data);
                            break;
                        case ChaatRequest.SigningResponse:
                            ReadSigningResponse((SigningResponse)message.Data);
                            break;
                        case ChaatRequest.SendActiveUsers:
                            UpdateUsers((SendActiveUsers)message.Data);
                            break;
                    }
                }
                catch
                {
                    Connected = false;
                    Disconnected?.Invoke();
                    break;
                }
            }
        }

        private void ReadSigningResponse(SigningResponse data)
        {
            Signing = data.SigningResult;
            SigningResult?.Invoke(data.SigningResult, data.ErrorMessage, data.Type);
        }

        public void SingUp(string login, string password)
        {
            if (stream is null)
            {
                NoConnecting?.Invoke();
                return;
            }
            SignUp signUpRequest = new SignUp(login, password);
            stream.Write(signUpRequest);
        }

        public void SingIn(string login, string password)
        {
            if (stream is null)
            {
                NoConnecting?.Invoke();
                return;
            }
            SignIn signInRequest = new SignIn(login, password);
            stream.Write(signInRequest);
        }

        public void GiveMeActiveUsers()
        {
            stream.Write(new GiveMeActiveUsers());
        }

        private void UpdateUsers(SendActiveUsers names)
        {
            ActiveUsers = names.UsersNames.ToList();
            if(UsersUpdated != null)
                UsersUpdated();
        }

        private void ReadMessage(TextMessage message)
        {
            MessageRecived?.Invoke(message.Name,message.Text, message.TimeOfSend);
        }

        public void WriteMessage(TextMessage message)
        {
            //TextMessage mes = new TextMessage(Login, message, Target);
            stream.Write(message);
        }
    }
}
