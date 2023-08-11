using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkConfig;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

public interface IPeer
{
    bool IsProxy { get; }

    bool IsConnected { get; }

    Guid Id { get; }

    Owner OwnerType { get; }

    Socket PeerSocket { get; }

    public INetworkBase NetworkRef { get; }

    void ConnectToServer();

    void ConnectToClient();

    void Disconnect();

    void SendPacket(Packet packet);

    //void StartReceive();
}