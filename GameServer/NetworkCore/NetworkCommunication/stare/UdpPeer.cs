/*using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace NetworkCore.NetworkCommunication
{
    public class UdpPeer : IPeer
    {
        public PacketHandlerManager _PacketHandlerManager { get; private set; }
        public bool IsProxy => false;
        public bool IsConnected => _UdpClient.Client.Connected;
        public Guid PeerId { get; private set; }
        //public PeerInfo _PeerInfo { get; private set; }
        private UdpClient _UdpClient;
        public UdpPeer(UdpClient udpClient)
        {
            _UdpClient = udpClient ?? throw new ArgumentNullException(nameof(udpClient));
            PeerId = Guid.NewGuid();
        }
        public void Disconnect()
        {
            _UdpClient.Close();
        }
        public void SendPacket(Packet packet)
        {
            byte[] data = PacketSerializationManager.serializePacket(packet);
            _UdpClient.SendAsync(data, data.Length);
        }
        public void HandlePacket(Packet packet)
        {

        }
        public void StartReceive()
        {

        }
    }
}
*/