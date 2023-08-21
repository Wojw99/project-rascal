using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkMessage.old;
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

        public Guid Id { get; private set; }

        public Owner OwnerType { get; private set; }

        public Socket PeerSocket { get; private set; }

        private NetworkBase NetworkRef { get; set; } // storing reference to NetworkServer or NetworkClient

        public TcpPeer(NetworkBase networkBase, Socket peerSocket, Guid peerId, 
            Owner ownerType)
        {
            NetworkRef = networkBase;
            PeerSocket = peerSocket;
            Id = peerId;
            OwnerType = ownerType;
        }

        public async Task ConnectToServer()
        {
            if(OwnerType == Owner.client)
            {
                IsConnected = true;
                await Console.Out.WriteLineAsync("Connection approved.");
                Task handleReadIncomingTcpData = Task.Run(async () => await ReadIncomingData());
            }
        }

        public async Task ConnectToClient()
        {
            if(OwnerType == Owner.server)
            {
                IsConnected = true;
                await Console.Out.WriteLineAsync("Connection approved.");
                Task handleReadIncomingTcpData = Task.Run(async () => await ReadIncomingData());
            }
        }

        public async Task Disconnect()
        {
            IsConnected = false;
            await Console.Out.WriteLineAsync("Connection closed.");
        }

        public async Task SendPacket(PacketBase packet)
        {
            if(IsConnected)
            {
                //NetworkRef.Watch.Start();
                await AddToOutgoingPacketQueue(packet);
            }
            // LOGGER: Console.WriteLine($"[SEND] Packet with type: {packet._type} was sent to peer with Guid: {this.Id}");
        }

        private async Task ReadIncomingData()
        {
            while (NetworkRef.IsRunning)
            {
                // On first 4 bytes we storing size of all serialized data in packet.
                // So by first we receive packet size to know what amount of bytes to read,
                // and transform binary data into correct packet.
                byte[] PacketSizeByte = new byte[sizeof(int)];
                int bytesRead = await PeerSocket.ReceiveAsync(new ArraySegment<byte>(PacketSizeByte), SocketFlags.None);

                if (bytesRead != sizeof(int))
                {
                    await Console.Out.WriteLineAsync("Incorrect size of packet size");
                    continue;
                }

                int packetSize = BitConverter.ToInt32(PacketSizeByte, 0);

                if (packetSize <= sizeof(int))
                {
                    await Console.Out.WriteLineAsync("Incorrect size of packet");
                    continue;
                }

                byte[] packetData = new byte[packetSize - sizeof(int)];

                bytesRead = await PeerSocket.ReceiveAsync(new ArraySegment<byte>(packetData), SocketFlags.None);

                if (bytesRead <= 0)
                {
                    await Console.Out.WriteLineAsync("No data received");
                    continue;
                }

                byte[] combinedData = PacketSizeByte.Concat(packetData).ToArray();

                // Packet Type is on 5 field of array (check PacketBase class for serializing)
                PacketType receivedPacketType = (PacketType)combinedData[4];

                PacketBase recognizedPacket = LoadPacket(receivedPacketType, combinedData);
                await AddToIncomingPacketQueue(recognizedPacket);
            }
        }

        private PacketBase LoadPacket(PacketType packetType, byte[] receivedData)
        {
            switch (packetType)
            {
                //case PacketType.LOGIN_REQUEST:
                //    break;

                //case PacketType.LOGIN_RESPONSE:
                //    break;

                case PacketType.CHARACTER_LOAD_REQUEST:
                    return new CharacterLoadRequestPacket(receivedData);

                case PacketType.CHARACTER_LOAD_RESPONSE:
                    return new CharacterLoadResponsePacket(receivedData);

                case PacketType.CHARACTER_LOAD_SUCCES:

                    return new CharacterLoadSuccesPacket(receivedData);

                case PacketType.CHARACTER_STATE_PACKET:
                    return new CharacterStatePacket(receivedData);

                case PacketType.CHARACTER_MOVE_PACKET:
                    return new CharacterMovePacket(receivedData);

                case PacketType.CLIENT_DISCONNECT:
                    return new ClientDisconnectPacket(receivedData);

                default:
                    throw new ArgumentException("Uknown packet type");
            }
        }

        private async Task AddToIncomingPacketQueue(PacketBase packet)
        {
            lock (this)
            {
                NetworkRef.qPacketsIn.Enqueue(new OwnedPacket { Peer = this, PeerPacket = packet });
            }
        }

        private async Task AddToOutgoingPacketQueue(PacketBase packet)
        {
            NetworkRef.qPacketsOut.Enqueue(new OwnedPacket { Peer = this, PeerPacket = packet });
        }
    }
}
