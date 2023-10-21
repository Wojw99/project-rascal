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
using Assets.Code.Scripts.NetClient;
using Assets.Code.Scripts.NetClient.Base;

namespace NetClient
{
    public class ClientSingleton : TcpNetworkClient
    {
        public ServerPeer AuthServer { get; private set; }
        public ServerPeer GameServer { get; private set; }

        #region Singleton

        private static ClientSingleton instance;

        private ClientSingleton() 
        {
            OnPacketSent += ShowPacketInfo;
            Start();
            StartUpdate(TimeSpan.FromMilliseconds(20));
            StartPacketProcessing(50, 50, TimeSpan.FromMilliseconds(20));

            GameServer = new ServerPeer(this, "127.0.0.1", 8051);
            GameServer.Connect();
            GameServer.StartRead();

            AuthServer = new ServerPeer(this,"127.0.0.1", 8050);
            AuthServer.Connect();
            AuthServer.StartRead();

        }

        public static ClientSingleton GetInstance()
        { 
            if(instance == null)
                instance = new ClientSingleton();
                
            return instance;
        }

        #endregion

        public void ShowPacketInfo(string packetInfo)
        {
            Debug.Log("[SEND] " + packetInfo);
        }

        public override async Task Update()
        {

        }

        private bool MatchCharacterId(int CharacterVId)
        {
            return CharacterVId == CharacterStateEmissary.Instance.CharacterVId;
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
                    CharacterLoadEmissary.Instance.ReceiveCharacterData(characterLoadResponsePacket);

                #endregion

                #region Adventurer

                else if (packet is AdventurerLoadPacket adventurerLoadPacket)
                    AdventurerLoadEmissary.Instance.ReceiveNewAdventurerData(adventurerLoadPacket);

                else if (packet is AdventurerLoadCollectionPacket adventurerLoadCollection)
                {
                    Debug.Log("Twoje VId = " + CharacterStateEmissary.Instance.CharacterVId);
                    foreach (var pck in adventurerLoadCollection.PacketCollection)
                    {
                        Debug.Log(pck.AttributesPacket.CharacterVId);
                        AdventurerLoadEmissary.Instance.ReceiveNewAdventurerData(pck);
                    }
                }

                #endregion

                #region Attributes

                else if (packet is AttributesPacket attrPck)
                    if (MatchCharacterId(attrPck.CharacterVId))
                        CharacterStateEmissary.Instance.ReceiveAttributesData(attrPck);
                    else AdventurerStateEmissary.Instance.ReceiveAttributesData(attrPck);

                else if (packet is AttributesCollectionPacket AttrCollectionPacket)
                    foreach (AttributesPacket pck in AttrCollectionPacket.PacketCollection)
                        if (MatchCharacterId(pck.CharacterVId))
                            CharacterStateEmissary.Instance.ReceiveAttributesData(pck);
                        else AdventurerStateEmissary.Instance.ReceiveAttributesData(pck);

                else if (packet is AttributesUpdatePacket AttrUpdatePacket)
                    if (MatchCharacterId(AttrUpdatePacket.CharacterVId))
                        CharacterStateEmissary.Instance.ReceiveAttributesDataUpdate(AttrUpdatePacket);
                    else AdventurerStateEmissary.Instance.ReceiveAttributesDataUpdate(AttrUpdatePacket);

                else if (packet is AttributesUpdateCollectionPacket AttrUpdateCollectionPacket)
                    foreach (AttributesUpdatePacket pck in AttrUpdateCollectionPacket.PacketCollection)
                        if (MatchCharacterId(pck.CharacterVId))
                            CharacterStateEmissary.Instance.ReceiveAttributesDataUpdate(pck);
                        else AdventurerStateEmissary.Instance.ReceiveAttributesDataUpdate(pck);

                #endregion

                #region Transform

                else if (packet is TransformPacket trsPck)
                    if (MatchCharacterId(trsPck.CharacterVId))
                        Debug.Log("Wywoluje 1 - gracz");
                        //CharacterTransformEmissary.instance.ReceiveTransformationData(trsPck);
                    else AdventurerTransformEmissary.Instance.ReceiveTransformationData(trsPck);

                else if (packet is TransformCollectionPacket TrsColletionPacket)
                    foreach (TransformPacket pck in TrsColletionPacket.PacketCollection)
                        if (!MatchCharacterId(pck.CharacterVId))
                            AdventurerTransformEmissary.Instance.ReceiveTransformationData(pck);
     
                #endregion
            }
            catch(Exception ex)
            {
                Debug.LogError(ex.ToString());
            }

        }

        public async Task RegisterNewAccount(string login, string password)
        {
            //await ConnectToAuthServer();

            if(AuthServer.IsConnected)
            {

            }
        }
    }
}
