using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;

namespace NetworkCore.NetworkCommunication
{
    public class TcpPeer : IPeer
    {
        public PacketHandlerManager _PacketHandlerManager { get; private set; }
        public bool IsProxy => false;
        public bool IsConnected => _TcpClient.Connected;
        public Guid PeerId { get; private set; }
        //public PeerInfo _PeerInfo { get; private set; }

        private TcpClient _TcpClient;
        private byte[] ReceiveBuffer = new byte[1024];
        public TcpPeer(TcpClient tcpClient, PacketHandlerManager packetHandlerManager)
        {
            _TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
            PeerId = Guid.NewGuid();
            _PacketHandlerManager = packetHandlerManager;
        }
        public void Disconnect()
        {
            _TcpClient.Close();
        }
        public void SendPacket(Packet packet)
        {
            NetworkStream stream = _TcpClient.GetStream();
            byte[] data = PacketSerializationManager.serializePacket(packet);
            stream.Write(data, 0, data.Length);
        }
        /*public void HandlePacket(Packet packet)
        {
            Console.WriteLine("Otrzymano nowy pakiet: {0}", packet._type);
            _PacketHandlerManager.HandlePacket(ref packet);
        }*/
        public void StartReceive() // al'a "connect"
        {
            NetworkStream stream = _TcpClient.GetStream();
            stream.BeginRead(ReceiveBuffer, 0, ReceiveBuffer.Length, HandleReceivedData, this);
        }

        private void HandleReceivedData(IAsyncResult result)
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
                        _PacketHandlerManager.HandlePacket(ref packet);

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
        }
    }
}
