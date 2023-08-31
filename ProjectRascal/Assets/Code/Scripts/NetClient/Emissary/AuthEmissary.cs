using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient.Emissary
{
    public class AuthEmissary : MonoBehaviour
    {
        GameCharacter PlayerCharacter;

        public delegate void PacketReceived();

        public PacketReceived OnLoginResponsePacketReceived;

        #region Singleton

        public static AuthEmissary instance;

        private void Awake() {
            instance = this;
        }

        private AuthEmissary() {
            OnLoginResponsePacketReceived = 
        }

        private void LoadCharacter()
        {

        }
        #endregion
    }
}
