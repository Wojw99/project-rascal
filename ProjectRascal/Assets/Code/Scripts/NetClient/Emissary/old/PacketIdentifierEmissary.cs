/*using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class PacketIdentifierEmissary
    {
        public void ReceivePacket(CharacterStatePacket state)
        {
            if(CharacterStateEmissary.instance.vId == state.CharacterVId)
            if (name != state.Name)
            {
                name = state.Name;
                OnNameChanged?.Invoke();
            }

            if (currentHealth != state.CurrentHealth)
            {
                currentHealth = state.CurrentHealth;
                OnCurrentHealthChanged?.Invoke();
            }

            if (currentMana != state.CurrentMana)
            {
                currentMana = state.CurrentMana;
                OnCurrentManaChanged?.Invoke();
            }

            if (maxHealth != state.MaxHealth)
            {
                maxHealth = state.MaxHealth;
                OnMaxHealthChanged?.Invoke();
            }

            if (maxMana != state.MaxMana)
            {
                maxMana = state.MaxMana;
                OnMaxManaChanged?.Invoke();
            }
        }

        public void ReceivePacket(CharacterStateUpdatePacket stateUpdate)
        {
            if (stateUpdate.Name != null && stateUpdate.Name != name)
            {
                name = stateUpdate.Name;
                OnNameChanged?.Invoke();
            }

            if (stateUpdate.CurrentHealth != null && stateUpdate.CurrentHealth != currentHealth)
            {
                currentHealth = (float)stateUpdate.CurrentHealth;
                OnCurrentHealthChanged?.Invoke();
            }

            if (stateUpdate.MaxHealth != null && stateUpdate.MaxHealth != maxHealth)
            {
                maxHealth = (float)stateUpdate.MaxHealth;
                OnMaxHealthChanged?.Invoke();
            }

            if (stateUpdate.CurrentMana != null && stateUpdate.CurrentMana != currentMana)
            {
                currentMana = (float)stateUpdate.CurrentMana;
                OnCurrentManaChanged?.Invoke();
            }

            if (stateUpdate.MaxMana != null && stateUpdate.MaxMana != maxMana)
            {
                maxMana = (float)stateUpdate.MaxMana;
                OnMaxManaChanged?.Invoke();
            }

            *//*if (stateUpdate.Attack != null && stateUpdate.Attack != attack) {
                attack = (float)stateUpdate.Attack;
                OnAttackChanged?.Invoke();
            }

            if (stateUpdate.Magic != null && stateUpdate.Magic != magic) {
                magic = (float)stateUpdate.Magic;
                OnMagicChanged?.Invoke();
            }*//*
        }


        #region Singleton

        public static PacketIdentifierEmissary instance;

        private void Awake()
        {
            instance = this;
        }

        #endregion
    }
}
*/