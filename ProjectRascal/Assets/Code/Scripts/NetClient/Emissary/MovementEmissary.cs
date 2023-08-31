using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCore.NetworkMessage;
using Assets.Code.Scripts.NetClient.Clients;
using NetworkCore.Packets;
using Assets.Code.Scripts.NetClient;
using System.Collections.Concurrent;
using Assets.Code.Scripts.NetClient.Attributes;
using Assets.Code.Scripts.NetClient.Emissary;
using UnityEditor.ShaderGraph.Internal;
using Assets.Code.Scripts.NetClient.Holder;

public class MovementEmissary : MonoBehaviour
{
    private float timeSinceLastPacket = 0f;
    private float packetSendInterval = 0.1f;

    public void ReceivePacket(CharacterTransformPacket packet)
    {
        if(packet.CharacterVId == PlayerDataHolder.instance.VId)
        {
            PlayerDataHolder.instance.UpdateTransform(new Vector3(
                packet.PosX, packet.PosY, packet.PosZ), new Vector3(packet.RotX, packet.RotY, packet.RotZ));
        }
        else
        {
            AdventurersDataHolder.instance.UpdateTransform(packet.CharacterVId, new Vector3(
                packet.PosX, packet.PosY, packet.PosZ), new Vector3(packet.RotX, packet.RotY, packet.RotZ));
        }
    }

    public void CommitSendPlayerCharacterTransfer(int characterVId, float posX, float posY, float posZ, 
        float rotX, float rotY, float rotZ)
    {
        timeSinceLastPacket += Time.deltaTime;

        if (timeSinceLastPacket >= packetSendInterval)
        {
            GameClient.instance.GameServerPeer.SendPacket(
                new CharacterTransformPacket(characterVId, posX, posY, posZ, rotX, rotY, rotZ));
        }
    }


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
