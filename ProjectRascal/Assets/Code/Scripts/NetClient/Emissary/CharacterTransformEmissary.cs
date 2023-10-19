﻿using NetClient;
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

        public static CharacterTransformEmissary instance;

        private void Awake()
        {
            instance = this;
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

        float GetInterpolationTime(int AdventurerVId)
        {
            // Send ping packet to measure network latency.

            return 0.1f; // Przykładowa stała wartość
        }

        public async void CommitSendPlayerCharacterTransfer(int characterVId, float posX, float posY, float posZ,
            float rotX, float rotY, float rotZ, AdventurerState adventurerState)
        {
            ClientSingleton client = await ClientSingleton.GetInstanceAsync();

            await client.GameServer.SendPacket(
                new TransformPacket(characterVId, posX, posY, posZ, rotX, rotY, rotZ, adventurerState));
        }
    }
}
