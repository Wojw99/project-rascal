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
using NetworkCore.NetworkData.stare;
using NetworkCore.Packets;
using ServerApplication.GameService.Base;

namespace ServerApplication.GameService
{
    // Operations on CharacterStateUpdatePacket are thread-safe by using locks.
    public class PlayerConnection : TcpPeer
    {
        private TestServer ServerRef { get; set; }
        
        public Character CharacterObj { get; set; }

        private CharacterStateUpdatePacket CharacterStateUpdate { get; set; }

        private readonly object stateLock = new object();

        public PlayerConnection(TestServer serverRef, Socket peerSocket, Guid peerId,
            Owner ownerType, int connCounter) : base(serverRef, peerSocket, peerId, ownerType)
        {
            ServerRef = serverRef;
            CharacterObj = new Character();
            CharacterStateUpdate = new CharacterStateUpdatePacket(-1);
        }

        public void LoadCharacterFromDatabase(string username, int UniqueId)
        {
            CharacterObj = new Character(UniqueId, "nowy gracz", 10, 10, 10, 10, 0, 0, 0, 0);
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

        public void SetPosition(float posX, float posY, float posZ, float rot)
        {
            lock(stateLock)
            {
                CharacterObj.PositionX = posX;
                CharacterObj.PositionY = posY;
                CharacterObj.PositionZ = posZ;
                CharacterObj.Rotation = rot;

                CharacterStateUpdate.PosX = posX;
                CharacterStateUpdate.PosY = posY;
                CharacterStateUpdate.PosZ = posZ;
                CharacterStateUpdate.Rot = rot;

                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetPosition(CharacterMovePacket movePacket)
        {
            lock (stateLock)
            {
                CharacterObj.PositionX = movePacket.PosX;
                CharacterObj.PositionY = movePacket.PosY;
                CharacterObj.PositionZ = movePacket.PosZ;
                CharacterObj.Rotation = movePacket.Rot;

                CharacterStateUpdate.PosX = movePacket.PosX;
                CharacterStateUpdate.PosY = movePacket.PosY;
                CharacterStateUpdate.PosZ = movePacket.PosZ;
                CharacterStateUpdate.Rot = movePacket.Rot;

                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetPositionX(float posX)
        {
            lock(stateLock)
            {
                CharacterObj.PositionX = posX;
                CharacterStateUpdate.PosX = posX;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetPositionY(float posY)
        {
            lock(stateLock)
            {
                CharacterObj.PositionY = posY;
                CharacterStateUpdate.PosY = posY;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetPositionZ(float posZ)
        {
            lock(stateLock)
            {
                CharacterObj.PositionZ = posZ;
                CharacterStateUpdate.PosZ = posZ;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetRotation(float rot)
        {
            lock(stateLock)
            {
                CharacterObj.Rotation = rot;
                CharacterStateUpdate.Rot = rot;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
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
