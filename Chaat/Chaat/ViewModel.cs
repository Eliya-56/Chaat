using SimpleChat;
using SimpleChat.Requests;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Chaat
{
    public class ViewModel
    {
        Client client;
        public string Login { get; set; }
        public List<string> ActiveUsers { get; private set; }
        public bool Signing
        {
            get
            {
                return client.Signing;
            }
        }

        public event Action<string, string, DateTime> MessegeRecieved;
        public event Action<bool, string, SignType> SigningResult;
        public event Action UsersUpdated;
        public event Action Disconnected;

        public ViewModel(IPAddress IP, int port, string login)
        {
            ActiveUsers = new List<string>();
            Login = login;
            client = new Client(IP, port);
            client.MessageRecived += OnMessage;
            client.SigningResult += OnSigningResult;
            client.UsersUpdated += OnUsersUpdated;
            client.Disconnected += OnDiconnected;
        }

        public bool ConnectToServer()
        {
            return client.Connect();
        }

        public void SingnIn(string login, string password)
        {
            client.SingIn(login, password);
        }

        public void SingnUp(string login, string password)
        {
            client.SingUp(login, password);
        }

        public void SendMessage(string text, string target)
        {
            client.WriteMessage(new TextMessage(Login, text, target));
        }

        public void UpdateUsers()
        {
            client.GiveMeActiveUsers();
        }

        //Проброс событий, происходящих при собщении от сервера
        private void OnMessage(string name, string text, DateTime time)
        {
            MessegeRecieved?.Invoke(name, text, time);
        }

        private void OnSigningResult(bool result, string message, SignType type)
        {
            SigningResult?.Invoke(result, message, type);
        }

        private void OnUsersUpdated()
        {
            ActiveUsers = client.ActiveUsers;
            ActiveUsers.Add("common");
            UsersUpdated?.Invoke();
        }

        private void OnDiconnected()
        {
            Disconnected?.Invoke();
            client = null;
        }

    }
}
