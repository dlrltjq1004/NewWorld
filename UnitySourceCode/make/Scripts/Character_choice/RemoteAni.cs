using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteAni : MonoBehaviour {

    public const int ANI_IDLE = 0;
    public const int ANI_WALK = 1;
    public const int ANI_ATTACK = 2;
    public const int ANI_ATKIDLE = 3;
    public const int ANI_DIE = 4;
    public const int ANI_SKILL1 = 5;
    public const int ANI_RUN = 6;
    public Animator anim;

    void Start()
    {

        anim = GameObject.FindWithTag("RemotePlayer").GetComponent<Animator>();

    }

    public void ChangeAni(int aniNumber)

    {
        if(anim == null)
        {
            GameObject.FindWithTag("RemotePlayer").GetComponent<Animator>();
        }
        anim.SetInteger("aniName", aniNumber);
    }

    void Update()
    {

    }
}
