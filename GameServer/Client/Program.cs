using Client;
using NetworkCore.NetworkMessage;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    class Client
    {
        public static void SendEnemyShootPacket(NetworkStream stream)
        {
            Packet packet = new Packet(PacketType.packet_enemy_shoot);
            packet.WriteInt("playerId", 1242325);
            packet.WriteInt("targetId", 4355321);

            byte[] data = PacketSerializationManager.serializePacket(packet);
            stream.Write(data, 0, data.Length);
        }

        public static void SendPlayerMovePacket(NetworkStream stream)
        {
            Packet packet = new Packet(PacketType.packet_player_move);
            packet.WriteInt("playerId", 456546);
            packet.ReadField<double>("posX");
            packet.ReadField<double>("posY");
            packet.ReadField<string>("test");

            byte[] data = PacketSerializationManager.serializePacket(packet);
            stream.Write(data, 0, data.Length);
        }

        public static void SendTestPacket(NetworkStream stream)
        {
            Packet packet = new Packet(PacketType.packet_test_packet);
            packet.WriteInt("int", 2147483647);
            packet.WriteShort("short", 32767);
            packet.WriteLong("long", 9223372036854775807);
            packet.WriteDouble("double", 1.7976931348623157E+308);
            packet.WriteFloat("float", (float)3.4028235E+38);
            packet.WriteString("string", "testowanie pakietów");

            byte[] data = PacketSerializationManager.serializePacket(packet);
            stream.Write(data, 0, data.Length);
        }
        static void Main(string[] args)
        {
            //try
            //{
                ClientBase client = new ClientBase();
                client.Start("localhost", 8050);
           /* }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Thread.Sleep(10000);
            }*/
        }
    }
}