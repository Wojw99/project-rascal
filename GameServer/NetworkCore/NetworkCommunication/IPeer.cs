using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
public interface IPeer
{
    bool IsProxy { get; }

    bool IsConnected { get; }

    Guid GUID { get; }

    Owner OwnerType { get; }

    Socket PeerSocket { get; }

    void Connect();

    void Disconnect();

    Task SendPacket(PacketBase packet);
}