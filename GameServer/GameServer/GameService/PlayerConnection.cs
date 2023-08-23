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
        
        private Character CharacterObj { get; set; }

        private CharacterStateUpdatePacket CharacterStateUpdate { get; set; }

        private readonly object stateLock = new object();

        public PlayerConnection(TestServer serverRef, Socket peerSocket, Guid peerId,
            Owner ownerType, int connCounter) : base(serverRef, peerSocket, peerId, ownerType)
        {
            ServerRef = serverRef;
            CharacterObj = new Character();
            CharacterStateUpdate = new CharacterStateUpdatePacket();
        }

        public void LoadCharacterFromDatabase(string username, int UniqueId)
        {
            CharacterObj = new Character(UniqueId, "nowy gracz", 10, 10, 0, 0, 0, 0);
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

        public void SetHealth(int health)
        {
            lock (stateLock)
            {
                CharacterObj.Health = health;
                CharacterStateUpdate.Health = health;
                ServerRef._World.AddNewCharacterStateUpdate(Id, CharacterStateUpdate);
            }
        }

        public void SetMana(int mana)
        {
            lock(stateLock)
            {
                CharacterObj.Mana = mana;
                CharacterStateUpdate.Mana = mana;
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
