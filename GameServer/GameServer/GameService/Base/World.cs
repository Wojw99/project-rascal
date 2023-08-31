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

        private ConcurrentDictionary<Guid, CharacterAttrUpdatePacket> CharactersStatesUpdated = new ConcurrentDictionary<Guid, CharacterAttrUpdatePacket> ();

        private ConcurrentDictionary<Guid, CharacterTransformPacket> CharactersTransforms = new ConcurrentDictionary<Guid, CharacterTransformPacket>();

        public int IdCounter = 0;

        public int PlayerCount { get { return ConnectedPlayers.Count; } }

        public int EnemyCount { get { return Enemies.Count; } }

        public World() { }

        // Send updated character status to all players.
        public async Task Update()
        {
            // Sending position and rotation.

            if(CharactersTransforms.Count > 0)
            {
                CharacterTransformsPacket transformsPacket = new CharacterTransformsPacket();

                foreach (var transform in CharactersTransforms)
                {
                    transformsPacket.PacketCollection.Add(transform.Value);
                }

                foreach (var player in ConnectedPlayers.Values)
                {
                    await player.SendPacket(transformsPacket);
                }

                CharactersTransforms.Clear();
            }


            // Sending character States updates.

            if (CharactersStatesUpdated.Count > 0)
            {
                CharactersAttrsUpdatePacket statesPacket = new CharactersAttrsUpdatePacket();

                // Add updated states to packet
                foreach (var characterState in CharactersStatesUpdated)
                {
                    statesPacket.PacketCollection.Add(characterState.Value);
                }  

                // Sending to all players.
                foreach (var player in ConnectedPlayers.Values)
                {
                    await player.SendPacket(statesPacket);
                }

                // Calling method OnChacterStateSend() which clearing player packet object
                // (setting all values of his state packet to null). We calling it after packet send,
                // because "statesPacket" have reference to packet from player connections, so we
                // cannot clear that before send packet.
                foreach (var characterState in CharactersStatesUpdated)
                {
                    ConnectedPlayers[characterState.Key].OnCharactedStateSend();
                }

                CharactersStatesUpdated.Clear();
            }
        }
        public void AddNewCharacterStateUpdate(Guid ConnId, CharacterAttrUpdatePacket updatedState)
        {
            CharactersStatesUpdated[ConnId] = updatedState;
        }

        public void AddNewCharacterTransform(Guid ConnId, CharacterTransformPacket transform)
        {
            CharactersTransforms[ConnId] = transform;
        }

        public async Task ShowConnectedPlayers()
        {
            await Console.Out.WriteLineAsync("-------------------------------");
            await Console.Out.WriteLineAsync($"Loaded {PlayerCount} players. ");
            await Console.Out.WriteLineAsync("-------------------------------");
            await Console.Out.WriteLineAsync("Player characters: ");

            foreach (var player in ConnectedPlayers.Values)
            {
                await Console.Out.WriteLineAsync("--------------------------");
                await player.CharacterObj.Show();
                await Console.Out.WriteLineAsync("--------------------------");
            }
        }

        // For testing purposes searching by his characterVId
        public PlayerConnection GetPlayerObj(int characterVId)
        {
            foreach(var player in ConnectedPlayers.Values)
            {
                if (player.CharacterObj.Vid == characterVId)
                    return player;
            }

            throw new ArgumentException($"Cannot find character with following id: {characterVId}");
        }

        public PlayerConnection GetPlayerObj(Guid playerGuid)
        {
            return ConnectedPlayers[playerGuid];
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

        // Send current connected player states to that player

        public async Task AddNewPlayer(PlayerConnection playerConn)
        {
            if(!ConnectedPlayers.TryAdd(playerConn.Id, playerConn))
                throw new InvalidOperationException($"Failed to add player with id {playerConn.Id}.");

            // Send character states of currently connected players to new player.
            CharacterStatesPacket characterStatesPacket = new CharacterStatesPacket();

            foreach (var character in ConnectedPlayers)
            {
                if (character.Key != playerConn.Id)
                {
                    characterStatesPacket.PacketCollection.Add(new CharacterStatePacket(character.Value.CharacterObj));
                }
            }

            await playerConn.SendPacket(characterStatesPacket);
        }

        public async Task RemovePlayer(PlayerConnection playerConn) 
        {
            if (!ConnectedPlayers.TryRemove(playerConn.Id, out PlayerConnection removedPlayer)) // idk we need object of removed player.
                throw new InvalidOperationException($"Failed to remove player with id {playerConn.Id}.");

            // Send packet to all players to remove player character from their collections.
            foreach (var player in ConnectedPlayers)
            {
                if (playerConn.Id != player.Key)
                {
                    await player.Value.SendPacket(new CharacterExitPacket(playerConn.CharacterObj.Vid));
                }
            }
        }
    }
}
