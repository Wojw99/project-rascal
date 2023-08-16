using NetworkCore.NetworkMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkCommunication
{
    public abstract class NetworkBase
    {
        public Stopwatch Watch { get; } = new Stopwatch(); // testing speed

        private readonly object IncomingLock = new object();
        private readonly object OutgoingLock = new object();
        public ConcurrentQueue<OwnedPacket> qPacketsIn { get; set; } = new ConcurrentQueue<OwnedPacket>();
        public ConcurrentQueue<OwnedPacket> qPacketsOut { get; set; } = new ConcurrentQueue<OwnedPacket>();
        public bool IsRunning { get; set; }

        public abstract Task SendOutgoingPacket(OwnedPacket receiver);
        public abstract Task OnPacketReceived(IPeer clientPeer, Packet packet);

        public async Task Update(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval = default)
        {
            Task.Run(async () =>
            {
                await ProcessIncomingPackets(maxIncomingPacketCount, packetProcessInterval);
            });

            Task.Run(async () =>
            {
                await ProcessOutgoingPackets(maxOutgoingPacketCount, packetProcessInterval);
            });
        }

        private protected async Task ProcessIncomingPackets(UInt32 maxIncomingPacketCount, TimeSpan packetProcessInterval = default)
        {

            while (IsRunning)
            {
                
                UInt32 packetCount = 0;

                while (packetCount < maxIncomingPacketCount && !qPacketsIn.IsEmpty)
                {
                    lock(IncomingLock)
                    {
                        if (qPacketsIn.TryDequeue(out var ownedPacket))
                        {
                            OnPacketReceived(ownedPacket.Peer, ownedPacket.PeerPacket);
                            packetCount++;
                        }
                    }
                }

                if (packetProcessInterval != default)
                {
                    await Task.Delay(packetProcessInterval);
                }
                
            }
        }

        private protected async Task ProcessOutgoingPackets(UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval = default)
        {
            while (IsRunning)
            {
                while (!qPacketsOut.IsEmpty)
                {
                    lock (OutgoingLock)
                    {
                        if (qPacketsOut.TryDequeue(out OwnedPacket outgoingPacket))
                        {
                            SendOutgoingPacket(outgoingPacket); // Przykładowa metoda wysyłająca wychodzące pakiety
                        }
                    }
                    
                }

                if (packetProcessInterval != default)
                {
                    await Task.Delay(packetProcessInterval);
                }
            }
        }
    }
}
