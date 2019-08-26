using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.SpellIndicators.Demo;

public class PlayerFSM2 : MonoBehaviour {

    /** 열거형 
     * 상수의 일종으로 const 키워드 대신 enum 을 사용합니다.
       열거형은 const와 다르게 값을 입력하지 않아도 자동으로 0,1,2,3 이
       대입됩니다. 이렇게 선언된 열거형은 구분 이외의 목적은 따로 없습니다.*/

    public enum State
    {
        Idle,
        Move,
        Attack,
        AttackWait,
        Dead,
        Chase
    }

    public State currentState = State.Idle;

    Vector3 curTargetPos;

    GameObject curEnemy;

    private float chaseDistance = 13f;       // 추적 시작 거리
    private float attackDistance = 3f;       // 공격 사정거리   해당 값만큼 들어올때 공격 시작
    private float reChaseDistance = 4f;      // 공격 사정거리에서 벗어날 경우 재추적 거리

    private float rotAnglePerSecond = 360f;  // 초당 회전 속도
    public float moveSpeed = 5f;            // 추적 이동 속도

    private float skillDistance = 10f;
    private float attackDelay = 2f;
    private float attackTimer = 0f;

    public bool Sendone = true;

    public string chaseTarget;
    public string rcvChaseStart;
    public string rcvChaseTarget;
    public string rcvMyid;
    public string TrackingStart = "";

    public GameObject playerCharacter;

    Network network;

    PlayerAni myAni;

    PlayerParams myParams;
    MonsterParams curEnemyParams;
    AttakButton basicAttack;

    Transform player;

    RemoteParams remoteParams;
    RemoteAni remoteAni;
    public Transform remotePlayer;
    RemoteFSM remoteFSM;

    Transform monster;

    TowerParams towerParams;
    Transform redTowerPos;

   public SkillButton skillButton;
    JoyStick joy;

    float respawnCount = 5f;
    float respawnTimer = 0f;

    // 초기화
    void Start () {
        playerCharacter = GameObject.FindWithTag("Player");

        player = GameObject.FindWithTag("Player").transform;
        myParams = GetComponent<PlayerParams>();
        myAni = GetComponent<PlayerAni>();
        myParams.InitParams();
        myParams.deadEvent.AddListener(ChangeToPlayerDead);

        remotePlayer = GameObject.FindWithTag("RemotePlayer").transform;
        remoteParams = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteParams>();
        remoteAni = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteAni>();
        remoteFSM = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteFSM>();

        monster = GameObject.Find("Spider").transform;
        skillButton = GameObject.Find("TaiLungSpell1").GetComponent<SkillButton>();

        curEnemyParams = GameObject.Find("Monster").GetComponent<MonsterParams>();


        basicAttack = GameObject.Find("AttackButton").GetComponent<AttakButton>();

        redTowerPos = GameObject.FindWithTag("RedTower").transform;

        towerParams = GameObject.FindWithTag("RedTower").GetComponent<TowerParams>();

        network = GameObject.Find("NetworkManager").GetComponent<Network>();


        joy = GameObject.Find("JoystickBackGround").GetComponent<JoyStick>();
        

    }

    public void PlayerFSM2Init()
    {
        playerCharacter = GameObject.FindWithTag("Player");

        player = GameObject.FindWithTag("Player").transform;
        myParams = GetComponent<PlayerParams>();
        myAni = GetComponent<PlayerAni>();
        myParams.InitParams();
        myParams.deadEvent.AddListener(ChangeToPlayerDead);

        remotePlayer = GameObject.FindWithTag("RemotePlayer").transform;
        remoteParams = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteParams>();
        remoteAni = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteAni>();

        monster = GameObject.Find("Spider").transform;
        skillButton = GameObject.Find("TaiLungSpell1").GetComponent<SkillButton>();

        curEnemyParams = GameObject.Find("Monster").GetComponent<MonsterParams>();


        basicAttack = GameObject.Find("AttackButton").GetComponent<AttakButton>();

        redTowerPos = GameObject.FindWithTag("RedTower").transform;

        towerParams = GameObject.FindWithTag("RedTower").GetComponent<TowerParams>();

        network = GameObject.Find("NetworkManager").GetComponent<Network>();


        joy = GameObject.Find("JoystickBackGround").GetComponent<JoyStick>();
    }

    IEnumerator PlayerDestroyDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(playerCharacter.gameObject);
    }

    //  플레이어 사망 이벤트
    public void ChangeToPlayerDead()
    {
        Debug.Log("로컬 플레이어 사망하였습니다.");
        remotePlayer.gameObject.SendMessage("PlayerDead");
        remoteFSM.rcvChaseTarget = "";
        ChangeState(State.Dead, PlayerAni.ANI_DIE);
        StartCoroutine(PlayerDestroyDelay());
        
    }

    // 몬스터 사망 이벤트
    public void CurrentEnemyDead()
    {
        ChangeState( State.Idle, PlayerAni.ANI_IDLE );
        print("거미 처치");

        //monster = null;
    }
    // 리모트 플레이어 사망 이벤트 
    public void RemotePlayerDead()
    {
        ChangeState(State.Idle, PlayerAni.ANI_IDLE);
        print("리모트 플레이어 처치");

        //remotePlayer = null;
    }

    public void redTowerDead()
    {
        ChangeState(State.Idle, PlayerAni.ANI_IDLE);
    }

    // 스킬 데미지 이벤트
    public void SkillCalculate()
    {
        if (skillButton.skill1Start == true)
        {
            skillButton.skill1Start = false;
            int attackPower = 100;
            remoteParams.SetEnemyAttack(attackPower);
            myAni.ChangeAni(PlayerAni.ANI_IDLE);
            print("플레이어 타이렁 캐릭터 스킬 1 발동");
        }
    }
    
    //  데미지 이벤트 
    public void AttackCalculate()
    {
        int attackPower = 15;
        if (skillButton == null)
        {
            skillButton = GameObject.Find("TaiLungSpell1").GetComponent<SkillButton>();
        }

         if( skillButton.skill1Start == true )
        {
            
            
            remoteParams.SetEnemyAttack(attackPower);
           
            print("플레이어 타이렁 캐릭터 스킬 1 발동");
        }

        //if (curEnemy == null)
        //    return;
        Debug.Log("AttackCalculate to chaseTarget:" + chaseTarget);
        if (chaseTarget.Equals(""))
            return;

        if( chaseTarget.Equals("remotePlayer"))
        {
            
            remoteParams.SetEnemyAttack( attackPower );
            print( "Attack" + remoteParams.name );
        }
       else if(chaseTarget.Equals("monster"))
        {
            
            curEnemyParams.SetEnemyAttack(attackPower);
            print("Attack" + monster.name);
        } else if(chaseTarget.Equals("redTower"))
        {
            
            towerParams.SetEnemyAttack(attackPower);
            print("Attack" + towerParams.name);
        }       
    }

    public void AttackEnemy ( GameObject enemy )
    {
        if (curEnemy != null && curEnemy == enemy)
        {
            return;
        }

        curEnemyParams = enemy.GetComponent<MonsterParams>();

        if( curEnemyParams.isDead == false )
        {
            curEnemy = enemy;
            curTargetPos = curEnemy.transform.position;
            print(curEnemy.name);

            ChangeState(State.Move, PlayerAni.ANI_WALK);
        } else
        {
            curEnemyParams = null;
        }

        
    }

   // 플레이어 상태 변경 함수 
   public void ChangeState( State newState, int aniNumber )
    {
         if( currentState == newState )
        {
            return;
        }

        myAni.ChangeAni( aniNumber );
        currentState = newState;
    }
    
    // 플레이어 캐릭터 상태 업데이트 
    void UpdateState()
    {
        switch ( currentState )
        {
            case State.Idle:
                IdleState();
                break;

            case State.Move:
                MoveState();
                break;
            case State.Chase:
                ChaseState();
                break;

            case State.Attack:
                AttackState();
                break;

            case State.AttackWait:
                AttackWaitState();
                break;

            case State.Dead:
                DeadState();
                break;
        }

    }


    IEnumerator TrackingStandby()
    {
        yield return new WaitForSeconds(0.1f);
     

    }

    // 기본 상태 
    void IdleState()
    {
        if( skillButton.skill1Start == true )
        {
            if( basicAttack.attackFlag == true && GetDistanceFromPlayer( remotePlayer ) <= skillDistance)
            {
                basicAttack.attackFlag = false;
                TurnToDestination( remotePlayer );
                //MoveToDestination( remotePlayer );
                joy.MoveFlag = false;
                myAni.ChangeAni( PlayerAni.ANI_SKILL1 );

            }
        }

        // 로컬 플레이어 추적 시작 이벤트
        // 추적 시작거리에 타겟이 있으면 추적 시작
        if ( basicAttack.attackFlag == true && GetDistanceFromPlayer( remotePlayer ) <= chaseDistance )
        {
            if (remoteParams.isDead == true)
            {
                ChangeState(State.Idle, PlayerAni.ANI_IDLE);
                Debug.Log("리모트 플레이어 가 사망 했음으로 리턴 후 State.Idle 로 변경 ");
                return;
            }
            basicAttack.attackFlag = false;

            if (Sendone)
            {               
                network.Send(network.myId + "," + "TrackingStart" + "," + "remotePlayer");
                Sendone = false;
                Debug.Log( " 한번만 보냄 " + Sendone );
                
            }
            Debug.Log("Send Target value : " + "remotePlayer");
           

    }

        else if ( basicAttack.attackFlag == true  && GetDistanceFromPlayer( monster ) <= chaseDistance )
        {
            if (curEnemyParams.isDead == true)
            {               
                Debug.Log("몬스터 isDead true");
                return;
            }

            basicAttack.attackFlag = false;
            if (Sendone)
            {              
                network.Send(network.myId + "," + "TrackingStart" + "," + curEnemyParams.name );
                Sendone = false;
                Debug.Log(" 한번만 보냄 " + Sendone);
                
            }
            Debug.Log("Send Target value : " + "monster");
            //StartCoroutine(TrackingStandby());
            if (rcvChaseStart.Equals("TrackingStart") && rcvChaseTarget.Equals(curEnemyParams.name))
            {
                ChangeState(State.Chase, PlayerAni.ANI_WALK);
                chaseTarget = "monster";
                Debug.Log("플레이어 몬스터 추적 시작 chaseTarget to value : " + chaseTarget);
            }



        } else if ( basicAttack.attackFlag == true && GetDistanceFromPlayer(redTowerPos) <= chaseDistance)
        {
            basicAttack.attackFlag = false;
            if (towerParams.isDead == true)
            {
                ChangeState(State.Idle, PlayerAni.ANI_IDLE);
                Debug.Log("포탑이 이미 파괴되었음으로 리턴 후 State.Idle 로 변경 ");
                return;
            }

            if(redTowerPos == null)
            {
               redTowerPos = GameObject.Find("109").transform;
            }
            
            if (Sendone)

            {                
                network.Send(network.myId + "," + "TrackingStart" + "," + "redTower");
                Sendone = false;
                Debug.Log(" 한번만 보냄 "+ Sendone);
                TrackingStart = "";
            }
            Debug.Log("Send Target value : " + "redTower");


        } else
        {
            return;
        }
    }

    void MoveState()
    {
       
    }

 

    void ChaseState()
    {
    
        if ( chaseTarget.Equals("remotePlayer") )
        {
            

            if( GetDistanceFromPlayer(remotePlayer) <= attackDistance )
            {

                TrackingStart = "";
                ChangeState( State.Attack, PlayerAni.ANI_ATTACK );
            } else
            {
                
                TurnToDestination( remotePlayer );
                MoveToDestination( remotePlayer );
                myAni.ChangeAni( PlayerAni.ANI_WALK );

            }
        }
        else if ( chaseTarget.Equals("monster") )
        {
            if ( curEnemyParams.isDead == true )
            {
                ChangeState(State.Idle, PlayerAni.ANI_IDLE);
                Debug.Log("몬스터가 사망 했음으로 리턴 후 State.Idle 로 변경 ");
                return;
            }

            if ( GetDistanceFromPlayer( monster ) <= attackDistance )
            {
                TrackingStart = "";
                ChangeState( State.Attack, PlayerAni.ANI_ATTACK );
            }
            else
            {
                TurnToDestination( monster );
                MoveToDestination( monster );
                myAni.ChangeAni(  PlayerAni.ANI_WALK );
            }
        } else if (chaseTarget.Equals("redTower"))
        {
            if (towerParams.isDead == true)
            {
                ChangeState( State.Idle, PlayerAni.ANI_IDLE );
                Debug.Log("타워가 파괴 되었 음으로 리턴 후 State.Idle 로 변경 ");
                return;
            }

            if (GetDistanceFromPlayer(redTowerPos) <= attackDistance)
            {
                TrackingStart = "";
                ChangeState(State.Attack, PlayerAni.ANI_ATTACK);
                Debug.Log("레드타워 추적 끝 공격 시작 ");

            }
            else
            {
                TurnToDestination( redTowerPos );
                MoveToDestination( redTowerPos );
                myAni.ChangeAni( PlayerAni.ANI_WALK );
            }

        }

    }

    void AttackState()
    {
        attackTimer = 0f;

        if( chaseTarget.Equals("remotePlayer") )
        {
            transform.LookAt( remotePlayer.position );
            ChangeState( State.AttackWait, PlayerAni.ANI_ATKIDLE );
        }
       else if( chaseTarget.Equals("monster") )
        {
            transform.LookAt(monster.position);
            ChangeState(State.AttackWait, PlayerAni.ANI_ATKIDLE);
        }
        else if( chaseTarget.Equals("redTower") )
        {
            Debug.Log("레드타워 공격 준비 attackTimer value + " + attackTimer );
            transform.LookAt(redTowerPos.position);
            ChangeState(State.AttackWait, PlayerAni.ANI_ATKIDLE);
        }       
    }

    void AttackWaitState()
    {
        Debug.Log("AttackWaiState chaseTarget value : " + chaseTarget );
        if( chaseTarget.Equals("remotePlayer") )
        {
            if ( remoteParams.isDead == true )
            {
                return;
            }

            if ( attackTimer > attackDelay )
            {
                transform.LookAt( remotePlayer.position );
                ChangeState(State.Attack, PlayerAni.ANI_ATTACK);

                attackTimer = 0f;
            }
            attackTimer += Time.deltaTime;
            //Debug.Log("attackTimer value :" + attackTimer);
        }
       else if(chaseTarget.Equals("monster"))
        {
            if (attackTimer > attackDelay)
            {
                transform.LookAt(monster.position);
                ChangeState(State.Attack, PlayerAni.ANI_ATTACK);

                attackTimer = 0f;
            }
            attackTimer += Time.deltaTime;
            Debug.Log("attackTimer2 value :" + attackTimer);
        } else if(chaseTarget.Equals("redTower"))
        {
            if (attackTimer > attackDelay)
            {
                transform.LookAt(redTowerPos.position);
                ChangeState(State.Attack, PlayerAni.ANI_ATTACK);

                attackTimer = 0f;
            }
            

            attackTimer += Time.deltaTime;
            Debug.Log("attackTimer3 value :" + attackTimer);
            Debug.Log("attackDelay3 value :" + attackDelay);

        }


    }

    void DeadState()
    {

    }

    public void MoveTo( Vector3 tPos)
    {
        curEnemyParams = null;
        curTargetPos = tPos;

        ChangeState(State.Move, PlayerAni.ANI_WALK );
    }

    //  타겟 방향으로 방향 변경
    void TurnToDestination(Transform TargetPlayer)
    {
        Quaternion lookRotation = Quaternion.LookRotation(TargetPlayer.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }
    //  타겟 위치로 이동 
    void MoveToDestination(Transform TargetPlayer)
    {
        transform.position = Vector3.MoveTowards(transform.position, TargetPlayer.position, moveSpeed * Time.deltaTime);
    }
    //   타겟과 플레이어 간의 거리를 구함
    float GetDistanceFromPlayer(Transform TargetPlayer)
    {
        float distance = Vector3.Distance(transform.position, TargetPlayer.position);

        return distance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.tag == "Enemy" )
        {
            Debug.Log(other.gameObject.tag);
        }
    }

    void Update () {

        //if(Input.GetKeyDown("space"))
        //{
        //    Debug.Log("내 추적 시작 거리 : " + chaseDistance );
        //   Debug.Log("리모트 플레이어와의 거리" + GetDistanceFromPlayer(remotePlayer));
        //   Debug.Log( "몬스터와의 거리" + GetDistanceFromPlayer(monster));
        //   Debug.Log( "포탑과의 거리" + GetDistanceFromPlayer(redTowerPos));
        //}
       
        UpdateState();

	}
}
