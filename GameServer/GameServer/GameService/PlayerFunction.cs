using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.Packets;
using NetworkCore.Packets.Attributes;
using ServerApplication.GameService.Base;

namespace ServerApplication.GameService
{
    public class PlayerFunction
    {
        public static async Task OnPlayerMove(PlayerConnection playerConn, PlayerMovePacket packet)
        {
            await Console.Out.WriteLineAsync("udalo sie");
        }

        public static async Task OnPlayerStateChanged(PlayerConnection playerConn, PlayerStatePacket packet)
        {
            playerConn._Player.pId = packet.Id ?? playerConn._Player.pId;
            playerConn._Player.pName = packet.Name ?? playerConn._Player.pName;
            playerConn._Player.pHealth = packet.Health ?? playerConn._Player.pHealth;
            playerConn._Player.pMana = packet.Mana ?? playerConn._Player.pMana;
            playerConn._Player.pPositionX = packet.PosX ?? playerConn._Player.pPositionX; // if packet.posX != null
            playerConn._Player.pPositionY = packet.PosY ?? playerConn._Player.pPositionY;
            playerConn._Player.pPositionZ = packet.PosZ ?? playerConn._Player.pPositionZ;
            playerConn._Player.pRotation = packet.Rot ?? playerConn._Player.pRotation;

            await playerConn._Player.Show();
            
        }
    }
}
