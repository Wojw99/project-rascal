#ifndef __PACKET_H__
#define __PACKET_H__

#include "../Common/CommonIncludes.h"

struct Field // metadane
{ 
    unsigned int position;
    unsigned int length;
};

class Packet
{
private:
    PacketType _type;
    uint16_t _size;
    std::unordered_map<std::string, Field> _fields;
    uint8_t _fieldsCount;
    std::vector<uint8_t> _data;
    

public:
    Packet() = default;
    Packet(PacketType type);
    ~Packet();

public:
    PacketType& getType();

    std::unordered_map<std::string, Field>& getFields();

    std::vector<uint8_t>& getData();

    uint16_t& getSize();

    uint8_t& getFieldsCount();

    void setType(const PacketType& type);

    void setFields(const std::unordered_map<std::string, Field>& fields);

    void setData(const std::vector<uint8_t>& data);

    void setSize(const uint16_t& size);

    void setFieldsCount(const uint8_t& count);

    template <typename T>
    void addField(const std::string& name);

    template <typename T>
    void setField(const std::string& fieldName, const T& value);

    template <typename T>
    void addFieldAndSet(const std::string& name, const T& value);

    template <typename T>
    T getField(const std::string& fieldName);
};

#endif