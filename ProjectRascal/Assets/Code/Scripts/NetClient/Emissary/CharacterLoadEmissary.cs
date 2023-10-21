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

        private static CharacterLoadEmissary instance;

        public static CharacterLoadEmissary Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CharacterLoadEmissary>();
                    if (instance == null)
                        instance = new GameObject("AdventurerStateEmissary").AddComponent<CharacterLoadEmissary>();
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);
        }

        #endregion

        public void ReceiveCharacterData(CharacterLoadResponsePacket packet)
        {
            if(packet.Success)
            {
                CharacterStateEmissary.Instance.ReceiveAttributesData(packet.AttributesPacket);
                CharacterTransformEmissary.Instance.ReceiveTransformationData(packet.TransformPacket);
                OnCharacterLoadSucces?.Invoke();
            }
            else
                OnCharacterLoadFailed?.Invoke();        
        }

        public IEnumerator CommitSendCharacterLoadRequest(string authToken)
        {
            Debug.Log("test");
            ClientSingleton client = ClientSingleton.GetInstance();

            yield return UnityTaskUtils.RunTaskAsync(async () => await client.GameServer.SendPacket(new CharacterLoadRequestPacket(authToken)));


            PacketBase packet = null;

            yield return UnityTaskUtils.RunTaskWithResultAsync(async () => await client._PacketHandler.WaitForResponsePacket(
                       client.GameServer.GUID, PacketType.CHARACTER_LOAD_RESPONSE), result =>
            {
                packet = result;
            });

            if(packet  != null)
                ReceiveCharacterData(packet as CharacterLoadResponsePacket);
        }

        public async void CommitSendCharacterLoadSucces(bool loadSucces)
        {
            await ClientSingleton.GetInstance().GameServer.SendPacket(new CharacterLoadSuccesPacket(loadSucces));
        }
    }
}
