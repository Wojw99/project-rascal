using NetworkCore.NetworkCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.NetClient
{
    public class ServerConnection : TcpPeer
    {
        public ClientNetwork NetworkRef { get; private set; }

        public ServerConnection(ClientNetwork serverRef, Socket peerSocket, Guid peerId,
            Owner ownerType, int connCounter) : base(serverRef, peerSocket, peerId, ownerType)
        {
            NetworkRef = serverRef;
        }

    }
}
