using NetworkCore.NetworkMessage;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Net.Sockets;
using System.Diagnostics;

namespace NetworkCore.NetworkMessage
{
    public class PacketHandler
    {
        private Dictionary<Guid, Dictionary<Type, Delegate>> eventHandlers
            = new Dictionary<Guid, Dictionary<Type, Delegate>>();

        private ConcurrentQueue<OwnedPacket> packetQueue = new ConcurrentQueue<OwnedPacket>();
        private ConcurrentBag<OwnedPacket> responsePackets = new ConcurrentBag<OwnedPacket>();

        private CancellationTokenSource cancellationSource = new CancellationTokenSource();
        private List<Task> processingTasks = new List<Task>();

        private TimeSpan timeLimitForResponse = TimeSpan.FromSeconds(10);

        public delegate void PacketReceivedInfo(string info);
        public event PacketReceivedInfo? OnPacketReceived;

        public PacketHandler()
        {
            StartProcessing();
        }

        public void AddHandler(Guid peerId, Type packetType, Delegate handler)
        {
            if (!eventHandlers.ContainsKey(peerId))
            {
                eventHandlers[peerId] = new Dictionary<Type, Delegate>();
            }

            eventHandlers[peerId][packetType] = handler;
        }

        public void RemoveHandler(Guid peerId, Type packetType)
        {
            if (eventHandlers.ContainsKey(peerId))
            {
                eventHandlers[peerId].Remove(packetType);
            }
        }

        public void AddPacket(OwnedPacket ownedPacket)
        {
            if (ownedPacket.PeerPacket.IsResponse)
                responsePackets.Add(ownedPacket);
            else
                packetQueue.Enqueue(ownedPacket);
        }

        public void StartProcessing()
        {
            for (int i = 0; i < Environment.ProcessorCount; i++)
            {
                processingTasks.Add(Task.Run(ProcessPackets, cancellationSource.Token));
            }
        }

        public void StopProcessing()
        {
            cancellationSource.Cancel();
            Task.WhenAll(processingTasks).Wait();
        }

        private async Task ProcessPackets()
        {
            while (!cancellationSource.Token.IsCancellationRequested)
            {
                if (packetQueue.TryDequeue(out OwnedPacket ownedPacket))
                {
                    if (eventHandlers.ContainsKey(ownedPacket.Peer.GUID) && eventHandlers[ownedPacket.Peer.GUID].
                        ContainsKey(ownedPacket.PeerPacket.GetType()))
                    {
                        eventHandlers[ownedPacket.Peer.GUID][ownedPacket.PeerPacket.GetType()].
                            DynamicInvoke(ownedPacket.PeerPacket);

                        OnPacketReceived?.Invoke(ownedPacket.PeerPacket.GetInfo());
                    }
                }
                else
                    await Task.Delay(1);
            }
        }

        public async Task<PacketBase> WaitForResponsePacket(Guid peerId, PacketType responseType)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            while (timer.Elapsed < timeLimitForResponse)
            {
                if (responsePackets.TryTake(out OwnedPacket ownedPacket) &&
                    ownedPacket.Peer.GUID == peerId &&
                    ownedPacket.PeerPacket.TypeId == responseType)
                {
                    return ownedPacket.PeerPacket;
                }
                else
                    await Task.Delay(1);
            }

            throw new TimeoutException("Timeout occurred while trying to get response packet.");
        }
    }
}

    