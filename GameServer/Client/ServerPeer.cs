using NetworkCore.NetworkCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ServerPeer : TcpPeer
    {
        public ServerPeer(NetworkBase networkBase, Socket peerSocket, Guid peerId, Owner ownerType) : base(networkBase, peerSocket, peerId, ownerType)
        {
        }
    }
}
