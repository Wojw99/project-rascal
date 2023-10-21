using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetClient;
using UnityEngine;
using NetworkCore.NetworkMessage;
using Assets.Code.Scripts.NetClient.Emissary;
using NetworkCore.Packets;

namespace Assets.Code.Scripts.NetClient.Base
{
    public class ServerPeer : TcpPeer
    {
        private IPEndPoint ServerIpEndpoint;
        private NetworkBase NetworkRef;
        
        /*public PacketReceived OnCharacterLoadResponsePacketReceived;
        public PacketReceived OnAdventurerExitPacketReceived;
        public PacketReceived OnAdventurerLoadCollectionPacketReceived;
        public PacketReceived OnAdventurerLoadPacketReceived;
        public PacketReceived OnAttributesCollectionPackettReceived;
        public PacketReceived OnAttributesPackettReceived;
        public PacketReceived OnAttributesUpdateCollectionPacketReceived;
        public PacketReceived OnAttributesUpdatePacketReceived;
        public PacketReceived OnCharacterLoadSuccesPacketReceived;
        public PacketReceived OnClientDisconnectPacketReceived;
        public PacketReceived OnClientLoginRequestPacketReceived;
        public PacketReceived OnClientLoginResponsePacketReceived;
        public PacketReceived OnPingRequestPacketReceived;
        public PacketReceived OnPingResponsePacketReceived;
        public PacketReceived OnTransformCollectionPacketReceived;*/

        public ServerPeer(NetworkBase network, string serverIpAddress, int serverTcpPort)
            : base(network, new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp),
                  Guid.NewGuid(), Owner.client)
        {
            
            ServerIpEndpoint = new IPEndPoint(IPAddress.Parse(serverIpAddress), serverTcpPort);
        }

        public void ConnectToServer()
        {
            Debug.Log("Trying to connect to server...");
            PeerSocket.Connect(ServerIpEndpoint);

            if(PeerSocket.Connected)
            {
                Debug.Log($"Succesfully connected with server, on port = " +
                    $"{ServerIpEndpoint.Port}, adress = {ServerIpEndpoint.Address}." );
            }
        }

        /*public override void OnPacketReceived(PacketBase packet)
        {

            //Debug.Log($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {serverPeer.Id}, SIZE = {packet.CalculateTotalSize()}");

            #region Character

            if (packet is CharacterLoadResponsePacket characterLoadResponsePacket)
                OnCharacterLoadResponsePacketReceived?.Invoke(characterLoadResponsePacket);

            //CharacterLoadEmissary.instance.ReceiveCharacterData(characterLoadResponsePacket);


            #endregion

            #region Adventurer

            else if (packet is AdventurerLoadPacket adventurerLoadPacket)
                OnAdventurerLoadPacketReceived?.Invoke(adventurerLoadPacket);
                    
            //AdventurerLoadEmissary.instance.ReceiveNewAdventurerData(adventurerLoadPacket);

                else if (packet is AdventurerLoadCollectionPacket adventurerLoadCollection)
                    foreach (var pck in adventurerLoadCollection.PacketCollection)
                        AdventurerLoadEmissary.instance.ReceiveNewAdventurerData(pck);

                #endregion

                #region Attributes

                else if (packet is AttributesPacket attrPck)
                    if (MatchCharacterId(attrPck.CharacterVId))
                        CharacterStateEmissary.instance.ReceiveAttributesData(attrPck);
                    else AdventurerStateEmissary.instance.ReceiveAttributesData(attrPck);

                else if (packet is AttributesCollectionPacket AttrCollectionPacket)
                    foreach (AttributesPacket pck in AttrCollectionPacket.PacketCollection)
                        if (MatchCharacterId(pck.CharacterVId))
                            CharacterStateEmissary.instance.ReceiveAttributesData(pck);
                        else AdventurerStateEmissary.instance.ReceiveAttributesData(pck);

                else if (packet is AttributesUpdatePacket AttrUpdatePacket)
                    if (MatchCharacterId(AttrUpdatePacket.CharacterVId))
                        CharacterStateEmissary.instance.ReceiveAttributesDataUpdate(AttrUpdatePacket);
                    else AdventurerStateEmissary.instance.ReceiveAttributesDataUpdate(AttrUpdatePacket);

                else if (packet is AttributesUpdateCollectionPacket AttrUpdateCollectionPacket)
                    foreach (AttributesUpdatePacket pck in AttrUpdateCollectionPacket.PacketCollection)
                        if (MatchCharacterId(pck.CharacterVId))
                            CharacterStateEmissary.instance.ReceiveAttributesDataUpdate(pck);
                        else AdventurerStateEmissary.instance.ReceiveAttributesDataUpdate(pck);

                #endregion

                #region Transform

                else if (packet is TransformPacket trsPck)
                    if (MatchCharacterId(trsPck.CharacterVId))
                        Debug.Log("Wywoluje 1 - gracz");
                    //CharacterTransformEmissary.instance.ReceiveTransformationData(trsPck);
                    else AdventurerTransformEmissary.instance.ReceiveTransformationData(trsPck);

                else if (packet is TransformCollectionPacket TrsColletionPacket)
                {
                    //Debug.Log("JEST JEST JEST JEST JEST JEST JEST, Size = " + TrsColletionPacket.PacketCollection.Count);
                    foreach (TransformPacket pck in TrsColletionPacket.PacketCollection)
                    {
                        //Debug.Log(MatchCharacterId(pck.CharacterVId));
                        if (MatchCharacterId(pck.CharacterVId))
                        {
                            Debug.Log("Wywoluje 1 - gracz");
                            //CharacterTransformEmissary.instance.ReceiveTransformationData(pck);
                            // trzeba poprawic gdyz serwer wysyla ten pakiet rowniez to gracza
                        }
                        else
                        {
                            // Debug.Log("Wywoluje 2 - adventurer");
                            AdventurerTransformEmissary.instance.ReceiveTransformationData(pck);
                        }
                    }
                }



                #endregion

        }*/
    }
}
