using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkData;
using NetworkCore.NetworkMessage;
using NetworkCore.NetworkUtility;
using NetworkCore.Packets;
using ServerApplication.GameService.Base;

namespace ServerApplication.GameService.Player
{
    public class PlayerPeer : TcpPeer
    {
        private World WorldRef { get; set; }

        public Character PlayerCharacter { get; set; }

        private AttributesUpdatePacket CharacterStateUpdate { get; set; }

        private TransformPacket CharacterTransform { get; set; }

        private readonly object stateLock = new object();

        public PlayerPeer(PacketHandler packetHandler, PacketSender packetSender, World worldRef, Socket peerSocket, Guid peerId,
            Owner ownerType) : base(packetHandler, packetSender, peerSocket, peerId, ownerType)
        {
            WorldRef = worldRef;
            PlayerCharacter = new Character();
            CharacterStateUpdate = new AttributesUpdatePacket(-1);
            CharacterTransform = new TransformPacket(-1);

            PacketHandlerRef.AddHandler(this.GUID, typeof(CharacterLoadRequestPacket), ReceiveLoadRequest);
            PacketHandlerRef.AddHandler(this.GUID, typeof(CharacterLoadSuccesPacket), ReceiveLoadStatus);
            PacketHandlerRef.AddHandler(this.GUID, typeof(TransformPacket), ReceiveTransform);
            PacketHandlerRef.AddHandler(this.GUID, typeof(ClientDisconnectPacket), ReceiveDisconnect);
            PacketHandlerRef.AddHandler(this.GUID, typeof(PingRequestPacket), ReceivePingRequest);

        }

        private async void ReceiveLoadRequest(CharacterLoadRequestPacket packet)
        {
            if (packet.AuthToken == "gracz")
            {
                // load username from token
                string username = "some_username_from_token";

                // load player object by username
                LoadCharacterFromDatabase(username, WorldRef.IdCounter++); // by now overloaded with unique identifiers from server app.

                // send response with player object
                RequestSendPacket(new CharacterLoadResponsePacket(PlayerCharacter));

            }
            else
            {
                // CharacterLoadResponsePacket Succes is false by default.
                RequestSendPacket(new CharacterLoadResponsePacket());

                //disconnet Connection
                Disconnect();
            }
        }

        private async void ReceiveLoadStatus(CharacterLoadSuccesPacket packet)
        {
            if (packet.Succes == true)
            {
                // In that method we also run OnCharacterLoad(), which simply sends to new connected
                // player current states of all players.
                await WorldRef.AddNewPlayerAsync(this);

            }
        }

        private void ReceiveTransform(TransformPacket packet)
        {
            PlayerCharacter.PositionX = packet.PosX;
            PlayerCharacter.PositionY = packet.PosY;
            PlayerCharacter.PositionZ = packet.PosZ;
            PlayerCharacter.RotationX = packet.RotX;
            PlayerCharacter.RotationY = packet.RotY;
            PlayerCharacter.RotationZ = packet.RotZ;

            CharacterTransform.PosX = packet.PosX;
            CharacterTransform.PosY = packet.PosY;
            CharacterTransform.PosZ = packet.PosZ;
            CharacterTransform.RotX = packet.RotX;
            CharacterTransform.RotY = packet.RotY;
            CharacterTransform.RotZ = packet.RotZ;
            CharacterTransform.State = packet.State; //AdventurerState.Running;

            WorldRef.AddNewCharacterTransform(GUID, CharacterTransform);
        }

        private async void ReceiveDisconnect(ClientDisconnectPacket packet)
        {
            //await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {playerConn.PeerSocket.RemoteEndPoint}. ");

            if (packet.AuthToken == "gracz") // we need that to check?
            {
                // save player state into database
            }

            WorldRef.RemovePlayer(this); // delete from world
            Disconnect(); // disconnect from server
        }

        private async void ReceivePingRequest(PingRequestPacket packet)
        {
            RequestSendPacket(new PingResponsePacket());
        }

        private void LoadCharacterFromDatabase(string username, int UniqueId)
        {
            PlayerCharacter = new Character(UniqueId, "nowy gracz", 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 5, 5, AdventurerState.Idle);
            CharacterTransform.CharacterVId = UniqueId;
            CharacterStateUpdate.CharacterVId = UniqueId;
        }

        private void PlayerAttack()
        {

        }

        public void SetName(string name)
        {
            lock (stateLock)
            {
                PlayerCharacter.Name = name;
                CharacterStateUpdate.Name = name;
                WorldRef.AddNewCharacterStateUpdate(GUID, CharacterStateUpdate);
            }
        }

        public void SetCurrentHealth(float currentHealth)
        {
            lock (stateLock)
            {
                PlayerCharacter.CurrentHealth = currentHealth;
                CharacterStateUpdate.CurrentHealth = currentHealth;
                WorldRef.AddNewCharacterStateUpdate(GUID, CharacterStateUpdate);
            }
        }

        public void SetMaxHealth(float maxHealth)
        {
            lock (stateLock)
            {
                PlayerCharacter.MaxHealth = maxHealth;
                CharacterStateUpdate.MaxHealth = maxHealth;
                WorldRef.AddNewCharacterStateUpdate(GUID, CharacterStateUpdate);
            }
        }

        public void SetCurrentMana(float currentMana)
        {
            lock (stateLock)
            {
                PlayerCharacter.CurrentMana = currentMana;
                CharacterStateUpdate.CurrentMana = currentMana;
                WorldRef.AddNewCharacterStateUpdate(GUID, CharacterStateUpdate);
            }
        }

        public void SetMaxMana(float maxMana)
        {
            lock (stateLock)
            {
                PlayerCharacter.MaxMana = maxMana;
                CharacterStateUpdate.MaxMana = maxMana;
                WorldRef.AddNewCharacterStateUpdate(GUID, CharacterStateUpdate);
            }
        }

        // maybe change name to smthing like SetPlayerCharacterState
        public void SetAdventurerState(AdventurerState state)
        {
            lock (stateLock)
            {
                PlayerCharacter.State = state;
                CharacterStateUpdate.State = state;
                WorldRef.AddNewCharacterStateUpdate(GUID, CharacterStateUpdate);
            }
        }

        // Change names - now we have additonal enum AdventurerState
        public void OnCharactedStateSend()
        {
            lock (stateLock)
            {
                CharacterStateUpdate.Clear();
            }
        }
    }
}
