using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public enum PacketType
    {
        // AUTHORIZATION
        LOGIN_REQUEST = 0x1,
        LOGIN_RESPONSE = 0x2,

        // CHARACTER LOAD
        CHARACTER_LOAD_REQUEST = 0x10, // Client-side request to load his character object.
        CHARACTER_LOAD_RESPONSE = 0x11, // In response send character objest to client.
        CHARACTER_LOAD_SUCCES = 0X12, // Client-side succes operation of loading his character received from server.

        // CHARACTER 
        CHARACTER_STATE_PACKET = 0x20, // To send single character state (full).
        CHARACTER_STATES_PACKET = 0x21, // To send all character states on world.
        CHARACTER_STATE_UPDATE_PACKET = 0x22,  // To send single character state update.
        CHARACTER_STATES_UPDATE_PACKET = 0x23, // To send all updated characters states on World.
        CHARACTER_MOVE_PACKET = 0x24,

        // 
        CLIENT_DISCONNECT = 0x30,

        // DIAGNOSTIC
        PING_REQUEST = 0x40,
        PING_RESPONSE = 0x41,
    }
}
