#ifndef __PACKET_MANAGER_H__
#define __PACKET_MANAGER_H__

#include "Packet.h"
//#include "../Common/CommonIncludes.h" // Circular dependency


class PacketManager
{
public:
	inline static Packet createPacket(PacketType type);
	
	inline static std::vector<uint8_t> serializePacket(Packet& packet); 

	inline static Packet deserialize_packet(std::vector<uint8_t> data);

};
#endif