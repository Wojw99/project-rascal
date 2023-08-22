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

        public void StartRead()
        {
            if(IsConnected == true)
            {
                Task handleReadIncomingTcpData = Task.Run(async () => await ReadIncomingData());
            }
        }

/*        public async Task ConnectToClient()
        {
            if(IsConnected == false)
            {
                if(OwnerType == Owner.server)
                {
                    IsConnected = true;
                    await Console.Out.WriteLineAsync("Connection approved.");
                    Task handleReadIncomingTcpData = Task.Run(async () => await ReadIncomingData());
                }
            }
        }*/

        public void Connect()
        {
            if(!IsConnected)
            {
                IsConnected = true;
            }
            //await Console.Out.WriteLineAsync("Connection open.");
        }

        public void Disconnect()
        {
            if(IsConnected)
            {
                IsConnected = false;
            }
            //await Console.Out.WriteLineAsync("Connection closed.");
        }

        public void SendPacket(PacketBase packet)
        {
            if(IsConnected)
            {
                NetworkRef.AddToOutgoingPacketQueue(this, packet);
            }
        }

        private async Task ReadIncomingData()
        {
            while (IsConnected)
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    // Read only 4 bytes to packetSizeBytes (Packet Size is serialized on first 4 bytes of packet).
                    byte[] packetSizeBytes = new byte[sizeof(int)];
                    await PeerSocket.ReceiveAsync(new ArraySegment<byte>(packetSizeBytes), SocketFlags.None);

                    // convert byte array with 4 fields to int.
                    int packetSize = BitConverter.ToInt32(packetSizeBytes, 0);


                    int bytesRead = 0;
                    byte[] buffer = new byte[1024]; 

                    // read a whole packet
                    while (bytesRead < packetSize - sizeof(int))
                    {
                        int bytesReceived = await PeerSocket.ReceiveAsync(new ArraySegment<byte>(buffer), SocketFlags.None);
                        if (bytesReceived <= 0)
                        {
                            await Console.Out.WriteLineAsync("No data received");
                            break;
                        }
                        await memoryStream.WriteAsync(buffer, 0, bytesReceived);
                        bytesRead += bytesReceived;
                    }

                    // I dont know is that correct. My plan is read data untill we dont have full packet.
                    if (bytesRead < packetSize - sizeof(int))
                    {
                        await Console.Out.WriteLineAsync("Insufficient data received");
                        continue; 
                    }

                    // Combined data into single packet data
                    byte[] combinedData = packetSizeBytes.Concat(memoryStream.ToArray()).ToArray();

                    // On 4 index in array is packet type
                    PacketType receivedPacketType = (PacketType)combinedData[4];

                    // Load the derivative packet by type and correct packet.
                    PacketBase recognizedPacket = LoadPacket(receivedPacketType, combinedData);

                    // Assign complete packet to queue (complete mean to create derivative from base).
                    NetworkRef.AddToIncomingPacketQueue(this, recognizedPacket);
                }
            }
        }

        /*        private async Task ReadIncomingData()
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

                        //final check is size of packet correct.
                        if(combinedData.Length != packetSize)
                        {
                            await Console.Out.WriteLineAsync("Expected size and size of packet not match.");
                            continue;
                        }

                        PacketType receivedPacketType = (PacketType)combinedData[4];
                        // Packet Type is on 5 field of array (check PacketBase class for serializing)

                        PacketBase recognizedPacket = LoadPacket(receivedPacketType, combinedData);
                        await AddToIncomingPacketQueue(recognizedPacket);

                    }
                }*/

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
    }
}
