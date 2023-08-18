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
        public static async Task OnPlayerMove(PlayerConnection playerConn, PlayerMovePacket movePacket)
        {
            // Changing only position in the Player Object. Here we are sure that PlayerMovePacket
            // must store non-null values.
            if(movePacket.PlayerVid == playerConn._Player.pVid) // always check is Id correct.
            {
                playerConn._Player.pPositionX = movePacket.PosX;
                playerConn._Player.pPositionY = movePacket.PosY;
                playerConn._Player.pPositionZ = movePacket.PosZ;
                playerConn._Player.pRotation = movePacket.Rot;
            }

            await playerConn.ServerRef._World.SendPacketToConnectedPlayers(playerConn.Id, movePacket);
        }

        public static async Task OnPlayerStateChanged(PlayerConnection playerConn, PlayerStatePacket statePacket)
        {
            // Trying to change only values that changes, because not all of them must be assigned.
            // If value is not assigned, getter from packet returns null, so we checking
            // is variable from packet is null or not.
            if(statePacket.PlayerVId == playerConn._Player.pVid) // always check is Id correct.
            // important fact is we checking Vid assigned in playerConn object, so we are sure
            // that correct connection send correct id.
            // For example - some players can want to hack and send packet with incorrect id
            // (If we have that id encrypted, they must know encryption keys for that)
            // but if client with his player/character id = 2, send hacked PlayerStatePacket with id of
            // other player, server can check is assigned player object (determined with connection)
            // on server is correct with id in packet.
            {
                playerConn._Player.pName = statePacket.Name ?? playerConn._Player.pName;
                playerConn._Player.pHealth = statePacket.Health ?? playerConn._Player.pHealth;
                playerConn._Player.pMana = statePacket.Mana ?? playerConn._Player.pMana;
                playerConn._Player.pPositionX = statePacket.PosX ?? playerConn._Player.pPositionX;
                playerConn._Player.pPositionY = statePacket.PosY ?? playerConn._Player.pPositionY;
                playerConn._Player.pPositionZ = statePacket.PosZ ?? playerConn._Player.pPositionZ;
                playerConn._Player.pRotation = statePacket.Rot ?? playerConn._Player.pRotation;
            }

            // for testing purposes
            await playerConn._Player.Show();

            await playerConn.ServerRef._World.SendPacketToConnectedPlayers(playerConn.Id, statePacket);
            // Important note: I dont run "SendPlayerStateToConnectedPlayers" method from World object, 
            // because I want to send values that only changed.
            

        }
    }
}
