/*
 *  IMPORTANT INFORMATION: 
 * 
 *  My intention for the "NetworkCore" project is to be common code 
 *  for client and server projects. So that project is "Class Library" type, 
 *  and when compiled, there is generating .dll file with that library.
 * 
 *  In "TEST" project I linked that library, so we can use classes from
 *  that project.
 *  
 */


using NetworkCore;

namespace TEST
{
    internal class PacketOperationExample
    {
        static void Main(string[] args)
        {
            Packet packet = new Packet(PacketType.packet_player_move);

            packet.WriteInt("ID", 12412);
            packet.WriteString("name", "Adam");
            packet.WriteInt("age", 14);

            Console.WriteLine(packet.ReadField<int>("ID"));
            Console.WriteLine(packet.ReadField<string>("name"));
            Console.WriteLine(packet.ReadField<int>("age"));

            Console.WriteLine("Hello, World!");
        }
    }
}