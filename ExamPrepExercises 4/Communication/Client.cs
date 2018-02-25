using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExamPrepExercises_4.Communication
{
    public class Client
    {
        private Action<string> guiUpdate;
        private TcpClient client { get; set; }
        private Socket ClientSocket { get; set; }
        private byte[] Buffer { get; set; }

        public Client(Action<string> guiUpdate)
        {
            this.guiUpdate = guiUpdate;
            client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Loopback,8055));
            ClientSocket = client.Client;
            Buffer = new byte[512];
            Task.Factory.StartNew(Receive);
        }

        private void Receive()
        {
            string message = "";
            while (true)
            {
                int length = ClientSocket.Receive(Buffer);

                message += Encoding.UTF8.GetString(Buffer, 0, length);
                if (message.Contains("\r\n"))
                {
                    guiUpdate(message);
                    message = "";
                }
            }
        }

        public void Send(string message)
        {
            ClientSocket.Send(Encoding.UTF8.GetBytes(message));

        }
    }
}
