using NetworkCore.NetworkMessage;
using NetworkCore.NetworkConfig;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using NetworkCore.NetworkMessage.old;
using System.Threading;

namespace NetworkCore.NetworkCommunication
{
    public abstract class NetworkBase
    {
        //private readonly object IncomingLock = new object();

        //private readonly object OutgoingLock = new object();
        private ConcurrentQueue<OwnedPacket> qPacketsIn { get; set; } 
            = new ConcurrentQueue<OwnedPacket>();

        private ConcurrentQueue<OwnedPacket> qPacketsOut { get; set; } 
            = new ConcurrentQueue<OwnedPacket>();

        // If errors with that ResponsePackets dictionary - move it to TcpPeer class. The problem can
        // be with adding to that dictionary packets with same keys and from another peers.
        public ConcurrentDictionary<PacketType, OwnedPacket> ResponsePackets { get; set; } 
            = new ConcurrentDictionary<PacketType, OwnedPacket>();

        public bool IsRunning { get; set; }

        public UInt32 InPacketCounter { get; private set; } = 0;

        public UInt32 OutPacketCounter { get; private set; } = 0;

        public abstract Task OnPacketReceived(IPeer Peer, PacketBase packet);

        protected NetworkBase()
        {
        }

        public async Task SendOutgoingPacket(OwnedPacket receiver)
        {
            byte[] dataToSend = receiver.PeerPacket.Serialize();
            await receiver.Peer.PeerSocket.SendAsync(new ArraySegment<byte>(dataToSend), SocketFlags.None);
            //await Console.Out.WriteLineAsync($"[SEND] packed with type: {receiver.PeerPacket.TypeId} from peer with Guid: {receiver.Peer.Id}");
        }

        public void RunPacketProcessingInBackground(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval)
        {
            //var processIncomingTask = Task.Run(ProcessIncomingPackets(maxIncomingPacketCount, packetProcessInterval));
            //var processOutgoingTask = Task.Run(ProcessOutgoingPackets(maxOutgoingPacketCount, packetProcessInterval));

            var processIncomingTask = Task.Run(() => ProcessIncomingPackets(maxIncomingPacketCount, packetProcessInterval));
            var processOutgoingTask = Task.Run(() => ProcessOutgoingPackets(maxOutgoingPacketCount, packetProcessInterval));
        }

        private protected async Task ProcessIncomingPackets(UInt32 maxIncomingPacketCount, TimeSpan packetProcessInterval)
        {
            while (IsRunning)
            {
                
                UInt32 packetCount = 0;

                while (packetCount < maxIncomingPacketCount && !qPacketsIn.IsEmpty)
                {
                    if (qPacketsIn.TryDequeue(out var ownedPacket))
                    {
                        await OnPacketReceived(ownedPacket.Peer, ownedPacket.PeerPacket);

                        packetCount++;
                    }
                }

                InPacketCounter += packetCount;


                //await Task.Delay(PacketProcessInterval);
                Thread.Sleep(packetProcessInterval);
            }
        }

        private protected async Task ProcessOutgoingPackets(UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval)
        {
            while (IsRunning)
            {
                UInt32 packetCount = 0;

                while (packetCount < maxOutgoingPacketCount && !qPacketsOut.IsEmpty)
                {
                    if (qPacketsOut.TryDequeue(out OwnedPacket outgoingPacket))
                    {
                        await SendOutgoingPacket(outgoingPacket);
                        packetCount++;
                    }
                }

                OutPacketCounter += packetCount;

                //await Task.Delay(PacketProcessInterval);
                Thread.Sleep(packetProcessInterval);
            }
        }

        public void AddToIncomingPacketQueue(IPeer peer, PacketBase packet)
        {
            qPacketsIn.Enqueue(new OwnedPacket { Peer = peer, PeerPacket = packet });
        }

        public void AddToOutgoingPacketQueue(IPeer peer, PacketBase packet)
        {
            qPacketsOut.Enqueue(new OwnedPacket { Peer = peer, PeerPacket = packet });
        }

        public void AddToResponsePacketsCollection(IPeer peer, PacketBase packet)
        {
            ResponsePackets.TryAdd(packet.TypeId, new OwnedPacket { Peer = peer, PeerPacket = packet });
        }

        public async Task<PacketBase> WaitForResponsePacket(TimeSpan interval, TimeSpan timeLimit, PacketType packetType)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            while(timer.ElapsedMilliseconds < timeLimit.TotalMilliseconds)
            {
                if (ResponsePackets.TryRemove(packetType, out OwnedPacket packet))
                {
                    return packet.PeerPacket;
                }

                await Task.Delay(interval);
            }
            throw new TimeoutException("Timeout occured while trying to get response packet. ");
        }
    }
}
