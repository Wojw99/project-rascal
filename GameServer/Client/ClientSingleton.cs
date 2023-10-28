using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class ClientSingleton : TcpNetworkClient
    {
        public ServerPeer AuthServer { get; private set; }
        public ServerPeer GameServer { get; private set; }

        #region Singleton

        private static ClientSingleton instance;

        private ClientSingleton() : base()
        {
            GameServer = new ServerPeer(_PacketHandler, _PacketSender, "127.0.0.1", 8051);
            AuthServer = new ServerPeer(_PacketHandler, _PacketSender, "127.0.0.1", 8050);

            AddHandlers();

            _PacketSender.OnPacketSent += ShowSentPacketInfo;
            _PacketHandler.OnPacketReceived += ShowReceivedPacketInfo;
            GameServer.Connect();
            GameServer.StartRead();

            AuthServer.Connect();
            AuthServer.StartRead();

            GameServer.ConnectToServer();
            //AuthServer.ConnectToServer();

            Start();
            StartUpdate();
            //NetworkTimeSyncEmissary.Instance.StartSynchronize();
        }

        public static ClientSingleton GetInstance()
        {
            if (instance == null)
                instance = new ClientSingleton();

            return instance;
        }

        #endregion

        private void ShowSentPacketInfo(string info)
        {
            Console.WriteLine("[SEND] " + info);
        }

        private void ShowReceivedPacketInfo(string info)
        {
            Console.WriteLine("[RECEIVED] " + info);

        }

        public override async Task Update()
        {
            /*Stopwatch watch = new Stopwatch();

            watch.Start();
            GameServer.RequestSendPacket(new PingRequestPacket());
            watch.Stop();
            await Console.Out.WriteLineAsync($"Ping= {watch.ElapsedMilliseconds} ms.");
            
            watch.Restart();
            await _PacketHandler.WaitForResponsePacket(GameServer.GUID, PacketType.PING_RESPONSE);
            watch.Stop();
            await Console.Out.WriteLineAsync($"Ping-Pong = {watch.ElapsedMilliseconds} ms.");*/
        }

        public void AddHandlers()
        {
            /*#region Character

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
        }*/
        }
    }
}
