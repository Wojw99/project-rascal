using Assets.Code.Scripts.NetClient.Attributes;
using Assets.Code.Scripts.NetClient.Clients;
using NetClient;
using NetworkCore.Packets;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Code.Scripts.NetClient.Holder;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class PlayerCharacterLoadEmissary
    {
        public bool PlayerCharacterLoadSucces { get; private set; } = false;

        public delegate void CharacterLoad();

        public event CharacterLoad OnCharacterLoadSucces;

        public event CharacterLoad OnCharacterLoadFailed;

        public void ReceivePacket(CharacterLoadResponsePacket packet)
        {
            if(packet.Success)
            {
                PlayerDataHolder.instance.InitPlayerCharacter(packet);

                PlayerCharacterLoadSucces = true;
                OnCharacterLoadSucces?.Invoke();
            }
            else
            {
                PlayerCharacterLoadSucces = false;
                OnCharacterLoadFailed?.Invoke();
            }         
        }

        public async void CommitSendCharacterLoadRequest(string authToken)
        {
            if(!PlayerCharacterLoadSucces) 
            { 
                await GameClient.instance.GameServerPeer.SendPacket(new CharacterLoadRequestPacket(authToken));

                PacketBase packet = await TcpNetworkClient.GetInstance().WaitForResponsePacket(TimeSpan.FromMilliseconds(20), 
                    TimeSpan.FromSeconds(50), PacketType.CHARACTER_LOAD_RESPONSE);

                ReceivePacket(packet as CharacterLoadResponsePacket);
            }
        }

        public async void CommitSendCharacterLoadSucces(bool loadSucces)
        {
            await GameClient.instance.GameServerPeer.SendPacket(new CharacterLoadSuccesPacket(loadSucces));
        }

        #region Singleton

        public static PlayerCharacterLoadEmissary instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

    }
}
