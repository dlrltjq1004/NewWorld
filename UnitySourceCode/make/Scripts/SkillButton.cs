using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Werewolf.SpellIndicators.Demo
{
    public class SkillButton : MonoBehaviour
    {


        public bool skill1Start = false;
        public GameObject skill1_end;
        public SplatManager Splats;
        public PlayerAni myAni;
        public PlayerFSM2 playerFsm;
        public JoyStick joy;
        public Animator playerAnimator;
        private Network network;
        private float Timer;
        public bool receivedSkileOne = false;

        // Use this for initialization
        void Start()
        {

            //Splats = GameObject.Find("SplatManager").GetComponent<SplatManager>();
            myAni = GameObject.FindWithTag("Player").GetComponent<PlayerAni>();
            playerFsm = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
            playerAnimator = GameObject.FindWithTag("Player").GetComponent<Animator>();

            joy = GameObject.Find("JoystickBackGround").GetComponent<JoyStick>();
            network = GameObject.Find("NetworkManager").GetComponent<Network>();

            Timer = 0f;
        }

        IEnumerator moveTimer()
        {
            yield return new WaitForSeconds( 4f );
            joy.moveSpeed = 6f;
            //network.remoteMoveSpeed = 6f;
            //myAni.ChangeAni(PlayerAni.ANI_WALK);
            skill1Start = false;
            //myAni.ChangeAni(PlayerAni.ANI_SKILL1);
            network.Send(network.myId + "," + "SkillStop");
        }

        public void OnClickExit()
        {
            network.Send(network.myId + "," + "SkillOne");
            //if(receivedSkileOne = true)
            //{

            //}
            skill1Start = true;
            joy.moveSpeed = 8f;
            //network.remoteMoveSpeed = 8f;
            Debug.Log("스킬버튼 클릭 리모트 스피드 벨류 "+network.remoteMoveSpeed);
            StartCoroutine( moveTimer() );
           
          
        }
    }
}

