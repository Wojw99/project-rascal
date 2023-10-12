/*using System;
using System.Collections.Generic;
using UnityEngine;
using NetworkCore.Packets;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Assets.Code.Scripts.NetClient.Holder.PlayerDataHolder;
using Assets.Code.Scripts.NetClient.Attributes;

namespace Assets.Code.Scripts.NetClient.Holder
{
    public class AdventurersDataHolder : MonoBehaviour
    {
        public List<Attributes> AdventurerChrAttrCollection { get; private set; }
            = new List<Attributes>();

        public List<TransformData> AdventurerChrTransformCollection { get; private set; }
            = new List<TransformData>();

        public delegate void AdventurerAttributeUpdate(int adventurerId);

        public event AdventurerAttributeUpdate OnAdventurerNameUpdate;
        public event AdventurerAttributeUpdate OnAdventurerCurrentHealthUpdate;
        public event AdventurerAttributeUpdate OnAdventurerCurrentManaUpdate;
        public event AdventurerAttributeUpdate OnAdventurerMaxHealthUpdate;
        public event AdventurerAttributeUpdate OnAdventurerMaxManaUpdate;
        public event AdventurerAttributeUpdate OnAdventurerAttackUpdate;
        public event AdventurerAttributeUpdate OnAdventurerMagicUpdate;

        public event AdventurerAttributeUpdate OnAdventurerTransformUpdate;

        public void AddNewAdventurer(CharacterStatePacket statePacket)
        {
            AdventurerChrAttrCollection.Add(new CharacterAttributes(statePacket.CharacterVId, statePacket.Name, 
                statePacket.CurrentHealth, statePacket.CurrentMana, statePacket.MaxHealth, statePacket.MaxMana, 0, 0));

            AdventurerChrTransformCollection.Add(new TransformData(statePacket.CharacterVId, 
                new Vector3(statePacket.PosX, statePacket.PosY, statePacket.PosZ), Vector3.zero));
        }

        public void DeleteAdventurer(int adventurerVId)
        {
            AdventurerChrAttrCollection.Remove(AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId));
            AdventurerChrTransformCollection.Remove(AdventurerChrTransformCollection.Find(attr => attr.CharacterVId == adventurerVId));
        }

        public void UpdateName(int adventurerVId, string name)
        {
            AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).name = name;
            OnAdventurerNameUpdate?.Invoke(adventurerVId); 
        }

        public void UpdateCurrentHealth(int adventurerVId, float currentHealth)
        {
            AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).currentHealth = currentHealth;
            OnAdventurerCurrentHealthUpdate?.Invoke(adventurerVId);
        }

        public void UpdateCurrentMana(int adventurerVId, float currentMana)
        {
            AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).currentMana = currentMana;
            OnAdventurerCurrentManaUpdate?.Invoke(adventurerVId);
        }

        public void UpdateMaxHealth(int adventurerVId, float maxHealth)
        {
            AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).maxHealth = maxHealth;
            OnAdventurerMaxHealthUpdate?.Invoke(adventurerVId);
        }

        public void UpdateMaxMana(int adventurerVId, float maxMana)
        {
            AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).maxMana = maxMana;
            OnAdventurerMaxManaUpdate?.Invoke(adventurerVId);
        }

        public void UpdateTransform(int adventurerVId, Vector3 position, Vector3 rotation)
        {
            TransformData transform = AdventurerChrTransformCollection.Find(attr => attr.CharacterVId == adventurerVId);
            transform.Position = position;
            transform.Rotation = rotation;

            OnAdventurerTransformUpdate?.Invoke(adventurerVId);
        }

        public string GetName(int adventurerVId)
        {
            return AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).name;
        }

        public float GetCurrentHealth(int adventurerVId)
        {
            return AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).currentHealth;
        }

        public float GetCurrentMana(int adventurerVId)
        {
            return AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).currentMana;
        }

        public float GetMaxHealth(int adventurerVId)
        {
            return AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).maxHealth;
        }

        public float GetMaxMana(int adventurerVId)
        {
            return AdventurerChrAttrCollection.Find(attr => attr.characterVId == adventurerVId).maxMana;
        }

        #region Singleton

        public static AdventurersDataHolder instance;

        private void Awake()
        {
            instance = this;
        }

        private AdventurersDataHolder()
        {

        }

        #endregion
    }
}
*/