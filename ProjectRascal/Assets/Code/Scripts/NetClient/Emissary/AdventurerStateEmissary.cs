﻿using Assets.Code.Scripts.NetClient.Attributes;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class AdventurerStateEmissary : MonoBehaviour
    {
        #region Singleton

        private static AdventurerStateEmissary instance;

        public static AdventurerStateEmissary Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<AdventurerStateEmissary>();
                    if (instance == null)
                        instance = new GameObject("AdventurerStateEmissary").AddComponent<AdventurerStateEmissary>();
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

        public List<AdventurerAttributesData> AdventurerChrAttrCollection { get; private set; }
            = new List<AdventurerAttributesData>();

        public delegate void AdventurerAttributeUpdate(int adventurerId);

        public event AdventurerAttributeUpdate OnAdventurerNameUpdate;
        public event AdventurerAttributeUpdate OnAdventurerCurrentHealthUpdate;
        public event AdventurerAttributeUpdate OnAdventurerCurrentManaUpdate;
        public event AdventurerAttributeUpdate OnAdventurerMaxHealthUpdate;
        public event AdventurerAttributeUpdate OnAdventurerMaxManaUpdate;
        public event AdventurerAttributeUpdate OnAdventurerAttackUpdate;
        public event AdventurerAttributeUpdate OnAdventurerMagicUpdate;
        public event AdventurerAttributeUpdate OnAdventurerMoveSpeedUpdate;
        public event AdventurerAttributeUpdate OnAdventurerAttackSpeedUpdate;
        public event AdventurerAttributeUpdate OnAdventurerStateUpdate;

        public event AdventurerAttributeUpdate OnAdventurerTransformUpdate;

        public AdventurerAttributesData GetAdventurerAttributes(int AdventurerVId)
        {
            return AdventurerChrAttrCollection.Find(attr => attr.CharacterVId == AdventurerVId);
        }

        public void ReceiveAttributesData(AttributesPacket AttrPacket)
        {
            AdventurerAttributesData Attr = AdventurerChrAttrCollection.Find(attr => attr.CharacterVId == AttrPacket.CharacterVId);
            if (Attr != null)
            {
                Attr.Name = AttrPacket.Name;
                Attr.CurrentHealth = AttrPacket.CurrentHealth;
                Attr.CurrentMana = AttrPacket.CurrentMana;
                Attr.MaxHealth = AttrPacket.MaxHealth;
                Attr.MaxMana = AttrPacket.MaxMana;
            }
            else // if doesnt found - Add new Adventurer
            {
                AddAdventurer(new AdventurerAttributesData(AttrPacket.CharacterVId, AttrPacket.Name, 
                    AttrPacket.CurrentHealth, AttrPacket.CurrentMana, AttrPacket.MaxHealth, AttrPacket.MaxMana, 
                    0, 0, AttrPacket.MoveSpeed, AttrPacket.AttackSpeed, AttrPacket.State));
            }

        }

        public void ReceiveAttributesDataUpdate(AttributesUpdatePacket AttrUpdatePacket)
        {
            AdventurerAttributesData Attr = AdventurerChrAttrCollection.Find(attr => attr.CharacterVId == AttrUpdatePacket.CharacterVId);
            
            if(Attr != null)
            {
                if (AttrUpdatePacket.Name != null)
                {
                    Attr.Name = AttrUpdatePacket.Name;
                    OnAdventurerNameUpdate?.Invoke(Attr.CharacterVId);
                }

                if (AttrUpdatePacket.CurrentHealth != null)
                {
                    Attr.CurrentHealth = AttrUpdatePacket.CurrentHealth.Value;
                    OnAdventurerCurrentHealthUpdate?.Invoke(Attr.CharacterVId);
                }

                if (AttrUpdatePacket.CurrentMana != null)
                {
                    Attr.CurrentMana = AttrUpdatePacket.CurrentMana.Value;
                    OnAdventurerCurrentManaUpdate?.Invoke(Attr.CharacterVId);
                }

                if (AttrUpdatePacket.MaxHealth != null)
                {
                    Attr.MaxHealth = AttrUpdatePacket.MaxHealth.Value;
                    OnAdventurerMaxHealthUpdate?.Invoke(Attr.CharacterVId);
                }

                if (AttrUpdatePacket.MaxMana != null)
                {
                    Attr.MaxMana = AttrUpdatePacket.MaxMana.Value;
                    OnAdventurerMaxManaUpdate?.Invoke(Attr.CharacterVId);
                }

                if (AttrUpdatePacket.MoveSpeed != null)
                {
                    Attr.MoveSpeed = AttrUpdatePacket.MoveSpeed.Value;
                    OnAdventurerMoveSpeedUpdate?.Invoke(Attr.CharacterVId);
                }

                if (AttrUpdatePacket.AttackSpeed != null)
                {
                    Attr.AttackSpeed = AttrUpdatePacket.AttackSpeed.Value;
                    OnAdventurerAttackSpeedUpdate?.Invoke(Attr.CharacterVId);
                }

                if (AttrUpdatePacket.State != null)
                {
                    Attr.State = AttrUpdatePacket.State.Value;
                    OnAdventurerStateUpdate?.Invoke(Attr.CharacterVId);
                }

            }
            else
            {
                // doesnt found
            }
        }

        public void AddAdventurer(AdventurerAttributesData Attrs)
        {
            // If adventurer doesnt exist, add new adventurer.
            if(!AdventurerChrAttrCollection.Any(attr => attr.CharacterVId == Attrs.CharacterVId))
                AdventurerChrAttrCollection.Add(Attrs);
        }

        public void DeleteAdventurer(int adventurerVId)
        {
            AdventurerChrAttrCollection.Remove(AdventurerChrAttrCollection.Find(attr => attr.CharacterVId == adventurerVId));
        }
    }
}
