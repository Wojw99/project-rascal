using Assets.Code.Scripts.NetClient.Attributes;
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
            TransformData Transform = AdventurerChrTransformCollection.Find(transform => transform.ObjectVId == TransformPacket.CharacterVId);
            if (Transform != null)
            {
                Transform.Position.x = TransformPacket.PosX;
                Transform.Position.y = TransformPacket.PosY;
                Transform.Position.z = TransformPacket.PosZ;
                Transform.Rotation.x = TransformPacket.RotX;
                Transform.Rotation.y = TransformPacket.RotY;
                Transform.Rotation.z = TransformPacket.RotZ;
            }
            else // if doesnt found - Add new Adventurer
            {
                AddAdventurerTransform(new TransformData(TransformPacket.CharacterVId,
                    new Vector3(TransformPacket.PosX, TransformPacket.PosY, TransformPacket.PosZ),
                    new Vector3(TransformPacket.RotX, TransformPacket.RotY, TransformPacket.RotZ)));
            }

            OnAdventurerTransformChanged.Invoke(TransformPacket.CharacterVId);
        }

        public void AddAdventurerTransform(TransformData transformData)
        {
            // If adventurer doesnt exist, add new adventurer.
            if (!AdventurerChrTransformCollection.Any(transform => transform.ObjectVId == transformData.ObjectVId))
                AdventurerChrTransformCollection.Add(transformData);
        }
    }
}
