// Move that class to NetworkCore project.

using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections.Concurrent;
using NetworkCore.NetworkCommunication;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;

namespace NetClient
{
    public abstract class TcpNetworkClient : NetworkBase
    {
        public void StartUpdate(TimeSpan interval)
        {
            Task handleUpdate = Task.Run(async () =>
            {
                while (IsRunningFlag)
                {
                    await Update();
                    await Task.Delay(interval);
                }
            });
        }

        /*ublic async Task <TcpPeer> CreateTcpServerConnection(string serverIpAddress, int serverTcpPort)
        {
            Socket ServerTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await Console.Out.WriteLineAsync("Trying to connect to server...");
            await ServerTcpSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIpAddress), (int)serverTcpPort));

            if(ServerTcpSocket.Connected)
            {
                return new TcpPeer(this, ServerTcpSocket, Guid.NewGuid(), Owner.client);
            }
            else
            {
                throw new Exception($"Cannot connect to server, with IP: {serverIpAddress}, PORT: {serverTcpPort}");
            }
        }*/

        public void Start()
        {
            IsRunningFlag = true;
        }

        public void Stop()
        {
            IsRunningFlag = false;
        }

        public abstract Task Update();
        //public override abstract Task OnPacketReceived(IPeer clientPeer, PacketBase packet);
    }
}
