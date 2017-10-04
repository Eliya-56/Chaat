using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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

namespace Chaat
{
    /// <summary>
    /// Логика взаимодействия для ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : Window
    {
        public ConnectWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                var IP = IPAddress.Parse(ServerIPtext.Text);
                var port = int.Parse(PortText.Text);
                App.VM = new ViewModel(IP, port, "UnrecognizedUser");
                App.VM.ConnectToServer();
            }
            catch
            {
                ErrorMessage.Text = "Can't connect to IP: " + ServerIPtext.Text + " , port: " + PortText.Text;
                return;
            }

            LoginWindow loginWin = new LoginWindow();
            loginWin.Show();
            this.Close();
        }
    }
}
