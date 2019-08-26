using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Assets.make.Scripts
{
    class Client : MonoBehaviour
    {
        private Socket _clientsSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private byte[] _recieveBuffer = new byte[8142];

        public void SetupServer()
        {
            try
            {
                _clientsSocket.Connect(new IPEndPoint(IPAddress.Parse("13.125.250.235"), 8888));

            } catch(SocketException ex)
            {
                Debug.Log(ex.Message);
            }
            _clientsSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);

        }

        public void ReceiveCallback(IAsyncResult AR)
        {
            // 얼마나 많은 바이트가 수신되는지 확인하고 EndRecieve를 호출하여 핸드 셰이크를 마무리합니다.
            int recieved = _clientsSocket.EndReceive(AR);

            if (recieved <= 0)            
                return;

            // 수신 한 데이터를 새로운 버퍼에 복사하여 null 바이트를 방지합니다.
            byte[] recData = new byte[recieved];
            Buffer.BlockCopy(_recieveBuffer,0,recData,0,recieved);
            // 여기서 원하는대로 데이터를 처리하면 모든 바이트가 recData에 저장됩니다.

            // 다시 수신을 시작합니다.
            _clientsSocket.BeginReceive(_recieveBuffer,0,_recieveBuffer.Length,SocketFlags.None,new AsyncCallback(ReceiveCallback),null);
        }

        public void SendData(byte[] data)
        {
            SocketAsyncEventArgs socketAsyncData = new SocketAsyncEventArgs();
            socketAsyncData.SetBuffer(data,0,data.Length);
            _clientsSocket.SendAsync(socketAsyncData);
        }

        void Start()
        {   string toSend = "Hello World";
            int toSendLen = System.Text.Encoding.ASCII.GetByteCount(toSend);
            byte[] toSendBytes = System.Text.Encoding.ASCII.GetBytes(toSend);
            byte[] toSendLenBytes = System.BitConverter.GetBytes(toSendLen);
            SendData(toSendBytes);
        }
    }
}
