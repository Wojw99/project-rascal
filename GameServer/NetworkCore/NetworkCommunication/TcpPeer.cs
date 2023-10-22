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
            while (IsConnected)
            { 
                // Receive first 4 bytes to read packet size.
                byte[] packetSizeInBytes = await PacketBase.ReceiveDataFromSocket(PeerSocket, 4);
                int packetSize = BitConverter.ToInt32(packetSizeInBytes);

                // Receive other bytes by: (packetSize) - (size_which_we_arleady_received)
                byte[] packetData = await PacketBase.ReceiveDataFromSocket(PeerSocket, packetSize - sizeof(int));

                // Connect both into one array.
                Memory<byte> combinedDataMemory = new byte[packetSize];
                packetSizeInBytes.CopyTo(combinedDataMemory);
                packetData.CopyTo(combinedDataMemory.Slice(sizeof(int)));
                byte[] resultPacket = combinedDataMemory.ToArray();

                // Get packet type.
                PacketType type = PacketBase.GetPacketTypeFromBytes(resultPacket);

                //Add packet to PacketHandler.
                NetworkRef._PacketHandler.AddPacket(new OwnedPacket { Peer = this, 
                    PeerPacket = PacketBase.CreatePacketFromType(type, resultPacket)});
            }
        }
    }
}
