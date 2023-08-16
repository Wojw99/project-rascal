/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using ServerApplication.GameService;
using static System.Net.Mime.MediaTypeNames;

namespace ServerApplication.Old
{
    public class Player
    {
        GlobalData GlobalPlayerPosition = new GlobalData();
        NetworkServer networkRef;

        public static void HandleEnemyShootPacket(Packet packet)
        {
            int playerId = packet.Read<int>("playerId");
            int targetId = packet.Read<int>("targetId");

            //Console.WriteLine($"Packet received: [playerId: {playerId}, targetId: {targetId}]");
            // wykonaj działania / wywołaj funkcje z innego namespace/folderu

        }



        public void HandlePlayerMovePacket(IPeer peer, PlayerMovePacket packet)
        {
            networkRef.SendPacketToAllClients(packet);

            Console.WriteLine($"Details: {packet.ToString()}");

            peer.SendPacket(packet);


            // 1. dodaj dane z PlayerMovePacket do globalnej struktury

            // 2. zaserializuj dane z globalnej struktury do GlobalPlayerPositionPacket

            // 3. Wyślij pakiet z odpowiedzią

            //GlobalPlayerPositionPacket responsePacket = new GlobalPlayerPositionPacket();

            //peer.SendPacket(responsePacket);
            // dodaj do globalnych danych



            // wykonaj działania / wywołaj funkcje z innego namespace/folderu
        }


        public static Packet HandleGlobalPlayerPosition()
        {
            Packet packet = new Packet(PacketType.packet_global_player_position);

            packet.Write("playerNum", GlobalPlayerPositions.Count);
            foreach (var playerPos in GlobalPlayerPositions)
            {
                packet.Write("playerId", playerPos.Key);
                packet.Write("posX", playerPos.Value.Item1);
                packet.Write("posY", playerPos.Key);
            }

            return packet;
        }

        public static void HandleTestPacket(Packet packet)
        {
            int n1 = packet.Read<int>("int");
            short n2 = packet.Read<short>("short");
            long n3 = packet.Read<long>("long");
            double n4 = packet.Read<double>("double");
            float n5 = packet.Read<float>("float");
            string n6 = packet.Read<string>("string");

            //Console.WriteLine($"Packet received: [int: {n1}, short: {n2}, long: {n3}, double: {n4}, float: {n5}, string: {n6}]");
            // wykonaj działania / wywołaj funkcje z innego namespace/folderu       
        }
    }
}
*/