/*//using NetworkCore;

using System.Text;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data;
using NetworkCore.NetworkCommunication;
using System.Net.Sockets;
using NetworkCore.NetworkMessage;

namespace ServerApplication
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Packet testPacket = new Packet(PacketType.packet_test_packet);
            // initializing with maximal values of each type
            testPacket.WriteInt("int", 2147483647);
            testPacket.WriteShort("short", 32767);
            testPacket.WriteLong("long", 9223372036854775807);
            testPacket.WriteDouble("double", 1.7976931348623157E+308);
            testPacket.WriteFloat("float", (float)3.4028235E+38);
            testPacket.WriteString("string", "testowanie pakietów");

            byte[] data = PacketManager.serializePacket(testPacket);
            Packet newPacket = PacketManager.DeserializeByteData(data);

            Server server = new Server();
            
            server.HandlePacket(newPacket);
        }
    }
}*/