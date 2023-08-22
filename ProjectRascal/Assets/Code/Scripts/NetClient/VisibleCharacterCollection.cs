using NetworkCore.NetworkData;
using NetworkCore.Packets;
using NetworkCore.NetworkCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Assets.Code.Scripts.NetClient
{
    // This class is representing other Players connected to server. We can receive
    // other players state, 
    public class VisibleCharacterCollection
    {
        //private NetworkClient NetworkRef;
        public ConcurrentDictionary<int, Character> VisiblePlayers { get; private set; }

        public VisibleCharacterCollection()
        {
            //NetworkRef = networkRef;
            VisiblePlayers = new ConcurrentDictionary<int, Character>();
        }

        public int PlayerCount()
        {
            return VisiblePlayers.Count;
        }

        // a'la AddPlayer
        public async Task OnCharacterStateReceived(CharacterStatePacket statePacket)
        {
            int playerVId = statePacket.CharacterVId; // note that statePacket.PlayerVid cannot be null.

            // Trying to find player with specified PlayerVid
            if (VisiblePlayers.TryGetValue(playerVId, out Character foundedPlayer))
            {
                // Changing existing player attributes
                foundedPlayer.Name = statePacket.Name ?? foundedPlayer.Name;
                foundedPlayer.Health = statePacket.Health ?? foundedPlayer.Health;
                foundedPlayer.Mana = statePacket.Mana ?? foundedPlayer.Mana;
                foundedPlayer.PositionX = statePacket.PosX ?? foundedPlayer.PositionX; // if statePacket.posX != null
                foundedPlayer.PositionY = statePacket.PosY ?? foundedPlayer.PositionY;
                foundedPlayer.PositionZ = statePacket.PosZ ?? foundedPlayer.PositionZ;
                foundedPlayer.Rotation = statePacket.Rot ?? foundedPlayer.Rotation;
                await Console.Out.WriteLineAsync($"Received New Player State: ");

                // For testing purposes, we are showing the player state which we received.
                await foundedPlayer.Show();
            }
            else // No existing player found. Try add new player.
            { 
                Character chr = new Character(statePacket);
                if(!VisiblePlayers.TryAdd(chr.Vid, chr))
                {
                    throw new ArgumentException($"Cannot add player with {chr.Vid} pVid. Specified id exists in collection. "); // if Vid is encrypted in future, we dont wanna to show that.
                }
                await Console.Out.WriteLineAsync($"Received existing Player State: ");
                await chr.Show();
            }

        }

        public async Task OnCharacterMoveReceived(CharacterMovePacket movePacket)
        {
            
        }

    }
}
