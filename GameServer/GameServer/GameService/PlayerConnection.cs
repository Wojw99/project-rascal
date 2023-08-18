using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkData;
using NetworkCore.Packets;

namespace ServerApplication.GameService
{
    public class PlayerConnection : TcpPeer
    {
        public TestServer ServerRef { get; private set; }
        public Player _Player { get; private set; }

        public PlayerConnection(TestServer serverRef, Socket peerSocket, Guid peerId,
            Owner ownerType, int connCounter) : base(serverRef, peerSocket, peerId, ownerType)
        {
            _Player = new Player();
            ServerRef = serverRef;
        }
        // Overload with id, because we dont have database for now, and I want
        // to have unique id's.
        public void LoadPlayerFromDatabase(string username, int UniqueId)
        {
            // load player attributes from database by "username"

            // set player
            _Player = new Player(UniqueId, "nowy gracz", 10, 10, 0, 0, 0, 0);

        }
    }
}
