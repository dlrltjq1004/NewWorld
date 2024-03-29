﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterParams : CharacterParams {

    public string name;

    public int exp { get; set; }
    public int rewardMoney { get; set; }

    public Image hpBar;

    public override void InitParams()
    {
        name = "Spider";
        level = 1;
        maxHp = 50;
        curHp = maxHp;
        attackMin = 5;
        attackMax = 5;
        defense = 1;

        exp = 10;
        rewardMoney = 50;

        isDead = false;

        InitHpBarSize();
    }

    void InitHpBarSize()
    {
        hpBar.rectTransform.localScale = new Vector3( 1f, 1f, 1f );
    }

    protected override void UpdateAfterReceiveAttack()
    {
        base.UpdateAfterReceiveAttack();

        if (curHp == 0)
        {
            hpBar.rectTransform.localScale = new Vector3(0f, 1f, 1f);
            Destroy(hpBar.gameObject);
        }

        hpBar.rectTransform.localScale = new Vector3((float)curHp / (float)maxHp, 1f, 1f );
    }
}
