using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ConsoleApplication1
{
    public class socketTest
    {
        public static void Send()
        {
            int port = 6001;
            string host = "192.168.0.100";//服务器端ip地址

            IPAddress ip = IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(ipe);

            //send message
            string sendStr = "send to server : hello";
            byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);
            clientSocket.Send(sendBytes);
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.AcceptSocket = clientSocket;
            e.Completed += new EventHandler<SocketAsyncEventArgs>(Complete);
            e.SetBuffer(sendBytes, 0, sendBytes.Length);
            //clientSocket.SendAsync(e);
            clientSocket.Send(sendBytes, SocketFlags.None);
            //receive message
            //string recStr = "";
            //byte[] recBytes = new byte[4096];
            //int bytes = clientSocket.Receive(recBytes, recBytes.Length, 0);
            //recStr += Encoding.ASCII.GetString(recBytes, 0, bytes);
            //Console.WriteLine(recStr);
            Console.ReadKey();
            
        }

        private static void Complete(object sender, SocketAsyncEventArgs e)
        {
            //e.AcceptSocket.Close();
        }

    }
}
