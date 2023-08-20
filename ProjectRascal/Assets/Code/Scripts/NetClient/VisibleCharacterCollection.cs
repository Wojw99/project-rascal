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
        public ConcurrentDictionary<int, Player> VisiblePlayers { get; private set; }

        public VisibleCharacterCollection()
        {
            //NetworkRef = networkRef;
            VisiblePlayers = new ConcurrentDictionary<int, Player>();
        }

        public int PlayerCount()
        {
            return VisiblePlayers.Count;
        }

        // a'la AddPlayer
        public async Task OnPlayerStateReceived(CharacterStatePacket statePacket)
        {
            int playerVId = statePacket.PlayerVId; // note that statePacket.PlayerVid cannot be null.

            // Trying to find player with specified PlayerVid
            if (VisiblePlayers.TryGetValue(playerVId, out Player foundedPlayer))
            {
                // Changing existing player attributes
                foundedPlayer.pName = statePacket.Name ?? foundedPlayer.pName;
                foundedPlayer.pHealth = statePacket.Health ?? foundedPlayer.pHealth;
                foundedPlayer.pMana = statePacket.Mana ?? foundedPlayer.pMana;
                foundedPlayer.pPositionX = statePacket.PosX ?? foundedPlayer.pPositionX; // if statePacket.posX != null
                foundedPlayer.pPositionY = statePacket.PosY ?? foundedPlayer.pPositionY;
                foundedPlayer.pPositionZ = statePacket.PosZ ?? foundedPlayer.pPositionZ;
                foundedPlayer.pRotation = statePacket.Rot ?? foundedPlayer.pRotation;
                await Console.Out.WriteLineAsync($"Received New Player State: ");

                // For testing purposes, we are showing the player state which we received.
                await foundedPlayer.Show();
            }
            else // No existing player found. Try add new player.
            { 
                Player player = new Player(statePacket);
                if(!VisiblePlayers.TryAdd(player.pVid, player))
                {
                    throw new ArgumentException($"Cannot add player with {player.pVid} pVid. Specified id exists in collection. "); // if Vid is encrypted in future, we dont wanna to show that.
                }
                await Console.Out.WriteLineAsync($"Received existing Player State: ");
                await player.Show();
            }

        }

        public async Task OnPlayerMoveReceived(PlayerMovePacket movePacket)
        {
            
        }

    }
}
