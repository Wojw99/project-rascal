using Assets.Code.Scripts.NetClient;
using Assets.Code.Scripts.NetClient.Emissary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Code.Scripts
{
    public class AdventurerCharacter : GameCharacter
    {
        public AdventurerCharacter(AdventurerAttributesData data)
        {
            this.vId = data.CharacterVId;
            this.name = data.Name;
            this.currentHealth = data.CurrentHealth;
            this.maxHealth = data.MaxHealth;
            this.currentMana = data.CurrentMana;
            this.maxMana = data.MaxMana;
            this.attack = 0;
            this.magic = 0;
        }

        public void SetName(string name)
        {
            this.name = name;
        }

        public void SetCurrentHealth(float currentHealth)
        {
            this.currentHealth = currentHealth;
        }

        public void SetCurrentMana(float currentMana)
        {
            this.currentMana = currentMana;
        }

        public void SetMaxHealth(float maxHealth)
        {
            this.maxHealth = maxHealth;
        }

        public void SetMaxMana(float maxMana)
        {
            this.maxMana = maxMana;
        }
    }
}
