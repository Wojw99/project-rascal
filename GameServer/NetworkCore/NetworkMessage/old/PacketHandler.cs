/*using System;
using System.Collections.Generic;
using System.Text;
using NetworkCore.NetworkMessage.old;

namespace NetworkCore.NetworkMessage
{
    public class PacketHandler
    {
        public delegate void RequestHandler(Packet packet);
        public delegate Packet ResponseHandler();

        public PacketType _PacketType { get; private set; }
        private RequestHandler RequestPacketHandler;
        private ResponseHandler ResponsePacketHandler;

        public PacketHandler(PacketType packetType, RequestHandler requestFunction, ResponseHandler responseFunction)
        {
            _PacketType = packetType;
            RequestPacketHandler = requestFunction;
            ResponsePacketHandler = responseFunction;
        }

        public void HandleRequest(Packet packet)
        {
            if (_PacketType == packet.PacketType)
            {
                RequestPacketHandler.Invoke(packet);
            }
            // throw
        }

        public Packet HandleResponse()
        {
            Packet packet = ResponsePacketHandler.Invoke();

            return packet;
            // throw
        }
    }
}
*/