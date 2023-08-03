using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace NetworkCore
{
    public enum PacketType
    {
        packet_player_move = 10,
        packet_player_level_up = 11,
        packet_player_teleport = 12,
        packet_player_shoot = 13,
        packet_enemy_shoot = 20,
        packet_monster_pos = 30,
    }

    public class Packet
    {
        private List<ByteField> _fields;
        private PacketType _type;
        private uint _size;
        //private ByteBuffer _data;

        public Packet(PacketType type)
        {
            _fields = new List<ByteField>();
            _type = type;
            _size = sizeof(PacketType);
        }

        public void WriteShort(string fieldName, short value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value, _size);
            _fields.Add(field);
            _size += sizeof(short);
        }

        public void WriteInt(string fieldName, int value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value, _size);
            _fields.Add(field);
            _size += sizeof(int);
        }

        public void WriteLong(string fieldName, long value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value, _size);
            _fields.Add(field);
            _size += sizeof(long);
        }

        public void WriteFloat(string fieldName, float value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value, _size);
            _fields.Add(field);
            _size += sizeof(float);
        }

        public void WriteString(string fieldName, string value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value, _size);
            _fields.Add(field);
            _size += (uint)value.Length;
        }

        public void WriteDouble(string fieldName, double value)
        {
            ByteField field = new ByteField();
            field.Init(fieldName, value, _size);
            _fields.Add(field);
            _size += sizeof(float);
        }

        /*public void addField<T>(string fieldName, T value)
        {
             
            //ByteField field = new ByteField()
        }

        public void WriteField<T>(string fieldName, T value)
        {

        }*/

        public T ReadField<T>(string fieldName)
        {
            foreach(var field in _fields)
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
