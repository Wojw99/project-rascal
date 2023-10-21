using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkCommunication
{
    public class TcpPeer : IPeer
    {
        public bool IsProxy => false;

        public bool IsConnected { get; private set; }

        public Guid GUID { get; private set; }

        public Owner OwnerType { get; private set; }

        public Socket PeerSocket { get; private set; }

        private NetworkBase NetworkRef { get; set; } // storing reference to NetworkServer or NetworkClient

        public TcpPeer(NetworkBase networkBase, Socket peerSocket, Guid peerId, 
            Owner ownerType)
        {
            NetworkRef = networkBase;
            PeerSocket = peerSocket;
            GUID = peerId;
            OwnerType = ownerType;
        }

        public void StartRead()
        {
            if(IsConnected == true)
            {
                Task handleReadIncomingTcpData = Task.Run(async () => await ReadIncomingData());
            }
        }

        public void Connect()
        {
            if(!IsConnected)
            {
                IsConnected = true;
            }
        }

        public void Disconnect()
        {
            if(IsConnected)
            {
                IsConnected = false;
            }
        }

        public async Task SendPacket(PacketBase packet)
        {
            if(IsConnected)
            {
                await NetworkRef.SendOutgoingPacket(new OwnedPacket { Peer = this, PeerPacket = packet });
                //NetworkRef.AddToOutgoingPacketQueue(this, packet);
            }
        }

        private async Task ReadIncomingData()
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                while (IsConnected)
                { 
                    memoryStream.SetLength(0);

                    byte[] packetSizeBytes = new byte[sizeof(int)];
                    int bytesRead = await PeerSocket.ReceiveAsync(new ArraySegment<byte>(packetSizeBytes), SocketFlags.None);

                    int packetSize = BitConverter.ToInt32(packetSizeBytes, 0);

                    byte[] packetData = new byte[packetSize - sizeof(int)];

                    bytesRead = await PeerSocket.ReceiveAsync(new ArraySegment<byte>(packetData), SocketFlags.None);

                    if (bytesRead <= 0)
                    {
                        await Console.Out.WriteLineAsync("No data received");
                        continue;
                    }

                    Memory<byte> combinedDataMemory = new byte[packetSize];
                    packetSizeBytes.CopyTo(combinedDataMemory);
                    packetData.CopyTo(combinedDataMemory.Slice(sizeof(int)));

                    byte[] packetBuffer = combinedDataMemory.ToArray();

                    // On 4 index in array is packet type
                    PacketType receivedPacketType = (PacketType)packetBuffer[4];

                    NetworkRef._PacketHandler.AddPacket(new OwnedPacket { Peer = this, 
                        PeerPacket = PacketBase.CreatePacketFromType(receivedPacketType, packetBuffer)});
                }
            }
        }
    }
}
