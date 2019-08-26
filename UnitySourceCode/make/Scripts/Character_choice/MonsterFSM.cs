using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : MonoBehaviour {

    /**   */

    public enum State
    {
        Idle,
        Chase,
        Attack,
        Dead,
        NoState
    }

    public State currentState = State.Idle;

    MonsterAni myAni;
    MonsterParams myParams;

    Transform player;
    PlayerParams playerParams;

    Transform remotePlayer;
    RemoteParams remoteParams;

    string chasePlayer = "";
    public string monsterChasePlayer = "";
    float chaseDistance = 7f;    // 추적시작 거리 
    float attackDistance = 2.5f; // 공격 범위 
    float reChaseDistance = 4f;  // 재 추적 거리

    float rotAnglePerSecond = 360f; // 회전 반경
    float moveSpeed = 3f;           // 이동속도

    float attackDelay = 2f;         // 공격속도
    float attackTimer = 0f; 

	void Start () {

        myAni = GetComponent<MonsterAni>();
        myParams = GetComponent<MonsterParams>();
        myParams.deadEvent.AddListener( CallDeadEvent );

        ChangeState( State.Idle, MonsterAni.IDLE );

        player = GameObject.FindWithTag("Player").transform;
        playerParams = player.gameObject.GetComponent<PlayerParams>();

        remotePlayer = GameObject.FindWithTag("RemotePlayer").transform;
        remoteParams = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteParams>();
    }

    public void AttackCalculate()
    {
        playerParams.SetEnemyAttack(  myParams.GetRandomAttack() );
    }

    void CallDeadEvent()
    {
        ChangeState( State.Dead, MonsterAni.DIE );
        player.gameObject.SendMessage("CurrentEnemyDead");
        remotePlayer.gameObject.SendMessage("CurrentEnemyDead");
    }

    void UpdateState()
    {
        switch( currentState )
        {
            case State.Idle:
                IdleState();
                break;
            case State.Chase:
                ChaseState();
                break;
            case State.Attack:
                AttackState();
                break;
            case State.Dead:
                DeadState();
                break;
            case State.NoState:
                break;
        }
    }

    public void ChangeState ( State newState, string aniName )
    {
        if( currentState == newState )
            return;

        currentState = newState;
        myAni.ChangAni( aniName );
        
    }

    void IdleState()
    {  
       
        if( player == null )
        {
            player = GameObject.FindWithTag("Player").transform;
        }

        if( GetDistanceFromPlayer(player) < chaseDistance )
        {
            ChangeState( State.Chase, MonsterAni.WALK );
            chasePlayer = "player";
        } 
          
    }

    void ChaseState()
    {  
        if( GetDistanceFromPlayer(player) < attackDistance )
        {
            ChangeState( State.Attack, MonsterAni.ATTACK );            
        } else
        {            
                TurnToDestination(player);
                MoveToDestination(player);
        }
    }

    void AttackState()
    {
        if (player.GetComponent<PlayerFSM2>().currentState == PlayerFSM2.State.Dead)
            ChangeState( State.NoState, MonsterAni.IDLE );

        if (chasePlayer.Equals("player"))
        {
            if ( GetDistanceFromPlayer(player) > reChaseDistance)
            {
                attackTimer = 0f;
                ChangeState( State.Chase, MonsterAni.WALK );

            } else
            {
                if ( attackTimer > attackDelay )
                {
                    transform.LookAt(player.position);
                    myAni.ChangAni(MonsterAni.ATTACK);
                
                attackTimer = 0f;

                }
            attackTimer += Time.deltaTime;
        }

        }
      
    }
    void DeadState()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    void NoState()
    {

    }

    // 몬스터 타겟 방향으로 방향 변경
    void TurnToDestination( Transform TargetPlayer )
    {
        Quaternion lookRotation = Quaternion.LookRotation( TargetPlayer.position - transform.position);

        transform.rotation = Quaternion.RotateTowards( transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond );
    }
    // 몬스터 타겟 위치로 이동 
    void MoveToDestination( Transform TargetPlayer )
    {
        transform.position = Vector3.MoveTowards( transform.position, TargetPlayer.position, moveSpeed * Time.deltaTime );
    }

    float GetDistanceFromPlayer( Transform TargetPlayer )
    {
        float distance = Vector3.Distance( transform.position, TargetPlayer.position);

        return distance;
    }
	
	void Update () {

        UpdateState();
	}
}
