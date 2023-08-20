using NetworkCore.NetworkData;
using NetworkCore.Packets;
using NetworkCore.NetworkCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Client
{
    // This class is representing other Players connected to server. We can receive
    // other players state, 
    public class VisibleCharactersCollection
    {
        //private NetworkClient NetworkRef;
        public ConcurrentDictionary<int, Character> VisiblePlayers { get; private set; }
        public ConcurrentDictionary<int, Enemy> VisibleEnemies { get; private set; }

        public VisibleCharactersCollection()
        {
            //NetworkRef = networkRef;
            VisiblePlayers = new ConcurrentDictionary<int, Character>();
            VisibleEnemies = new ConcurrentDictionary<int, Enemy>();
        }

        public int PlayerCount()
        {
            return VisiblePlayers.Count;
        }

        // a'la AddPlayer
        public async Task OnCharacterStateReceived(CharacterStatePacket statePacket)
        {
            int charVId = statePacket.CharacterVId; // note that statePacket.PlayerVid cannot be null.

            // Trying to find player with specified PlayerVid
            if (VisiblePlayers.TryGetValue(charVId, out Character foundedCharacter))
            {
                // Changing existing player attributes
                foundedCharacter.Name = statePacket.Name ?? foundedCharacter.Name;
                foundedCharacter.Health = statePacket.Health ?? foundedCharacter.Health;
                foundedCharacter.Mana = statePacket.Mana ?? foundedCharacter.Mana;
                foundedCharacter.PositionX = statePacket.PosX ?? foundedCharacter.PositionX; // if statePacket.posX != null
                foundedCharacter.PositionY = statePacket.PosY ?? foundedCharacter.PositionY;
                foundedCharacter.PositionZ = statePacket.PosZ ?? foundedCharacter.PositionZ;
                foundedCharacter.Rotation = statePacket.Rot ?? foundedCharacter.Rotation;
                await Console.Out.WriteLineAsync($"Received New Player State: ");

                // For testing purposes, we are showing the player state which we received.
                await foundedCharacter.Show();
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
