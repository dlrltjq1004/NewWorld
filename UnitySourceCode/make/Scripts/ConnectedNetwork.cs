using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ConnectedNetwork : MonoBehaviour
{
    public string myId, myBase, yuerCharacterName, success;
    public Socket clientSocket;
    private Thread clientReceiveThread;
    // Use this for initialization
    void Start()
    {
        myId = "Android";
        myBase = "2";
        yuerCharacterName = "";
        success = "";
        Connected();
        Send(myId);
        ConnectToTcpServer();
    }


    public void Connected()
    {

        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("13.125.250.235"), 8888);

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(serverAddress);
    }

    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ReceiveReadyData));
            clientReceiveThread.IsBackground = true;
            Debug.Log("쓰레드 스타트");
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On Client connect exception" + e);
        }

    }

    public void ReceiveReadyData()
    {

        //** 2언어(서로 다른 언어 ex. Java, C# ) 통신간 데이터 수신 방법    **/
        // 1.바이이트 최대 사이즈 정의 
        //    1).ex.byte[] rcvLenBytes = new byte[1024];
        //    2).clientSocket.Receive(rcvLenBytes);
        // 2.전달받은 바이트 데이터 인코딩
        //    1) int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
        //    2) byte[] rcvBytes = new byte[rcvLen];
        //    3) String rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);
        //    4) 출력 Debug.Log(rev);

        // Receiving
        while (true)
        {
            try
            {
                byte[] rcvLenBytes = new byte[2048];
                clientSocket.Receive(rcvLenBytes);
                int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
                byte[] rcvBytes = new byte[rcvLen];
                clientSocket.Receive(rcvBytes);
                String rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

                ArrayList array = new ArrayList();

                String[] rcvs = rcv.Split(',');
                for (int i = 0; i < rcvs.Length; i++)
                {
                    if (!rcvs[i].Equals(myId))
                    {
                        if (rcvs[i].Equals("TaiLung"))
                        {
                            yuerCharacterName = "TaiLung";
                            Debug.Log("상대 캐릭터 :" + yuerCharacterName);
                        }
                        else if (rcvs[i].Equals("Mantis"))
                        {
                            yuerCharacterName = "Mantis";
                            Debug.Log("상대 캐릭터: " + yuerCharacterName);
                        }
                        else if (rcvs[i].Equals("success"))
                        {
                            success = "success";
                            Debug.Log("유저 준비 완료:" + success);
                        }

                    }


                }
                Debug.Log("Client received: " + rcv);
            }
            catch (Exception e)
            {
                clientSocket.Close();

            }


        }
    }
    public void Send(String data)
    {
        /**  2언어(서로 다른 언어 ex. Java, C# ) 통신간 데이터 송신 방법  **/
        // 서버로 데이터 전송시 bufferSize 지정하여 보내줘야함 ex.byte[] bytes = new byte[1024];
        // 전송할 데이터 String msg = "HelloWorld";
        // 스트링 문자열 byte[] sendData = Encoding.ASCII.GetBytes(msg);을 인코딩 후 바이트 배열에 담은 후 문자열의 길이 값을 구한다 ex.int msgCount = sendData.Length;
        // socket.Send() 시 1 byte[] , int , int ,SocketFlages 를 순서대로 넣어줘야 한다. 
        // 첫번째 : 보낼 데이터를 바이트 배열로 변환후 입력 
        // 두번째 : 문자열의 첫 시작점을 알려준다 0
        // 세번째 : 문자열 길이의 끝을 알려준다. msg.Length 
        // 네번째 : SocketFlages 의 타입을 정해준다. (보낼때 여러가지 옵션을 정해줄 수 있다. SocketFlages.None) 
        // Send 사용법 : socket.Send(bytes);
        // Send 사용법 : socket.Send(sendData, 0, msgCount, SocketFlages.None);

        
        //  string toSend = data;
        string toSend = data;
        // Sending
        Debug.Log(toSend);
        int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
        byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
        byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);

        clientSocket.Send(toSendLenBytes);
        clientSocket.Send(toSendBytes);

    }


    private void OnDestroy()
    {
       
      //clientReceiveThread.Interrupt();
        //clientReceiveThread.Abort();
    }





}
