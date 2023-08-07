using NetworkCore.NetworkMessage;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using static NetworkCore.NetworkMessage.PacketHandlerManager;

namespace NetworkCore.NetworkMessage
{
    public class PacketHandlerManager
    {
        public delegate void PacketHandler(Packet packet);

        private Dictionary<PacketType, PacketHandler> PacketHandlers;

        public PacketHandlerManager()
        {
            PacketHandlers = new Dictionary<PacketType, PacketHandler>();
        }

        public void RegisterHandler(PacketType packetType, PacketHandler handler)
        {
            if (!PacketHandlers.ContainsKey(packetType))
            {
                PacketHandlers.Add(packetType, handler);
            }
            else
            {
                throw new Exception("Handler on that packet type already exists. Try to unregister that packet.");
            }
        }

        public void UnregisterHandler(PacketType packetType)
        {
            if (PacketHandlers.ContainsKey(packetType))
            {
                PacketHandlers.Remove(packetType);
            }
        }

        public void InitHandlers(Dictionary<PacketType, PacketHandler> packetHandlers)
        {
            if(PacketHandlers.Count > 0)
            {
                throw new InvalidOperationException("Handlers cannot be initialized while " +
                    "there are already registered handlers. Unregister existing handlers " +
                    "before initializing new ones.");
            }

            PacketHandlers = packetHandlers; 
        }

        public void HandlePacket(ref Packet packet)
        {
            if (PacketHandlers.TryGetValue(packet._type, out PacketHandler handler))
            {
                handler(packet);
            }
            else
            {
                throw new Exception($"Unknown packet type {packet._type}");
            }
        }
    }
}
