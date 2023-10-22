using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetClient;
using UnityEngine;
using NetworkCore.NetworkMessage;
using Assets.Code.Scripts.NetClient.Emissary;
using NetworkCore.Packets;

namespace Assets.Code.Scripts.NetClient.Base
{
    public class ServerPeer : TcpPeer
    {
        private IPEndPoint ServerIpEndpoint;

        public ServerPeer(PacketHandler handlerRef, string serverIpAddress, int serverTcpPort)
            : base(handlerRef, new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
                  Guid.NewGuid(), Owner.client)
        {
            ServerIpEndpoint = new IPEndPoint(IPAddress.Parse(serverIpAddress), serverTcpPort);
        }

        public void ConnectToServer()
        {
            Debug.Log("Trying to connect to server...");
            PeerSocket.Connect(ServerIpEndpoint);

            if(PeerSocket.Connected)
            {
                Debug.Log($"Succesfully connected with server, on port = " +
                    $"{ServerIpEndpoint.Port}, adress = {ServerIpEndpoint.Address}." );
            }
        }
    }
}
