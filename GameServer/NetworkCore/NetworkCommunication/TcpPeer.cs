using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        public INetworkBase NetworkRef { get; private set; } // storing reference to NetworkServer or NetworkClient

        private byte[] ReceiveBuffer = new byte[1024];

        public TcpPeer(INetworkBase networkBase, Socket peerSocket, Guid peerId, 
            Owner ownerType)
        {
            NetworkRef = networkBase;
            PeerSocket = peerSocket;
            Id = peerId;
            OwnerType = ownerType;
        }

        public void ConnectToServer()
        {
            if(OwnerType == Owner.client)
            {
                IsConnected = true;
                Task handleReadIncomingTcpData = Task.Run(async () => await ReadIncomingData());
            }
        }

        public void ConnectToClient()
        {
            if(OwnerType == Owner.server)
            {
                IsConnected = true;
                Task handleReadIncomingTcpData = Task.Run(async () => await ReadIncomingData());
            }
        }

        public void Disconnect()
        {
            IsConnected = false;
            //_TcpClient.Close();
        }

        public async Task SendPacket(Packet packet)
        {
            byte[] dataToSend = PacketSerializationManager.serializePacket(packet);
            await PeerSocket.SendAsync(new ArraySegment<byte>(dataToSend), SocketFlags.None);
            // LOGGER: Console.WriteLine($"[SEND] Packet with type: {packet._type} was sent to peer with Guid: {this.Id}");
        }

        public async Task ReadIncomingData()
        {
            while (NetworkRef.IsRunning)
            {
                byte[] PacketSizeByte = new byte[sizeof(int)];
                // int bytesRead =
                int bytesRead = await PeerSocket.ReceiveAsync(new ArraySegment<byte>(PacketSizeByte), SocketFlags.None);

                if(bytesRead != sizeof(int))
                {
                    await Console.Out.WriteLineAsync("");
                    continue;
                }

                int packetSize = BitConverter.ToInt32(PacketSizeByte, 0);

                if (packetSize <= 0)
                {
                    await Console.Out.WriteLineAsync("Incorrect size of packet");
                    continue;
                }

                // disregard PacketSizeByte
                byte[] packetData = new byte[packetSize - sizeof(int)];

                bytesRead = await PeerSocket.ReceiveAsync(new ArraySegment<byte>(packetData), SocketFlags.None);

                if (bytesRead <= 0)
                {
                    await Console.Out.WriteLineAsync("");
                    continue;
                }

                
                byte[] combinedData = PacketSizeByte.Concat(packetData).ToArray();

                Packet packet = PacketSerializationManager.DeserializeByteData(combinedData);
                await AddToIncomingPacketQueue(packet);

                //await ProcessReceivedData(bytesRead);
            }
        }

        /*private async Task ProcessReceivedData(int bytesRead)
        {
            if (bytesRead > 0)
            {
                // Assuming the first 4 bytes represent the packet size
                int packetSize = BitConverter.ToInt32(ReceiveBuffer, 0);

                if (packetSize <= bytesRead)
                {
                    byte[] packetBuffer = new byte[packetSize];
                    Buffer.BlockCopy(ReceiveBuffer, 0, packetBuffer, 0, packetSize);

                    Packet packet = PacketSerializationManager.DeserializeByteData(packetBuffer);
                    await AddToIncomingPacketQueue(packet);

                    int remainingDataSize = bytesRead - packetSize;
                    if (remainingDataSize > 0)
                    {
                        Buffer.BlockCopy(ReceiveBuffer, packetSize, ReceiveBuffer, 0, remainingDataSize);
                    }

                    await ProcessReceivedData(remainingDataSize);
                }
                else
                {
                    // Not enough data received yet, wait for more
                    await ReadIncomingData();
                }
            }
            else
            {
                // Connection closed or error occurred
                //Console.WriteLine("Connection closed or error occurred.");
                // Handle disconnection or other actions
            }
        }*/

        public async Task AddToIncomingPacketQueue(Packet packet)
        {
            NetworkRef.qPacketsIn.Enqueue(new OwnedPacket { Peer = this, Packet = packet });
            
            /*if(OwnerType == Owner.server)
            {
            }
            else
            {
                NetworkRef.qPacketsIn.Enqueue(new OwnedPacket { Peer = null , Packet = packet });
            }*/
        }

        /*private void HandleReceivedData(IAsyncResult result)
        {
            if (_TcpClient.Connected)
            {
                int bytesRead = _TcpClient.GetStream().EndRead(result);

                if(bytesRead > 0)
                {
                    // first 4 bytes are the serialized packet size
                    int packetSize = BitConverter.ToInt32(ReceiveBuffer, 0); // 0 -4 bajtów

                    // creating new buffor, according to packet size.
                    byte[] packetBuffor = new byte[packetSize];

                    if (ReceiveBuffer.Length >= packetSize)
                    {
                        // copy 'packetSize' elements from main buffor to buffor, which can be serialized.
                        Buffer.BlockCopy(ReceiveBuffer, 0, packetBuffor, 0, packetSize);
                        Packet packet = PacketSerializationManager.DeserializeByteData(packetBuffor);
                        
                        // substraction of processed data
                        int remainingDataSize = ReceiveBuffer.Length - packetSize;
                        Buffer.BlockCopy(ReceiveBuffer, packetSize, ReceiveBuffer, 0, remainingDataSize);

                        //HandlePacket(packet); 
                        // I'm using a reference to PacketHandlerManager object from GameServer class, so it can be done.
                        PacketHandler handler = _PacketHandlerManager.GetHandler(packet._type);

                        handler.HandleRequest(packet);
 
                        SendPacket(handler.HandleResponse());

                        // Idk if we need it.
                        //Array.Resize(ref ReceiveBuffer, remainingDataSize); // Skrócenie bufora do nowego rozmiaru

                        StartReceive();
                    }
                }
                else
                {
                    Console.WriteLine("Połączenie zamkniete. Brak danych do przetworzenia...");
                    //StartReceive();
                }
                

            }
        }*/
    }
}
