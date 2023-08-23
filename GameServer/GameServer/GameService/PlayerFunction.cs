using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.Packets;
using ServerApplication.GameService.Base;

namespace ServerApplication.GameService
{
    public class PlayerFunction
    {
        public static async Task OnCharacterMove(PlayerConnection playerConn, CharacterMovePacket movePacket)
        {
            // Changing only position in the Player Object. Here we are sure that PlayerMovePacket
            // must store non-null values.
            if(movePacket.CharacterVId == playerConn.CharacterObj.Vid) // always check is Id correct.
            {
                playerConn.CharacterObj.PositionX = movePacket.PosX;
                playerConn.CharacterObj.PositionY = movePacket.PosY;
                playerConn.CharacterObj.PositionZ = movePacket.PosZ;
                playerConn.CharacterObj.Rotation = movePacket.Rot;
            }

            await playerConn.ServerRef._World.SendPacketToConnectedPlayers(playerConn.Id, movePacket);
        }

        public static async Task OnCharacterStateChanged(PlayerConnection playerConn, CharacterStateUpdatePacket statePacket)
        {
            // Trying to change only values that changes, because not all of them must be assigned.
            // If value is not assigned, getter from packet returns null, so we checking
            // is variable from packet is null or not.
            if(statePacket.CharacterVId == playerConn.CharacterObj.Vid) // always check is Id correct.
            // important fact is we checking Vid assigned in playerConn object, so we are sure
            // that correct connection send correct id.
            // For example - some players can want to hack and send packet with incorrect id (id of other player)
            // (If we have that id encrypted, they must know encryption keys for that)
            // but if client with his player/character id = 2, send hacked PlayerStatePacket with id of
            // other player, server can check is assigned player object (determined with connection) 
            // on server is correct with id in packet.
            {
                playerConn.CharacterObj.Name = statePacket.Name ?? playerConn.CharacterObj.Name;
                playerConn.CharacterObj.Health = statePacket.Health ?? playerConn.CharacterObj.Health;
                playerConn.CharacterObj.Mana = statePacket.Mana ?? playerConn.CharacterObj.Mana;
                playerConn.CharacterObj.PositionX = statePacket.PosX ?? playerConn.CharacterObj.PositionX;
                playerConn.CharacterObj.PositionY = statePacket.PosY ?? playerConn.CharacterObj.PositionY;
                playerConn.CharacterObj.PositionZ = statePacket.PosZ ?? playerConn.CharacterObj.PositionZ;
                playerConn.CharacterObj.Rotation = statePacket.Rot ?? playerConn.CharacterObj.Rotation;
            }

            // for testing purposes
            //await playerConn.CharacterObj.Show();

            //await playerConn.ServerRef._World.SendPacketToConnectedPlayers(playerConn.Id, statePacket);
            // Important note: I dont run "SendPlayerStateToConnectedPlayers" method from World object, 
            // because I want to send values that only changed.
            

        }
    }
}
