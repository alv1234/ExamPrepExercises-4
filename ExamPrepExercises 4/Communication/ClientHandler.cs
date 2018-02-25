using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ExamPrepExercises_4.Communication
{
    public class ClientHandler
    {
        private Socket socket;
        private Action<string> guiUpdate;
        public byte[] Buffer { get; set; }

        public ClientHandler(Socket socket, Action<string> guiUpdate)
        {
            this.socket = socket;
            this.guiUpdate = guiUpdate;
            Buffer = new byte[512];
            Task.Factory.StartNew(Receive);
        }

        private void Receive()
        {
            string message = "";
            while (true)
            {
                int length = socket.Receive(Buffer);
                
                message += Encoding.UTF8.GetString(Buffer, 0, length);
                if (message.Contains("\r\n"))
                {
                    guiUpdate(message);
                    message = "";
                }

            }
        }

        internal void Send(string message)
        {
            socket.Send(Encoding.UTF8.GetBytes(message));
        }
    }
}
