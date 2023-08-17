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
    public class VisiblePlayersCollection
    {
        //private NetworkClient NetworkRef;
        public ConcurrentDictionary<int, Player> VisiblePlayers { get; private set; }

        public VisiblePlayersCollection()
        {
            //NetworkRef = networkRef;
            VisiblePlayers = new ConcurrentDictionary<int, Player>();
        }

        // a'la AddPlayer
        public async Task OnPlayerStateReceived(PlayerStatePacket packet)
        {
            
            //if(VisiblePlayers.)
            int? playerId = 0;
            playerId = packet.Id;

            if(playerId != null)
            {
                if (VisiblePlayers.TryGetValue((int)playerId, out Player foundedPlayer))
                {
                    // changing existing player attributes
                    foundedPlayer.pName = packet.Name ?? foundedPlayer.pName;
                    foundedPlayer.pHealth = packet.Health ?? foundedPlayer.pHealth;
                    foundedPlayer.pMana = packet.Mana ?? foundedPlayer.pMana;
                    foundedPlayer.pPositionX = packet.PosX ?? foundedPlayer.pPositionX; // if packet.posX != null
                    foundedPlayer.pPositionY = packet.PosY ?? foundedPlayer.pPositionY;
                    foundedPlayer.pPositionZ = packet.PosZ ?? foundedPlayer.pPositionZ;
                    foundedPlayer.pRotation = packet.Rot ?? foundedPlayer.pRotation;
                    await Console.Out.WriteLineAsync($"Received New Player State: ");
                    await foundedPlayer.Show();
                }
                else {
                    // no existing player found. Try add new player.
                    Player player = new Player(packet);
                    VisiblePlayers.TryAdd(player.pId, player);
                    await Console.Out.WriteLineAsync($"Received existing Player State: ");
                    await player.Show();
                }
            }
            else
            {
                throw new ArgumentNullException("Found player with null id. Null id is incorrect.");
            }
        }

    }
}
