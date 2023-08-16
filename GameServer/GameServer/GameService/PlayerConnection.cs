using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.Packets;
using ServerApplication.GameService.Base;

namespace ServerApplication.GameService
{
    public class PlayerConnection : TcpPeer
    {
        public TestServer ServerRef { get; private set; }
        public Player _Player { get; private set; }

        public PlayerConnection(TestServer serverRef, Socket peerSocket, Guid peerId,
            Owner ownerType, int connCounter) : base(serverRef, peerSocket, peerId, ownerType)
        {
            _Player = new Player(connCounter);
            ServerRef = serverRef;
        }
    }
}
