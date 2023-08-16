using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.Packets;

namespace ServerApplication.GameService
{
    public class PlayerFunction
    {
        public static async Task OnPlayerMove(PlayerConnection playerConn, PlayerMovePacket packet)
        {
            
        }

        public static async Task OnPlayerStateChanged(PlayerConnection playerConn, PlayerStatePacket packet)
        {
            playerConn._Player.attributes = packet.PlayerAttributes; // trzeba bedzie zastepowac te wartosci, które rzeczywiscie sie zmienily


            playerConn.ServerRef._World.SendPlayerState(playerConn);
        }
    }
}
