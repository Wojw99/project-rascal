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
using System.Collections;
using NetworkCore.NetworkCommunication;
using static NetworkCore.NetworkCommunication.TcpPeer;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class CharacterLoadEmissary : MonoBehaviour 
    {

        public delegate void CharacterLoad();

        public event CharacterLoad OnCharacterLoadSucces;

        public event CharacterLoad OnCharacterLoadFailed;

        #region Singleton

        public static CharacterLoadEmissary instance;

        private void Awake()
        {
            instance = this;
            ClientSingleton.GetInstance().GameServer.OnCharacterLoadResponsePacketReceived += 
                (packet) => ReceiveCharacterData(packet as CharacterLoadResponsePacket);

            ClientSingleton.GetInstance().GameServer.OnCharacterLoadResponsePacketReceived += ReceiveCharacterData;
        }

        #endregion

        public void ReceiveCharacterData(CharacterLoadResponsePacket packet)
        {
            //Debug.Log("ReceiveCharacterData called, with succes = " + packet.Success);
            //Debug.Log("W pakiecie id = " + packet.AttributesPacket.CharacterVId);
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

        public IEnumerator CommitSendCharacterLoadRequest(string authToken)
        {
            ClientSingleton client = null;

            yield return UnityTaskUtils.RunTaskWithResultAsync(() => ClientSingleton.GetInstanceAsync(), result =>
            {
                client = result;
            });


            yield return UnityTaskUtils.RunTaskAsync(async () => await client.GameServer.SendPacket(new CharacterLoadRequestPacket(authToken)));


            PacketBase packet = null;

            yield return UnityTaskUtils.RunTaskWithResultAsync(async () => await client.WaitForResponsePacket(TimeSpan.FromMilliseconds(20),
                       TimeSpan.FromSeconds(50), PacketType.CHARACTER_LOAD_RESPONSE), result =>
            {
                packet = result;
            });

            if(packet  != null)
                yield return UnityTaskUtils.RunTaskAsync(async () => await ReceiveCharacterData(packet as CharacterLoadResponsePacket));
           

        }

        public async void CommitSendCharacterLoadSucces(bool loadSucces)
        {
            ClientSingleton client = await ClientSingleton.GetInstanceAsync();
            await client.GameServer.SendPacket(new CharacterLoadSuccesPacket(loadSucces));
        }

        

    }
}
