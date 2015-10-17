using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ChessClient
{

    class ServerConnection
    {
        private IPAddress ipServer;
        private int localPort = 12345;
        private int serverPort = 23456;
        private int NumberClient;
        private Table table;

         

        public ServerConnection(string ips, Table tb)
        {
            ipServer = IPAddress.Parse(ips);
            Random rnd = new Random();
            NumberClient = rnd.Next(1000000);
            table = tb;
        }

        public void SendMessage(string messageStep)
        {
            try
            {
                Thread thread = new Thread( new ThreadStart(ThreadFuncReceive));
                //создание фонового потока
                thread.IsBackground = true;
                //запуск потока
                thread.Start();
            }

            catch (FormatException formExc)
            {
                MessageBox.Show("Преобразование невозможно :" + formExc);
            }
            catch (Exception exc)
            {
                MessageBox.Show("Ошибка : " + exc.Message);
            }

        }


       private void ThreadFuncReceive()
       {
           string[] arMessage = new string[3];
            try
            {
                do
                {
                    //подключение к локальному хосту
                    UdpClient uClient = new UdpClient(localPort);
                    IPEndPoint ipEnd = null;
                    //получание дейтаграммы
                    byte[] responce = uClient.Receive(ref ipEnd);
                    //преобразование в строку
                    string strResult = Encoding.Unicode.GetString(responce);
                    //вывод на экран
                    MessageBox.Show(strResult);
                    uClient.Close();
                    arMessage = new string[3];
                    arMessage = strResult.Split('-');
                    if (arMessage[0] == "0")
                    {
                        table.GameOver();
                        break;
                    }
                } while (arMessage[0] == NumberClient.ToString());
                table.MoveFigure(arMessage[1], arMessage[2]);

            }
            catch (SocketException sockEx)
            {
                MessageBox.Show("Ошибка сокета: " + sockEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка : " + ex.Message);
            }
        }

       private void SendData(string datagramm)
        {
            UdpClient uClient = new UdpClient();
            //подключение к удаленному хосту
            IPEndPoint ipEnd = new IPEndPoint(ipServer, serverPort);
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(datagramm);
                uClient.Send(bytes, bytes.Length, ipEnd);
            }
            catch (SocketException sockEx)
            {
                MessageBox.Show("Ошибка сокета: " + sockEx.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка : " + ex.Message);
            }
            finally
            {
                //закрытие экземпляра класса UdpClient
                uClient.Close();
            }
        }


        
    }

      
       
}

