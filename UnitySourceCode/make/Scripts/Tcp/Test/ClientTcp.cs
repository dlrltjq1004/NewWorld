using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using System.Net;
using System.Text;
using System;
using System.IO;

public class ClientTcp : MonoBehaviour {

    private Socket socket;
    private ClientTcp client;

    private void Start()
    { 
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPAddress ipAddress = IPAddress.Parse("13.125.250.235");
        IPEndPoint ipendPoint = new IPEndPoint(ipAddress, 8888);
        client = this;

        socket.Connect(ipendPoint);

        Send();
        Debug.Log("메시지 전송");
    }


    void Update () {
		
        if(Input.GetKeyDown("space"))
        {
          
            
        }

	}
    /*
    private static byte[] MessageToByteArray(string message, Encoding encoding)
    {

        var byteCount = encoding.GetByteCount(message);
        if (byteCount > byte.MaxValue)
            throw new ArgumentException("Message size is greater than 255 bytes in the provided encoding");
        var byteArray = new byte[byteCount + 1];
        byteArray[0] = (byte)byteCount;
        encoding.GetBytes(message, 0, message.Length, byteArray, 1);
        return byteArray;
    } 
    */

    private void Send()
    {
        string inputData = "HelloWorld ";

        /*    var byteArray = MessageToByteArray(inputData,Encoding.ASCII);

            using (var tcpClient = new TcpClient())
            {
                tcpClient.Connect("13.125.250.235",8888);
                using (var networkStream = tcpClient.GetStream())
                using (var bufferedStream = new BufferedStream(networkStream))
                {
                    bufferedStream.Write(byteArray, 0, byteArray.Length);
                //    bufferedStream.Write(byteArray, 0, byteArray.Length);
                //    bufferedStream.Write(byteArray, 0, byteArray.Length);
                }
            } */
        client = new ClientTcp();

      //  ns = client.GetStream(); 
        byte[] bytes = new byte[1024];

            byte[] sendData = Encoding.Default.GetBytes(inputData);

        socket.Send(bytes);
        socket.Send(sendData);
            

    }


}
