using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExamPrepExercises_4.Communication
{
    public class Server
    {
        private Action<string> guiUpdate;
        private List<ClientHandler> Clients { get; set; }
        private Socket ServerSocket { get; set; }

        public Server(Action<string> guiUpdate)
        {
            this.guiUpdate = guiUpdate;
            Clients = new List<ClientHandler>();
            ServerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ServerSocket.Bind(new IPEndPoint(IPAddress.Loopback, 8055));
            ServerSocket.Listen(2);
            Task.Factory.StartNew(Accept);
        }

        private void Accept()
        {
            while (true)
            {
                Clients.Add(new ClientHandler(ServerSocket.Accept(), guiUpdate));
            }        }

        internal void InformAllCients(string message)
        {
            foreach (var item in Clients)
            {
                item.Send(message);
            }
        }
    }
}
