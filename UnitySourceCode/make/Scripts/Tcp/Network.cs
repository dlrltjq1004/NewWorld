using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;
using System.Text;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Werewolf.SpellIndicators.Demo;

[Serializable]
class Network : MonoBehaviour
    {
    
    private Thread clientReceiveThread;
    public Socket clientSocket;
    private TcpClient socketConnection;
    public Transform player;
    PlayerAni playerAni;

    public Transform player2;
    public Rigidbody rigidboby;
    private Vector3 pos;
    private Vector3 JoyVec;
    private float rot;
    private float turnSpeed = 5f;
    private Quaternion remotePlayerRotation = Quaternion.identity;
    public string rcv = null;
    public string myId = "";
    public string myTeam = "";
    public float revRotate;
    string rcvId = "";
    public bool MoveFlag = false;
    bool Stop = true;
    bool yeah = true;
    bool attackControl;
    bool ScriptCall = true;
    bool TcpCall = true;
    public RemoteAni remoteAni;
    float remoteRotation1, remoteRotation2, remoteRotation3;
    public string attackFlag;

    public static Network singleton;
        SkillButton skillOne;

    PlayerFSM playerFsm;
   public PlayerFSM2 playerFsm2;
   public RemoteFSM remoteFSM;
   public JoyStick joy;
   public float remoteMoveSpeed = 6f;
    public string ScriptControl = "";
    public string skillcheck;
  
    MonsterFSM monsterFSM;
    AndroidPlugin androidPlugin;
    ReadyBtn readBtn;
    ExampleClass exampleClass;
 
    //public static void Main(string[] args)
    //{
    public void Start()
        {
        //androidPlugin = GameObject.Find("NetworkManager").GetComponent<AndroidPlugin>();
       

        Debug.Log("네트워크 스크립트");
        //joy = GameObject.Find("JoystickBackGround").GetComponent<JoyStick>();
        Debug.Log("네트워크 스크립트2");
        singleton = this;

        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerAni = GameObject.FindWithTag("Player").GetComponent<PlayerAni>();

        player2 = GameObject.FindWithTag("RemotePlayer").GetComponent<Transform>();
        remoteAni = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteAni>();
        //rigidboby = GameObject.FindWithTag("RemotePlayer").GetComponent<Rigidbody>();

        //myId = androidPlugin.text;
        //myId = "Nova";
        //myTeam = "1";
        //myId = "Nova";

        //Debug.Log("내 아이디: "+ myId);

        //Connected();
        //Send(myId + "," + "myTeamCall");
        //ConnectToTcpServer();
        //clientSocket.Close();
        //myId = PlayerPrefs.GetString("myId");
        exampleClass = GameObject.Find("IDManager").GetComponent<ExampleClass>();
        myId = exampleClass.myId;
        Debug.Log("Unity Network Script myId: " + myId);
        //Connected();
        //Send(myId);
        //ConnectToTcpServer();

        playerFsm = GameObject.FindWithTag("RemotePlayer").GetComponent<PlayerFSM>();
        playerFsm2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();

        remoteFSM = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteFSM>();

        monsterFSM = GameObject.Find("Spider").GetComponent<MonsterFSM>();


        Connected();
        Send(myId);
        ConnectToTcpServer();

            skillOne = GameObject.Find("TaiLungSpell1").GetComponent<SkillButton>();
        //StartCoroutine(PlayerSet());
    }

   

    IEnumerator PlayerSet ()
    {
        yield return new WaitForSeconds(10f);
        Connected();
        Send(myId);
        ConnectToTcpServer();

    }

    public void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("remoteMoveSpeed value: " + remoteMoveSpeed);
            Debug.Log("리모트 캐릭 애니 런 스킬 발동: " + skillOne.receivedSkileOne);
        }

        if (MoveFlag == true)
        {
         
            Debug.Log("Network Script void Update if MoveFlag Log ");
            player2.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
            Debug.Log("Network Script void Update if MoveFlag Log 1");
            player2.transform.Translate(Vector3.forward * Time.deltaTime * remoteMoveSpeed);
            Debug.Log("Network Script void Update if MoveFlag Log 2");
            remoteAni.ChangeAni(RemoteAni.ANI_WALK);
            Debug.Log("Network Script void Update if MoveFlag Log 3");
            Stop = true;

            //if (skillcheck.Equals("SkillOne") && skillOne.receivedSkileOne == true)
            //{
            //    //skill1Start = true;
            //    //joy.moveSpeed = 8f;
            //    //remoteMoveSpeed = 8f;
            //    remoteMoveSpeed = 8f;
            //    remoteAni.ChangeAni(RemoteAni.ANI_RUN);
            //    StartCoroutine(RemotemoveTimer());
            //    Debug.Log("리모트 캐릭 애니 런");
            //    skillOne.receivedSkileOne = false;

            //    //StartCoroutine(RemotemoveTimer());
            //}
            
          
        }

        else if (MoveFlag == false && Stop == true)
        {
            remoteAni.ChangeAni(RemoteAni.ANI_IDLE);

            Stop = false;
        }

        if (attackFlag.Equals("remotePlayer") && attackControl == true)
        {
            playerFsm2.ChangeState(PlayerFSM2.State.Chase, PlayerAni.ANI_WALK);
            attackControl = false;
            attackFlag = "";
        }
        else if (attackFlag.Equals("redTower"))
        {
            playerFsm2.ChangeState(PlayerFSM2.State.Chase, PlayerAni.ANI_WALK);
            attackControl = false;
            attackFlag = "";
            Debug.Log("네트워크 스크립트 상태 체인지 > 추적");
        }

     
    }
        IEnumerator RemotemoveTimer()
        {
            yield return new WaitForSeconds(4f);
            remoteMoveSpeed = 6f;

            //myAni.ChangeAni(PlayerAni.ANI_WALK);
            skillOne.receivedSkileOne = false;
            skillcheck = "SkillStop";
            //myAni.ChangeAni(PlayerAni.ANI_SKILL1);
        }
        IEnumerator SendMoveStart()
    {
        if (joy.MoveFlag == true)
        {
           
            string playerRotation = player.transform.eulerAngles.y.ToString();
            playerRotation = playerRotation.Substring(1,playerRotation.Length -2);
            //Debug.Log("플레이어 방향:"+playerRotation);
            Send(myId + "," + playerRotation + "," + "MoveFlag");
            yield return new WaitForSeconds(0.05f);
        } 
    }

   

    public void Connected()
    {

        IPEndPoint serverAddress = new IPEndPoint(IPAddress.Parse("13.124.87.189"),8888);

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientSocket.Connect(serverAddress);
    }

    public void Send(string data)
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

        string toSend = data;
        
        // Sending
        int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
        byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
        byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
        clientSocket.Send(toSendLenBytes);
        clientSocket.Send(toSendBytes);
    
    }
    

    public void ReceiveData()
    {

        //** 2언어(서로 다른 언어 ex. Java, C# ) 통신간 데이터 수신 방법    **/
        // 1.바이트 최대 사이즈 정의 
        //    1).ex.byte[] rcvLenBytes = new byte[1024];
        //    2).clientSocket.Receive(rcvLenBytes);
        // 2.전달받은 바이트 데이터 인코딩
        //    1) int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
        //    2) byte[] rcvBytes = new byte[rcvLen];
        //    3) String rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);
        //    4) 출력 Debug.Log(rev);

        // Receiving
        try
        {
            while (true)
        {  

                // 소켓이 비어있다면 종료 
                if (clientSocket == null)
                {
                    clientSocket.Close();
                    clientReceiveThread.Interrupt();
                    Debug.Log("Socket Exception");
                }

                byte[] rcvLenBytes = new byte[4];
                clientSocket.Receive(rcvLenBytes);
                int rcvLen = System.BitConverter.ToInt32(rcvLenBytes, 0);
                byte[] rcvBytes = new byte[rcvLen];
                clientSocket.Receive(rcvBytes);
                rcv = System.Text.Encoding.ASCII.GetString(rcvBytes);

                String[] received = rcv.Split(',');
                rcvId = received[0];
                Debug.Log("Client received: " + rcv);
                
                for(int i = 0; i < received.Length; i++)
                {

                   
                       
                  
                        if (received[i].Equals("MoveFlag"))
                        {

                            MoveFlag = true;
                            float x = float.Parse(received[1]);
                            float y = float.Parse(received[2]);
                            JoyVec = new Vector3(x, y, 0);

                            remoteFSM.ChangeState(RemoteFSM.State.Idle, RemoteAni.ANI_WALK);

                            Debug.Log("MoveFlag:" + MoveFlag);
                        }
                        else if (received[i].Equals("MoveStop"))
                        {

                            MoveFlag = false;

                            Debug.Log("MoveFlag:" + MoveFlag);
                        }

                        if (received[0].Equals(myId))
                        {
                            Debug.Log("내 아이디 맞아 ~ : " + received[i]);
                            // 받은 데이터가 내 아이디와 같다면 내 캐릭터에게 데이터를 넣어 컨트롤한다.
                            //if (received[i].Equals("MoveStop"))
                            //{
                            //    joy.MoveFlag = false;
                            //    //playerAni.ChangeAni( PlayerAni.ANI_IDLE );
                            //    Debug.Log("플레이어 캐릭터 이동 정지");
                            //}

                            if (received[i].Equals("TrackingStart"))
                            {

                                playerFsm2.rcvMyid = received[0];
                                playerFsm2.rcvChaseStart = received[1];
                                playerFsm2.rcvChaseTarget = received[2];
                                Debug.Log("추적 시작 한다 ~");
                                Debug.Log("Network Script Received to Player TrackingStart data value : " + received[1]);
                                Debug.Log("Network Script Received to Player TrackingTarget data value : " + received[2]);
                                Debug.Log("추적 시작 i value : " + received[2]);
                                if (received[2].Equals("remotePlayer"))
                                {
                                    Debug.Log("네트워크 스크립트 추적 타겟 리모트 플레이어");
                                    playerFsm2.chaseTarget = "remotePlayer";
                                    Debug.Log("1");
                                    attackFlag = "remotePlayer";
                                    attackControl = true;
                                    Debug.Log("네트워크 스크립트 플레이어가 리모트 플레이어를 추적 :" + playerFsm2.chaseTarget);
                                }
                                else if (received[2].Equals("redTower"))
                                {
                                    Debug.Log("타워 추적");
                                    playerFsm2.chaseTarget = "redTower";
                                    attackFlag = "redTower";
                                    attackControl = true;
                                    Debug.Log("IdleState redTower chaseTarget to value : " + playerFsm2.chaseTarget);
                                }
                            }
                        }
                        else
                        {
                            // 데이터를 보낸 아이디가 내 아이디가 아니라면 상대 캐릭터를 컨트롤 한다.
                            Debug.Log("내 아이디 아니야 ~ : " + received[0]);

                            //if (received[i].Equals("MoveStop"))
                            //{
                            //    MoveFlag = false;
                            //}

                            if (received[i].Equals("TrackingStart"))
                            {
                                remoteFSM.rcvMyid = received[0];
                                remoteFSM.rcvChaseStart = received[1];
                                remoteFSM.rcvChaseTarget = received[2];
                                Debug.Log("리모트플레이어 추적 시작 ~");
                                Debug.Log("Network Script Received to remotePlayer TrackingStart data value : " + received[1]);
                                Debug.Log("Network Script Received to remotePlayer TrackingTarget data value : " + received[2]);
                            }
                            if (received[i].Equals("MonsterTrackingStart"))
                            {
                                monsterFSM.monsterChasePlayer = received[2];
                                Debug.Log("몬스터 동기화 추적 시작 ~");
                            }

                            //if(received[i].Equals("SkillOne"))
                            //{


                            //skillOne.receivedSkileOne = true;
                            //skillcheck = "SkillOne";
                            ////remoteMoveSpeed = 8f;
                            //Debug.Log("received value : "+skillOne.receivedSkileOne);
                            //}
                            //if (received[i].Equals("SkillStop"))
                            //{
                            //    skillOne.receivedSkileOne = false;
                            //skillcheck = "SkillStop";
                            //Debug.Log("received value : " + skillOne.receivedSkileOne);
                            //}

                        }
                }
            }
            // clientSocket.Close();  
        }
        catch (System.ObjectDisposedException e)
        {
            Debug.Log("Socket Exception");
            singleton.clientReceiveThread.Interrupt();
            singleton.clientReceiveThread.Abort();
            singleton.clientSocket.Close();
            
        }
    }
    private void ConnectToTcpServer()
    {
        try{


            singleton.clientReceiveThread = new Thread(new ThreadStart(ReceiveData));
            singleton.clientReceiveThread.IsBackground = true;
            Debug.Log("쓰레드 스타트");
            singleton.clientReceiveThread.Start();
        } catch(Exception e)
        {
            Debug.Log("On Client connect exception"+ e);
        }
       
    }

    private void OnApplicationQuit()
    {


        singleton.clientReceiveThread.Interrupt();
        singleton.clientReceiveThread.Abort();
        singleton.clientSocket.Close();
        Debug.Log("프로그램 종료");
    }

}



