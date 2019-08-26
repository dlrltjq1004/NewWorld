using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;


public class PlayerInfo : MonoBehaviour {

    public int Id;
    public string Name;
    public double Gold;

    public PlayerInfo(int id, string name , double gold)
    {
        this.Id = id;
        this.Name = name;
        this.Gold = gold;
    }

}

public class JsonTest : MonoBehaviour
{
    public List<PlayerInfo> PlayerInfoList = new List<PlayerInfo>();

    private void Start()
    {
        SavePlayerInfo();
    }
    public void SavePlayerInfo()
    {
        Debug.Log("SvePlayerInfo()");

    }
}
