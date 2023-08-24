using NetworkCore.NetworkData;
using NetworkCore.Packets;
using NetworkCore.NetworkCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using NetworkCore.Packets;

namespace Client
{
    // This class is representing other Players connected to server. We can receive
    // other players state, 
    public class VisibleCharactersCollection
    {
        public ConcurrentDictionary<int, Character> VisiblePlayers { get; private set; }

        public ConcurrentDictionary<int, Enemy> VisibleEnemies { get; private set; }

        public int PlayerCount { get { return VisiblePlayers.Count; } }

        public VisibleCharactersCollection()
        {
            //NetworkRef = networkRef;
            VisiblePlayers = new ConcurrentDictionary<int, Character>();
            VisibleEnemies = new ConcurrentDictionary<int, Enemy>();
        }

        public async Task ShowCharacters()
        {
            await Console.Out.WriteLineAsync("----------------------------------------");
            await Console.Out.WriteLineAsync($"Loaded {PlayerCount} player characters. ");
            await Console.Out.WriteLineAsync("----------------------------------------");
            foreach (var character in VisiblePlayers.Values)
            {
                await Console.Out.WriteLineAsync("--------------------------");
                await character.Show();
                await Console.Out.WriteLineAsync("--------------------------");
            }
        }


        // a'la AddPlayer
        public void OnCharacterStateReceived(CharacterStatePacket statePacket)
        {
            if (VisiblePlayers.ContainsKey(statePacket.CharacterVId))
            {
                VisiblePlayers[statePacket.CharacterVId] = statePacket.GetCharacter();
            }
            else
            {
                VisiblePlayers.TryAdd(statePacket.CharacterVId, statePacket.GetCharacter());
            }
        }

        public void OnCharacterStateUpdateReceived(CharacterStateUpdatePacket statePacket)
        {
            int charVId = statePacket.CharacterVId;

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
            }
            else // No existing player found. Try add new player. Note that we probably dont need this, because alter succesfull
            // loaded character, server send CharacterStatePacket always. So in this case, the else will not execute even once.
            {
                Character chr = new Character(statePacket);
                if (!VisiblePlayers.TryAdd(chr.Vid, chr))
                {
                    throw new ArgumentException($"Cannot add player with {chr.Vid} pVid. Specified id exists in collection. "); // if Vid is encrypted in future, we dont wanna to show that.
                }
            }
            Console.WriteLine("test");
        }

        public async Task OnCharacterMoveReceived(CharacterMovePacket movePacket)
        {
            
        }

    }
}
