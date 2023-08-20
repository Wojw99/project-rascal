using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.Packets
{
    // Used for sending request to load character. We using AuthToken to check
    // is player token good. Note that first token set is on auth-server side, so
    // we must check token in game-server. In the future we will sending character
    // id also. (or character slot number, which choose client.)
    public class CharacterLoadRequestPacket : Packet
    {
        //public int VId { get { return Read<int>("VId"); } }

        /*public PlayerLoadRequestPacket(int VId) : base(typeof(PlayerLoadRequestPacket))
        {
            Write("VId", VId);
        }*/

        public string AuthToken { get { return Read<string>("AuthToken"); } }

        public CharacterLoadRequestPacket(string authToken) : base(typeof(CharacterLoadRequestPacket))
        {
            Write("AuthToken", authToken);
        }

        public CharacterLoadRequestPacket(Packet packet) : base(packet) { } // packet is PlayerLoadRequestPacket - trzeba dodac

        public CharacterLoadRequestPacket(byte[] data) : base(data) { }

        public override string ToString()
        {
            return "";
        }
    }
}
