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
    public class TcpNetworkClient : NetworkBase
    {
        #region Singleton

        private static TcpNetworkClient Instance;

        private TcpNetworkClient()
        {
            IsRunning = true;
            StartUpdate(TimeSpan.FromMilliseconds(20));
            RunPacketProcessingInBackground(50, 50, TimeSpan.FromMilliseconds(20));
        }

        public static TcpNetworkClient GetInstance()
        {
            if (Instance == null)
            {
                Instance = new TcpNetworkClient();
            }
            return Instance;
        }

        #endregion

        #region private

        private void StartUpdate(TimeSpan interval)
        {
            Task handleUpdate = Task.Run(async () =>
            {
                while (IsRunning)
                {
                    await Update();
                    await Task.Delay(interval);
                }
            });
        }

        private async Task Update()
        {

        }

        #endregion

        public async Task <TcpPeer> CreateTcpServerConnection(string serverIpAddress, int serverTcpPort)
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
        }

        public async Task Stop()
        {
            IsRunning = false;
        }

        public override async Task OnPacketReceived(IPeer serverPeer, PacketBase packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {serverPeer.Id}");

            if(packet is CharacterTransformPacket chrMovePck)
            {
                MovementEmissary.instance.ReceivePacket(chrMovePck);
            }
            else if (packet is CharacterStatePacket chrStatePck)
            {
                CharacterStateEmissary.instance.ReceivePacket(chrStatePck);
            }
            else if (packet is CharacterStatesPacket chrStatesPck)
            {
                CharacterStateEmissary.instance.ReceivePacket(chrStatesPck);
            }
            else if (packet is CharacterAttrUpdatePacket chrStateUpdatePck)
            {
                CharacterStateEmissary.instance.ReceivePacket(chrStateUpdatePck);
            }
            else if (packet is CharactersAttrsUpdatePacket chrStatesUpdatePck)
            {
                CharacterStateEmissary.instance.ReceivePacket(chrStatesUpdatePck);
            }

        }
    }
}
