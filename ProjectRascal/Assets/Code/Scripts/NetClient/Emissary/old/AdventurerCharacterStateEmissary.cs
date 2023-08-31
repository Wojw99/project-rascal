/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using Assets.Code.Scripts.NetClient.Clients;
using Unity.VisualScripting;
using System.Collections.Concurrent;
using Assets.Code.Scripts.NetClient;

public class AdventurerCharacterStateEmissary : MonoBehaviour
{
    //public GameCharacter CharacterState;

    public ConcurrentDictionary<int, CharacterAttributes> Characters { get; private set; } 
        = new ConcurrentDictionary<int, CharacterAttributes>();

    public delegate void AttributeChanged(int characterVId);

    public event AttributeChanged OnNameChanged;
    public event AttributeChanged OnCurrentHealthChanged;
    public event AttributeChanged OnCurrentManaChanged;
    public event AttributeChanged OnMaxHealthChanged;
    public event AttributeChanged OnMaxManaChanged;
    public event AttributeChanged OnAttackChanged;
    public event AttributeChanged OnMagicChanged;

    public void ReceivePacket(CharacterStatePacket state)
    {
        CharacterAttributes attr = new CharacterAttributes();
        attr.currentHealth = state.CurrentHealth;

        Characters[state.CharacterVId] = new CharacterAttributes { currentHealth: state.CurrentHealth }
        name = state.Name;

    }

    public void ReceivePacket(CharacterStateUpdatePacket stateUpdate)
    {
        if (stateUpdate.Name != null && stateUpdate.Name != name) {
            name = stateUpdate.Name;
            OnNameChanged?.Invoke();
        }

        if (stateUpdate.CurrentHealth != null && stateUpdate.CurrentHealth != currentHealth) {
            currentHealth = (float)stateUpdate.CurrentHealth;
            OnCurrentHealthChanged?.Invoke();
        }

        if (stateUpdate.MaxHealth != null && stateUpdate.MaxHealth != maxHealth) {
            maxHealth = (float)stateUpdate.MaxHealth;
            OnMaxHealthChanged?.Invoke();
        }

        if (stateUpdate.CurrentMana != null && stateUpdate.CurrentMana != currentMana) {
            currentMana = (float)stateUpdate.CurrentMana;
            OnCurrentManaChanged?.Invoke();
        }

        if (stateUpdate.MaxMana != null && stateUpdate.MaxMana != maxMana) {
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

        OnCharacterStateUpdatePacketReceived?.Invoke();
    }


    #region Singleton

    public static CharacterStateEmissary instance;

    private void Awake()
    {
        instance = this;
    }

    public void CommitAttackMonster()
    {

        //GameClient.instance.GameServerPeer.SendPacket();
    }

    #endregion
}
*/