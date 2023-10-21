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
        private TestServer ServerRef { get; set; }

        private World WorldRef { get; set; }

        public Character PlayerCharacter { get; set; }

        private AttributesUpdatePacket CharacterStateUpdate { get; set; }

        private TransformPacket CharacterTransform { get; set; }

        private readonly object stateLock = new object();

        public PlayerPeer(TestServer serverRef, World worldRef, Socket peerSocket, Guid peerId,
            Owner ownerType) : base(serverRef, peerSocket, peerId, ownerType)
        {
            ServerRef = serverRef;
            WorldRef = worldRef;
            PlayerCharacter = new Character();
            CharacterStateUpdate = new AttributesUpdatePacket(-1);
            CharacterTransform = new TransformPacket(-1);

            ServerRef._PacketHandler.AddHandler(this.GUID, typeof(CharacterLoadRequestPacket), ReceiveLoadRequest);
            ServerRef._PacketHandler.AddHandler(this.GUID, typeof(CharacterLoadSuccesPacket), ReceiveLoadStatus);
            ServerRef._PacketHandler.AddHandler(this.GUID, typeof(TransformPacket), ReceiveTransform);
            ServerRef._PacketHandler.AddHandler(this.GUID, typeof(ClientDisconnectPacket), ReceiveDisconnect);
            ServerRef._PacketHandler.AddHandler(this.GUID, typeof(PingRequestPacket), ReceivePingRequest);

        }

        public async void ReceiveLoadRequest(CharacterLoadRequestPacket packet)
        {
            if (packet.AuthToken == "gracz")
            {
                // load username from token
                string username = "some_username_from_token";

                // load player object by username
                LoadCharacterFromDatabase(username, 1); // by now overloaded with unique identifiers from server app.

                // send response with player object
                await SendPacket(new CharacterLoadResponsePacket(PlayerCharacter));

            }
            else
            {
                // CharacterLoadResponsePacket Succes is false by default.
                await SendPacket(new CharacterLoadResponsePacket());

                //disconnet Connection
                Disconnect();
            }
        }

        public async void ReceiveLoadStatus(CharacterLoadSuccesPacket packet)
        {
            if (packet.Succes == true)
            {
                // In that method we also run OnCharacterLoad(), which simply sends to new connected
                // player current states of all players.
                await WorldRef.AddNewPlayer(this);

            }
        }

        public void ReceiveTransform(TransformPacket packet)
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

        public async void ReceiveDisconnect(ClientDisconnectPacket packet)
        {
            //await Console.Out.WriteLineAsync($"[CLIENT CONNECTION CLOSED], with info: {playerConn.PeerSocket.RemoteEndPoint}. ");

            if (packet.AuthToken == "gracz") // we need that to check?
            {
                // save player state into database
            }

            await WorldRef.RemovePlayer(this); // delete from world
            Disconnect(); // disconnect from server
        }

        public async void ReceivePingRequest(PingRequestPacket packet)
        {
            await SendPacket(new PingResponsePacket());
        }
        
        public void LoadCharacterFromDatabase(string username, int UniqueId)
        {
            PlayerCharacter = new Character(UniqueId, "nowy gracz", 10, 10, 10, 10, 0, 0, 0, 0, 0, 0, 5, 5, AdventurerState.Idle);
            CharacterTransform.CharacterVId = UniqueId;
            CharacterStateUpdate.CharacterVId = UniqueId;
        }

        public void PlayerAttack()
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

      /*  public void SetTransform(float posX, float posY, float posZ, float rotX, float rotY, float rotZ)
        {
            lock (stateLock)
            {
                PlayerCharacter.PositionX = posX;
                PlayerCharacter.PositionY = posY;
                PlayerCharacter.PositionZ = posZ;
                PlayerCharacter.RotationX = rotX;
                PlayerCharacter.RotationY = rotY;
                PlayerCharacter.RotationZ = rotZ;

                CharacterTransform.PosX = posX;
                CharacterTransform.PosY = posY;
                CharacterTransform.PosZ = posZ;
                CharacterTransform.RotX = rotX;
                CharacterTransform.RotY = rotY;
                CharacterTransform.RotZ = rotZ;

                WorldRef.AddNewCharacterTransform(GUID, CharacterTransform);
            }
        }

        public void SetPosition(TransformPacket packet)
        {
            lock (stateLock)
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
        }*/

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
