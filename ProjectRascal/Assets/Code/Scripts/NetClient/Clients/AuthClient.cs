/*using NetClient;
using NetworkCore.NetworkCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code.Scripts.NetClient.Clients
{
    public class AuthClient : MonoBehaviour
    {
        public static AuthClient instance;

        public TcpPeer AuthServerPeer;

        private void Awake()
        {
            instance = this;
        }

        public async void Start()
        {
            AuthServerPeer = await TcpNetworkClient.GetInstance().CreateTcpServerConnection("127.0.0.1", 8050);
            AuthServerPeer.Connect();
            AuthServerPeer.StartRead();
        }
    }
}
*/