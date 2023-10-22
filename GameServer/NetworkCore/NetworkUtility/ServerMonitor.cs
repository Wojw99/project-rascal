/*using NetworkCore.NetworkCommunication;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkUtility
{
    public class ServerMonitor
    {
        public TcpNetworkServer ServerRef { get; }

        public ServerMonitor(TcpNetworkServer serverRef)
        {
            ServerRef = serverRef;
        }

        public async Task ShowServerInfo()
        {
            if(ServerRef.IsRunning())
                await Console.Out.WriteLineAsync("Server status: RUNNING. ");
            else
                await Console.Out.WriteLineAsync("Server status: CLOSED. ");

            await Console.Out.WriteLineAsync($"Total packets received: {ServerRef.InPacketCounter}");
            await Console.Out.WriteLineAsync($"Total packets send: {ServerRef.OutPacketCounter}");
            //await Console.Out.WriteLineAsync($"Incoming Packet Queue size: {ServerRef.qPacketsIn.Count}");
            //await Console.Out.WriteLineAsync($"Outgoing Packet Queue size:{ServerRef.qPacketsOut.Count}");
        }

       *//* public static async Task <long>SendPingRequest(TcpPeer serverPeer)
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
        }*//*
    }
}
*/