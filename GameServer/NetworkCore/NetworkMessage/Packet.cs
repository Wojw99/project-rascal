using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;
using System.Xml.Linq;
using System.Net.Sockets;

namespace NetworkCore.NetworkMessage
{
    public enum PacketType
    {
        packet_player_move = 10,
        packet_player_level_up = 11,
        packet_player_teleport = 12,
        packet_player_shoot = 13,
        packet_enemy_shoot = 20,
        packet_monster_pos = 30,
        packet_test_packet = 31,
    }

    public class Packet
    {
        public List<ByteField> _fields { get; private set; }
        public PacketType _type { get; private set; }
        //public uint _size { get; private set; }    // rozmiar pakietu jest obliczany podczas serializacji //

        public Packet(PacketType type)
        {
            _fields = new List<ByteField>();
            _type = type;
            //_size = sizeof(PacketType);
        }

        public void initFields(List<ByteField> fields)
        {
            _fields.Clear();
            _fields = fields;
        }

        public void WriteShort(string fieldName, short value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value);
            _fields.Add(field);
            //_size += sizeof(short);
        }

        public void WriteInt(string fieldName, int value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value);
            _fields.Add(field);
            //_size += sizeof(int);
        }

        public void WriteLong(string fieldName, long value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value);
            _fields.Add(field);
            //_size += sizeof(long);
        }

        public void WriteFloat(string fieldName, float value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value);
            _fields.Add(field);
            //_size += sizeof(float);
        }

        public void WriteString(string fieldName, string value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value);
            _fields.Add(field);
            //_size += (uint)value.Length;
        }

        public void WriteDouble(string fieldName, double value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value);
            _fields.Add(field);
            //_size += sizeof(float);
        }

        public void WriteBytes(string fieldName, byte[] data, FieldType type)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, data, type);
            _fields.Add(field);
            //_size += (uint)data.Length;
        }

        public T ReadField<T>(string fieldName)
        {
            foreach (var field in _fields)
            {
                if (field._name == fieldName)
                {
                    return field.Read<T>();
                }
            }

            throw new Exception("Cannot find specific fieldName in dictionary. ");
        }
    }
}

