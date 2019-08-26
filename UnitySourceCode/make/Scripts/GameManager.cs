using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    /*
      타워가 파괴 되었다는 것을 알기 위해서는  TowerParams 클래스의 isDead 변수 를 참조해야 한다.

    */

       
    TowerParams towerParams;  
    CharacterCreate characterCreate;
    public bool GameEnd = false;
    public bool success = true;
    public GameObject winUI;
    public GameObject loseUI;

 
    
    void Start () {

        towerParams = GameObject.Find("109").GetComponent<TowerParams>();
        characterCreate = GetComponent<CharacterCreate>();
    }

    


    void Update () {

        if (Input.GetKey(KeyCode.Escape))
        {
            //Application.Quit();
            
        }

        if (GameEnd == true)
        {
            
            return;
        }

        if (towerParams.isDead == true )
        {
            if(characterCreate.myTeam.Equals("1") && success == true)
            {
                winUI.SetActive(true);
                GameEnd = true;
                StartCoroutine(ApplicationQuit());
                Debug.Log("승리!");
                success = false;
            } else
            {
              
                    loseUI.SetActive(true);
                    GameEnd = true;
                    StartCoroutine(ApplicationQuit());
                    Debug.Log("패배!");
                   
                
               

            }
        }
	}

    IEnumerator ApplicationQuit()
    {
        yield return new WaitForSeconds(3f);
        //AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        //activity.Call("moveActivity");
        Application.Quit();

            }

    private void OnApplicationQuit()
    {
        
    }

   
}
