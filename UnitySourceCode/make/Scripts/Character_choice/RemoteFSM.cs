using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Werewolf.SpellIndicators.Demo;
public class RemoteFSM : MonoBehaviour {

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

    private float chaseDistance = 10f;       // 추적 시작 거리
    private float attackDistance = 3f;       // 공격 사정거리   해당 값만큼 들어올때 공격 시작
    private float reChaseDistance = 4f;      // 공격하다 대상이 도망갈 경우 얼마나 멀리 떨어져야 다시 추적 할 것인지 지정

    private float rotAnglePerSecond = 360f;  // 초당 회전 속도
    private float moveSpeed = 5f;            // 추적 이동 속도

    private float attackDelay = 2f;          // 공격속도 2초
    private float attackTimer = 0f;          // 공격 속도 초기값

    private bool oneSend = true;

    public string chaseTarget;
    public string rcvChaseStart;
    public string rcvChaseTarget;
    public string rcvMyid;

    Network network;


    RemoteParams myParams;
    RemoteAni myAni;
    Transform remotePlayer;

    Transform player;
    PlayerParams playerParams;
    PlayerAni playerAni;

    Transform fromTarget;

    MonsterParams curEnemyParams;
    AttakButton basicAttack;

    

    Transform monster;

    TowerParams towerParams;
    Transform redTowerPos;
    SkillButton skillBtn;
    float respawnTimer = 0f;

    void Start () {

        remotePlayer = GameObject.FindWithTag("RemotePlayer").transform;
        myParams = GetComponent<RemoteParams>();
        myAni = GetComponent<RemoteAni>();
        myParams.InitParams();
        myParams.deadEvent.AddListener(RemoteChangeToPlayerDead);


        player = GameObject.FindWithTag("Player").transform;
        playerParams = GameObject.FindWithTag("Player").GetComponent<PlayerParams>();
        playerAni = GameObject.FindWithTag("Player").GetComponent<PlayerAni>();

        monster = GameObject.Find("Spider").transform;
        redTowerPos = GameObject.FindWithTag("RedTower").transform;

        curEnemyParams = GameObject.Find("Monster").GetComponent<MonsterParams>();

        towerParams = GameObject.FindWithTag("RedTower").GetComponent<TowerParams>();

        network = GameObject.Find("NetworkManager").GetComponent<Network>();

        Debug.Log("리모트 FSM Start ");
        skillBtn = GameObject.Find("TaiLungSpell1").GetComponent<SkillButton>();
    }

    IEnumerator RemotePlayerDestroyDelay()
    {
        yield return new WaitForSeconds(3f);
        Destroy(remotePlayer.gameObject);
    }

    public void RemoteChangeToPlayerDead()
    {
      
        ChangeState(State.Dead, RemoteAni.ANI_DIE);
        player.gameObject.SendMessage("RemotePlayerDead");
        Debug.Log("리모트 플레이어 사망 이벤트");

        rcvChaseTarget = "";
        rcvChaseStart = "";
        StartCoroutine(RemotePlayerDestroyDelay());
        
    }

    public void CurrentEnemyDead()
    {
        ChangeState(State.Idle, RemoteAni.ANI_IDLE);
        print("거미 처치");

        rcvChaseTarget = "";
        rcvChaseStart = "";
        //monster = null;
    }

    public void PlayerDead()
    {
        ChangeState( State.Idle, RemoteAni.ANI_IDLE );
        print("로컬 플레이어 처치");

        rcvChaseStart = "";
        rcvChaseTarget = "";
        //player = null;
    }

    public void redTowerDead()
    {
        ChangeState(State.Idle, RemoteAni.ANI_IDLE);
        print("타워 파괴");

        rcvChaseStart = "";
        rcvChaseTarget = "";
        //redTowerPos = null;
    }

    public void AttackCalculate()
    {
        int attackPower = 15;
        Debug.Log("AttackCalculate to rcvChaseTarget value :" + rcvChaseTarget);
        if (rcvChaseTarget.Equals(""))
        {
            Debug.Log("rcvChaseTarget null PointException");
            return;
        }

        if(rcvChaseTarget.Equals( "remotePlayer" ) )
        {
            
            playerParams.SetEnemyAttack( attackPower );
            print( "Attack" + player.name );
        }
       else if (rcvChaseTarget.Equals("Spider") )
        {
            
            curEnemyParams.SetEnemyAttack( attackPower );
            print( "Attack" + monster.name );
        }
        else if (rcvChaseTarget.Equals( "redTower" ) )
        {
            
            towerParams.SetEnemyAttack( attackPower );
            print( "Attack" + towerParams.name );
        }
    }

    public void ChangeState(State newState, int aniNumber)
    {
        if (currentState == newState)
        {
            return;
        }

        myAni.ChangeAni(aniNumber);
        currentState = newState;
    }

    void UpdateState()
    {
        switch (currentState)
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
    
    void IdleState()
    {

        // 로컬 플레이어 추적 시작 이벤트
        // 추적 시작거리에 타겟이 있으면 추적 시작
        if (skillBtn.receivedSkileOne == true)
        {
                basicAttack.attackFlag = false;
            RemoteTurnToDestination(player);
                //MoveToDestination( remotePlayer );
                network.MoveFlag = false;
                myAni.ChangeAni(RemoteAni.ANI_SKILL1);

            
        }

        if ( rcvChaseStart.Equals("TrackingStart") )
            {                  
                // 추적할 타겟 지정 몬스터(거미)
                if(rcvChaseTarget.Equals("remotePlayer"))
                {
                  if( playerParams.isDead == true )
                  {
                    return;
                  }
                fromTarget = player;
                ChangeState(State.Chase, RemoteAni.ANI_WALK);
                Debug.Log("리모트 플레이어가 상대 플레이어를 추적!");
                }
               else if (rcvChaseTarget.Equals( curEnemyParams.name ))
                {
                // 추적할 거미가 이미 죽어 있다면 함수 종료
                if (curEnemyParams.isDead == true)
                {
                    return;
                }

                fromTarget = monster;
                ChangeState(State.Chase, RemoteAni.ANI_WALK);
                Debug.Log("리모트 플레이어 거미 추적");
               }
                // 타겟지정 레드 타워
                else if (rcvChaseTarget.Equals("redTower"))
                {
                // 추적할 레드 타워가 이미 파괴 되었다면 함수 종료
                if (towerParams.isDead == true)
                {
                    return;
                }
                fromTarget = redTowerPos;
                ChangeState(State.Chase, RemoteAni.ANI_WALK);
                Debug.Log("리모트 플레이어 타워 추적: ");
                }
            
            Debug.Log("리모트 플레이어 추적할 타겟  : " + fromTarget);
            
            } // 추적 끝
    }


    void MoveState()
    {
        
    }



    void ChaseState()
    {

               
                // 공격대상이 리모트 플레이어의 공격 범위에 들어 온다면 정지 후 공격
            if ( RemoteGetDistanceFromPlayer( fromTarget ) <= attackDistance )
            {
                ChangeState( State.Attack, RemoteAni.ANI_ATTACK );
                Debug.Log("리모트 플레이어 공격 사거리 도달 공격 시작");
            }
               // 내 공격 사정 범위가 아니 라면 따라간다. 
            else
            {
                
                RemoteTurnToDestination( fromTarget );
                RemoteMoveToDestination( fromTarget );
                myAni.ChangeAni( RemoteAni.ANI_WALK );
                Debug.Log("타겟 위치로 방향 전환 및 이동");
                Debug.Log( "ChaseState MoveTo ChaseTarget value : " + chaseTarget );
                Debug.Log( "ChaseState MoveTo fromTarget value : " + fromTarget );
        }
    }

    void AttackState()
    {
        attackTimer = 0f;  
            
            // 공격시 공격 대상의 방향으로 급격하게 뱡향 전환
            remotePlayer.transform.LookAt( fromTarget.position );
            ChangeState( State.AttackWait, RemoteAni.ANI_ATKIDLE );
    }

    void AttackWaitState()
    {
        //if (playerParams.isDead == true)
        //{

        //    return;
        //}

        // 2초 간격으로 자동 공격 
        if ( attackTimer > attackDelay )
            {
                transform.LookAt(fromTarget.position);                // 플레이어 캐릭터를 타겟 방향으로 급격히 돌아 서게 한다.
                ChangeState(State.Attack, RemoteAni.ANI_ATTACK);      // 캐릭터 상태 > 어택으로 변경
            Debug.Log("AttackWaitState to if attackTiemr > attackDelay in");
            attackTimer = 0f;
            }
            attackTimer += Time.deltaTime;
        Debug.Log("AttackWaitState End");
    }

    void DeadState()
    {

    }

    //  타겟 방향으로 방향 변경
    void RemoteTurnToDestination(Transform TargetPlayer)
    {
        Quaternion lookRotation = Quaternion.LookRotation(TargetPlayer.position - remotePlayer.transform.position);

        transform.rotation = Quaternion.RotateTowards(remotePlayer.transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }
    //  타겟 위치로 이동 
    void RemoteMoveToDestination(Transform TargetPlayer)
    {
        transform.position = Vector3.MoveTowards(remotePlayer.transform.position, TargetPlayer.position, moveSpeed * Time.deltaTime);
    }
    //   타겟과 플레이어 간의 거리를 구함
    float RemoteGetDistanceFromPlayer(Transform TargetPlayer)
    {
        float distance = Vector3.Distance(remotePlayer.transform.position, TargetPlayer.position);

        return distance;
    }

    // 트리거 충돌 이벤트
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log(other.gameObject.tag);
        }
    }

    void Update()
    {

        UpdateState();

    }
}


    

   