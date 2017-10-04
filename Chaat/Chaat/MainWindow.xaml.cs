using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chaat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool NotExit = false;
        ObservableCollection<MessagesBox> Users = new ObservableCollection<MessagesBox>();

        public MainWindow()
        {
            InitializeComponent();
            ActiveUsers.ItemsSource = Users;
            App.VM.MessegeRecieved += MessageRecieved;
            this.Closed += (sender,args) => { if (!NotExit) { NotExit = false; Environment.Exit(0); } };
            App.VM.UsersUpdated += UserUpdated;
            App.VM.Disconnected += Diconnected;
        }

        private void Diconnected()
        {
            this.Dispatcher.BeginInvoke(
                (Action)(
                    () =>
                    {
                        ConnectWindow conwin = new ConnectWindow();
                        conwin.Show();
                        NotExit = true;
                        Close();
                    }
                ));
        }

        private void UserUpdated()
        {
            this.Dispatcher.BeginInvoke(
                (Action)(
                () =>
                {
                    //Users.Clear();
                    foreach (var activeUser in App.VM.ActiveUsers)
                    {
                        if(!Users.Select(x => x.UserName).Contains(activeUser))
                            Users.Add(new MessagesBox(activeUser, Messages));
                    }
                }));
            this.Dispatcher.BeginInvoke(
                (Action)(
                () =>
                {
                    List<MessagesBox> toDelete = new List<MessagesBox>();
                    foreach (var user in Users)
                    {
                        if (!App.VM.ActiveUsers.Contains(user.UserName))
                        {
                            user.DeActive();
                            toDelete.Add(user);
                        }
                    }
                    foreach (var item in toDelete)
                    {
                        Users.Remove(item);
                    }
                }));
        }

        private void MessageRecieved(string name, string text, DateTime time)
        {
            this.Dispatcher.BeginInvoke((Action)( () => AddUserMessage(name, text, time)));
            Console.Beep(100, 100);
        }

        private void ActiveUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var user in Users)
            {
                user.DeActive();
            }
            var index = ((ComboBox)sender).SelectedIndex;
            if (index < 0)
                return;
            Users[index].Active();
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if ((UserText.Text == "") || ActiveUsers.SelectedItem == null)
                return;

            var target = ((MessagesBox)ActiveUsers.SelectedItem).UserName;
            var text = UserText.Text;
            App.VM.SendMessage(text,target);
            AddMyMessage();
        }

        private void AddMyMessage()
        {
            TextBlock author = new TextBlock();
            author.TextAlignment = TextAlignment.Right;
            author.TextWrapping = TextWrapping.Wrap;
            author.Foreground = new SolidColorBrush(Colors.Red);
            author.Background = new SolidColorBrush(Color.FromRgb(0x9C, 0xFC, 0x9E));
            author.FontSize = 12;
            author.Text += Environment.NewLine +  App.VM.Login + "    ";
            Users[ActiveUsers.SelectedIndex].Messages.Children.Add(author);
            TextBlock textMessage = new TextBlock();
            textMessage.TextAlignment = TextAlignment.Right;
            textMessage.TextWrapping = TextWrapping.Wrap;
            textMessage.FontSize = 16;
            textMessage.Foreground = new SolidColorBrush(Colors.Black);
            textMessage.Background = new SolidColorBrush(Color.FromRgb(0x9C, 0xFC, 0x9E));
            textMessage.Text += UserText.Text + "    ";
            Users[ActiveUsers.SelectedIndex].Messages.Children.Add(textMessage);

            TextBlock sendTime = new TextBlock();
            sendTime.TextAlignment = TextAlignment.Right;
            sendTime.TextWrapping = TextWrapping.Wrap;
            sendTime.FontSize = 10;
            sendTime.Foreground = new SolidColorBrush(Colors.Black);
            sendTime.Background = new SolidColorBrush(Color.FromRgb(0x9C, 0xFC, 0x9E));
            sendTime.Text += DateTime.Now;
            Users[ActiveUsers.SelectedIndex].Messages.Children.Add(sendTime);
            UserText.Text = "";
            TextScroll.ScrollToEnd();
            UserText.Focus();
        }

        private void AddUserMessage(string name, string text, DateTime time)
        {
            string target = "";
            if (name.Contains("common"))
            {
                target = name.Substring(name.Length - 6);
                name = name.Substring(0, name.Length - 6);
                if (name == App.VM.Login)
                    return;
            }
            else
                target = name;
            TextBlock author = new TextBlock();
            author.TextAlignment = TextAlignment.Left;
            author.TextWrapping = TextWrapping.Wrap;
            author.Foreground = new SolidColorBrush(Colors.Blue);
            author.Background = new SolidColorBrush(Color.FromRgb(0x6C, 0xAC, 0x6E));
            author.FontSize = 12;
            author.Text += Environment.NewLine + name + "    ";

            var user = Users.FirstOrDefault(x => x.UserName == target);
            if (user != null)
                user.Messages.Children.Add(author);
            else
                return;

            TextBlock textMessage = new TextBlock();
            textMessage.TextAlignment = TextAlignment.Left;
            textMessage.TextWrapping = TextWrapping.Wrap;
            textMessage.FontSize = 16;
            textMessage.Foreground = new SolidColorBrush(Colors.Black);
            textMessage.Background = new SolidColorBrush(Color.FromRgb(0x6C, 0xAC, 0x6E));
            textMessage.Text += text + "    ";
            user.Messages.Children.Add(textMessage);

            TextBlock sendTime = new TextBlock();
            sendTime.TextAlignment = TextAlignment.Left;
            sendTime.TextWrapping = TextWrapping.Wrap;
            sendTime.FontSize = 10;
            sendTime.Foreground = new SolidColorBrush(Colors.Black);
            sendTime.Background = new SolidColorBrush(Color.FromRgb(0x6C, 0xAC, 0x6E));
            sendTime.Text += time;
            user.Messages.Children.Add(sendTime);
            TextScroll.ScrollToEnd();
        }
    }
}
