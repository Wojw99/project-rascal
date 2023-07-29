#include "Packet.h"

Packet::Packet(PacketType type) : _type(type) {}

Packet::~Packet() {}

PacketType& Packet::getType() { return this->_type; }

std::unordered_map<std::string, Field>& Packet::getFields() { return this->_fields; }

std::vector<uint8_t>& Packet::getData() { return this->_data; }

uint16_t& Packet::getSize() { return this->_size; };

uint8_t& Packet::getFieldsCount() { return this->_fieldsCount; };

void Packet::setType(const PacketType& type) { this->_type = type; }

void Packet::setFields(const std::unordered_map<std::string, Field>& fields) { this->_fields = fields; }

void Packet::setData(const std::vector<uint8_t>& data) { this->_data = data; }

void Packet::setSize(const uint16_t& size) { this->_size = size; };

void Packet::setFieldsCount(const uint8_t& count) { this->_fieldsCount = count; };

template <typename T>
void Packet::addField(const std::string& fieldName)
{
    Field field;
    field.position = _data.size();
    field.length = sizeof(T);

    // dodawanie nowego pola do mapy
    _fields[fieldName] = field;

    // zwiekszanie rozmiaru wektora _data aby zarezerwowac miejsce na nowe dane
    _data.resize(_data.size() + field.length);

    // ustawianie rozmiaru danych
    _size = _data.size();
};

//void Packet::addFieldsByType(PacketType type)
//{
//    if (type == PacketType::player_move) {
//	    this->addField<int16_t>("playerID");
//        this->addField<int16_t>("playerX");
//        this->addField<int16_t>("playerY");
//    }
//    else if (type == PacketType::player_shoot) {
//        this->addField<int16_t>("playerID");
//        this->addField<int16_t>("targetID");
//    }
//    else if (type == PacketType::player_teleport) {
//        this->addField<int16_t>("playerID");
//        this->addField<int16_t>("targetMap");
//        this->addField<int16_t>("mapX");
//        this->addField<int16_t>("mapY");
//    }
//}

// -------------------------------------------------------------------------------
// setField
// -------------------------------------------------------------------------------

template <typename T>
void Packet::setField(const std::string& fieldName, const T& value) {
    auto field = _fields.find(fieldName);
    const uint8_t* bytes = reinterpret_cast<const uint8_t*>(&value);
    std::copy(bytes, bytes + sizeof(T), _data.begin() + field->second.position);
}

template <>
void Packet::setField<uint8_t>(const std::string& fieldName, const uint8_t& value) {

}

template <>
void Packet::setField<std::string>(const std::string& fieldName, const std::string& value) {

}

// -------------------------------------------------------------------------------
//  addFieldAndSet
// -------------------------------------------------------------------------------

template <typename T>
void Packet::addFieldAndSet(const std::string& fieldName, const T& value)
{

}

template <>
void Packet::addFieldAndSet<uint8_t>(const std::string& fieldName, const uint8_t& value)
{

}

template <>
void Packet::addFieldAndSet<std::string>(const std::string& fieldName, const std::string& value)
{

}

// -------------------------------------------------------------------------------
// getField
// -------------------------------------------------------------------------------

template <typename T>
T Packet::getField(const std::string& fieldName)
{
    auto field = _fields.find(fieldName);
    return field;
}

template <>
uint8_t Packet::getField<uint8_t>(const std::string& fieldName)
{
    return 0;
}

template <>
std::string Packet::getField<std::string>(const std::string& fieldName)
{
    return fieldName;
}

#pragma pack(push)
#pragma pack(1)
struct PlayerMovePacket
{
    //static const PacketType PacketTypeValue = PacketType::player_move; // Dodaj to pole
    PacketType type;
    uint32_t size; // liczba bez znaku 32 bitowa
    int playerId;
    double x;
    double y;
};
#pragma(pop)

struct PlayerShootPacket
{
    //static const PacketType PacketTypeValue = PacketType::player_shoot; // Dodaj to pole
    PacketType type;
    int size;
    int shooterId;
    int targetId;
    double damage;
};