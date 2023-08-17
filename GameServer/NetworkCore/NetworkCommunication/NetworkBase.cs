using NetworkCore.NetworkMessage;
using NetworkCore.NetworkConfig;
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

        private UInt32 MaxIncomingPacketCount { get; set; }

        private UInt32 MaxOutgoingPacketCount { get; set; }

        private TimeSpan PacketProcessInterval { get; set; }

        public abstract Task SendOutgoingPacket(OwnedPacket receiver);

        public abstract Task OnPacketReceived(IPeer clientPeer, Packet packet);

        protected NetworkBase()
        {
            MaxIncomingPacketCount = Constants.DEFAULT_MAX_INCOMING_PACKET_COUNT;
            MaxOutgoingPacketCount = Constants.DEFAULT_MAX_OUTGOING_PACKET_COUNT;
            PacketProcessInterval = TimeSpan.FromMilliseconds(Constants.DEFAULT_PACKET_PROCESS_INTERVAL_MS);
        }

        protected NetworkBase(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval)
        {
            if (packetProcessInterval == default)
                PacketProcessInterval = TimeSpan.FromMilliseconds(Constants.DEFAULT_PACKET_PROCESS_INTERVAL_MS);
            else PacketProcessInterval = packetProcessInterval;

            if (maxIncomingPacketCount == 0 || maxOutgoingPacketCount == 0)
                throw new ArgumentException("maxIncomingPacketCount and maxOutgoingPacketCount values must be greater than 0.");

            MaxIncomingPacketCount = maxIncomingPacketCount;
            MaxOutgoingPacketCount = maxOutgoingPacketCount;

            // LOGGER : INITIALIZED NetworkBase with default values
        }

        public async Task RunPacketProcessingInBackground()
        {
            /*Task.Run(async () =>
            {
                await ProcessIncomingPackets();
            });

            Task.Run(async () =>
            {
                await ProcessOutgoingPackets();
            });*/

            var processIncomingTask = Task.Run(ProcessIncomingPackets);
            var processOutgoingTask = Task.Run(ProcessOutgoingPackets);

            //await Task.WhenAll(processIncomingTask, processOutgoingTask); 
        }

        private protected async Task ProcessIncomingPackets()
        {

            while (IsRunning)
            {
                
                UInt32 packetCount = 0;

                while (packetCount < MaxIncomingPacketCount && !qPacketsIn.IsEmpty)
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


                await Task.Delay(PacketProcessInterval);
            }
        }

        private protected async Task ProcessOutgoingPackets()
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

                await Task.Delay(PacketProcessInterval);
               
            }
        }
    }
}
