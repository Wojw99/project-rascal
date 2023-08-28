using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCore.NetworkMessage;

public class MovementEmissary : MonoBehaviour
{
    #region Singleton

    public static MovementEmissary instance;

    private void Awake()
    {
        instance = this;
    }

    private MovementEmissary()
    {

    }


    #endregion

    private string currentSignal = "";

    public delegate void PacketReceived();

    public event PacketReceived OnMovePacketReceived;

    // public event EventHandler<OnSignalChangedEventArgs> OnSignalChanged;
    /*public delegate void SignalEventDelegate(string signal);
    public event SignalEventDelegate OnSignalChanged;*/

    /*public Vector3 Position
    {
        *//*set
        {
            currentSignal = value;
            OnMovePacketReceived?.Invoke(value);
        }*//*
    }*/

    /*public void Clear()
    {
        currentSignal = "";
    }*/
}
