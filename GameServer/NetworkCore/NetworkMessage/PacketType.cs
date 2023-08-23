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
        CHARACTER_LOAD_REQUEST = 0x10,
        CHARACTER_LOAD_RESPONSE = 0x11,
        CHARACTER_LOAD_SUCCES = 0X12,

        // CHARACTER 
        CHARACTER_STATE_PACKET = 0x20,
        CHARACTER_STATE_UPDATE_PACKET = 0x21,
        CHARACTER_STATES_UPDATE_PACKET = 0x22,
        CHARACTER_MOVE_PACKET = 0x23,

        // 
        CLIENT_DISCONNECT = 0x30,

        // DIAGNOSTIC
        PING_REQUEST = 0x40,
        PING_RESPONSE = 0x41,
    }
}
