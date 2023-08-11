using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
                //_TcpClient.Connect();

            }
        }

        public void ConnectToClient()
        {
            if(OwnerType == Owner.server)
            {
                Task handleReadIncomingTcpData = Task.Run(async () => await ReadIncomingData());
            }
        }

        public void Disconnect()
        {
            //_TcpClient.Close();
        }

        public void SendPacket(Packet packet)
        {
            /*NetworkStream stream = _TcpClient.GetStream();
            byte[] data = PacketSerializationManager.serializePacket(packet);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Wysyłam pakiet zwrotny...");*/
        }

        public async Task ReadIncomingData()
        {
            if(NetworkRef.IsRunning)
            {
                int bytesRead = PeerSocket.Receive(ReceiveBuffer);

                if (bytesRead > 0)
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
                        await AddToIncomingPacketQueue(packet);

                        // substraction of processed data
                        int remainingDataSize = ReceiveBuffer.Length - packetSize;
                        Buffer.BlockCopy(ReceiveBuffer, packetSize, ReceiveBuffer, 0, remainingDataSize);

                        /* //HandlePacket(packet); 
                        PacketHandler handler = NetworkRef._PacketHandlerManager.GetHandler(packet._type);

                        handler.HandleRequest(packet);

                        SendPacket(handler.HandleResponse());*/

                        // Idk if we need it.
                        //Array.Resize(ref ReceiveBuffer, remainingDataSize); // Skrócenie bufora do nowego rozmiaru

                        await ReadIncomingData();
                    }
                }
                else
                {
                    Console.WriteLine("Brak danych do przetworzenia...");
                    //StartReceive();
                }

            }
        }

        public async Task AddToIncomingPacketQueue(Packet packet)
        {
            if(OwnerType == Owner.server)
            {
                NetworkRef.qPacketsIn.Enqueue(new OwnedPacket { Peer = this, Packet = packet });
            }
            else
            {
                NetworkRef.qPacketsIn.Enqueue(new OwnedPacket { Peer = null , Packet = packet });
            }
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
