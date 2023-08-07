/*using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace NetworkCore
{
    public enum FieldType
    {
        field_null = 0,
        field_int = 1,
        field_short = 2,
        field_long = 3,
        field_double = 4,
        field_float = 5,
        field_string = 6,
    }

    public class ByteField
    {
        public FieldType _type { get; private set; }
        public string _name { get; private set; }
        //private List<byte> _buffer;
        public byte[] _buffer { get; private set; }

        //private Type _type2;
        //private uint _offset; // ile bajtów od początku pakietu musisz się przesunąć, aby dotrzeć do określonego pola
        //private uint _size;

        public ByteField()
        {
            _type = FieldType.field_null;
            // _buffer = new List<byte>();
            _buffer = new byte[0];
            _name = string.Empty;
        }

        public void Init<T>(string name, T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _type = FieldTypeMapper.GetFieldType(typeof(T));
            //_buffer = new List<byte>();

            _name = name;

            //FieldTypeMapper.GetFieldType()

            switch (_type)
            {
                case FieldType.field_int:
                    {
                        _buffer = BitConverter.GetBytes((int)(object)value);
                        //_offset = packetSize + sizeof(int);
                        //_size = sizeof(int);
                        return;
                    }
                case FieldType.field_short:
                    {
                        _buffer.AddRange(BitConverter.GetBytes((short)(object)value));
                        //_offset = packetSize + sizeof(short);
                        //_size = sizeof(short);
                        return;
                    }
                case FieldType.field_long:
                    {
                        _buffer.AddRange(BitConverter.GetBytes((long)(object)value));
                        //_offset = packetSize + sizeof(long);
                        //_size = sizeof(long);
                        return;
                    }
                case FieldType.field_double:
                    {
                        _buffer.AddRange(BitConverter.GetBytes((double)(object)value));
                        //_offset = packetSize + sizeof(double);
                        //_size = sizeof(double);
                        return;
                    }
                case FieldType.field_float:
                    {
                        _buffer.AddRange(BitConverter.GetBytes((float)(object)value));
                        //_offset = packetSize + sizeof(float);
                        //_size = sizeof(float);
                        return;
                    }
                case FieldType.field_string:
                    {
                        string strVal = (string)(object)value;
                        _buffer.AddRange(BitConverter.GetBytes(strVal.Length));
                        _buffer.AddRange(Encoding.ASCII.GetBytes((string)(object)value));
                        //_offset = packetSize + (uint)strVal.Length;
                        //_size = (uint)strVal.Length;
                        return;
                    }
            }
            throw new Exception("Wrong type for field.");
        }

        *//* public void Init(byte[] data)
         {
             using (MemoryStream stream = new MemoryStream(data))
             {
                 using (BinaryReader reader = new BinaryReader(stream))
                 {
                     _type = (FieldType)reader.ReadByte();
                     _name = reader.ReadString();
                     _offset = reader.ReadUInt32();
                     _size = reader.ReadUInt32();
                     _buffer.AddRange(reader.ReadBytes((int)(_size)));
                 }
             }
         }*//*

        public T Read<T>()
        {
            if (_buffer.Count == 0)
                throw new Exception("Trying to read empty ByteField buffer.");

            FieldType targetFieldType = FieldTypeMapper.GetFieldType(typeof(T));

            if (targetFieldType != _type)
                throw new Exception("Trying to read incorrect value.");

            switch (targetFieldType)
            {
                case FieldType.field_int:
                    {
                        return (T)(object)BitConverter.ToInt32(_buffer.ToArray(), 0);
                    }
                case FieldType.field_short:
                    {
                        return (T)(object)BitConverter.ToInt16(_buffer.ToArray(), 0);
                    }
                case FieldType.field_long:
                    {
                        return (T)(object)BitConverter.ToInt64(_buffer.ToArray(), 0);
                    }
                case FieldType.field_double:
                    {
                        return (T)(object)BitConverter.ToDouble(_buffer.ToArray(), 0);
                    }
                case FieldType.field_float:
                    {
                        return (T)(object)BitConverter.ToSingle(_buffer.ToArray(), 0);
                    }
                case FieldType.field_string:
                    {
                        int strLength = BitConverter.ToInt32(_buffer.ToArray(), 0);
                        string strValue = Encoding.ASCII.GetString(_buffer.ToArray(), sizeof(int), strLength);
                        return (T)(object)strValue;
                    }
            }

            throw new Exception($"Field type mismatch. Expected type {typeof(T)}, but actual type is {_type}.");
        }

        public byte[] ToByteArray()
        {

            *//*using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    // Zapisz wartości atrybutów do strumienia
                    writer.Write((byte)_type);
                    //writer.Write(_name);
                    //writer.Write(_offset);
                    //writer.Write(_size);
                    writer.Write(_buffer.ToArray());
                }

                // Zwróć dane bajtowe jako tablicę bajtów
                return stream.ToArray();
            }*//*
            return _buffer.ToArray();
        }

        *//*public byte[] ToArray()
        {
            return _buffer.ToArray();
        }

        public int Count()
        {
            return _buffer.Count;
        }

        public int Length()
        {
            return _buffer.Count - _readPos;
        }

        public void Clear()
        {
            _buffer.Clear();
            _readPos = 0;
        }*/

        /*public void WriteBytes(byte[] bytes)
        {
            _buffer.AddRange(bytes);

            _buffUpdated = true;
        }*/


        /*public int ReadInt(bool peek = true)
        {
            if (_buffer.Count > _readPos)
            {
                if (_buffUpdated)
                {
                    _readBuff = _buffer.ToArray();
                }
            }
            int ret = BitConverter.ToInt32(_readBuff, _readPos);

        }

        public byte[] ReadBytes(int length, bool peek = true)
        {

        }*//*
    }
}
*/