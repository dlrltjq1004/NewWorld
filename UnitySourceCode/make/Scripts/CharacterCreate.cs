

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreate : MonoBehaviour
{

    public string myCharacterName, myTeam, youerCharacter, myId;
    private Quaternion caramaQuaternion = Quaternion.identity;
    private Quaternion characterQuaternion = Quaternion.identity;
    private Quaternion remoteQuaternion = Quaternion.identity;
    private Quaternion hpBarQuaternion = Quaternion.identity;
    public Transform hpBar;
    public Transform remoteHpBar;

    public GameObject myTaiLungSpell;
    
    public GameObject myMantisSpell;

    private Network network;

    private void Awake()
    {
        myTeam = PlayerPrefs.GetString("myTeam");
        //myBase = "2";
        myCharacterName = PlayerPrefs.GetString("myCharacter");
        youerCharacter = PlayerPrefs.GetString("yuerCharacter");
        //network = GameObject.Find("NetworkManager").GetComponent<Network>();
        myId = PlayerPrefs.GetString("myId");

        //myTaiLungSpell1 = GameObject.Find("TaiLungSpell1");
        //myTaiLungSpell2 = GameObject.Find("TaiLungSpell2");
        //myTaiLungSpell3 = GameObject.Find("TaiLungSpell3");

        //myMantisSpell1 = GameObject.Find("MantisSpell1");
        //myMantisSpell2 = GameObject.Find("MantisSpell2");
        //myMantisSpell3 = GameObject.Find("MantisSpell3");

        Debug.Log("내가 선택한 캐릭터:" + myCharacterName);
        Debug.Log("내 진영:" + myTeam);
        Debug.Log("상대 캐릭" + youerCharacter);

        if (myTeam.Equals("1"))
        {
            // 내가 선택한 캐릭터 생성
            GameObject myCharacter = Resources.Load(myCharacterName) as GameObject;
            Vector3 pos = new Vector3(45.02f, 60.3f, 99.59f);
            characterQuaternion.eulerAngles = new Vector3(0, 100, 0);
            myCharacter.transform.rotation = characterQuaternion;
            Instantiate(myCharacter, pos, myCharacter.transform.rotation).name = myId;
            
            if (myCharacterName.Equals("TaiLung"))
            {
                myTaiLungSpell.SetActive(true);

            }
            else if (myCharacterName.Equals("Mantis"))
            {
                myMantisSpell.SetActive(true);

            }

            // 상대방이 선택한 캐릭터 생성
            if (myCharacterName.Equals("TaiLung"))
            {
                GameObject YouerCharacter = Resources.Load("RemoteTaiLung") as GameObject;
                Vector3 remotePos = new Vector3(254.85f, 60.3f, 100.41f);
                remoteQuaternion.eulerAngles = new Vector3(0, -110, 0);
                YouerCharacter.transform.rotation = remoteQuaternion;
                Instantiate(YouerCharacter, remotePos, YouerCharacter.transform.rotation).tag = "RemotePlayer";
                GameObject.FindWithTag("RemotePlayer").name = "RemotePlayer";
                //remoteHpBar = GameObject.Find("RemotePlayerHpBarPos").GetComponent<Transform>();              
                //hpBarQuaternion.eulerAngles = new Vector3(-154, -49, -48);
                
                //remoteHpBar.rotation = hpBarQuaternion;
                //Debug.Log("리모트 플레이어 체력바 로그: " + remoteHpBar.name);
            }
            else if (myCharacterName.Equals("Mantis"))
            {
                GameObject YouerCharacter = Resources.Load("RemoteMantis") as GameObject;
                Vector3 remotePos = new Vector3(254.85f, 60.3f, 100.41f);
                remoteQuaternion.eulerAngles = new Vector3(0, -110, 0);
                YouerCharacter.transform.rotation = remoteQuaternion;
                Instantiate(YouerCharacter, remotePos, YouerCharacter.transform.rotation).tag = "RemotePlayer";
                GameObject.FindWithTag("RemotePlayer").name = "RemotePlayer";
            }



            //내 플레이어 카메라 생성
            GameObject camara = Resources.Load("PlayCamara2") as GameObject;
            caramaQuaternion.eulerAngles = new Vector3(0, 100, 0);
            camara.transform.rotation = caramaQuaternion;
            Instantiate(camara, new Vector3(45f, 60f, 100f), camara.transform.rotation);
            Debug.Log("내 진영 블루" + myTeam);
        }
        else if (myTeam.Equals("2"))
        {
            GameObject myCharacter = Resources.Load(myCharacterName) as GameObject;
            Vector3 pos = new Vector3(254.85f, 60.3f, 100.41f);
            characterQuaternion.eulerAngles = new Vector3(0, -110, 0);
            myCharacter.transform.rotation = characterQuaternion;
            Instantiate(myCharacter, pos, myCharacter.transform.rotation).name = myId;
            
            //hpBar = GameObject.Find("PlayerHpBarPos").transform;            
            //hpBarQuaternion.eulerAngles = new Vector3(-154, -150, -48);            
            //hpBar.transform.rotation = hpBarQuaternion;
            //Debug.Log("플레이어 체력바 이름 로그 : " + hpBar.name );

            if (myCharacterName.Equals("TaiLung"))
            {
                myTaiLungSpell.SetActive(true);

            }
            else if (myCharacterName.Equals("Mantis"))
            {
                myMantisSpell.SetActive(true);

            }
            
            // 상대방이 선택한 캐릭터 생성
            if (myCharacterName.Equals("TaiLung"))
            {
                GameObject YouerCharacter = Resources.Load("RemoteTaiLung") as GameObject;
                Vector3 remotePos = new Vector3(45.02f, 60.3f, 99.59f);
                remoteQuaternion.eulerAngles = new Vector3(0, 100, 0);
                YouerCharacter.transform.rotation = remoteQuaternion;
                Instantiate(YouerCharacter, remotePos, YouerCharacter.transform.rotation).tag = "RemotePlayer";
                GameObject.Find("RemotePlayerHpBarPos").name = "RemoteTaiLung";
                //remoteHpBar = GameObject.FindWithTag("RemotePlayer").GetComponent<Transform>();
                //hpBarQuaternion.eulerAngles = new Vector3(-90, 0, -325);
             
                //remoteHpBar.rotation = hpBarQuaternion;
            }
            else if (myCharacterName.Equals("Mantis"))
            {
                GameObject YouerCharacter = Resources.Load("RemoteMantis") as GameObject;
                Vector3 remotePos = new Vector3(45.02f, 60.3f, 99.59f);
                remoteQuaternion.eulerAngles = new Vector3(0, 100, 0);
                YouerCharacter.transform.rotation = remoteQuaternion;
                Instantiate(YouerCharacter, remotePos, YouerCharacter.transform.rotation).tag = "RemotePlayer";
                GameObject.FindWithTag("RemotePlayer").name = "RemoteMantis";
            }

            //GameObject YouerCharacter = Resources.Load("RemoteTaiLung") as GameObject;
            //Vector3 remotePos = new Vector3(45.02f, 60.3f, 99.59f);
            //remoteQuaternion.eulerAngles = new Vector3(0, 100, 0);
            //YouerCharacter.transform.rotation = remoteQuaternion;
            //Instantiate(YouerCharacter, remotePos, YouerCharacter.transform.rotation).tag = "RemotePlayer";
            //GameObject.FindWithTag("RemotePlayer").name = "RemoteTaiLung";



            //내 플레이어 카메라 생성
            GameObject camara = Resources.Load("PlayCamara2") as GameObject;
            caramaQuaternion.eulerAngles = new Vector3(0, 92.65f, 0);
            camara.transform.rotation = caramaQuaternion;
            Instantiate(camara, new Vector3(256.2f, 63.7f, 93.7f), camara.transform.rotation);


        }
    }



}
