using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace NetworkCore
{
    public enum FieldType
    {
        field_int = 0,
        field_short = 1,
        field_long = 2,
        field_double = 3,
        field_float = 4,
        field_string = 5,
    }

    public class ByteField
    {
        private FieldType _type;
        //private Type _type2;
        public string _name { get; private set; }
        private uint _offset; // ile bajtów od początku pakietu musisz się przesunąć, aby dotrzeć do określonego pola
        private uint _size;
        private List<byte> _buffer;



        public void Init<T>(string name, T value, uint packetSize)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _type = FieldTypeMapper.GetFieldType(typeof(T));
            _buffer = new List<byte>();
            _name = name;

            switch (_type) {
            case FieldType.field_int: {
                _buffer.AddRange(BitConverter.GetBytes((int)(object)value));
                _offset = packetSize + sizeof(int);
                _size = sizeof(int);
                return;
            }
            case FieldType.field_short: {
                _buffer.AddRange(BitConverter.GetBytes((short)(object)value));
                _offset = packetSize + sizeof(short);
                _size = sizeof(short);
                return;
            }
            case FieldType.field_long: {
                _buffer.AddRange(BitConverter.GetBytes((long)(object)value));
                _offset = packetSize + sizeof(long);
                _size = sizeof(long);
                return;
            }
            case FieldType.field_double: {
                _buffer.AddRange(BitConverter.GetBytes((double)(object)value));
                _offset = packetSize + sizeof(double);
                _size = sizeof(double);
                return;
            }
            case FieldType.field_float: {
                _buffer.AddRange(BitConverter.GetBytes((float)(object)value));
                _offset = packetSize + sizeof(float);
                _size = sizeof(float);
                return;
            }
            case FieldType.field_string: {
                string strVal = (string)(object)value;
                _buffer.AddRange(BitConverter.GetBytes(strVal.Length));
                _buffer.AddRange(Encoding.ASCII.GetBytes((string)(object)value));
                _offset = packetSize + (uint)strVal.Length;
                _size = (uint)strVal.Length;
                return;
            }}
            throw new Exception("Wrong type for field.");
        }

        public T Read<T>()
        {
            if (_buffer.Count == 0)
                throw new Exception("Trying to read empty ByteField buffer.");

            FieldType targetFieldType = FieldTypeMapper.GetFieldType(typeof(T));

            if(targetFieldType != _type)
                throw new Exception("Trying to read incorrect value.");

            switch(targetFieldType) {
            case FieldType.field_int: {
                return (T)(object)BitConverter.ToInt32(_buffer.ToArray(), 0);
            }
            case FieldType.field_short: {
                return (T)(object)BitConverter.ToInt16(_buffer.ToArray(), 0);
            }
            case FieldType.field_long: {
                return (T)(object)BitConverter.ToInt64(_buffer.ToArray(), 0);
            }
            case FieldType.field_double: {
                return (T)(object)BitConverter.ToDouble(_buffer.ToArray(), 0);
            }
            case FieldType.field_float: {
                return (T)(object)BitConverter.ToSingle(_buffer.ToArray(), 0);
            }
            case FieldType.field_string: {
                int strLength = BitConverter.ToInt32(_buffer.ToArray(), 0);
                string strValue = Encoding.ASCII.GetString(_buffer.ToArray(), sizeof(int), strLength);
                return (T)(object)strValue;
            }}
            
            throw new Exception($"Field type mismatch. Expected type {typeof(T)}, but actual type is {_type}.");
        }

        /*public byte[] ToArray()
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

        }*/
    }
}
