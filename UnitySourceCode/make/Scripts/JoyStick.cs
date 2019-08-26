using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using Werewolf.SpellIndicators.Demo;

public class JoyStick : NetworkBehaviour {

    // 공개
    public Transform Player;        // 플레이어.
    private Transform Player2;
    public Transform Stick;         // 조이스틱.
    public Animator anim;

    Network network;
    
    // 비공개
    private Vector3 StickFirstPos;  // 조이스틱의 처음 위치.
    private Vector3 JoyVec;         // 조이스틱의 벡터(방향)
    private float Radius; // 조이스틱 배경의 반 지름.
    private AttakButton attack;
    public  bool MoveFlag;          // 플레이어 움직임 스위치.
    public bool remoteMoveFlag = false; // 리모트 플레이어의 이동 스위치
    public string attackFlag= "";      // 리모트 플레이이어의 공격 스위치
    public bool attackEnd = true;       // 리모트 플레이어의 공격 끝
    public bool attackControl = true;   // 리모트 플레이어의 공격 코드 한번만 실행 되게 하기위한 bool 변수   
    public bool stop = true;
    public float moveSpeed = 6f;

    public Transform player2;
    public RemoteAni remoteAni;
    string playerMove;
    string myid = "";
    CharacterCreate characterCreate;
    public PlayerAni myAni;

    public  PlayerFSM2 playerFsm2;
    SkillButton skillButton;
    GameManager gameManager;
    ExampleClass exampleClass;

    void Start()
    {
        
        skillButton = GameObject.Find("TaiLungSpell1").GetComponent<SkillButton>();

        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        anim = GameObject.FindWithTag("Player").GetComponent<Animator>();

        player2 = GameObject.FindWithTag("RemotePlayer").GetComponent<Transform>();
        playerFsm2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
        attack = GameObject.Find("AttackButton").GetComponent<AttakButton>();
        myAni = GameObject.FindWithTag("Player").GetComponent<PlayerAni>();

        characterCreate = new CharacterCreate();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


        //Player = GameObject.Find(myid).GetComponent<Transform>();
        Debug.Log("JoyStick Scripts myid value: " + myid);
        // Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        //EventTrigger eventTrigger = new EventTrigger();
        Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
        StickFirstPos = Stick.transform.position;

        // 캔버스 크기에대한 반지름 조절.
        float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
        Radius *= Can;

        MoveFlag = false;

        network = GameObject.Find("NetworkManager").GetComponent<Network>();
        myid = network.myId;
        Debug.Log("네트워크 스크립트 내 아이디  : " + network.myId);
        remoteAni = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteAni>();
    }

    //public void JoyStickInit()
    //{
    //    skillButton = GameObject.Find("TaiLungSpell1").GetComponent<SkillButton>();
    //    Stick = GameObject.Find("JoyStick").GetComponent<Transform>();

    //    Player = GameObject.FindWithTag("Player").GetComponent<Transform>();

    //    anim = GameObject.FindWithTag("Player").GetComponent<Animator>();

    //    playerFsm2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
    //    attack = GameObject.Find("AttackButton").GetComponent<AttakButton>();
    //    myAni = GameObject.FindWithTag("Player").GetComponent<PlayerAni>();

    //    characterCreate = new CharacterCreate();
    //    gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();


    //    //Player = GameObject.Find(myid).GetComponent<Transform>();
    //    Debug.Log("JoyStick Scripts myid value: " + myid);
    //    // Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    //    //EventTrigger eventTrigger = new EventTrigger();
    //    Radius = GetComponent<RectTransform>().sizeDelta.y * 0.5f;
    //    StickFirstPos = Stick.transform.position;

    //    // 캔버스 크기에대한 반지름 조절.
    //    float Can = transform.parent.GetComponent<RectTransform>().localScale.x;
    //    Radius *= Can;

    //    MoveFlag = false;

    //    network = GameObject.Find("NetworkManager").GetComponent<Network>();
    //    myid = network.myId;
    //    Debug.Log("네트워크 스크립트 내 아이디  : " + network.myId);
    //}

    private void Awake()
    {
        
    }
    IEnumerator JoySet ()
    {
        yield return new WaitForSeconds(0.1f);
       

    }

    private void Update()
    {
        if(playerFsm2 == null)
        {
            playerFsm2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
            Debug.Log("JoyStick Script playerFsm2 가 Null PointException : PlayerFSN2 를 다시 지정해줌  ");
        }

        if (MoveFlag)
            Player.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);


        //if (remoteMoveFlag && stop == false)
        //{
        //    Debug.Log("Network Script void Update if MoveFlag Log ");
        //    player2.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
        //    Debug.Log("Network Script void Update if MoveFlag Log 1");
        //    player2.transform.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        //    Debug.Log("Network Script void Update if MoveFlag Log 2");

        //    remoteAni.ChangeAni(RemoteAni.ANI_WALK);
        //    Debug.Log("Network Script void Update if MoveFlag Log 3");
        //    stop = true;
        //}

        //else if (remoteMoveFlag == false && stop == true)
        //{
        //    remoteAni.ChangeAni(RemoteAni.ANI_IDLE);

        //    stop = false;
        //}

        //if (attackFlag.Equals("remotePlayer") && attackControl == true)
        //{
        //    playerFsm2.ChangeState(PlayerFSM2.State.Chase, PlayerAni.ANI_WALK);
        //    attackControl = false;
        //    attackFlag = "";
        //}
        //else if (attackFlag.Equals("redTower"))
        //{
        //    playerFsm2.ChangeState(PlayerFSM2.State.Chase, PlayerAni.ANI_WALK);
        //    attackControl = false;
        //    attackFlag = "";
        //    Debug.Log("네트워크 스크립트 상태 체인지 > 추적");
        //}


        //playerMove = Player.transform.position.ToString();
        //playerMove = playerMove.Substring(1, playerMove.Length - 2);

        //Debug.Log(playerMove);



        //  anim.Play("1_walk");
        //if (anim.GetBool("Move"))
        //{
        //    anim.Play("1_walk");

        //}
    }
    

    // 드래그
    public void Drag(BaseEventData _Data)
    {
        if(gameManager.GameEnd == true)
        {
            Debug.Log("JoyStick Script Drag() Log = GameEnd");
            return;
        }
        
        if (playerFsm2.currentState == PlayerFSM2.State.Dead)
        {
            Debug.Log("JoyStick Script Player State Dead return");
            return;
        }
           

        

        playerFsm2.chaseTarget = "";
        MoveFlag = true;
        attackControl = true;
        attack.attackFlag = false;
        playerFsm2.Sendone = true;
        playerFsm2.ChangeState( PlayerFSM2.State.Idle, PlayerAni.ANI_WALK );
        myAni.ChangeAni(PlayerAni.ANI_WALK);

        if (skillButton.skill1Start == true)
        {
            myAni.ChangeAni(PlayerAni.ANI_RUN);
        }

        PointerEventData Data = _Data as PointerEventData;
        Vector3 Pos = Data.position;

        // 조이스틱을 이동시킬 방향을 구함.(오른쪽,왼쪽,위,아래)
        JoyVec = (Pos - StickFirstPos).normalized;

        // 조이스틱의 처음 위치와 현재 내가 터치하고있는 위치의 거리를 구한다.
        float Dis = Vector3.Distance(Pos, StickFirstPos);

        // 거리가 반지름보다 작으면 조이스틱을 현재 터치하고 있는곳으로 이동. 
        if (Dis < Radius)
            Stick.position = StickFirstPos + JoyVec * Dis;
        // 거리가 반지름보다 커지면 조이스틱을 반지름의 크기만큼만 이동.
        else
            Stick.position = StickFirstPos + JoyVec * Radius;

        Player.eulerAngles = new Vector3(0, Mathf.Atan2(JoyVec.x, JoyVec.y) * Mathf.Rad2Deg, 0);
        //anim.SetBool("Move", MoveFlag);
        


        //string playerRotation = Player.transform.eulerAngles.ToString();
        //playerRotation = playerRotation.Substring(1, playerRotation.Length - 2);

        network.Send(network.myId + "," + JoyVec.x + "," + JoyVec.y + "," + "MoveFlag");
            Debug.Log("JoyVec.x:" + JoyVec.x + "JoyVec.y:" + JoyVec.y);
    }

    // 드래그 끝.
    public void DragEnd()
    {
        network.Send(network.myId + "," + "MoveStop");
        Stick.position = StickFirstPos; // 스틱을 원래의 위치로.
        JoyVec = Vector3.zero;          // 방향을 0으로.
        MoveFlag = false;
        attackControl = false;

        //anim.SetBool("Move", MoveFlag);
        myAni.ChangeAni(PlayerAni.ANI_IDLE);
        // 내 이동이 끝났다는것을 알림
       
    }
}