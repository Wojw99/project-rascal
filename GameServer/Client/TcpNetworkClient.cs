using Client;
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

namespace Client
{
    public class TcpNetworkClient : NetworkBase
    {
        public TcpNetworkClient() : base() { }

        public VisibleCharactersCollection CharactersCollection { get; private set; }

        public Character ClientPlayer { get; set; }

        public async Task <TcpPeer> CreateTcpServerConnection(string serverIpAddress, int serverTcpPort)
        {
            Socket ServerTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            await Console.Out.WriteLineAsync("Trying to connect to server...");
            await ServerTcpSocket.ConnectAsync(new IPEndPoint(IPAddress.Parse(serverIpAddress), (int)serverTcpPort));

            if(ServerTcpSocket.Connected)
            {
                IsRunningFlag = true;
                return new TcpPeer(this, ServerTcpSocket, Guid.NewGuid(), Owner.client);
            }
            else
            {
                throw new Exception($"Cannot connect to server, with IP: {serverIpAddress}, PORT: {serverTcpPort}");
            }
        }

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

        private async Task Update()
        {

        }

        public async Task Stop()
        {
            IsRunningFlag = false;
        }

        //public override abstract Task<bool> OnServerConnect(IPeer serverPeer);
        //public abstract Task OnNewConnection(Socket ServerTcpSocket, Guid newConnectionId, Owner ownerType);
        //public abstract Task OnServerDisconnect(IPeer serverPeer);
        public override async Task OnPacketReceived(IPeer serverPeer, PacketBase packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {serverPeer.Id}");

            // Updated states of all players in world.
            if (packet is AttributesUpdatePacket chrStatesUpdate)
            {
                foreach (var stateUpdate in chrStatesUpdate.PacketCollection)
                {
                    if (stateUpdate.CharacterVId == ClientPlayer.Vid)
                    {
                        // Except position
                        ClientPlayer.Name = stateUpdate.Name ?? ClientPlayer.Name;
                        ClientPlayer.CurrentHealth = stateUpdate.CurrentHealth ?? ClientPlayer.CurrentHealth;
                        ClientPlayer.MaxHealth = stateUpdate.MaxHealth ?? ClientPlayer.MaxHealth;
                        ClientPlayer.CurrentMana = stateUpdate.CurrentMana ?? ClientPlayer.CurrentMana;
                        ClientPlayer.MaxMana = stateUpdate.MaxMana ?? ClientPlayer.MaxMana;
                    }
                    else // Not our character id
                    {
                        CharactersCollection.OnCharacterStateUpdateReceived(stateUpdate);
                    }
                }
            }

            // Single updated state.
            else if (packet is AttributesUpdateCollectionPacket chrStateUpdate)
            {
                if (chrStateUpdate.CharacterVId == ClientPlayer.Vid)
                {
                    // Except position
                    ClientPlayer.Name = chrStateUpdate.Name ?? ClientPlayer.Name;
                    ClientPlayer.CurrentHealth = chrStateUpdate.CurrentHealth ?? ClientPlayer.CurrentHealth;
                    ClientPlayer.MaxHealth = chrStateUpdate.MaxHealth ?? ClientPlayer.MaxHealth;
                    ClientPlayer.CurrentMana = chrStateUpdate.CurrentMana ?? ClientPlayer.CurrentMana;
                    ClientPlayer.MaxMana = chrStateUpdate.MaxMana ?? ClientPlayer.MaxMana;
                }
                else // Not our character id
                {
                    CharactersCollection.OnCharacterStateUpdateReceived(chrStateUpdate);
                }
            }

            // States of all players in world.
            else if (packet is AttributesCollectionPacket chrStates)
            {
                foreach (var state in chrStates.PacketCollection)
                {
                    if (state.CharacterVId == ClientPlayer.Vid)
                    {
                        ClientPlayer = state.GetCharacter();
                    }
                    else
                    {
                        CharactersCollection.OnCharacterStateReceived(state);
                    }
                }
            }

            // Single state
            else if (packet is AttributesPacket statePacket)
            {
                if (statePacket.CharacterVId == ClientPlayer.Vid)
                {
                    
                    ClientPlayer = statePacket.GetCharacter();
                }
                else
                {
                    CharactersCollection.OnCharacterStateReceived(statePacket);
                }
            }
        }
    }
}
