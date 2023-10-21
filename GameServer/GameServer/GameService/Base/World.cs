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
        private ConcurrentDictionary<Guid, PlayerConnection> ConnectedPlayers = new ConcurrentDictionary<Guid, PlayerConnection>();

        private ConcurrentDictionary<int, Enemy> Enemies = new ConcurrentDictionary<int, Enemy>();

        private ConcurrentDictionary<Guid, AttributesUpdatePacket> CharactersStatesUpdated = new ConcurrentDictionary<Guid, AttributesUpdatePacket> ();

        private ConcurrentDictionary<Guid, TransformPacket> CharactersTransforms = new ConcurrentDictionary<Guid, TransformPacket>();

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
                TransformCollectionPacket transformsPacket = new TransformCollectionPacket();

                foreach (var transform in CharactersTransforms)
                {
                    transformsPacket.PacketCollection.Add(transform.Value);
                }

                foreach (var player in ConnectedPlayers.Values)
                {
                    // To not send a player's transformation pack to the same player.
                    //TransformPacket? pck = transformsPacket.PacketCollection.Find(attr => attr.CharacterVId == player.CharacterObj.Vid);
                    // if (pck != null)
                        //transformsPacket.PacketCollection.Remove(pck);

                    await player.SendPacket(transformsPacket);
                }

                CharactersTransforms.Clear();
            }


            // Sending character States updates.

            if (CharactersStatesUpdated.Count > 0)
            {
                AttributesUpdateCollectionPacket statesPacket = new AttributesUpdateCollectionPacket();

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
            AdventurerLoadCollectionPacket adventurerLoadCollection = new AdventurerLoadCollectionPacket();

            foreach (var adventurer in ConnectedPlayers)
            {
                if (adventurer.Key != playerConn.Id) // Except logged in player.
                {
                    AdventurerLoadPacket adventurerLoad = new AdventurerLoadPacket();

                    adventurerLoad.AttributesPacket = new AttributesPacket(adventurer.Value.CharacterObj.Vid,
                        adventurer.Value.CharacterObj.Name, adventurer.Value.CharacterObj.CurrentHealth, adventurer.Value.CharacterObj.MaxHealth,
                        adventurer.Value.CharacterObj.CurrentMana, adventurer.Value.CharacterObj.MaxMana, adventurer.Value.CharacterObj.MoveSpeed,
                        adventurer.Value.CharacterObj.AttackSpeed, adventurer.Value.CharacterObj.State);

                    adventurerLoad.TransformPacket = new TransformPacket(adventurer.Value.CharacterObj.Vid,
                        adventurer.Value.CharacterObj.PositionX, adventurer.Value.CharacterObj.PositionY, adventurer.Value.CharacterObj.PositionZ,
                        adventurer.Value.CharacterObj.RotationX, adventurer.Value.CharacterObj.RotationY, adventurer.Value.CharacterObj.RotationZ,
                        adventurer.Value.CharacterObj.State);

                    adventurerLoadCollection.PacketCollection.Add(adventurerLoad);
                }
            }

            await playerConn.SendPacket(adventurerLoadCollection); // Send packet to one.

            // Next step is to send a full state of currently logged character to others.

            AdventurerLoadPacket adventurerLoadPacket = new AdventurerLoadPacket();

            adventurerLoadPacket.AttributesPacket = new AttributesPacket(playerConn.CharacterObj.Vid, 
                playerConn.CharacterObj.Name, playerConn.CharacterObj.CurrentHealth, playerConn.CharacterObj.MaxHealth, 
                playerConn.CharacterObj.CurrentMana, playerConn.CharacterObj.MaxMana, playerConn.CharacterObj.MoveSpeed,
                playerConn.CharacterObj.AttackSpeed, playerConn.CharacterObj.State);

            adventurerLoadPacket.TransformPacket = new TransformPacket(playerConn.CharacterObj.Vid, 
                playerConn.CharacterObj.PositionX, playerConn.CharacterObj.PositionY, playerConn.CharacterObj.PositionZ,
                playerConn.CharacterObj.RotationX, playerConn.CharacterObj.RotationY, playerConn.CharacterObj.RotationZ,
                playerConn.CharacterObj.State);

            await this.SendPacketToConnectedPlayers(playerConn.Id, adventurerLoadPacket); // Send packet to all.
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
                    await player.Value.SendPacket(new AdventurerExitPacket(playerConn.CharacterObj.Vid));
                }
            }
        }
    }
}
