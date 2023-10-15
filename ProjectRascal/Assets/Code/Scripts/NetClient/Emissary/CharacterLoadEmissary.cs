using Assets.Code.Scripts.NetClient.Attributes;
using NetClient;
using NetworkCore.Packets;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class CharacterLoadEmissary : MonoBehaviour 
    {

        public delegate void CharacterLoad();

        public event CharacterLoad OnCharacterLoadSucces;

        public event CharacterLoad OnCharacterLoadFailed;

        public async Task ReceiveCharacterData(CharacterLoadResponsePacket packet)
        {
            Debug.Log("ReceiveCharacterData called, with succes = " + packet.Success);
            Debug.Log("W pakiecie id = " + packet.AttributesPacket.CharacterVId);
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
            ClientSingleton Client = await ClientSingleton.GetInstanceAsync();

            Debug.Log("Wysylam pakiet wczytania postaci.");

            await Client.GameServer.SendPacket(new CharacterLoadRequestPacket(authToken));
            
            PacketBase packet = await Client.WaitForResponsePacket(TimeSpan.FromMilliseconds(20), 
                TimeSpan.FromSeconds(50), PacketType.CHARACTER_LOAD_RESPONSE);

            Debug.Log("Otrzymalem pakiet zwrotny");

            await ReceiveCharacterData(packet as CharacterLoadResponsePacket);
        }

        public async void CommitSendCharacterLoadSucces(bool loadSucces)
        {
            ClientSingleton client = await ClientSingleton.GetInstanceAsync();
            await client.GameServer.SendPacket(new CharacterLoadSuccesPacket(loadSucces));
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
