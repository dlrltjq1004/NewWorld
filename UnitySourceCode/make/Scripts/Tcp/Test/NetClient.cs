using System;

using Unity.Network;

using UnityEngine;



public class NetClient

{

    private const string UserName = "UnityClient";



    private LocalClient _client;



    public event Chat OnChat;



    public NetClient()

    {



    }



    public void Start()

    {

        _client = new LocalClient();

        _client.OnConnected += Connected;

        _client.Start();

    }



    public void Close()

    {

        _client.Close();

    }



    private void Connected(LocalClient client)

    {

        Connect connect = new Connect()

        {

            Type = PacketType.Connect,

            UserName = UserName,

        };



        _client.UserName = UserName;

        _client.SendPacket(connect);

    }



    private void ConnectError(Exception ex)

    {

        Debug.Log("접속 에러\n" + ex.ToString());

    }



    private void ReceiveObject(LocalClient client, Packet packet)

    {

        if (packet == null)

            return;



        switch (packet.Type)

        {

            case PacketType.Chat: OnChatMessage(client, packet); break;

        }

    }



    private void OnChatMessage(LocalClient client, Packet packet)

    {

        ChatMessage message = packet as ChatMessage;

        if (message == null)

            return;



        if (OnChat != null)

            OnChat(message.Message);

    }



    private void SendPacket(LocalClient client, Packet packet)

    {

        client.SendPacket(packet);

    }



    public delegate void Chat(string message);

}