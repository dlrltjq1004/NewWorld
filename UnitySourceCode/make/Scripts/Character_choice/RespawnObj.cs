using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObj : MonoBehaviour {

    List<Transform> spawnPos = new List<Transform>();
    GameObject[] monsters;

    public GameObject monPrefab;
    public int spawnNumber = 1;
    public float respawnDelay = 3;

    int deadMonsters = 0;


	// Use this for initialization
	void Start () {

        MakeSpawnPos();

    }
	
    // Pos 위치에 스폰 시키기
    void MakeSpawnPos()
    {
        // 자신의 모든 자식 오브젝트를 찾음
        foreach( Transform pos in transform)
        {
            // 자식 오브젝트의 태그가 리스폰 일경우 
            // 리스트에 담는다.
            if( pos.tag == "Respawn" )
            {
                spawnPos.Add( pos );
            }
        }
        // 만약에 스폰 지역은 한곳인데 몬스터를  2개 이상 만들라고 한다면 에러가 날 것이기 때문에
        // 스폰 지역과 몬스터의 숫자를 맞춰  준다.
        // 만약에 스폰 넘버에서 지정한 숫자가  리스트에 들어있는 오브젝트의 숫자보다 클경우 
        // 스폰 넘버를 스폰 카운트 와 같도록 한다.
        if( spawnNumber > spawnPos.Count )
        {
            spawnNumber = spawnPos.Count;

            monsters = new GameObject[spawnNumber];

            MakeMonsters();
        }
    }

    void MakeMonsters()
    {
        for ( int i = 0; i < spawnNumber; i++ )
        {
            GameObject mon = Instantiate(monPrefab, spawnPos[i].position, Quaternion.identity) as GameObject;
            mon.SetActive(false);
            monsters[i] = mon;
        }
    }

    void SpawnMonster()
    {
        for ( int i = 0; i < monsters.Length; i++ )
        {
            monsters[i].SetActive( true );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.gameObject.tag == "Player" )
        {
            SpawnMonster();
            GetComponent<SphereCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
