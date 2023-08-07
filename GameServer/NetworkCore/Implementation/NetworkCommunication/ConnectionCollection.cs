using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace NetworkCore.NetworkCommunication
{
    public class ConnectionCollection
    {

        private List<IPeer> _Peers;
        private readonly object Lock = new object();

        public ConnectionCollection()
        {
            _Peers = new List<IPeer>();
        }
        public void Connect(IPeer peer)
        {
            lock (Lock)
            {
                if (_Peers.Contains(peer))
                    throw new InvalidOperationException("Peer already connected.");
                _Peers.Add(peer);

                peer.StartReceive();
            }
        }

        public void Disconnect(IPeer peer)
        {
            lock (Lock)
            {
                if (!_Peers.Contains(peer))
                    throw new InvalidOperationException("Cannot disconnect peer, which is not connected. ");
                _Peers.Remove(peer);
            }
        }

        /* private void StartReceive(IPeer peer)
         {
             lock(Lock)
             {
                 var targetPeer = _Peers.Find(p => p == peer);

                 if (targetPeer != null)
                 {
                     targetPeer.
                 }
             }
         }*/

        /* public void SendPacket(Packet packet, IPeer peer)
         {
             lock(Lock)
             {
                 if( !_Peers.Contains(peer))
                     throw new InvalidOperationException("Peer doesnt exists. Cannot send a packet. ");

                 var targetPeer = _Peers.Find(p => p == peer);

                 if (targetPeer != null)
                 {
                     targetPeer.SendPacket(packet);
                 }
             }
         }*/

        public void Clear()
        {
            _Peers.Clear();
        }
    }
}
