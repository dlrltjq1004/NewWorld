using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientHandleNetworkData : MonoBehaviour {

    private delegate void Packet_(byte[] data);
    private static Dictionary<int, Packet_> Packets;

    public static void InitializeNetorkPackages()
    {
        Debug.Log("Initialize Network Packages");
        Packets = new Dictionary<int, Packet_>
            {
                {(int)ServerPackets.SconnectionOK, HandleConnectionOK}
            };

    }

    public void Awake()
    {
        InitializeNetorkPackages();

    }

    public static void HandleNetworkInformation(byte[] data)
    {
        int packetnum; PacketBuffer buffer = new PacketBuffer();
        Packet_ Packet; 
        buffer.WriteBytes(data);
        packetnum = buffer.ReadInteger();
        buffer.Dispose();
        if (Packets.TryGetValue(packetnum, out Packet))
        {
            Packet.Invoke(data);
        }
    }

    private static void HandleConnectionOK(byte[] data)
    {
        PacketBuffer buffer = new PacketBuffer();
        buffer.WriteBytes(data);
        buffer.ReadInteger();
        string msg = buffer.ReadString();
        buffer.Dispose();

        //add your code you want to execute here:
        Debug.Log(msg);

        
        ClientTCP.ThankYouServer();
    }
}
