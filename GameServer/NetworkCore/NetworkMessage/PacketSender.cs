using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;

namespace NetworkCore.NetworkMessage
{
    public class PacketSender
    {
        private ConcurrentQueue<OwnedPacket> packetQueue = new ConcurrentQueue<OwnedPacket>();
        
        private CancellationTokenSource cancellationSource = new CancellationTokenSource();
        private List<Task> processingTasks = new List<Task>();

        public delegate void PacketSentInfo(string info);
        public event PacketSentInfo? OnPacketSent;

        public PacketSender()
        {
            StartProcessing();
        }
        public void EnqueuePacket(OwnedPacket ownedPacket)
        {
            packetQueue.Enqueue(ownedPacket);
        }

        public void EnqueuePacket(IPeer peer, PacketBase packet)
        {
            packetQueue.Enqueue(new OwnedPacket {Peer = peer, PeerPacket = packet });
        }

        private void StartProcessing()
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
                if (packetQueue.TryDequeue(out OwnedPacket sender))
                {
                    byte[] dataToSend = sender.PeerPacket.Serialize();
                    await sender.Peer.PeerSocket.SendAsync(new ArraySegment<byte>(dataToSend), SocketFlags.None);
                    OnPacketSent?.Invoke(sender.PeerPacket.GetInfo());
                }
                else
                    await Task.Delay(1);
            }
        }
    }
}
