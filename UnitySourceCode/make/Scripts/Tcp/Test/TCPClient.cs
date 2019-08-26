
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using System.IO;


public class TCPClient : MonoBehaviour
{

    // IP주소
    public string m_IPAdress = "13.125.250.235";
    // 포트번호
    public const int kPort = 8888;
    // 싱글톤이란 생성하고자. 하는 인스턴스의 수를 오직 하나로 제한하는 디자인 패턴이다.
    // ?? ... 이유는 차차 알아가자.
    private static TCPClient singleton;
    // 소켓
    private Socket m_Socket;

    // 게임오브젝터가 최초로 생성되었을 때.
    void Awake()
    {
        //소켓 생성
        m_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //m_Socket.NoDelay = true;
        //theStream = m_Socket.GetStream();

        // System.Net.PHostEntry ipHostInfo = Dns.Resolve("host.contoso.com");
        // System.Net.IPAddress remoteIPAddress = ipHostInfo.AddressList[0];

        // 주소 할당
        System.Net.IPAddress remoteIPAddress = System.Net.IPAddress.Parse(m_IPAdress);
        System.Net.IPEndPoint remoteEndPoint = new System.Net.IPEndPoint(remoteIPAddress, kPort);

        // 싱글톤에 자기 자신 넣고
        singleton = this;
        // 연결 요청
        m_Socket.Connect(remoteEndPoint);
    }

    // 매 프레임마다 실행
    void Update()
    {
      
        // 테스트를 위해 임시로 Send() 호출
        if (Input.GetKeyDown("space"))
        {
            Send();
            Debug.Log("전송");
        }

    }

    // 게임 꺼지기 직전에. OnDestroy();
    void OnApplicationQuit()
    {
        m_Socket.Close();
        m_Socket = null;
    }

    // 서버에 보내는 함수
    static public void Send()
    {
        // 싱글톤의 값이 null이면 종료.
        if (singleton.m_Socket == null)
            return;
      //  Encoding enc = new UTF8Encoding(true, true);
        // 보낼 스트링
        String inputData = "HelloWorld";
        // 스트링을 ByteArray로 변환. GetBytes()는 String을 Byte 배열로 변환해주는 함수.
        byte[] sendData = Encoding.ASCII.GetBytes(inputData);
        // Byte 배열 선언 후 = 배열 생성
        // 배열의 크기는 1.
         byte[] prefix = new byte[1024];
        // prefix는 보내는 ByteArray의 크기를 앞서 보내는 것 같음.
        //prefix[0] = (byte)sendData.Length; 그래서 이렇게 보냈더니, 너무 많이 쌓이는 것임.
      //  prefix[0] = 1; // 그래서 1을 넣었더니 그래도 많음 ... ;;
        //SocketFlags socketFlags = new SocketFlags;
        //singleton.m_Socket.SendBufferSize = sendData.Length;

        // 어쨌든 가긴 가니까 이렇게 보냈음.
        singleton.m_Socket.Send(prefix);
        singleton.m_Socket.Send(sendData);

        // write랑 flush 이거 두개차이
        // 이상하게 유니티에서 write로 보내면 안감... Send로 보내야 감.
        // 근데 Send로 보내면 버퍼의 크기를 몰라서 ... 아까같은 현상이 일어 났음 ....


        //singleton.m_Socket.Close();

    }

     
}