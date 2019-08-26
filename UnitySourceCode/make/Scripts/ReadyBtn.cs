using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Assets.make.Scripts;

public class ReadyBtn : MonoBehaviour {
    public  Text myCharacterName;
   
    private bool gameStart;
    private string myCharacter;
  
    public string yuerCharacterName = "";
    public string myId, relativeId  ;
    public string success = "";
    String msg;
    public Button readyBtn;
    public GameObject characterView , taiLungView,mantisView;
    
    
    private int Timer = 0;
    private static Text CharacterName;

    public GameObject Num_A;   //1번
    public GameObject Num_B;   //2번
    public GameObject Num_C;   //3번
    bool loadScene = false;
    bool connected = false;
    bool threadStop;
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    private Socket clientSocket;
    private String camp ;
    private BufferedStream reader;
    private String campData;
    private string myTeam;
    // Client socket.

    // Size of receive buffer.
    public const int BufferSize = 1024;
    // Receive buffer.
    public byte[] buffer = new byte[BufferSize];
    // Received data string.
    public StringBuilder sb = new StringBuilder();
    StreamReader streamReader;
    NetworkStream stream;
    ConnectedNetwork conNetwork;
    
    Client client;
    public string remoteId = "";

    public static ReadyBtn singleton;

    //AndroidPlugin androidPlugin;
    Network network;
    ExampleClass exampleClass;
    // 초기화에 사용
    private volatile bool _shouldStop;
    
    void Awake()
    {
        
    }

    void Start()
    {
        exampleClass = GameObject.Find("IDManager").GetComponent<ExampleClass>();

        //androidPlugin = GameObject.Find("NetworkManager").GetComponent<AndroidPlugin>();

        //network = GameObject.Find("NetworkManager").GetComponent<Network>();

        singleton = this;
         myCharacterName = GameObject.Find("CharacterName").GetComponent<Text>();

        // 내 아이디 
        // 아이디 값은 안드로이드에서 로그인 한 아이디 값을 넣어 주는게 맞지만 테스팅을 위해 명시적으로 아이디 값을 지정했다.
        // 하지만 user1 과 user2 의 아이디 값이 달라야 하기 때문에 서로 다른 아이디로 포팅을 해줘야 하는 불편함이 있기 때문에
        // 테스팅 중에는 아이디 값을 난수로 사용 하기로 한다.

        //myId = androidPlugin.text;
        myId = "TestId2";
        myTeam = "1";
        // 내 진영 값

        success = "";
        Debug.Log("레디버튼 내 아이디 값 :"+myId);


        Connected();
        Send(myId + "," + "myTeamCall");
        ConnectToTcpServer();
        //시작시 카운트 다운 초기화
        Timer = 0;
        gameStart = false;
    }


    void Update()
    {     
        if(Input.GetKeyDown("space"))
        {
            Debug.Log("myTeam value: " + myTeam );
            exampleClass.myId = myId;
            //clientReceiveThread.Interrupt();
            //clientReceiveThread.Abort();
            SceneManager.LoadScene("LoadingScene");
            StartCoroutine(CloseDelay());
        }
        //StartCoroutine(CampChoice());
        if (!yuerCharacterName.Equals(""))
        {            
            PlayerPrefs.SetString("myId",myId);
            PlayerPrefs.SetString("remoteId", remoteId);
            PlayerPrefs.SetString("myTeam", myTeam); 
            PlayerPrefs.SetString("myCharacter", myCharacter);
            PlayerPrefs.SetString("yuerCharacter", yuerCharacterName);
            
           

            if (success.Equals("success"))
            {

                //network.ScriptControl = "NovaWorld";
                exampleClass.myId = myId;
                clientReceiveThread.Interrupt();
                clientReceiveThread.Abort();
                SceneManager.LoadScene("LoadingScene");
                StartCoroutine(CloseDelay());
            }           
        }     
    }
    
    IEnumerator CloseDelay()
    {
        yield return new WaitForSeconds(3f);
       
        clientSocket.Close();
    }
    
    public void RequestStop()
    {
        _shouldStop = true;
    } // Volatile is used as hint to the compiler that this data // member will be accessed by multiple threads. private volatile bool _shouldStop;
   

    public void Connected()
    {

        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("13.124.87.189"), 8888);

       clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
       clientSocket.Connect(serverAddress);
    }

   
    public void OnClickExit()
    {
    
                loadScene = true;
                gameStart = true;
                myCharacter =  myCharacterName.text;
        //    Debug.Log(myCharacter);
      
        readyBtn.interactable = false;

     
        if (myCharacter.Equals("타이렁"))
        {

            myCharacter = "TaiLung";
       //     Debug.Log("저장완료: "+myCharacter);
            characterView.SetActive(false);
            taiLungView.SetActive(true);
            PlayerPrefs.SetString("myCharacter",myCharacter);
           
        } else if (myCharacter.Equals("맨티스"))
        {
            myCharacter = "Mantis";
            characterView.SetActive(false);
            mantisView.SetActive(true);
            PlayerPrefs.SetString("myCharacter", myCharacter);
          
        }
        

        string userReady = "CharacterChoiceReady";

        //conNetwork.Send(myId + "," + myCharacter + ","+ userReady);
        Send(myId + "," + myCharacter + ","+ userReady);

   
    }

    IEnumerator CampChoice()
    {
        
        yield return new WaitForSeconds(1f);

        if (camp.Equals("red"))
        {
            Debug.Log("코루틴:" + camp);
            PlayerPrefs.SetString("CampBlue", camp);
        }
        else if (camp.Equals("blue"))
        {
            Debug.Log("코루틴:" + camp);
            PlayerPrefs.SetString("CampBlue", camp);
            SceneManager.LoadScene("LoadingScene");

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
        Debug.Log("전송 데이터: "+toSend);
        int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
        byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
        byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);

        singleton.clientSocket.Send(toSendLenBytes);
        singleton.clientSocket.Send(toSendBytes);

    }
    private void ConnectToTcpServer()
    {
        try
        {
            singleton.clientReceiveThread = new Thread(new ThreadStart(ReceiveReadyData));
            singleton.clientReceiveThread.IsBackground = true;
            Debug.Log("쓰레드 스타트");
            singleton.clientReceiveThread.Start();
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
                if (clientSocket == null)
                {
                    clientSocket.Close();
                    clientReceiveThread.Interrupt();
                    Debug.Log("Socket Exception");
                }


                byte[] rcvLenBytes = new byte[4];
                this.clientSocket.Receive(rcvLenBytes);
                int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
                byte[] rcvBytes = new byte[rcvLen];
                this.clientSocket.Receive(rcvBytes);
                String rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

                String[] rcvs = rcv.Split(',');
                for (int i = 0; i < rcvs.Length; i++)
                {
                    if(rcvs[i].Equals("myTeamCall"))
                    {
                        if(rcvs[0].Equals(myId))
                        {
                            myTeam = "1";
                        } else
                        {
                            myTeam = "2";
                        }
                    }

                    if(!rcvs[i].Equals(myId))
                    {
                        remoteId = rcvs[0];
                        Debug.Log("받은 아이디 :" + rcvs[0] );
                        if (rcvs[i].Equals("TaiLung"))
                        {
                            
                            yuerCharacterName = "TaiLung";
                            Debug.Log("상대 캐릭터 :" + yuerCharacterName);
                        }
                        else if (rcvs[i].Equals("Mantis"))
                        {
                            remoteId = rcvs[0];
                            yuerCharacterName = "Mantis";
                            Debug.Log("상대 캐릭터: " + yuerCharacterName);
                        }
                       
                    }

                    if (rcvs[i].Equals("success"))
                    {
                        success = "success";
                        Debug.Log("모든 유저 준비 완료:" + success);
                    }
                }

                if (rcv.IndexOf("<EOF>") > -1)
                {
                    break;
                }

                Debug.Log("Client received: " + rcv);

            }
        catch(Exception e)
            {
                clientReceiveThread.Interrupt();
                clientReceiveThread.Abort();
                clientSocket.Close();
                Debug.Log(e);

        }
    }
}

    

   
   

   

    private void ListenForData()
    {

        try
        {
         

             socketConnection = new TcpClient("13.124.87.189", 8888);

            Byte[] bytes = new Byte[1024];

            while (true)
            {

                // Get a stream object for reading 				

                using (NetworkStream stream = socketConnection.GetStream())
                {

                    int length;

                    // Read incomming stream into byte arrary. 					

                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {

                        var incommingData = new byte[length];

                        Array.Copy(bytes, 0, incommingData, 0, length);

                        // Convert byte array to string message. 						

                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                      
                        
                        Debug.Log("server message received as: " + serverMessage);

                        if (serverMessage.Equals("red"))
                        {

                            camp = "red";

                            Debug.Log(" 쓰레드 camp:" + camp);

                        }
                        else if (serverMessage.Equals("blue"))
                        {



                            camp = "blue";
                            gameStart = true;
                            Debug.Log("쓰레드:" + camp);

                        }
                    }

                }

            }

        }

        catch (SocketException socketException)
        {

            Debug.Log("Socket exception: " + socketException);

        }

    }

    private void OnApplicationQuit()
    {
        singleton.clientReceiveThread.Interrupt();
        singleton.clientReceiveThread.Abort();
        singleton.clientSocket.Close();
        //threadStop = false;
        //singleton.clientReceiveThread.Interrupt();
        //singleton.clientReceiveThread.Abort();
        //singleton.clientSocket.Close();

        Debug.Log("프로그램 종료");
    }




}



