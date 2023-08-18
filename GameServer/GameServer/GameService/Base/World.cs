using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using NetworkCore.Packets.Attributes;

namespace ServerApplication.GameService.Base
{
    // Purpose of this class is to interract with collection of Player connections.
    // In methods we could check is PlayerConnection connected. But I don't do that,
    // because every time player is disconnected, I also remove it from the list of connected players.
    // But have this thing in mind.
    public class World
    {
       // private TestServer ServerRef { get; }
        private ConcurrentDictionary<Guid, PlayerConnection> ConnectedPlayers = new ConcurrentDictionary<Guid, PlayerConnection>();
        private ConcurrentDictionary<Guid, Enemy> Enemies = new ConcurrentDictionary<Guid, Enemy>();

        public int IdCounter = 0;

        public World() { }


        // sending all information of Player (Player State) to other connected players
        public async Task SendPlayerStateToConnectedPlayers(PlayerConnection senderPeer)
        {
            // we overloading PlayerStatePacket with constructor, which initialize
            // values in packet by values in player object.
            PlayerStatePacket packet = new PlayerStatePacket(senderPeer._Player);
                
            foreach (var receiver in ConnectedPlayers)
            {
                if(receiver.Key != senderPeer.Id)
                {
                    await receiver.Value.SendPacket(packet);
                }
            }
        }

        public async Task SendPacketToConnectedPlayers(Guid senderPeerId, Packet packet)
        {
            foreach (var receiver in ConnectedPlayers)
            {
                if (receiver.Key != senderPeerId)
                {
                    await receiver.Value.SendPacket(packet);
                }
            }
        }


        public async Task AddNewPlayer(PlayerConnection playerConn)
        {
            if(!ConnectedPlayers.TryAdd(playerConn.Id, playerConn))
                throw new InvalidOperationException($"Failed to add player with id {playerConn.Id}.");
        }

        public async Task RemovePlayer(PlayerConnection playerConn) 
        {
            if (!ConnectedPlayers.TryRemove(playerConn.Id, out PlayerConnection removedPlayer)) // idk we need object of removed player.
                throw new InvalidOperationException($"Failed to remove player with id {playerConn.Id}.");
        }

        // this we could use when, not all attributes changes. For example we get an state of player which hp was changed - after that we could
        // reuse his statePacket which was send from him to server to send that to all other clients.
        // important note: now we have more general method - SendPacketToConnectedPlayers

        /*public async Task SendPlayerStateToConnectedPlayers(Guid senderPeerId, PlayerStatePacket statePacket)
        {
            foreach (var receiver in ConnectedPlayers)
            {
                if (receiver.Key != senderPeerId)
                {
                    await receiver.Value.SendPacket(statePacket);
                }
            }

        }*/
    }
}
