using System;

namespace Unity.Network
{
    public enum PacketType
    {
        None, Connect, Reconnect, Disconnect, Chat,
    }

    [Serializable]
    public class Packet
    {
        public PacketType Type;
    }

    [Serializable]
    public class Connect : Packet
    {
        public string UserName;
    }

    [Serializable]
    public class ChatMessage : Packet
    {
        public string Message;
    }
}