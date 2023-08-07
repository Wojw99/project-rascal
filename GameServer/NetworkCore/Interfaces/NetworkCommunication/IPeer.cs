using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using System;
using System.Net;

public interface IPeer
{
    PacketHandlerManager _PacketHandlerManager { get; }
    bool IsProxy { get; }
    bool IsConnected { get; }
    Guid PeerId { get; }
    //PeerInfo _PeerInfo { get; }

    void Disconnect();
    void SendPacket(Packet packet);
    //void HandlePacket(Packet packet);
    void StartReceive();
}