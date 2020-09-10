using System;
using System.Net;
using System.Net.Sockets;
namespace EchoServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Socket
            Socket listenfd = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
            //Bind 绑定 Socket.Bind绑定IP和端口。
            //IPAddress指定IP地址
            IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
            //IPEndPoint指定IP和端口
            IPEndPoint ipEp = new IPEndPoint(ipAdr,19999);
            listenfd.Bind(ipEp);
            //Listen 监听：服务端通过socket.Listen(backlog)开始监听, 等待客户端连接。该参数指定队列中最多可荣达当代的连接数。0为不限制
            listenfd.Listen(0);
            Console.WriteLine("[服务器]启动成功");
            //应答Accept 开启监听后 服务器调用socket.Accept接收客户端连接. 本例子使用的所有的Socket方法都是阻塞方法, 也就是说当没有客户端连接时, 服务器程序卡在Accept
            //不会往下执行,直到接收了客户端的连接。Accept返回一个新客户端的Sockect对象， 对于服务器来说，它有一个监听Socket（listenfd）用来监听Listen和应答Accept客户端
            //的连接,对于每个客户端还有一个专门的Socket(connfd)用来处理该客户端的数据
            while (true)
            {
                //Accept
                Socket connfd = listenfd.Accept();
                Console.WriteLine("[服务器]Accpet");
                //Receive
                byte[] readBuff = new byte[1024];
                int count = connfd.Receive(readBuff);
                string readStr = System.Text.Encoding.Default.GetString(readBuff,0,count);
                Console.WriteLine("[服务器接收]"+readStr);
                //Send
                byte[] sendBytes = System.Text.Encoding.Default.GetBytes(readStr);
                connfd.Send(sendBytes);
            }
        }
    }
}
