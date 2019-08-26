using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//get send from server to client;
//client has vto listen to serverpackets
public enum ServerPackets
{
    SconnectionOK = 1,
}

//get send from client to server
// server has to listen to clientpackets
public enum ClientPackets
{
    CThankYou = 1,
}
