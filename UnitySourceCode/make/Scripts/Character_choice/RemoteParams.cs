using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RemoteParams : CharacterParams {

    public string name { get; set; }

    public int curExp { get; set; }
    public int exp { get; set; }
    public int expToNextLevel { get; set; }
    public int money { get; set; }
    public int rewardMoney { get; set; }


    public Image hpBar;
    public MOBAEnergyBar energyBar;

    public override void InitParams()
    {
        name = PlayerPrefs.GetString("remoteId");
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
        hpBar.rectTransform.localScale = new Vector3(3f, 4f ,4f);
    }

   public void EnergyBar(int attackPower)
    {
        energyBar = GameObject.Find("HP").GetComponent<MOBAEnergyBar>();
        energyBar.Value -= 100;
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

       
        //hpBar.rectTransform.localScale = new Vector3( (float)curHp / (float)maxHp, 1f, 1f );
        //Debug.Log(hpBar.rectTransform.localScale = new Vector3((float)curHp / (float)maxHp, 1f, 1f));

    }
}
