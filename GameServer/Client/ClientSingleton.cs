using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using NetworkCore.NetworkData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Client
{
    public class ClientSingleton : TcpNetworkClient
    {
        public TcpPeer AuthServer { get; private set; }
        public TcpPeer GameServer { get; private set; }

        #region Singleton

        private static ClientSingleton Instance;

        private ClientSingleton() { }

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

        /*private bool MatchCharacterId(int CharacterVId)
        {
            return CharacterVId == CharacterStateEmissary.instance.CharacterVId;
        }*/

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
            await Console.Out.WriteLineAsync($"[RECEIVED] new packed with type: {packet.TypeId} from peer with Guid: {serverPeer.Id}");

           /* #region Character

            if (packet is CharacterLoadResponsePacket characterLoadResponsePacket)
                CharacterLoadEmissary.instance.ReceiveCharacterData(characterLoadResponsePacket);

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
                    CharacterTransformEmissary.instance.ReceiveTransformationData(trsPck);
                else AdventurerTransformEmissary.instance.ReceiveTransformationData(trsPck);

            else if (packet is TransformCollectionPacket TrsColletionPacket)
                foreach (TransformPacket pck in TrsColletionPacket.PacketCollection)
                    if (MatchCharacterId(pck.CharacterVId))
                        CharacterTransformEmissary.instance.ReceiveTransformationData(pck);
                    else AdventurerTransformEmissary.instance.ReceiveTransformationData(pck);

            #endregion*/

        }

        public async Task RegisterNewAccount(string login, string password)
        {
            await ConnectToAuthServer();

            if (AuthServer.IsConnected)
            {

            }
        }
    }
}
