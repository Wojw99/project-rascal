﻿using NetworkCore.NetworkMessage;
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

namespace NetworkCore.NetworkCommunication
{
    public abstract class NetworkBase
    {
        //private readonly object IncomingLock = new object();

        //private readonly object OutgoingLock = new object();
        public ConcurrentQueue<OwnedPacket> qPacketsIn { get; set; } = new ConcurrentQueue<OwnedPacket>();

        public ConcurrentQueue<OwnedPacket> qPacketsOut { get; set; } = new ConcurrentQueue<OwnedPacket>();

        public bool IsRunning { get; set; }

        private UInt32 MaxIncomingPacketCount { get; set; }

        private UInt32 MaxOutgoingPacketCount { get; set; }

        private TimeSpan PacketProcessInterval { get; set; }

        public UInt32 InPacketCounter { get; private set; } = 0;
        public UInt32 OutPacketCounter { get; private set; } = 0;

        public abstract Task OnPacketReceived(IPeer Peer, PacketBase packet);

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
        public async Task SendOutgoingPacket(OwnedPacket receiver)
        {
            byte[] dataToSend = receiver.PeerPacket.Serialize();
            await receiver.Peer.PeerSocket.SendAsync(new ArraySegment<byte>(dataToSend), SocketFlags.None);
            await Console.Out.WriteLineAsync($"[SEND] packed with type: {receiver.PeerPacket.TypeId} from peer with Guid: {receiver.Peer.Id}");
        }

        // "InBackground" mean that this method run packet processing methods on separate tasks.
        public async Task RunPacketProcessingInBackground()
        {
            var processIncomingTask = Task.Run(ProcessIncomingPackets);
            var processOutgoingTask = Task.Run(ProcessOutgoingPackets);
        }

        private protected async Task ProcessIncomingPackets()
        {
            while (IsRunning)
            {
                
                UInt32 packetCount = 0;

                while (packetCount < MaxIncomingPacketCount && !qPacketsIn.IsEmpty)
                {
                    if (qPacketsIn.TryDequeue(out var ownedPacket))
                    {
                        await OnPacketReceived(ownedPacket.Peer, ownedPacket.PeerPacket);
                        packetCount++;
                    }
                }

                InPacketCounter += packetCount;


                await Task.Delay(PacketProcessInterval);
            }
        }

        private protected async Task ProcessOutgoingPackets()
        {
            while (IsRunning)
            {
                UInt32 packetCount = 0;

                while (packetCount < MaxOutgoingPacketCount && !qPacketsOut.IsEmpty)
                {
                    if (qPacketsOut.TryDequeue(out OwnedPacket outgoingPacket))
                    {
                        await SendOutgoingPacket(outgoingPacket);
                        packetCount++;
                    }
                }

                OutPacketCounter += packetCount;

                await Task.Delay(PacketProcessInterval);
               
            }
        }
    }
}
