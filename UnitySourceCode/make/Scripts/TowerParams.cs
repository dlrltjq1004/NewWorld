using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class TowerParams : MonoBehaviour {

    public string name;

    public int maxHp;
    public int curHp;
    public int attackMax;
    

    public bool isDead;
   
    public int rewardMoney { get; set; }

    public Image hpBar;

    [System.NonSerialized]
    public UnityEvent deadEvent = new UnityEvent();

    private void Start()
    {
        InitParams();
    }

    public  void InitParams()
    {
        name = "Tower";
       
        maxHp = 50;
        curHp = maxHp;
        
        attackMax = 200;
                
        rewardMoney = 150;

        isDead = false;

        InitHpBarSize();
    }

    void InitHpBarSize()
    {
        hpBar.rectTransform.localScale = new Vector3(5f, 6f, 6f);
    }
    public int GetRandomAttack()
    {
        int randAttack = 200;

        return randAttack;
    }

    public void SetEnemyAttack(int enemyAttackPower)
    {
        curHp -= enemyAttackPower;
        UpdateAfterReceiveAttack();
    }

    // 캐릭터가 적으로부터 공격을 받은뒤 자동으로 실행 되는 함수
    protected virtual void UpdateAfterReceiveAttack()
    {

        print(name + "'s Hp: " + curHp);


        //hpBar.rectTransform.localScale -= new Vector3(1f,0f);
        hpBar.rectTransform.localScale -= new Vector3(1f, 0f, 0f);
        Debug.Log("hpBar" + hpBar.rectTransform.localScale.x);
        //(float)curHp / (float)maxHp


        if (curHp <= 0)
        {
            curHp = 0;
            isDead = true;
            deadEvent.Invoke();
        }
    }
}
