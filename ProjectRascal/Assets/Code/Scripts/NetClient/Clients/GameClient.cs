/*using NetClient;
using NetworkCore.NetworkCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NetworkCore.Packets;

namespace Assets.Code.Scripts.NetClient.Clients
{
    public class GameClient : MonoBehaviour
    {
        public static GameClient instance;

        public TcpPeer GameServerPeer;

        private void Awake()
        {
            instance = this;
        }

        public async void Start()
        {
            GameServerPeer = await Client.GetInstance().CreateTcpServerConnection("192.168.5.5", 8051);
            GameServerPeer.Connect();
            GameServerPeer.StartRead();
        }

        // ?????
        private void OnDestroy()
        {
            GameServerPeer.Disconnect();
        }
    }
}
*/