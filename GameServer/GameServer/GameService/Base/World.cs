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
using NetworkCore.NetworkUtility;
using ServerApplication.GameService.Player;

namespace ServerApplication.GameService.Base
{
    // Purpose of this class is to interract with collection of Player connections.
    // In methods we could check is PlayerConnection connected. But I don't do that,
    // because every time player is disconnected, I also remove it from the list of connected players.
    // But have this thing in mind.
    public class World
    {
        private ConcurrentDictionary<Guid, PlayerPeer> ConnectedPlayers = new ConcurrentDictionary<Guid, PlayerPeer>();

        private ConcurrentDictionary<int, Enemy> Enemies = new ConcurrentDictionary<int, Enemy>();

        private ConcurrentDictionary<Guid, AttributesUpdatePacket> CharactersStatesUpdated = new ConcurrentDictionary<Guid, AttributesUpdatePacket> ();

        private ConcurrentDictionary<Guid, TransformPacket> CharactersTransforms = new ConcurrentDictionary<Guid, TransformPacket>();

        public int IdCounter = 0;

        public int PlayerCount { get { return ConnectedPlayers.Count; } }

        public int EnemyCount { get { return Enemies.Count; } }

        public World() { }

        public async Task Update()
        {
            if(CharactersTransforms.Count > 0)
            {
                TransformCollectionPacket transformsPacket = new TransformCollectionPacket();

                foreach (var transform in CharactersTransforms)
                {
                    transformsPacket.PacketCollection.Add(transform.Value);
                }

                // Send transform to all players.
                foreach (var player in ConnectedPlayers.Values)
                    await player.SendPacket(transformsPacket);

                // Clear Transforms to not send duplicates.
                CharactersTransforms.Clear();
            }

            if (CharactersStatesUpdated.Count > 0)
            {
                AttributesUpdateCollectionPacket statesPacket = new AttributesUpdateCollectionPacket();

                // Add updated states to packet
                foreach (var characterState in CharactersStatesUpdated)
                    statesPacket.PacketCollection.Add(characterState.Value);

                // Sending player states updates to all players.
                foreach (var player in ConnectedPlayers.Values)
                    await player.SendPacket(statesPacket);

                // Calling method OnChacterStateSend() which clearing player packet object
                // (setting all values of his state packet to null). We calling it after packet send,
                // because "statesPacket" have reference to packet from player connections, so we
                // cannot clear that before send packet.
                foreach (var characterState in CharactersStatesUpdated)
                    ConnectedPlayers[characterState.Key].OnCharactedStateSend();

                CharactersStatesUpdated.Clear();
            }
        }
        public void AddNewCharacterStateUpdate(Guid ConnId, AttributesUpdatePacket updatedState)
        {
            CharactersStatesUpdated[ConnId] = updatedState;
        }

        public void AddNewCharacterTransform(Guid ConnId, TransformPacket transform)
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
                await player.PlayerCharacter.Show();
                await Console.Out.WriteLineAsync("--------------------------");
            }
        }

        // For testing purposes searching by his characterVId
        public PlayerPeer GetPlayerObj(int characterVId)
        {
            foreach(var player in ConnectedPlayers.Values)
                if (player.PlayerCharacter.Vid == characterVId)
                    return player;

            throw new ArgumentException($"Cannot find character with following id: {characterVId}");
        }

        public PlayerPeer GetPlayerObj(Guid playerGuid)
        {
            return ConnectedPlayers[playerGuid];
        }

        public async Task SendPacketToConnectedPlayers(Guid senderPeerId, PacketBase packet)
        {
            foreach (var receiver in ConnectedPlayers)
                if (receiver.Key != senderPeerId)
                    await receiver.Value.SendPacket(packet);
        }

        public async Task AddNewPlayer(PlayerPeer playerConn)
        {
            if(!ConnectedPlayers.TryAdd(playerConn.GUID, playerConn))
                throw new InvalidOperationException($"Failed to add player with id {playerConn.GUID}.");

            // Send character states of currently connected players to new player.
            AdventurerLoadCollectionPacket adventurerLoadCollection = new AdventurerLoadCollectionPacket();

            foreach (var adventurer in ConnectedPlayers)
            {
                if (adventurer.Key != playerConn.GUID) // Except logged in player.
                {
                    AdventurerLoadPacket adventurerLoad = new AdventurerLoadPacket();

                    adventurerLoad.AttributesPacket = new AttributesPacket(adventurer.Value.PlayerCharacter.Vid,
                        adventurer.Value.PlayerCharacter.Name, adventurer.Value.PlayerCharacter.CurrentHealth, adventurer.Value.PlayerCharacter.MaxHealth,
                        adventurer.Value.PlayerCharacter.CurrentMana, adventurer.Value.PlayerCharacter.MaxMana, adventurer.Value.PlayerCharacter.MoveSpeed,
                        adventurer.Value.PlayerCharacter.AttackSpeed, adventurer.Value.PlayerCharacter.State);

                    adventurerLoad.TransformPacket = new TransformPacket(adventurer.Value.PlayerCharacter.Vid,
                        adventurer.Value.PlayerCharacter.PositionX, adventurer.Value.PlayerCharacter.PositionY, adventurer.Value.PlayerCharacter.PositionZ,
                        adventurer.Value.PlayerCharacter.RotationX, adventurer.Value.PlayerCharacter.RotationY, adventurer.Value.PlayerCharacter.RotationZ,
                        adventurer.Value.PlayerCharacter.State);

                    adventurerLoadCollection.PacketCollection.Add(adventurerLoad);
                }
            }

            await playerConn.SendPacket(adventurerLoadCollection); // Send packet to one.

            // Next step is to send a full state of currently logged character to others.

            AdventurerLoadPacket adventurerLoadPacket = new AdventurerLoadPacket();

            adventurerLoadPacket.AttributesPacket = new AttributesPacket(playerConn.PlayerCharacter.Vid, 
                playerConn.PlayerCharacter.Name, playerConn.PlayerCharacter.CurrentHealth, playerConn.PlayerCharacter.MaxHealth, 
                playerConn.PlayerCharacter.CurrentMana, playerConn.PlayerCharacter.MaxMana, playerConn.PlayerCharacter.MoveSpeed,
                playerConn.PlayerCharacter.AttackSpeed, playerConn.PlayerCharacter.State);

            adventurerLoadPacket.TransformPacket = new TransformPacket(playerConn.PlayerCharacter.Vid, 
                playerConn.PlayerCharacter.PositionX, playerConn.PlayerCharacter.PositionY, playerConn.PlayerCharacter.PositionZ,
                playerConn.PlayerCharacter.RotationX, playerConn.PlayerCharacter.RotationY, playerConn.PlayerCharacter.RotationZ,
                playerConn.PlayerCharacter.State);

            await this.SendPacketToConnectedPlayers(playerConn.GUID, adventurerLoadPacket); // Send packet to all.
        }

        public async Task RemovePlayer(PlayerPeer playerConn) 
        {
            if (!ConnectedPlayers.TryRemove(playerConn.GUID, out PlayerPeer removedPlayer)) // idk we need object of removed player.
                throw new InvalidOperationException($"Failed to remove player with id {playerConn.GUID}.");

            // Send packet to all players to remove player character from their collections.
            foreach (var player in ConnectedPlayers)
                if (playerConn.GUID != player.Key)
                    await player.Value.SendPacket(new AdventurerExitPacket(playerConn.PlayerCharacter.Vid));
        }
    }
}
