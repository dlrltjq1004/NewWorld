using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAni : MonoBehaviour {

    public const string IDLE = "idle";
    public const string WALK = "walk";
    public const string ATTACK = "attack1_new";
    public const string DIE = "death1";

    Animation anim;

	// Use this for initialization
	void Start () {

        anim = GetComponentInChildren<Animation>();
	}

    public void ChangAni ( string aniName )
    {
        anim.CrossFade( aniName );
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
