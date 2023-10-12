/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using Assets.Code.Scripts.NetClient.Clients;
using Unity.VisualScripting;
using Assets.Code.Scripts.NetClient;
using System.Collections.Concurrent;

public class CharacterStateEmissary : MonoBehaviour
{
    public bool PlayerCharacterLoadSucces { get; private set; } = false;

    private int PlayerCharacterVId = -1;

    public delegate void PlayerAttributeChanged(object value);

    public delegate void AdventurerAttributeChanged(int adventurerId, object value);

    public event PlayerAttributeChanged OnPlayerNameChanged;
    public event PlayerAttributeChanged OnPlayerCurrentHealthChanged;
    public event PlayerAttributeChanged OnPlayerCurrentManaChanged;
    public event PlayerAttributeChanged OnPlayerMaxHealthChanged;
    public event PlayerAttributeChanged OnPlayerMaxManaChanged;
    public event PlayerAttributeChanged OnPlayerAttackChanged;
    public event PlayerAttributeChanged OnPlayerMagicChanged;

    public event AdventurerAttributeChanged OnAdventurerNameChanged;
    public event AdventurerAttributeChanged OnAdventurerCurrentHealthChanged;
    public event AdventurerAttributeChanged OnAdventurerCurrentManaChanged;
    public event AdventurerAttributeChanged OnAdventurerMaxHealthChanged;
    public event AdventurerAttributeChanged OnAdventurerMaxManaChanged;
    public event AdventurerAttributeChanged OnAdventurerAttackChanged;
    public event AdventurerAttributeChanged OnAdventurerMagicChanged;

    public void ReceivePacket(CharacterLoadResponsePacket packet)
    {
        PlayerCharacterVId = packet.StatePacket.CharacterVId;
        PlayerCharacterLoadSucces = true;

        ReceivePacket(packet.StatePacket);
    }

    public void ReceivePacket(CharacterStatesPacket statesPacket)
    {
        foreach (var statePacket in statesPacket.PacketCollection) {
            ReceivePacket(statePacket);
        }
    }

    public void ReceivePacket(CharacterStatesUpdatePacket statesUpdatePacket)
    {
        foreach (var stateUpdatePacket in statesUpdatePacket.PacketCollection) {
            ReceivePacket(stateUpdatePacket);
        }
    }

    public void ReceivePacket(CharacterStatePacket state)
    {
        if (PlayerCharacterLoadSucces) {
            if (state.CharacterVId == PlayerCharacterVId) {
                ChangePlayerAttributes(state);
            }
            else {
                ChangeAdventurerAttributes(state);
            }
        }
    }
    public void ReceivePacket(CharacterStateUpdatePacket stateUpdate)
    {
        if (PlayerCharacterLoadSucces) {
            if (stateUpdate.CharacterVId == PlayerCharacterVId) {
                UpdatePlayerAttributes(stateUpdate);
            }
            else {
                UpdateAdventurerAttributes(stateUpdate);
            }
        }
    }

    private void ChangePlayerAttributes(CharacterStatePacket statePacket)
    {
        OnPlayerNameChanged?.Invoke(statePacket.Name);
        OnPlayerCurrentHealthChanged?.Invoke(statePacket.CurrentHealth);
        OnPlayerCurrentManaChanged?.Invoke(statePacket.CurrentMana);
        OnPlayerMaxHealthChanged?.Invoke(statePacket.MaxHealth);
        OnPlayerMaxManaChanged?.Invoke(statePacket.MaxMana);
    }

    private void ChangeAdventurerAttributes(CharacterStatePacket statePacket)
    {
        int AdventurerId = statePacket.CharacterVId;
        OnAdventurerNameChanged?.Invoke(AdventurerId, statePacket.Name);
        OnAdventurerCurrentHealthChanged?.Invoke(AdventurerId, statePacket.CurrentHealth);
        OnAdventurerCurrentManaChanged?.Invoke(AdventurerId, statePacket.CurrentMana);
        OnAdventurerMaxHealthChanged?.Invoke(AdventurerId, statePacket.MaxHealth);
        OnAdventurerMaxManaChanged?.Invoke(AdventurerId, statePacket.MaxMana);
    }

    private void UpdatePlayerAttributes(CharacterStateUpdatePacket stateUpdatePacket)
    {
        if (stateUpdatePacket.Name != null) {
            OnPlayerNameChanged?.Invoke(stateUpdatePacket.Name);
        }

        if (stateUpdatePacket.CurrentHealth != null) {
            OnPlayerCurrentHealthChanged?.Invoke(stateUpdatePacket.CurrentHealth);
        }

        if (stateUpdatePacket.MaxHealth != null) {
            OnPlayerMaxHealthChanged?.Invoke(stateUpdatePacket.MaxHealth);
        }

        if (stateUpdatePacket.CurrentMana != null) {
            OnPlayerCurrentManaChanged?.Invoke(stateUpdatePacket.CurrentMana);
        }

        if (stateUpdatePacket.MaxMana != null) {
            OnPlayerMaxManaChanged?.Invoke(stateUpdatePacket.MaxMana);
        }
    }

    private void UpdateAdventurerAttributes(CharacterStateUpdatePacket stateUpdatePacket)
    {
        int AdventurerId = stateUpdatePacket.CharacterVId;

        if (stateUpdatePacket.Name != null) {
            OnAdventurerNameChanged?.Invoke(AdventurerId, stateUpdatePacket.Name);
        }

        if (stateUpdatePacket.CurrentHealth != null) {
            OnAdventurerCurrentHealthChanged?.Invoke(AdventurerId, stateUpdatePacket.CurrentHealth);
        }

        if (stateUpdatePacket.MaxHealth != null)
        {
            OnAdventurerMaxHealthChanged?.Invoke(AdventurerId, stateUpdatePacket.MaxHealth);
        }

        if (stateUpdatePacket.CurrentMana != null) {
            OnAdventurerCurrentManaChanged?.Invoke(AdventurerId, stateUpdatePacket.CurrentMana);
        }

        if (stateUpdatePacket.MaxMana != null) {
            OnAdventurerMaxManaChanged?.Invoke(AdventurerId, stateUpdatePacket.MaxMana);
        }
    }

    #region Singleton

    public static CharacterStateEmissary instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
}
*/