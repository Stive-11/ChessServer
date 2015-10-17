using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChessServer
{
    class Player
    {
        public Socket    SocketPlayer {  get; private set; }
        public bool      Color { get; private set; }
        public string    Name { get; private set; }
        public bool      Step { set; get; }
        private EndPoint epPlayer;
        private IAsyncResult sendResult, reciveResult;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];

        public Player (Socket socket, bool color, string name, EndPoint ep)
        {
            SocketPlayer = socket;
            Color = color;
            Name = name;
            epPlayer = ep;
        }

        public void SendStep(string message)
        {
            byte[] buffer = System.Text.Encoding.Unicode.GetBytes(message);
            sendResult = SocketPlayer.BeginSendTo(buffer, 0, buffer.Length, 
                SocketFlags.None, epPlayer,
                new AsyncCallback(Send_Complited), SocketPlayer);
        }

        private void Send_Complited(IAsyncResult ar)
        {
            Socket socket = (Socket) ar.AsyncState;
            socket.EndSend(sendResult);
            socket.Shutdown(SocketShutdown.Send);
            //socket.Close();

        }

        public string ReciveStep()
        {
            reciveResult = SocketPlayer.BeginReceiveFrom(buffer,
               0,
               BufferSize,
               SocketFlags.None,
               ref epPlayer,
               new AsyncCallback(Receive_Completed), null);   

            string message = "";
            return message;
        }

       private void Receive_Completed(IAsyncResult ia)
        {
            try
            {
                StateObject so = (StateObject)ia.AsyncState;
                Socket client = so.workSocket;
                if (socket == null)
                    return;
                int readed = client.EndReceiveFrom(RcptRes, ref ClientEP);

                String strClientIP = ((IPEndPoint)ClientEP).Address.ToString();
                String str = String.Format("\nПолучено от {0}\r\n{1}\r\n",
                    strClientIP, System.Text.Encoding.Unicode.GetString(so.buffer, 0, readed));

                textBox1.BeginInvoke(new AddTextDelegate(AddText), str);

                RcptRes = socket.BeginReceiveFrom(state.buffer,
                    0,
                    StateObject.BufferSize,
                    SocketFlags.None,
                    ref ClientEP,
                    new AsyncCallback(Receive_Completed),
                    state);
            }
            catch (SocketException ex)
            {
            }

        }

        public void Exit()
        {
            SocketPlayer.Shutdown(SocketShutdown.Both);
            SocketPlayer.Close();
        }
    }
}
