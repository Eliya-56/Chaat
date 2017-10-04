using SimpleChat;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("You opened test server for chaat. \nGo to chaatconf.txt to configure server \nOr create this file with string port XXX and connectionString XXX.");
            Console.WriteLine("Type start and press Enter to start server");
            string str = "";
            while (str != "start")
            {
                str = Console.ReadLine();
            }
            string connectionString = "";
            int port = -1;
            string IP = "";
            using (StreamReader reader = new StreamReader("chaatconf.txt", System.Text.Encoding.Default))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if(line.StartsWith("port"))
                        port = int.Parse(line.Substring(5));
                    else if(line.StartsWith("connectionString"))
                        connectionString = line.Substring(17);
                    else if (line.StartsWith("ip"))
                        IP = line.Substring(3);
                }
            }
            if(port == -1 || connectionString == "" || IP == "")
            {
                Console.WriteLine("Can't start. Check your configuration chaatconf.txt");
                Console.ReadLine();
                Environment.Exit(0);
            }
            Server server = new Server(IPAddress.Parse(IP), port, connectionString);
            //Server server = new Server(IPAddress.Parse("192.168.0.104"), 8888, @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UserDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }
    }
}
