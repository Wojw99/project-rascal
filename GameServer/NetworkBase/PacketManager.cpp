#include "PacketManager.h"

inline Packet PacketManager::createPacket(PacketType type)
{
	Packet packet;
	packet.setType(type);

	if (type == PacketType::player_move) {
		packet.addField<int16_t>("playerID");
		packet.addField<int16_t>("playerX");
		packet.addField<int16_t>("playerY");
	}
	else if (type == PacketType::player_shoot) {
		packet.addField<int16_t>("playerID");
		packet.addField<int16_t>("targetID");
	}
	else if (type == PacketType::player_teleport) {
		packet.addField<int16_t>("playerID");
		packet.addField<int16_t>("targetMap");
		packet.addField<int16_t>("mapX");
		packet.addField<int16_t>("mapY");
	}
	return packet;
}

inline std::vector<uint8_t> PacketManager::serializePacket(Packet& packet) {
	std::vector<uint8_t> packedData;
	msgpack::pack(packedData, packet.getType());
	msgpack::pack(packedData, packet.getSize());
	msgpack::pack(packedData, packet.getData());

	// serializacja struktur z mapy _fields
	// nie serializujemy pól mapy .first, które s¹ nazwami zmiennych.
	// nazwy zmiennych przypiszemy stricte w C# po analizie typu pakietu
	for (const auto& entry : packet.getFields()) {
		msgpack::pack(packedData, entry.second.position);
		msgpack::pack(packedData, entry.second.length);
	}
	
	return packedData;
}

inline Packet PacketManager::deserialize_packet(std::vector<uint8_t> data)
{
	std::string_view dataView(reinterpret_cast<const char*>(data.data()), data.size());

	msgpack::object_handle oh = msgpack::unpack(dataView.data(), dataView.size());

	msgpack::object obj = oh.get();
	PacketType type;
	obj.convert(type);

	Packet packet = PacketManager::createPacket(type);

	obj = oh.get();
	obj.convert(packet.getSize()); // konwertowanie obj na konkretny typ danych

	obj = oh.get();
	obj.convert(packet.getData());

	obj = oh.get();
	uint8_t fieldsCount;
	obj.convert(fieldsCount);
	packet.setFieldsCount(fieldsCount);

	Field field;
	for (int i = 0; i < fieldsCount; i++) {
		oh.get();
		obj.convert(field.position);
		oh.get();
		obj.convert(field.length);

		
	}


	

	return packet;
}