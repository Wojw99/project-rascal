using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;
using System.Xml.Linq;
using System.Net.Sockets;

namespace NetworkCore.NetworkMessage
{
    public class Packet
    {
        public List<ByteField> _fields { get; private set; }
        public PacketType _type { get; private set; }
        //public PacketDirection PacketDirection { get; private set; }
        //public uint _size { get; private set; }    // rozmiar pakietu jest obliczany podczas serializacji //

        public Packet(PacketType type)
        {
            _fields = new List<ByteField>();
            _type = type;
            //PacketDirection = packetDirection;
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

