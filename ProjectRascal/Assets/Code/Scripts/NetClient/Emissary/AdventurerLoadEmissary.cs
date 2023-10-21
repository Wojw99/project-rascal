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
    public class AdventurerLoadEmissary : MonoBehaviour
    {
        #region Singleton

        private static AdventurerLoadEmissary instance;

        public static AdventurerLoadEmissary Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AdventurerLoadEmissary>();
                    if (instance == null)
                        instance = new GameObject("AdventurerStateEmissary").AddComponent<AdventurerLoadEmissary>();
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

        public delegate void AdventurerLoad(int adventurerId);

        public event AdventurerLoad OnNewAdventurerLoad;

        public void ReceiveNewAdventurerCollectionData(AdventurerLoadCollectionPacket packet)
        {
            foreach(var pck in packet.PacketCollection)
                ReceiveNewAdventurerData(pck);
        }

        public void ReceiveNewAdventurerData(AdventurerLoadPacket adventurerLoadPacket)
        {
            AdventurerStateEmissary.Instance.AddAdventurer(new AdventurerAttributesData(adventurerLoadPacket.AttributesPacket));
            AdventurerTransformEmissary.Instance.AddAdventurerTransform(new TransformData(adventurerLoadPacket.TransformPacket));
            OnNewAdventurerLoad?.Invoke(adventurerLoadPacket.AttributesPacket.CharacterVId);
        }
    }
}
