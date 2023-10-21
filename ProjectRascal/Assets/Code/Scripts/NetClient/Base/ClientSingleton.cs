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
using UnityEditor;

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
            AuthServer = new ServerPeer(this,"127.0.0.1", 8050);
            
            AddHandlers();
            
            GameServer.Connect();
            GameServer.StartRead();
            AuthServer.Connect();
            AuthServer.StartRead();

            //NetworkTimeSyncEmissary.Instance.StartSynchronize();

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
        /// 

        public static void func(CharacterLoadResponsePacket pck)
        {

        }


        public void AddHandlers()
        {
            #region Character

            _PacketHandler.AddHandler(GameServer.GUID, typeof(CharacterLoadResponsePacket),
                new Action<CharacterLoadResponsePacket>((packet) => 
                CharacterLoadEmissary.Instance.ReceiveCharacterData(packet)));

            #endregion

            #region Adventurer

            _PacketHandler.AddHandler(GameServer.GUID, typeof(AdventurerLoadPacket),
                new Action<AdventurerLoadPacket>((packet) => 
                AdventurerLoadEmissary.Instance.ReceiveNewAdventurerData(packet)));

            _PacketHandler.AddHandler(GameServer.GUID, typeof(AdventurerLoadCollectionPacket),
                new Action<AdventurerLoadCollectionPacket>((packet) => 
                AdventurerLoadEmissary.Instance.ReceiveNewAdventurerCollectionData(packet)));

            #endregion

            #region Attributes

            _PacketHandler.AddHandler(GameServer.GUID, typeof(AttributesCollectionPacket),
                new Action<AttributesCollectionPacket>((packet) => {
                    foreach (AttributesPacket pck in packet.PacketCollection)
                        if (MatchCharacterId(pck.CharacterVId))
                            CharacterStateEmissary.Instance.ReceiveAttributesData(pck);
                        else AdventurerStateEmissary.Instance.ReceiveAttributesData(pck);
                }));

            _PacketHandler.AddHandler(GameServer.GUID, typeof(AttributesPacket),
                new Action<AttributesPacket>((packet) => {
                    if (MatchCharacterId(packet.CharacterVId))
                        CharacterStateEmissary.Instance.ReceiveAttributesData(packet);
                    else AdventurerStateEmissary.Instance.ReceiveAttributesData(packet);
                }));

            _PacketHandler.AddHandler(GameServer.GUID, typeof(AttributesUpdateCollectionPacket),
                new Action<AttributesUpdateCollectionPacket>((packet) => {
                    foreach (AttributesUpdatePacket pck in packet.PacketCollection)
                        if (MatchCharacterId(pck.CharacterVId))
                            CharacterStateEmissary.Instance.ReceiveAttributesDataUpdate(pck);
                        else AdventurerStateEmissary.Instance.ReceiveAttributesDataUpdate(pck);
                }));

            _PacketHandler.AddHandler(GameServer.GUID, typeof(AttributesUpdatePacket),
                new Action<AttributesUpdatePacket>((packet) => {
                    if (MatchCharacterId(packet.CharacterVId))
                        CharacterStateEmissary.Instance.ReceiveAttributesDataUpdate(packet);
                    else AdventurerStateEmissary.Instance.ReceiveAttributesDataUpdate(packet);
                }));

            #endregion

            #region Transform

            _PacketHandler.AddHandler(GameServer.GUID, typeof(TransformPacket),
                new Action<TransformPacket>((packet) => {
                    if (MatchCharacterId(packet.CharacterVId))
                        Debug.Log("Wywoluje 1 - gracz");
                    else AdventurerTransformEmissary.Instance.ReceiveTransformationData(packet);
                }));

            _PacketHandler.AddHandler(GameServer.GUID, typeof(TransformCollectionPacket),
                new Action<TransformCollectionPacket>((packet) => {
                    if (packet is TransformCollectionPacket TrsColletionPacket)
                        foreach (TransformPacket pck in TrsColletionPacket.PacketCollection)
                            if (!MatchCharacterId(pck.CharacterVId))
                                AdventurerTransformEmissary.Instance.ReceiveTransformationData(pck);
                }));

            #endregion
        }

        /*public override async Task OnPacketReceived(IPeer serverPeer, PacketBase packet)
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
            }}*/

    

        public async Task RegisterNewAccount(string login, string password)
        {
            //await ConnectToAuthServer();

            if(AuthServer.IsConnected)
            {

            }
        }
    }
}
