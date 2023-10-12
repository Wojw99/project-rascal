using Assets.Code.Scripts.NetClient.Attributes;
using NetClient;
using NetworkCore.Packets;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class CharacterLoadEmissary
    {

        public delegate void CharacterLoad();

        public event CharacterLoad OnCharacterLoadSucces;

        public event CharacterLoad OnCharacterLoadFailed;

        public void ReceiveCharacterData(CharacterLoadResponsePacket packet)
        {
            if(packet.Success)
            {
                CharacterStateEmissary.instance.ReceiveAttributesData(packet.AttributesPacket);
                CharacterTransformEmissary.instance.ReceiveTransformationData(packet.TransformPacket);

                OnCharacterLoadSucces?.Invoke();
            }
            else
            {
                OnCharacterLoadFailed?.Invoke();
            }         
        }

        public async void CommitSendCharacterLoadRequest(string authToken)
        {
            ClientSingleton Client = ClientSingleton.GetInstance();

            await Client.GameServer.SendPacket(new CharacterLoadRequestPacket(authToken));

            PacketBase packet = await Client.WaitForResponsePacket(TimeSpan.FromMilliseconds(20), 
                TimeSpan.FromSeconds(50), PacketType.CHARACTER_LOAD_RESPONSE);

            ReceiveCharacterData(packet as CharacterLoadResponsePacket);
        }

        public async void CommitSendCharacterLoadSucces(bool loadSucces)
        {
            await ClientSingleton.GetInstance().GameServer.SendPacket(new CharacterLoadSuccesPacket(loadSucces));
        }

        #region Singleton

        public static CharacterLoadEmissary instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

    }
}
