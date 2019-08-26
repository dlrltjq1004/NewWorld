using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerParams : CharacterParams {


    /** 프로퍼티 
     get { return money; } 
     set { set if( value < 0)
        {
           money = 0;
        }  
        if( value > 9999999 )
    {
        money = 9999999
    } else
        {
           money = value;
        }

    **/


    public string name { get; set; }

	public int curExp { get; set; }
    public int exp { get; set; }
    public int expToNextLevel { get; set; }
    public int money { get; set; }
    public int rewardMoney { get; set; }


    public Image hpBar;
    
    /** 오버라이드
     * **/
    public override void InitParams()
    {
        name = PlayerPrefs.GetString("myId");
        level = 1;
        maxHp = 50;
        curHp = maxHp;
        attackMin = 10;
        attackMax = 15;
        defense = 1;

        exp = 100;
        curExp = 0;
        rewardMoney = 300;
        expToNextLevel = 100 * level;
        money = 0;
        isDead = false;

        InitHpBarSize();
    }

    void InitHpBarSize()
    {
        hpBar.rectTransform.localScale = new Vector3( 3f,5f ,5f );
    }

    protected override void UpdateAfterReceiveAttack()
    {
            
        if (curHp == 0)
        {
            hpBar.rectTransform.localScale = new Vector3(0f, 1f, 1f);
            Destroy(hpBar.gameObject);
        }
        Debug.Log("hpBar" + hpBar.rectTransform.localScale.x);
        base.UpdateAfterReceiveAttack();

        hpBar.rectTransform.localScale -= new Vector3(0.5f, 0f, 0f);

        
        //hpBar.rectTransform.localScale = new Vector3( (float)curHp / (float)maxHp, 1f,1f);
        //Debug.Log(hpBar.rectTransform.localScale = new Vector3((float)curHp / (float)maxHp, 1f, 1f));
    }
}
