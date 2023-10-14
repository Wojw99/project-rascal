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
using NetworkCore.Packets;
using ServerApplication.GameService.Base;

namespace ServerApplication.GameService
{
    // Operations on CharacterStateUpdatePacket are thread-safe by using locks.
    public class PlayerConnection : TcpPeer
    {
        private TestServer ServerRef { get; set; }
        
        public Character CharacterObj { get; set; }

        private AttributesUpdatePacket CharacterStateUpdate { get; set; }

        private TransformPacket CharacterTransform { get; set; }

        private readonly object stateLock = new object();

        public PlayerConnection(TestServer serverRef, Socket peerSocket, Guid peerId,
            Owner ownerType, int connCounter) : base(serverRef, peerSocket, peerId, ownerType)
        {
            ServerRef = serverRef;
            CharacterObj = new Character();
            CharacterStateUpdate = new AttributesUpdatePacket(-1);
            CharacterTransform = new TransformPacket(-1);
        }

        public void LoadCharacterFromDatabase(string username, int UniqueId)
        {
            CharacterObj = new Character(UniqueId, "nowy gracz", 10, 10, 10, 10, 0, 0, 0, 0, 0, 0);
            CharacterTransform.CharacterVId = UniqueId;
            CharacterStateUpdate.CharacterVId = UniqueId;
        }

        public void SetName(string name)
        {
            lock(stateLock)
            {
                // połączenie z bazą...

                CharacterObj.Name = name;
                CharacterStateUpdate.Name = name;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetCurrentHealth(float currentHealth)
        {
            lock (stateLock)
            {
                CharacterObj.CurrentHealth = currentHealth;
                CharacterStateUpdate.CurrentHealth = currentHealth;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetMaxHealth(float maxHealth)
        {
            lock (stateLock)
            {
                CharacterObj.MaxHealth = maxHealth;
                CharacterStateUpdate.MaxHealth = maxHealth;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetCurrentMana(float currentMana)
        {
            lock (stateLock)
            {
                CharacterObj.CurrentMana = currentMana;
                CharacterStateUpdate.CurrentMana = currentMana;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetMaxMana(float maxMana)
        {
            lock(stateLock)
            {
                CharacterObj.MaxMana = maxMana;
                CharacterStateUpdate.MaxMana = maxMana;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetTransform(float posX, float posY, float posZ, float rotX, float rotY, float rotZ)
        {
            lock(stateLock)
            {
                CharacterObj.PositionX = posX;
                CharacterObj.PositionY = posY;
                CharacterObj.PositionZ = posZ;
                CharacterObj.RotationX = rotX;
                CharacterObj.RotationY = rotY;
                CharacterObj.RotationZ = rotZ;

                CharacterTransform.PosX = posX;
                CharacterTransform.PosY = posY;
                CharacterTransform.PosZ = posZ;
                CharacterTransform.RotX = rotX;
                CharacterTransform.RotY = rotY;
                CharacterTransform.RotZ = rotZ;

                ServerRef._World.AddNewCharacterTransform(Id, CharacterTransform);
            }
        }

        public void SetPosition(TransformPacket packet)
        {
            lock (stateLock)
            {
                CharacterObj.PositionX = packet.PosX;
                CharacterObj.PositionY = packet.PosY;
                CharacterObj.PositionZ = packet.PosZ;
                CharacterObj.RotationX = packet.RotX;
                CharacterObj.RotationY = packet.RotY;
                CharacterObj.RotationZ = packet.RotZ;

                CharacterTransform.PosX = packet.PosX;
                CharacterTransform.PosY = packet.PosY;
                CharacterTransform.PosZ = packet.PosZ;
                CharacterTransform.RotX = packet.RotX;
                CharacterTransform.RotY = packet.RotY;
                CharacterTransform.RotZ = packet.RotZ;

                ServerRef._World.AddNewCharacterTransform(Id, CharacterTransform);
            }
        }

        public void OnCharactedStateSend()
        {
            lock(stateLock)
            {
                CharacterStateUpdate.Clear();
            }
        }
    }
}
