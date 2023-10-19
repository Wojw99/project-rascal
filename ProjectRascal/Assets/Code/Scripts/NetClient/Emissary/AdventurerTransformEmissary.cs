﻿using Assets.Code.Scripts.NetClient.Attributes;
using NetworkCore.NetworkUtility;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class AdventurerTransformEmissary : MonoBehaviour
    {
        #region Singleton

        public static AdventurerTransformEmissary instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        public delegate void AdventurerTransformUpdate(int adventurerId);

        public event AdventurerTransformUpdate OnAdventurerTransformChanged;

        public List<TransformData> AdventurerChrTransformCollection { get; private set; }
            = new List<TransformData>();

        public TransformData GetAdventurerTransformData(int AdventurerVId)
        {
            return AdventurerChrTransformCollection.Find(attr => attr.ObjectVId == AdventurerVId);
        }

        public void ReceiveTransformationData(TransformPacket TransformPacket)
        {
            //Debug.Log("Szukam");
            TransformData Transform = AdventurerChrTransformCollection.Find(transform => transform.ObjectVId == TransformPacket.CharacterVId);
            if (Transform != null)
            {
                //Debug.Log("Jest");
                Transform.Position.x = TransformPacket.PosX;
                Transform.Position.y = TransformPacket.PosY;
                Transform.Position.z = TransformPacket.PosZ;
                Transform.Rotation.x = TransformPacket.RotX;
                Transform.Rotation.y = TransformPacket.RotY;
                Transform.Rotation.z = TransformPacket.RotZ;
                Transform.adventurerState = TransformPacket.State;
                OnAdventurerTransformChanged?.Invoke(TransformPacket.CharacterVId);
            }
            else
            {
                Debug.Log("Otrzymano pakiet pozycji aktualnego gracza - twojego");
            }
        }

        public void AddAdventurerTransform(TransformData transformData)
        {
            // If adventurer doesnt exist, add new adventurer.
            if (!AdventurerChrTransformCollection.Any(transform => transform.ObjectVId == transformData.ObjectVId))
                AdventurerChrTransformCollection.Add(transformData);
        }
    }
}
