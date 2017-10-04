using SimpleChat.Requests;
using SimpleChat.UserData;
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
    public class Server
    {
        string connectionString;
        TcpListener listener;
        List<ChaatStream> unsignedStreams = new List<ChaatStream>();
        IPEndPoint serverAddress;
        object syncObject = new object();
        //Dictionary<AuthData, ChaatStream> users = new Dictionary<AuthData, ChaatStream>();
        Dictionary<AuthData, ChaatStream> activeUsers = new Dictionary<AuthData, ChaatStream>();

        public event Action<string, string, string, DateTime> MessageRecived;
        public event Action<AuthData> UserSignUp;
        public event Action<AuthData> UserSignIn;
        public event Action<AuthData> BreakConnection;

        public Server(IPAddress ip, int port, string connectionString)
        {
            this.connectionString = connectionString;
            serverAddress = new IPEndPoint(ip, port);
            listener = new TcpListener(serverAddress);
            Thread listening = new Thread(Listen);
            listening.Start();
            Console.WriteLine(DateTime.Now + "  Server start: begin listening");
        }

        private void Listen()
        {
            while (true)
            {
                listener.Start();
                var client = listener.AcceptTcpClient();
                ChaatStream stream = new ChaatStream(client.GetStream(), (IPEndPoint)client.Client.RemoteEndPoint);
                unsignedStreams.Add(stream);
                Console.WriteLine(DateTime.Now + "  Connected " + stream.EndPoint.Address.ToString());
                Thread readingStream = new Thread(() => ChechSign(stream));
                readingStream.Start();
            }
        }

        private void ChechSign(ChaatStream stream)
        {
            try
            {
                while (true)
                {
                    if (activeUsers.Select(x => x.Value).Contains(stream))
                        break;
                    var message = stream.Read();
                    var RequestType = message.Request;
                    switch (RequestType)
                    {
                        case ChaatRequest.SignUp:
                            SingUp((SignUp)message.Data, stream);
                            break;
                        case ChaatRequest.SignIn:
                            SingIn((SignIn)message.Data, stream);
                            break;
                    }
                }
            }
            catch(IOException ex)
            {
                unsignedStreams.Remove(stream);
                Console.WriteLine(DateTime.Now + "  Disconnected unrecognized user, " + stream.EndPoint.Address.ToString());
            }
        }

        private void Read(AuthData user, ChaatStream stream)
        {
            try
            {
                while (true)
                {
                    var message = stream.Read();
                    var RequestType = message.Request;
                    switch (RequestType)
                    {
                        case ChaatRequest.Message:
                            ReadMessage((TextMessage)message.Data);
                            break;
                        case ChaatRequest.GiveMeActiveUsers:
                            SendActiveUsers(stream);
                            break;
                    }
                }
            }
            catch (IOException ex)
            {
                activeUsers.Remove(user);
                BreakConnection?.Invoke(user);
                Console.WriteLine(DateTime.Now + "  Disconnected " + user.Login  + ", " + stream.EndPoint.Address.ToString());
                foreach (var activeUser in activeUsers)
                {
                    SendActiveUsers(activeUser.Value);

                }
            }
        }

        private void SingIn(SignIn user, ChaatStream stream)
        {
            Console.WriteLine(DateTime.Now + "  Try to SignIn: login - " + user.User.Login + ", IP - " + stream.EndPoint.Address.ToString());
            string responseMessage = "";

            if (IsGoodAuth(user.User, out responseMessage))
            {
                Console.WriteLine(DateTime.Now + "  Good SignIn: login - " + user.User.Login + ", IP - " + stream.EndPoint.Address.ToString());
                activeUsers.Add(user.User, stream);
                UserSignIn?.Invoke(user.User);
                SigningResponse goodSignIn = new SigningResponse(true, responseMessage, SignType.In);
                stream.Write(goodSignIn);
                Thread readingUser = new Thread(() => Read(user.User, stream));
                readingUser.Start();
                unsignedStreams.Remove(stream);
                foreach (var activeUser in activeUsers)
                {
                    SendActiveUsers(activeUser.Value);

                }
            }
            else
            {
                SigningResponse badSignIn = new SigningResponse(false, "Bad signIn " + responseMessage, SignType.In);
                stream.Write(badSignIn);
                Console.WriteLine(DateTime.Now + "  Bad SignIn: login - " + user.User.Login + ". IP - " + stream.EndPoint.Address.ToString());
            }
        }

        private void SingUp(SignUp user, ChaatStream stream)
        {
            Console.WriteLine(DateTime.Now + "  Try to SignUp: login - " + user.User.Login + ", IP - " + stream.EndPoint.Address.ToString());
            string responseMessage = "";
            if (IsFreeLogin(user.User, out responseMessage))
            {
                Console.WriteLine(DateTime.Now + "  Good SignUp: login - " + user.User.Login + ", IP - " + stream.EndPoint.Address.ToString());
                PutUserInDb(user.User);
                activeUsers.Add(user.User, stream);
                SigningResponse goodSignUp = new SigningResponse(true, responseMessage, SignType.Up);
                stream.Write(goodSignUp);
                UserSignUp?.Invoke(user.User);
                Thread readingUser = new Thread(() => Read(user.User, stream));
                readingUser.Start();
                unsignedStreams.Remove(stream);
                foreach (var activeUser in activeUsers)
                {
                    SendActiveUsers(activeUser.Value);

                }
            }
            else
            {
                SigningResponse badSignUp = new SigningResponse(false, "Bad signUp " + responseMessage, SignType.Up);
                stream.Write(badSignUp);
                Console.WriteLine(DateTime.Now + "  Bad SignUp: login - " + user.User.Login + ", IP - " + stream.EndPoint.Address.ToString());
            }
        }

        private void ReadMessage(TextMessage message)
        {
            PutMessageInDb(message);
            WriteMessage(message);
            MessageRecived?.Invoke(message.Name ,message.Text, message.Target, message.TimeOfSend);
        }

        public void WriteMessage(TextMessage message)
        {
            if (message.Target == "common")
            {
                message.Name += "common";
            }
            Console.WriteLine(DateTime.Now + "  User " + message.Name + " send message to " + message.Target);
            //TextMessage mes = new TextMessage("server",message, "common");
            lock (syncObject)
            {
                foreach (var user in activeUsers)
                {
                    if (message.Target == "common")
                    {
                        user.Value.Write(message);
                    }
                    else if (message.Target == user.Key.Login)
                        user.Value.Write(message);
                }
            }
        }

        private void SendActiveUsers(ChaatStream stream)
        {
            lock (syncObject)
            {
                var names = activeUsers.Select(x => x.Key).Select(x => x.Login).ToArray();
                stream.Write(new SendActiveUsers(names));
                Console.WriteLine(DateTime.Now + "  Sending active users to " + ((IPEndPoint)stream.EndPoint).Address.ToString());
            }
        }


        //Для работы с БД
        public bool PutUserInDb(AuthData User)
        {
            bool IsOk = false;
            using (var context = new UserDataContext(connectionString))
            {
                try
                {
                    context.Users.Add(new User() { Auth = User, TimeOfRegistration = DateTime.Now });
                    context.SaveChanges();
                    IsOk = true;
                }
                catch
                {
                    IsOk = false;
                }
            }
            return IsOk;
        }

        public bool IsFreeLogin(AuthData User, out string Message)
        {
            bool freeLogin = true;
            using (var context = new UserDataContext(connectionString))
            {
                try
                {
                    if(context.Users.ToList().Select(x => x.Auth.Login).Contains(User.Login))
                    {
                        Message = "Login is used";
                        return false;
                    }
                }
                catch
                {
                    Message = "Server error";
                    freeLogin = false;
                }
            }
            Message = "Succesfull";
            return freeLogin;
        }

        public bool IsGoodAuth(AuthData User, out string Message)
        {
            bool good = false;
            if(activeUsers.Select(x => x.Key.Login).Contains(User.Login))
            {
                Message = "This user has already signIn";
                return false;
            }
            using (var context = new UserDataContext(connectionString))
            {
                try
                {
                    foreach (var user in context.Users.ToList())
                    {
                        if (user.Auth.Login == User.Login)
                        {
                            if (user.Auth.Password == User.Password)
                            {
                                Message = "Sucsses SignIn";
                                return true;
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    Message = "Server error";
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            Message = " invalid login or password";
            return good;

        }

        public bool PutMessageInDb(TextMessage message)
        {
            bool IsOk = false;
            using (var context = new UserDataContext(connectionString))
            {
                try
                {
                    var sender = context.Users.FirstOrDefault(x => x.Auth.Login == message.Name);
                    var reciever = context.Users.FirstOrDefault(x => x.Auth.Login == message.Target);
                    if (sender == null || reciever == null)
                        return false;
                    context.Messages.Add(new MessagesHistory { Reciver = reciever, Sender = sender, Text = message.Text, SendTime = DateTime.Now });
                    context.SaveChanges();
                    IsOk = true;
                }
                catch
                {
                    IsOk = false;
                }
            }
            return IsOk;
        }
    }
}
