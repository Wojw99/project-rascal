using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;

// authorization
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Client
{
    public class SimpleClient : TcpNetworkClient
    {
        string TestAuthToken = "gracz"; // in the future we have to create authorization service 
        // we have to store also other information in Token, like username, and by username we can receive data from Database to that client

        public VisibleCharactersCollection CharactersCollection { get; private set; }

        public Character ClientPlayer { get; set; }

        //private Dictionary<Guid, Character> CharacterCollection;

        public SimpleClient() : base()
        {
            ClientPlayer = new Character();
            CharactersCollection = new VisibleCharactersCollection();
        }

        public SimpleClient(UInt32 maxIncomingPacketCount, UInt32 maxOutgoingPacketCount, TimeSpan packetProcessInterval) 
            : base(maxIncomingPacketCount, maxOutgoingPacketCount, packetProcessInterval) 
        {
            ClientPlayer = new Character();
            CharactersCollection = new VisibleCharactersCollection();
        }

        protected override async Task Update()
        {
            
                
        }

        public override async Task OnPacketReceived(IPeer serverPeer, PacketBase packet)
        {
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {serverPeer.Id}");
         
            // Updated states of all players in world.
            if(packet is CharacterStatesUpdatePacket chrStatesUpdate)
            {
                foreach(var stateUpdate in chrStatesUpdate.PacketCollection)
                {
                    if(stateUpdate.CharacterVId == ClientPlayer.Vid)
                    {
                        // Except position
                        ClientPlayer.Name = stateUpdate.Name ?? ClientPlayer.Name;
                        ClientPlayer.Health = stateUpdate.Health ?? ClientPlayer.Health;
                        ClientPlayer.Mana = stateUpdate.Mana ?? ClientPlayer.Mana;
                    }
                    else // Not our character id
                    {
                        CharactersCollection.OnCharacterStateUpdateReceived(stateUpdate);
                    }
                }
            }

            // Single updated state.
            else if (packet is CharacterStateUpdatePacket chrStateUpdate)
            {
                if(chrStateUpdate.CharacterVId == ClientPlayer.Vid) 
                { 
                    // Escept position
                    ClientPlayer.Name = chrStateUpdate.Name ?? ClientPlayer.Name;
                    ClientPlayer.Health = chrStateUpdate.Health ?? ClientPlayer.Health;
                    ClientPlayer.Mana = chrStateUpdate.Mana ?? ClientPlayer.Mana;
                }
                else // Not our character id
                {
                    CharactersCollection.OnCharacterStateUpdateReceived(chrStateUpdate);
                }
            }

            // States of all players in world.
            else if (packet is CharacterStatesPacket chrStates)
            {
                foreach(var state in chrStates.PacketCollection)
                {
                    if(state.CharacterVId == ClientPlayer.Vid)
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
            else if (packet is CharacterStatePacket statePacket)
            {
                if(statePacket.CharacterVId == ClientPlayer.Vid) 
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
