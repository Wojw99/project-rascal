/*using Assets.Code.Scripts.NetClient.Attributes;
using Assets.Code.Scripts.NetClient.Emissary;
using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static CharacterStateEmissary;

namespace Assets.Code.Scripts.NetClient.Holder
{
    public class PlayerDataHolder : MonoBehaviour
    {
        private Attributes PlayerCharacterAttributes;
        private TransformData PlayerCharacterTransform;

        public delegate void PlayerAttributeUpdate();

        public event PlayerAttributeUpdate OnPlayerNameUpdate;
        public event PlayerAttributeUpdate OnPlayerCurrentHealthUpdate;
        public event PlayerAttributeUpdate OnPlayerCurrentManaUpdate;
        public event PlayerAttributeUpdate OnPlayerMaxHealthUpdate;
        public event PlayerAttributeUpdate OnPlayerMaxManaUpdate;
        public event PlayerAttributeUpdate OnPlayerAttackUpdate;
        public event PlayerAttributeUpdate OnPlayerMagicUpdate;

        public event PlayerAttributeUpdate OnPlayerTransformUpdate;

        public int VId { get { return PlayerCharacterAttributes.characterVId; } }
        public string Name { get { return PlayerCharacterAttributes.name; } }
        public float CurrentHealth { get { return PlayerCharacterAttributes.currentHealth; } }
        public float CurrentMana { get { return PlayerCharacterAttributes.currentMana; } }
        public float MaxHealth { get { return PlayerCharacterAttributes.maxHealth; } }
        public float MaxMana { get { return PlayerCharacterAttributes.maxMana; } }
        public Vector3 Position { get { return PlayerCharacterTransform.Position; } }
        public Vector3 Rotation { get { return PlayerCharacterTransform.Rotation; } }

        public void InitPlayerCharacter(CharacterLoadResponsePacket chrPacket)
        {
            PlayerCharacterAttributes.characterVId = chrPacket.StatePacket.CharacterVId;
            PlayerCharacterAttributes.name = chrPacket.StatePacket.Name;
            PlayerCharacterAttributes.currentHealth = chrPacket.StatePacket.CurrentHealth;
            PlayerCharacterAttributes.currentMana = chrPacket.StatePacket.CurrentMana;
            PlayerCharacterAttributes.maxHealth = chrPacket.StatePacket.MaxHealth;
            PlayerCharacterAttributes.maxMana = chrPacket.StatePacket.MaxMana;

            PlayerCharacterTransform.CharacterVId = chrPacket.StatePacket.CharacterVId;
            PlayerCharacterTransform.Position = new Vector3(chrPacket.StatePacket.PosX, chrPacket.StatePacket.PosY, chrPacket.StatePacket.PosZ);
            PlayerCharacterTransform.Rotation = Vector3.zero;
        }

        public void UpdateName(string name)
        {
            PlayerCharacterAttributes.name = name;
            OnPlayerNameUpdate?.Invoke();
        }

        public void UpdateCurrentHealth(float currentHealth)
        {
            PlayerCharacterAttributes.currentHealth = currentHealth;
            OnPlayerCurrentHealthUpdate?.Invoke();
        }

        public void UpdateCurrentMana(float currentMana)
        {
            PlayerCharacterAttributes.currentMana = currentMana;
            OnPlayerCurrentManaUpdate?.Invoke();
        }

        public void UpdateMaxHealth(float maxHealth)
        {
            PlayerCharacterAttributes.maxHealth = maxHealth;
            OnPlayerMaxHealthUpdate?.Invoke();
        }

        public void UpdateMaxMana(float maxMana)
        {
            PlayerCharacterAttributes.maxMana = maxMana;
            OnPlayerMaxManaUpdate?.Invoke();
        }

        public void UpdateTransform(Vector3 position, Vector3 rotation)
        {
            PlayerCharacterTransform.Position = position;
            PlayerCharacterTransform.Rotation = rotation;
            OnPlayerTransformUpdate?.Invoke();
        }

        #region Singleton

        public static PlayerDataHolder instance;

        private void Awake()
        {
            instance = this;
        }

        private PlayerDataHolder()
        {
            
        }

        #endregion
    }
}
*/