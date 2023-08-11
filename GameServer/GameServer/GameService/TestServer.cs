using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication.GameService
{
    public class TestServer : NetworkServer
    {
        public TestServer(bool allowPhysicalClients, int maxClients, string publicIpAdress,
            string serverName, ServerType serverType, int? tcpPort = null, int? udpPort = null) 
            : base(allowPhysicalClients, maxClients, publicIpAdress, serverName, serverType, tcpPort, udpPort)
        {
            
        }
        public override async Task OnPacketReceived(IPeer peer, Packet packet)
        {

        }

        public override async Task<bool> OnClientConnect(IPeer peer)
        {
            await Console.Out.WriteLineAsync( "Test funkcji");
            return true; // gdy połączenie się uda
        }

        public override async Task OnClientDisconnect(IPeer peer)
        {

        }
    }
}
