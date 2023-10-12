/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCore.NetworkMessage;
using NetworkCore.Packets;
using Unity.VisualScripting;
using Assets.Code.Scripts.NetClient;
using System.Collections.Concurrent;
using Assets.Code.Scripts.NetClient.Emissary;
using Assets.Code.Scripts.NetClient.Holder;
using System.Threading.Tasks;
using NetClient;

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

    public async Task LoadCharacterAttributes(int slotNum)
    {
        ClientSingleton Client = ClientSingleton.GetInstance();

        if (Client.GameServer.IsConnected)
        {

            await Client.GameServer.SendPacket(new CharacterLoadRequestPacket(AuthEmissary.instance.AuthToken));

            try
            {
                PacketBase packet = await Client.WaitForResponsePacket(TimeSpan.FromMilliseconds(20),
                    TimeSpan.FromSeconds(20), PacketType.CHARACTER_LOAD_RESPONSE); // Parametry: 1.intervał, 2.limit czasu, 3.typ pakietu

                if (packet is CharacterLoadResponsePacket characterLoadResponse)
                {
                    if (characterLoadResponse.Success == true)
                    {
                        //ClientNetwork.ClientPlayer = characterLoadResponse.GetCharacter();
                        PlayerDataHolder.instance.
                        Character ClientChar = characterLoadResponse.GetCharacter();


                        // Jeśli uda się wszystko załadować:
                        await GameServer.SendPacket(new CharacterLoadSuccesPacket(true));

                        await Console.Out.WriteLineAsync("Character loaded succesfully.");


                    }
                    else
                    {
                        await GameServer.SendPacket(new ClientDisconnectPacket(AuthToken));
                        GameServer.Disconnect();
                    }
                }
            }
            catch (TimeoutException ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                await GameServer.SendPacket(new ClientDisconnectPacket(AuthToken));
                GameServer.Disconnect();
            }
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