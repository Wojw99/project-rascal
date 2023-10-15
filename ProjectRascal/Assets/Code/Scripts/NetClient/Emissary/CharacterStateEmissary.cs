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
    public class CharacterStateEmissary : MonoBehaviour
    {
        #region Singleton

        public static CharacterStateEmissary instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion

        public delegate void PlayerAttributeUpdate();
        public delegate void PacketReceived();

        public event PlayerAttributeUpdate OnPlayerNameUpdate;
        public event PlayerAttributeUpdate OnPlayerCurrentHealthUpdate;
        public event PlayerAttributeUpdate OnPlayerCurrentManaUpdate;
        public event PlayerAttributeUpdate OnPlayerMaxHealthUpdate;
        public event PlayerAttributeUpdate OnPlayerMaxManaUpdate;
        public event PlayerAttributeUpdate OnPlayerAttackUpdate;
        public event PlayerAttributeUpdate OnPlayerMagicUpdate;

        public event PacketReceived OnPlayerStateChanged;

        public int CharacterVId { get; private set; }
        public string Name { get; private set; }
        public float CurrentHealth { get; private set; }
        public float CurrentMana { get; private set; }
        public float MaxHealth { get; private set; }
        public float MaxMana { get; private set; }

        public void ReceiveAttributesData(AttributesPacket AttrPacket)
        {
            CharacterVId = AttrPacket.CharacterVId;
            Debug.Log("Przydzielam id = " + AttrPacket.CharacterVId);
            Name = AttrPacket.Name;
            CurrentHealth = AttrPacket.CurrentHealth;
            CurrentMana = AttrPacket.CurrentMana;
            MaxHealth = AttrPacket.MaxHealth;
            MaxMana = AttrPacket.MaxMana;

            OnPlayerStateChanged?.Invoke();
        }
        public void ReceiveAttributesDataUpdate(AttributesUpdatePacket AttrUpdPacket)
        {
            if (AttrUpdPacket.Name != null)
            {
                Name = AttrUpdPacket.Name;
                OnPlayerNameUpdate?.Invoke();
            }

            if (AttrUpdPacket.CurrentHealth != null)
            {
                CurrentHealth = AttrUpdPacket.CurrentHealth.Value;
                OnPlayerCurrentHealthUpdate?.Invoke();
            }

            if (AttrUpdPacket.CurrentMana != null)
            {
                CurrentMana = AttrUpdPacket.CurrentMana.Value;
                OnPlayerCurrentManaUpdate?.Invoke();
            }

            if (AttrUpdPacket.MaxHealth != null)
            {
                MaxHealth = AttrUpdPacket.MaxHealth.Value;
                OnPlayerMaxHealthUpdate?.Invoke();
            }

            if (AttrUpdPacket.MaxMana != null)
            {
                MaxMana = AttrUpdPacket.MaxMana.Value;
                OnPlayerMaxManaUpdate?.Invoke();
            }
        }
    }
}
