using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Code.Scripts.NetClient.Emissary;

namespace NetClient
{
    public class ClientSingleton : TcpNetworkClient
    {
        public TcpPeer AuthServer { get; private set; }
        public TcpPeer GameServer { get; private set; }

        #region Singleton

        private static ClientSingleton Instance;

        private ClientSingleton() 
        {
            OnPacketSent += ShowPacketInfo;
        }

        public static async Task<ClientSingleton> GetInstanceAsync()
        {
            if (Instance == null)
            {
                Instance = new ClientSingleton();
                await Instance.Initialize();
            }
            return Instance;
        }

        private async Task Initialize()
        {
            Start();
            StartUpdate(TimeSpan.FromMilliseconds(20));
            StartPacketProcessing(50, 50, TimeSpan.FromMilliseconds(20));

            // Temporary
            await ConnectToGameServer();
        }

        #endregion

        public void ShowPacketInfo(string packetInfo)
        {
            Debug.Log("[SEND] " + packetInfo);
        }

        public async Task ConnectToAuthServer()
        {
            AuthServer = await CreateTcpServerConnection("127.0.0.1", 8050);
            AuthServer.Connect();
            AuthServer.StartRead();
        }

        public async Task ConnectToGameServer()
        {
            GameServer = await CreateTcpServerConnection("127.0.0.1", 8051);
            GameServer.Connect();
            GameServer.StartRead();
        }

        public override async Task Update()
        {

        }

        private bool MatchCharacterId(int CharacterVId)
        {
            Debug.Log("Sprawdzam id = " + CharacterVId + " z id = " + CharacterStateEmissary.instance.CharacterVId);
            Debug.Log("Wynik = " + (CharacterVId == CharacterStateEmissary.instance.CharacterVId));
            return CharacterVId == CharacterStateEmissary.instance.CharacterVId;
        }

        /// <summary>
        /// This method redirects packets from servers to the appropriate emissaries.
        /// </summary>
        /// <param name="serverPeer">Server connection peer. </param>
        /// <param name="packet">Arrived packet. </param>
        /// <remarks>
        /// ...
        /// </remarks>
        public override async Task OnPacketReceived(IPeer serverPeer, PacketBase packet)
        {
            try
            {
                Debug.Log($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {serverPeer.Id}, SIZE = {packet.CalculateTotalSize()}");

                #region Character

                if (packet is CharacterLoadResponsePacket characterLoadResponsePacket)
                {
                    Debug.Log("CharacterLoadResponsePacket with CharacterVId = " + characterLoadResponsePacket.AttributesPacket.CharacterVId);
                    CharacterLoadEmissary.instance.ReceiveCharacterData(characterLoadResponsePacket);
                }

                #endregion

                #region Adventurer

                else if (packet is AdventurerLoadPacket adventurerLoadPacket)
                    AdventurerLoadEmissary.instance.ReceiveNewAdventurerData(adventurerLoadPacket);

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
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.ToString());
            }

        }

        public async Task RegisterNewAccount(string login, string password)
        {
            await ConnectToAuthServer();

            if(AuthServer.IsConnected)
            {

            }
        }
    }
}
