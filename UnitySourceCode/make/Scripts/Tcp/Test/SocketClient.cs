using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;

public class SocketClient : MonoBehaviour
{

    Socket socket;

    string ipAdress = "13.125.250.235";
    int port = 8888;

    byte[] sendByte;

    // Use this for initialization
    void Start()
    {

        //create socket
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        //connect
        try
        {
            IPAddress ipAddr = IPAddress.Parse(ipAdress);
            IPEndPoint ipendPoint = new IPEndPoint(ipAddr, port);
            socket.Connect(ipendPoint);

        }
        catch (SocketException se)
        {
            Debug.Log("Socket connect error ! : " + se.ToString());

            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            try
            {
                Debug.Log("전송");
                StringBuilder sb = new StringBuilder("Hello world!");
              //  sb.Append("Test 1 - send data!!");
               
                int l = Encoding.Default.GetByteCount(sb.ToString());
                byte[] d = Encoding.Default.GetBytes(sb.ToString());
                socket.Send(d, l, 0);
            }
            catch (Exception e)
            {
                Debug.Log("Socket send or receive error ! : " + e.ToString());
            }

            socket.Disconnect(true);
            socket.Close();
        }
    }
}
