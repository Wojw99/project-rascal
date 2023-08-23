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

namespace ServerApplication.GameService.Base
{
    // Purpose of this class is to interract with collection of Player connections.
    // In methods we could check is PlayerConnection connected. But I don't do that,
    // because every time player is disconnected, I also remove it from the list of connected players.
    // But have this thing in mind.
    public class World
    {
        private ConcurrentDictionary<Guid, PlayerConnection> ConnectedPlayers = new ConcurrentDictionary<Guid, PlayerConnection>();
        private ConcurrentDictionary<int, Enemy> Enemies = new ConcurrentDictionary<int, Enemy>();
        private ConcurrentDictionary<Guid, CharacterStateUpdatePacket> CharactersStatesUpdated = new ConcurrentDictionary<Guid, CharacterStateUpdatePacket> { };

        public int IdCounter = 0;

        public World() { }

        public async Task Update()
        {
            CharacterStatesUpdatePacket statesPacket = new CharacterStatesUpdatePacket();

            // Add updated states to packet
            foreach(var characterState in CharactersStatesUpdated)
            {
                statesPacket.PacketCollection.Add(characterState.Value);
                ConnectedPlayers[characterState.Key].OnCharactedStateSend();
            }

            // Sending to all players
            foreach (var player in ConnectedPlayers.Values)
            {
                await player.SendPacket(statesPacket);
            }

            CharactersStatesUpdated.Clear();
        }

        public async Task SendPacketToConnectedPlayers(Guid senderPeerId, PacketBase packet)
        {
            foreach (var receiver in ConnectedPlayers)
            {
                if (receiver.Key != senderPeerId)
                {
                    await receiver.Value.SendPacket(packet);
                }
            }
        }

        public void AddNewPlayer(PlayerConnection playerConn)
        {
            if(!ConnectedPlayers.TryAdd(playerConn.Id, playerConn))
                throw new InvalidOperationException($"Failed to add player with id {playerConn.Id}.");
        }

        public void RemovePlayer(PlayerConnection playerConn) 
        {
            if (!ConnectedPlayers.TryRemove(playerConn.Id, out PlayerConnection removedPlayer)) // idk we need object of removed player.
                throw new InvalidOperationException($"Failed to remove player with id {playerConn.Id}.");
        }

        public void AddNewCharacterStateUpdate(Guid ConnId, CharacterStateUpdatePacket updatedState)
        {
            CharactersStatesUpdated[ConnId] = updatedState;
        }
    }
}
