using System;

using System.Collections.Generic;

using System.Net.Sockets;

using System.Threading;

using Unity.Network;



namespace CsConnector

{

    public class NetMultiServer
    {

        private readonly TcpListener _listener;



        private readonly Dictionary<string, LocalClient> _clientTable;

        private readonly Thread _listenerThread;



        public NetMultiServer()

        {

            _clientTable = new Dictionary<string, LocalClient>();



            _listener = new TcpListener(System.Net.IPAddress.Any, LocalClient.PortNumber);

            _listenerThread = new Thread(new ThreadStart(DoListen));

        }



        public void Start()

        {

            _listenerThread.Start();

        }



        public void Close()

        {

            _listener.Stop();

        }



        private void DoListen()

        {

            try

            {

                _listener.Start();

                do

                {

                    TcpClient tcpClient = _listener.AcceptTcpClient();

                    LocalClient localClient = new LocalClient(tcpClient);

                    localClient.OnReceiveObject += ReceiveObject;

                    localClient.Start();

                }

                while (true);

            }
        
            catch (Exception)
        
            {

            }//try

        }



        private void ReceiveObject(LocalClient client, Packet packet)

        {

            if (packet == null)

                return;


       
            switch (packet.Type)

            {

                case PacketType.Connect: OnConnected(client, packet); break;

            }

        }



        private void OnConnected(LocalClient client, Packet packet)

        {

            Connect connect = packet as Connect;

            if (connect == null)

                return;





            client.UserName = connect.UserName;

            _clientTable.Add(client.UserName, client);

        }



        private void SendPacket(LocalClient client, Packet packet)

        {

            client.SendPacket(packet);

        }

    }

}