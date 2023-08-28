using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCore.NetworkMessage;

public class CharacterStateEmissary : MonoBehaviour
{
    public delegate void PacketReceived();

    public event PacketReceived OnCharacterStatePacketReceived;

    #region Singleton

    public static CharacterStateEmissary instance;

    private void Awake()
    {
        instance = this;
    }

    private CharacterStateEmissary()
    {

    }
    #endregion
}
