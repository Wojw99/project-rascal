using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTests.Test.old
{
    public enum PacketType
    {
        // AUTHORIZATION
        LOGIN_REQUEST = 0x1,
        LOGIN_RESPONSE = 0x2,
        LOGIN_REQUEST_COLLECTION = 0x3,

        // CHARACTER LOAD
        CHARACTER_LOAD_REQUEST = 0x10, // Client-side request to load his character object.
        CHARACTER_LOAD_RESPONSE = 0x11, // In response send character objest to client.
        CHARACTER_LOAD_SUCCES = 0X12, // Client-side succes operation of loading his character received from server.
        ADVENTURER_LOAD_PACKET = 0x13, // When new player loaded, we sending that packet to all players.
        ADVENTURER_LOAD_COLLECTION_PACKET = 0x14,

        // CHARACTER / ADVENTURER STATE
        ATTRIBUTES_PACKET = 0x20, // To send single character state (full).
        ATTRIBUTES_COLLECTION_PACKET = 0x21, // To send all character states on world.

        ATTRIBUTES_UPDATE_PACKET = 0x22,  // To send single character state update.
        ATTRIBUTES_COLLECTION_UPDATE_PACKET = 0x23, // To send all updated characters states on World.

        TRANSFORM_PACKET = 0x24,
        TRANSFORM_COLLECTION_PACKET = 0X25,

        CHARACTER_EXIT_PACKET = 0x26,

        // 
        CLIENT_DISCONNECT = 0x30,

        // DIAGNOSTIC
        PING_REQUEST = 0x40,
        PING_RESPONSE = 0x41,
    }
}
