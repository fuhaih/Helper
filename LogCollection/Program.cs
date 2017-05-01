using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;

namespace LogCollection
{
    class Program
    {
        static void Main(string[] args)
        {
            Server ser = new Server(6000, 100);
            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse("192.168.0.100"), 6001);
            ser.Init();
            ser.Start(ipe);
            //ResiveSocket();
        }

        private static  void ResiveSocket()
        {
            int port = 6001;
            string host = "192.168.0.100";

            IPAddress ip =IPAddress.Parse(host);
            IPEndPoint ipe = new IPEndPoint(ip, port);

            Socket sSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sSocket.Bind(ipe);
            sSocket.Listen(0);
            Console.WriteLine("监听已经打开，请等待");

            //receive message
            Socket serverSocket = sSocket.Accept();
            Console.WriteLine("连接已经建立");
            string recStr = "";
            byte[] recByte = new byte[4096];
            int bytes = serverSocket.Receive(recByte, recByte.Length, 0);
            recStr += Encoding.ASCII.GetString(recByte, 0, bytes);

            //send message
            Console.WriteLine("服务器端获得信息:{0}", recStr);
            string sendStr = "send to client :hello";
            byte[] sendByte = Encoding.ASCII.GetBytes(sendStr);
            serverSocket.Send(sendByte, sendByte.Length, 0);
            serverSocket.Close();
            sSocket.Close();
            Console.ReadKey();
        }
    }
}
