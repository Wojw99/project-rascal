using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkCore.NetworkCommunication;
using NetworkCore.NetworkMessage;

namespace Client
{
    public class SimpleClient : NetworkClient
    {
        public SimpleClient() : base() 
        {

        }

        public override async Task OnPacketReceived(IPeer serverPeer, Packet packet)
        {
            
        }

        public override async Task<bool> OnServerConnect(IPeer serverPeer)
        {
            await Console.Out.WriteLineAsync("Test funkcji");
            return true; // gdy połączenie się uda
        }

        public override async Task OnServerDisconnect(IPeer serverPeer)
        {

        }
    }
}
