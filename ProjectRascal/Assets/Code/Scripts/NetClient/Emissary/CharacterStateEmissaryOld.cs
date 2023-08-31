using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using Assets.Code.Scripts.NetClient.Clients;
using Unity.VisualScripting;
using Assets.Code.Scripts.NetClient;
using System.Collections.Concurrent;
using Assets.Code.Scripts.NetClient.Emissary;
using Assets.Code.Scripts.NetClient.Holder;

public class CharacterStateEmissary : MonoBehaviour
{
    public void ReceivePacket(CharacterStatesPacket statesPacket)
    {
        foreach (var statePacket in statesPacket.PacketCollection)
        {
            ReceivePacket(statePacket);
        }
    }

    public void ReceivePacket(CharactersAttrsUpdatePacket updatesPacket)
    {
        foreach (var stateUpdatePacket in updatesPacket.PacketCollection)
        {
            ReceivePacket(stateUpdatePacket);
        }
    }

    public void ReceivePacket(CharacterStatePacket statePacket)
    {
        if (PlayerCharacterLoadEmissary.instance.PlayerCharacterLoadSucces)
        {
            if (statePacket.CharacterVId != PlayerDataHolder.instance.VId)
            {
                AdventurersDataHolder.instance.AddNewAdventurer(statePacket);
            }
        }
    }
    public void ReceivePacket(CharacterAttrUpdatePacket updatePacket)
    {
        if (PlayerCharacterLoadEmissary.instance.PlayerCharacterLoadSucces)
        {
            if (updatePacket.CharacterVId == PlayerDataHolder.instance.VId)
            {
                UpdatePlayerAttributes(updatePacket);
            }
            else
            {
                UpdateAdventurerAttributes(updatePacket);
            }
        }
    }

    private void UpdatePlayerAttributes(CharacterAttrUpdatePacket updatePacket)
    {
        if(updatePacket.Name != null)
            PlayerDataHolder.instance.UpdateName(updatePacket.Name);

        if(updatePacket.CurrentHealth != null)
            PlayerDataHolder.instance.UpdateCurrentHealth((float)updatePacket.CurrentHealth);

        if(updatePacket.CurrentMana != null)
            PlayerDataHolder.instance.UpdateCurrentMana((float)updatePacket.MaxHealth);

        if (updatePacket.MaxHealth != null)
            PlayerDataHolder.instance.UpdateMaxHealth((float)updatePacket.MaxHealth);

        if (updatePacket.MaxMana != null)
            PlayerDataHolder.instance.UpdateMaxHealth((float)updatePacket.MaxMana);
    }

    private void UpdateAdventurerAttributes(CharacterAttrUpdatePacket updatePacket)
    {
        if (updatePacket.Name != null)
            AdventurersDataHolder.instance.UpdateName(updatePacket.CharacterVId, updatePacket.Name);

        if (updatePacket.CurrentHealth != null)
            AdventurersDataHolder.instance.UpdateCurrentHealth(updatePacket.CharacterVId, (float)updatePacket.CurrentHealth);

        if (updatePacket.CurrentMana != null)
            AdventurersDataHolder.instance.UpdateCurrentMana(updatePacket.CharacterVId, (float)updatePacket.MaxHealth);

        if (updatePacket.MaxHealth != null)
            AdventurersDataHolder.instance.UpdateMaxHealth(updatePacket.CharacterVId, (float)updatePacket.MaxHealth);

        if (updatePacket.MaxMana != null)
            AdventurersDataHolder.instance.UpdateMaxHealth(updatePacket.CharacterVId, (float)updatePacket.MaxMana);
    }

    #region Singleton

    public static CharacterStateEmissary instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
}
