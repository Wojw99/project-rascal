using NetClient;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetworkCore.NetworkUtility;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class CharacterTransformEmissary : MonoBehaviour
    {
        #region Singleton

        private static CharacterTransformEmissary instance;

        public static CharacterTransformEmissary Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CharacterTransformEmissary>();
                    if (instance == null)
                        instance = new GameObject("AdventurerStateEmissary").AddComponent<CharacterTransformEmissary>();
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

        public delegate void PacketReceived();

        PacketReceived OnCharacterTransformReceived;

        public Vector3 Position { get; private set; }
        public Vector3 Rotation { get; private set; }

        public void ReceiveTransformationData(TransformPacket packet)
        {
            Position = new Vector3(packet.PosX, packet.PosY, packet.PosZ);
            Rotation = new Vector3(packet.RotX, packet.RotY, packet.RotZ);
            OnCharacterTransformReceived?.Invoke();
        }

        public async void CommitSendPlayerCharacterTransfer(int characterVId, float posX, float posY, 
            float posZ, float rotX, float rotY, float rotZ, AdventurerState adventurerState)
        {
            await ClientSingleton.GetInstance().GameServer.SendPacket(new TransformPacket
                (characterVId, posX, posY, posZ, rotX, rotY, rotZ, adventurerState));
        }
    }
}
