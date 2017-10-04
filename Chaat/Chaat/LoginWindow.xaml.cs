using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using SimpleChat.Requests;
using System.Windows.Threading;

namespace Chaat
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public bool Start { get; set; } = false;
        MainWindow mainWin = new MainWindow();

        public LoginWindow()
        {
            InitializeComponent();
            this.Closed += (sender, args) => 
            {
                if (!App.VM.Signing)
                    Environment.Exit(0);
            };
            App.VM.SigningResult += ResponseHandler;           
        }


        private void ResponseHandler(bool result, string message, SignType type)
        {
            if (result)
            {
                mainWin.Dispatcher.BeginInvoke(() => mainWin.Show());
                this.Dispatcher.BeginInvoke(() => Close());
                return;
               
            }
            else
            {
                if (type == SignType.Up)
                    ErrorMessage.Dispatcher.BeginInvoke(() => ErrorMessage.Text = message);
                else
                    ErrorMessage.Dispatcher.BeginInvoke(() => ErrorMessage.Text = message);

                Up.Dispatcher.BeginInvoke(() => Up.IsEnabled = true);
                In.Dispatcher.BeginInvoke(() => In.IsEnabled = true);
            }
        }

        private void Sign_Click(object sender, RoutedEventArgs e)
        {
            var pressBut = (Button)sender;
            var login = LoginText.Text;
            var password = PasswordText.Password;
            App.VM.Login = login;
            try
            {
                if (pressBut.Name == "Up")
                    App.VM.SingnUp(login, password);
                else
                    App.VM.SingnIn(login, password);
                Up.IsEnabled = false;
                In.IsEnabled = false;
            }
            catch
            {
                ErrorMessage.Text = "Can't send request to server";
                Up.IsEnabled = true;
                In.IsEnabled = true;
            }
        }


    }
}
