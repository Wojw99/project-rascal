using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using NetworkCore.Packets.Attributes;

namespace ServerApplication.GameService.Base
{
    public class World
    {
        private TestServer ServerRef { get; }
        private ConcurrentDictionary<Guid, PlayerConnection> ConnectedPlayers = new ConcurrentDictionary<Guid, PlayerConnection>();
        private ConcurrentDictionary<Guid, Enemy> Enemies = new ConcurrentDictionary<Guid, Enemy>();

        public int IdCounter = 0;

        public World(TestServer serverRef) 
        {
            ServerRef = serverRef;
        }

        public void SendPlayerState(PlayerConnection senderPeer)
        {
            PlayerStatePacket packet = new PlayerStatePacket(senderPeer._Player.attributes);
                
            foreach (var receiver in ConnectedPlayers)
            {
                if(receiver.Key != senderPeer.Id)
                {
                    senderPeer.SendPacket(packet);
                }
            }
            
        }

        public async Task AddNewPlayer(PlayerConnection playerConn)
        {
            if(ConnectedPlayers.TryAdd(playerConn.Id, playerConn))
            {
                await playerConn.ConnectToClient();
            }
        }

        public async Task RemovePlayer(PlayerConnection playerConn) 
        {
            if(ConnectedPlayers.TryRemove(playerConn.Id, out PlayerConnection removedPlayer))
            {
                await removedPlayer.Disconnect();
            }
        }
    }
}
