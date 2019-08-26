using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterParams : MonoBehaviour {

    /* CharacterParams는 플레이어 , 몬스터 파라미터 클래스의 부모 클래스 역할  */


    public int level { get; set; } // 현재 레벨
    public int maxHp { get; set; } // 최대 Hp
    public int curHp { get; set; } // 현재 Hp
    public int attackMin { get; set; } // 최소 공격력 
    public int attackMax { get; set; } // 최대 공격력 
    public int defense { get; set; }   // 방어력

    public bool isDead { get; set; }    // 캐릭터 사망 여부 

    [System.NonSerialized]
    public UnityEvent deadEvent = new UnityEvent();

    public PlayerParams player;
    public PlayerFSM2 playerFSM;

    public RemoteFSM remoteFSM;
    public PlayerParams remotePlayer;

    

    
    void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerParams>();
        playerFSM = GameObject.FindWithTag("RemotePlayer").GetComponent<PlayerFSM2>();

        remoteFSM = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteFSM>();
        remotePlayer = GameObject.FindWithTag("RemotePlayer").GetComponent<PlayerParams>();
        InitParams();
	}

    // CharacterParams를 상속한 자식 클래스에서 자신만의 명어를 해당 메소드에 추가 하면 자동으로 필요한 명어들이 실행된다.
    public virtual void InitParams()
    { 
     

    }

    public int GetRandomAttack()
    {
        int randAttack = Random.Range( attackMin, attackMax + 1);

        return randAttack;
    }
    public void SetEnemyAttack(int enemyAttackPower)
    {
        curHp -= enemyAttackPower;
        UpdateAfterReceiveAttack();
    }
        public void remoteSetEnemyAttack( int enemyAttackPower )
    {
        remotePlayer.curHp -= enemyAttackPower;
        remotePlayer.UpdateAfterReceiveAttack();


    }

    // 캐릭터가 적으로부터 공격을 받은뒤 자동으로 실행 되는 함수
    protected virtual void UpdateAfterReceiveAttack()
    { 
        
        print( name + "'s Hp: " + curHp);

        if ( curHp <= 0 )
        {
            curHp = 0;
            isDead = true;
            deadEvent.Invoke();
            Debug.Log("palyer is Dead = " + isDead);
            playerFSM.rcvChaseStart = "";
            playerFSM.rcvChaseTarget = "";
            remoteFSM.rcvChaseStart = "";
            remoteFSM.rcvChaseTarget = "";
        }

        //else if (remotePlayer.curHp <= 0)
        //{
        //    remotePlayer.curHp = 0;
        //    remotePlayer.isDead = true;
        //    remotePlayer.deadEvent.Invoke();
        //    Debug.Log("remotePlayer is Dead = " + isDead);
        //}
    }
	
    
}
