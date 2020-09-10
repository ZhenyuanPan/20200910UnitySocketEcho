using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//socket编程API所在命名空间
using System.Net.Sockets;
using System.Text;
using UnityEngine.UI;
using System.Linq;
public class Echo : MonoBehaviour
{
    //定义Socket
    Socket socket;

    private InputField inputField;
    private Text seedText;
    private Text receiveText;
    private Button ConnectBtn;
    private Button SendBtn;
    private void Start()
    {
        inputField = GetComponentInChildren<InputField>();
        seedText = inputField.transform.GetComponentInChildren<Text>();
        //ConnectBtn = GetComponentsInChildren<Button>();
        var btns = GetComponentsInChildren<Button>();
        ConnectBtn = btns.ToList<Button>().Find(e => e.transform.name == "ConnectBtn");
        SendBtn = btns.ToList<Button>().Find(e => e.transform.name == "SendBtn");
        receiveText = GetComponentsInChildren<Text>().ToList().Find(e => e.transform.name == "ReceiveText");
        ConnectBtn.onClick.AddListener(Connection);
        SendBtn.onClick.AddListener(Seed);
        receiveText.text = "";
    }

    //连接按钮
    private void Connection() 
    {
        //socket 创建socket对象 三个参数是地址族,嵌套字类型,协议
        socket = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        //connect 是一个阻塞方法: 程序会卡住直到服务器回应(接受, 拒绝或超时)
        socket.Connect("127.0.0.1",19999);
    }
    //发送按钮
    private void Seed() 
    {
        //Seed
        string sendStr = inputField.text;
        //将数据转换成byte数组
        byte[] sendBytes = Encoding.Default.GetBytes(sendStr);
        //客户端通过socket.Send发送数据.这也是一个阻塞方法。该方法接受一个bytes[]类型数组指明发送的内容 它的返回值指明发送数据的长度
        socket.Send(sendBytes);
        //Recv
        byte[] readBuff = new byte[1024];
        //客户端通过sockect.Receive接受客户端数据。他也是一个阻塞方法，没有收到服务端数据时程序会卡在Receive不会向下执行
        //他有个bytes[]类型的参数 储存接受的数据 返回值则表示接受数据的长度
        int count = socket.Receive(readBuff);
        string recvStr = Encoding.Default.GetString(readBuff,0,count);
        receiveText.text = recvStr;
        //close 关闭连接
        socket.Close();
    }
}
