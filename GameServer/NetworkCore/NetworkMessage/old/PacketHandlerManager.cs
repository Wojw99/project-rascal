/*using NetworkCore.NetworkMessage.old;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
//using static NetworkCore.NetworkMessage.PacketHandlerManager;

namespace NetworkCore.NetworkMessage
{
    public class PacketHandlerManager
    {
        //public delegate void PacketHandler(Packet packet);

        public delegate void RequestHandler(Packet packet);
        public delegate Packet ResponseHandler();

        //private Dictionary<PacketType, (RequestHandler, ResponseHandler)> PacketHandlers;
        private Dictionary<PacketType, PacketHandler> _PacketHandlers;
        //private List<PacketHandler> PacketHandlersList;

        public PacketHandlerManager()
        {
            //PacketHandlers = new Dictionary < PacketType, (RequestHandler, ResponseHandler)>();
            _PacketHandlers = new Dictionary<PacketType, PacketHandler>();
        }

        *//*public PacketHandlerManager(Dictionary<PacketType, (RequestHandler, ResponseHandler)> packetHandlers)
        {
            PacketHandlers = packetHandlers;
        }*//*

        public PacketHandlerManager(Dictionary<PacketType, PacketHandler> packetHandlers)
        {
            _PacketHandlers = packetHandlers;
        }

        public PacketHandler GetHandler(PacketType packetType)
        {
            if(_PacketHandlers.TryGetValue(packetType, out var handler))
            {
                return handler;
            }
            else
            {
                throw new Exception($"Cannot find handler with specific PacketType: {packetType}");
            }
            
        }

        public void RegisterHandler(PacketType packetType, PacketHandler handler)
        {
            if (!_PacketHandlers.ContainsKey(packetType))
            {
                _PacketHandlers.Add(packetType, handler);
            }
            else
            {
                throw new Exception("Handler on that packet type already exists. Try to unregister that packet.");

            }
        }

        public void InitHandlers(Dictionary<PacketType, PacketHandler> packetHandlers)
        {
            if(_PacketHandlers.Count > 0)
            {
                throw new InvalidOperationException("Handlers cannot be initialized while " +
                    "there are already registered handlers. Unregister existing handlers " +
                    "before initializing new ones.");
            }

            _PacketHandlers = packetHandlers; 
        }

        public void Clear()
        {
            _PacketHandlers.Clear();
        }

       *//* public void HandleRequest(Packet packet)
        {
            if (PacketHandlers.TryGetValue(packet._type, out var handlers))
            {
                handlers.Item1.Invoke(packet);
            }
            else
            {
                throw new Exception($"Unknown packet type {packet._type}");
            }
        }

        public Packet HandleResponse(PacketType packetType)
        {
            if (PacketHandlers.TryGetValue(packetType, out var handlers))
            {
                return handlers.Item2.Invoke();
            }
            else
            {
                throw new Exception($"Unknown packet type {packetType}");
            }
        }*/

        /*public T HandlePacket<T>(Packet packet, bool generateResponse)
        {
            if (PacketHandlers.TryGetValue(packet._type, out var handlers))
            {
                handlers.Item1.Invoke(packet);

                if (generateResponse && handlers.Item2 != null)
                {
                    return handlers.Item2.Invoke();
                }
            }
            else
            {
                throw new Exception($"Unknown packet type {packet._type}");
            }

            return default;
        }*//*
    }
}
*/