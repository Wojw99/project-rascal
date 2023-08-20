/*using NetworkCore.NetworkCommunication;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkUtility
{
    public static class ServerUtility
    {
        public static async Task <long>SendPingRequest(TcpPeer serverPeer)
        {
            Stopwatch Watch = new Stopwatch();
            Watch.Start();
            await serverPeer.SendPacket(new PingRequestPacket());
            Watch.Stop();
            return Watch.ElapsedMilliseconds;
        }

        // Note that we must handle PingServerPacket in OnPacketReceived and run that function.
        public static async Task ReceivePingResponse(PingResponsePacket pingPacket)
        {
            Watch.Stop();
            await Console.Out.WriteLineAsync($"Pinged peer with time = {Watch.ElapsedMilliseconds} ms.");
            Watch.Reset();
        }
    }
}
*/