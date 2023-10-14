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

        public static AdventurerLoadEmissary instance;

        private void Awake()
        {
            instance = this;
            Debug.Log("AdventurerLoadEmissary Awake");
        }

        #endregion

        public delegate void AdventurerLoad(int adventurerId);

        public event AdventurerLoad OnNewAdventurerLoad;

        public void ReceiveNewAdventurerData(AdventurerLoadPacket adventurerLoadPacket)
        {
            AdventurerStateEmissary.instance.AddAdventurer(new AdventurerAttributesData(adventurerLoadPacket.AttributesPacket));
            AdventurerTransformEmissary.instance.AddAdventurerTransform(new TransformData(adventurerLoadPacket.TransformPacket));
            OnNewAdventurerLoad.Invoke(adventurerLoadPacket.AttributesPacket.CharacterVId);
        }
    }
}
