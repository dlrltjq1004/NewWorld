using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnCountDown : MonoBehaviour {

    public GameObject greenRespawnTaiLung;
    public GameObject redRespawnTiaLung;

    public CharacterCreate characterCreate;
    private Network network;
    private JoyStick joy;
    private Quaternion characterQuaternion = Quaternion.identity;
    private Quaternion remoteQuaternion = Quaternion.identity;

    public Text greenRespawnCoundDown;
    public Text redRespawnCoundDown;

    float countDown = 10f;
    float respawnTimer = 0f;


    PlayerParams playerParams;
    PlayerFSM2 playerFSM2;
    PlayerAni playerAni;

    RemoteParams remoteParams;
    RemoteFSM remoteFSM;
    RemoteAni remoteAni;

    MonsterFSM monsterFSM;

    string myBase = "";
    string myCharacterName = "";

    // Use this for initialization
    void Start () {
        network = GameObject.Find("NetworkManager").GetComponent<Network>();
        myCharacterName = PlayerPrefs.GetString("myCharacter");
        joy = GameObject.Find("JoystickBackGround").GetComponent<JoyStick>();


        playerParams = GameObject.FindWithTag("Player").GetComponent<PlayerParams>();
        playerFSM2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();

        remoteParams = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteParams>();
        remoteFSM = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteFSM>();
        remoteAni = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteAni>();

        characterCreate = GetComponent<CharacterCreate>();
        //greenRespawnTaiLung = GameObject.Find("GreenRespawnTaiLung").GetComponent<GameObject>();
        //redRespawnTiaLung = GameObject.Find("RedRespawnTaiLung").GetComponent<GameObject>();

        //greenRespawnCoundDown = GameObject.Find("GreenRespawnCoundDown").GetComponent<Text>();
        //redRespawnCoundDown = GameObject.Find("RedRespawnCoundDown").GetComponent<Text>();

        myBase = characterCreate.myTeam;
        countDown = 10f;
        respawnTimer = 0f;
    }
	
	// Update is called once per frame
	void Update () {

        //if(playerParams == null)
        //{
        //    playerParams = GameObject.FindWithTag("Player").GetComponent<PlayerParams>();
        //}

        if(playerParams.isDead == true)
        {
            
            countDown -= Time.deltaTime;
            greenRespawnTaiLung.SetActive(true);
            greenRespawnCoundDown.text = "" + (int) countDown;
            Debug.Log("리스폰 스크립티 플레이어 isDead value: " + playerParams.isDead);
            if (countDown < 0 && playerParams.isDead == true)
            {
                greenRespawnTaiLung.SetActive(false); 
                Debug.Log("Player Base: " + myBase);
                if (myBase.Equals("1"))
                {
                    // 내가 선택한 캐릭터 생성
                    GameObject myCharacter = Resources.Load(myCharacterName) as GameObject;
                    Vector3 pos = new Vector3(45.02f, 60.3f, 99.59f);
                    characterQuaternion.eulerAngles = new Vector3(0, 100, 0);
                    myCharacter.transform.rotation = characterQuaternion;
                    Instantiate(myCharacter, pos, myCharacter.transform.rotation).name = network.myId;
                    playerParams = GameObject.FindWithTag("Player").GetComponent<PlayerParams>();
                    playerParams.isDead = false;
                    Debug.Log("리스폰 스크립트 블루 진영 Log 1");
                    playerParams = GameObject.FindWithTag("Player").GetComponent<PlayerParams>();
                    Debug.Log("리스폰 스크립트 블루 진영 Log 2");
                    joy.Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
                    Debug.Log("리스폰 스크립트 블루 진영 Log 3");
                    joy.anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
                    Debug.Log("리스폰 스크립트 블루 진영 Log 4");
                    joy.myAni = GameObject.FindWithTag("Player").GetComponent<PlayerAni>();
                    Debug.Log("리스폰 스크립트 블루 진영 Log 5");
                    //joy.Stick = GameObject.Find("JoyStick").GetComponent<Transform>();
                    joy.playerFsm2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
                    Debug.Log("리스폰 스크립트 블루 진영 Log 6");
                    //playerAni.anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
                    Debug.Log("리스폰 스크립트 블루 진영 Log 7");
                    network.playerFsm2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
                    network.player = GameObject.FindWithTag("Player").transform;
                    playerFSM2.remotePlayer = GameObject.FindWithTag("RemotePlayer").transform;


                    playerFSM2.ChangeState( PlayerFSM2.State.Idle, PlayerAni.ANI_IDLE );
                    Debug.Log("1 진영 리스폰 완료");
                    
                }
                else
                {
                  
                
                    GameObject myCharacter = Resources.Load(myCharacterName) as GameObject;
                    Vector3 pos = new Vector3(254.85f, 60.3f, 100.41f);
                    characterQuaternion.eulerAngles = new Vector3(0, -110, 0);
                    myCharacter.transform.rotation = characterQuaternion;
                    Instantiate(myCharacter, pos, myCharacter.transform.rotation).name = network.myId;
                    playerParams = GameObject.FindWithTag("Player").GetComponent<PlayerParams>();
                    playerParams.isDead = false;
                    Debug.Log("리스폰 스크립트 레드 진영 Log 1");
                    joy.Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
                    Debug.Log("리스폰 스크립트 레드 진영 Log 2");
                    joy.anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
                    Debug.Log("리스폰 스크립트 레드 진영 Log 3");
                    joy.myAni = GameObject.FindWithTag("Player").GetComponent<PlayerAni>();
                    Debug.Log("리스폰 스크립트 레드 진영 Log 4");
                    //joy.Stick = GameObject.Find("JoyStick").GetComponent<Transform>();
                    joy.playerFsm2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
                    Debug.Log("리스폰 스크립트 레드 진영 Log 5");
                    //playerAni.anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
                    Debug.Log("리스폰 스크립트 레드 진영 Log 6");
                    network.playerFsm2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
                    Debug.Log("리스폰 스크립트 레드 진영 Log 7");
                    network.player = GameObject.FindWithTag("Player").transform;
                    playerFSM2.remotePlayer = GameObject.FindWithTag("RemotePlayer").transform;

                    playerFSM2.ChangeState(PlayerFSM2.State.Idle, PlayerAni.ANI_IDLE);
                    Debug.Log("2 진영 리스폰 완료");
                }
            }
        } else if(remoteParams.isDead == true )
        {
            countDown -= Time.deltaTime;
            redRespawnTiaLung.SetActive(true);
            redRespawnCoundDown.text = "" + (int) countDown;
            if (countDown < 0 && remoteParams.isDead == true)
            {
                redRespawnTiaLung.SetActive(false);
                
                Debug.Log("RemotePlayer Base: " + myBase);
                if (myBase.Equals("1"))
                {
                  
                    GameObject YouerCharacter = Resources.Load("RemoteTaiLung") as GameObject;
                    Vector3 remotePos = new Vector3(254.85f, 60.3f, 100.41f);
                    remoteQuaternion.eulerAngles = new Vector3(0, -110, 0);
                    YouerCharacter.transform.rotation = remoteQuaternion;
                    Instantiate(YouerCharacter, remotePos, YouerCharacter.transform.rotation).tag = "RemotePlayer";
                    GameObject.FindWithTag("RemotePlayer").name = "RemotePlayer";
                    remoteParams = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteParams>();
                    remoteParams.isDead = false;

                    network.player2 = GameObject.FindWithTag("RemotePlayer").GetComponent<Transform>();
                    //network.playerFsm2 = GameObject.FindWithTag("RemotePlayer").GetComponent<PlayerFSM2>();
                    remoteAni.anim = GameObject.FindWithTag("RemotePlayer").GetComponent<Animator>();
                    playerFSM2.remotePlayer = GameObject.FindWithTag("RemotePlayer").transform;
                    network.remoteFSM = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteFSM>();
                    remoteFSM.ChangeState(RemoteFSM.State.Idle, RemoteAni.ANI_IDLE);
                }
                else
                {
                 
                 
                    // 내가 선택한 캐릭터 생성
                    GameObject YouerCharacter = Resources.Load("RemoteTaiLung") as GameObject;
                    Vector3 remotePos = new Vector3(45.02f, 60.3f, 99.59f);
                    remoteQuaternion.eulerAngles = new Vector3(0, 100, 0);
                    YouerCharacter.transform.rotation = remoteQuaternion;
                    Instantiate(YouerCharacter, remotePos, YouerCharacter.transform.rotation).tag = "RemotePlayer";
                    GameObject.FindWithTag("RemotePlayer").name = "RemoteTaiLung";
                    remoteParams = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteParams>();
                    remoteParams.isDead = false;

                    playerFSM2.remotePlayer = GameObject.FindWithTag("RemotePlayer").transform;
                    network.player2 = GameObject.FindWithTag("RemotePlayer").GetComponent<Transform>();
                    //network.playerFsm2 = GameObject.FindWithTag("RemotePlayer").GetComponent<PlayerFSM2>();
                    remoteAni.anim = GameObject.FindWithTag("RemotePlayer").GetComponent<Animator>();
                    network.remoteFSM = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteFSM>();
                    remoteFSM.ChangeState(RemoteFSM.State.Idle, RemoteAni.ANI_IDLE);
                }

            }
        }

      

    }
}
