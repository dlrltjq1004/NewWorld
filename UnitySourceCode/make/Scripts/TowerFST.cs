using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFST : MonoBehaviour {


    TowerParams myParams;
    Transform redTowerTransform;
    Quaternion redTowerRotation = Quaternion.identity;
    public GameObject  redTower;

    Transform player;
    PlayerParams playerParams;
    public ParticleSystem DestroyEffect;
    public AudioSource DestroyAudio;

    PlayerFSM2 playerFSM2;
    RemoteFSM remoteFSM;
   public GameObject DistroyRedTower;
	// Use this for initialization
	void Start () {

        myParams = GetComponent<TowerParams>();      
        myParams.deadEvent.AddListener(CallDeadEvent );
       

        player = GameObject.FindWithTag("Player").transform;
        playerParams = player.gameObject.GetComponent<PlayerParams>();

        
        remoteFSM = GameObject.FindWithTag("RemotePlayer").GetComponent<RemoteFSM>();

        redTower = GameObject.FindWithTag("RedTower").GetComponent<GameObject>();
        DistroyRedTower = GameObject.FindWithTag("RedTowerDistroy");

        
    }
	
    public void ShowHitEffect()
    {
        DestroyEffect.transform.parent = null;

        DestroyEffect.Play();
        DestroyAudio.Play();

        Destroy(DestroyEffect.gameObject, DestroyEffect.duration);
        Destroy(gameObject);
    }

    void CallDeadEvent()
    {   

        Debug.Log("포탑이 파괴 되었습니다.");
       
        //DistroyRedTower = Resources.Load("huo_ju_002") as GameObject;
        //Vector3 pos = redTowerTransform.transform.position;
        //redTowerRotation.eulerAngles = new Vector3(0, 100, 0);
        //DistroyRedTower.transform.rotation = redTowerRotation;

        //Instantiate(DistroyRedTower, pos, DistroyRedTower.transform.rotation).name = "redTowerDistroy";
        ShowHitEffect();
        //redTower.SetActive(false);
        
        DistroyRedTower.SetActive(true);
        if(playerFSM2 == null)
        {
            playerFSM2 = GameObject.FindWithTag("Player").GetComponent<PlayerFSM2>();
        }
        playerFSM2.gameObject.SendMessage("redTowerDead");
        remoteFSM.gameObject.SendMessage("redTowerDead");
        // 타워 파괴 애니메이션을 발동 
    }

	// Update is called once per frame
	void Update () {
		
	}
}
