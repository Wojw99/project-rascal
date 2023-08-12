using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace NetworkCore.NetworkMessage
{
    public class ByteField
    {
        public FieldType _type { get; private set; }
        public string _name { get; private set; }
        public byte[] _buffer { get; private set; }
        public int _bufferSize { get; private set; }

        public ByteField()
        {
            _type = FieldType.field_null;
            _name = "";
            _buffer = new byte[0];
            _bufferSize = 0;
        }

        public void Init<T>(string name, T value)
        {
            _type = FieldTypeMapper.GetFieldType(typeof(T));
            _name = name;

            switch (_type)
            {
                case FieldType.field_int:
                    _buffer = BitConverter.GetBytes((int)(object)value);
                    _bufferSize = sizeof(int);
                    return;
                case FieldType.field_short:
                    _buffer = BitConverter.GetBytes((short)(object)value);
                    _bufferSize = sizeof(short);
                    return;
                case FieldType.field_long:
                    _buffer = BitConverter.GetBytes((long)(object)value);
                    _bufferSize = sizeof(long);
                    return;
                case FieldType.field_double:
                    _buffer = BitConverter.GetBytes((double)(object)value);
                    _bufferSize = sizeof(double);
                    return;
                case FieldType.field_float:
                    _buffer = BitConverter.GetBytes((float)(object)value);
                    _bufferSize = sizeof(float);
                    return;
                case FieldType.field_string:
                    byte[] strBuffer = Encoding.UTF8.GetBytes((string)(object)value);
                    _bufferSize = sizeof(int) + strBuffer.Length;
                    _buffer = new byte[_bufferSize];
                    Buffer.BlockCopy(BitConverter.GetBytes(strBuffer.Length), 0, _buffer, 0, sizeof(int));
                    Buffer.BlockCopy(strBuffer, 0, _buffer, sizeof(int), strBuffer.Length);
                    return;
                default:
                    if (typeof(T).IsArray && typeof(T).GetElementType() == typeof(byte))
                        throw new ArgumentException("Byte array (byte[]) is not allowed for Init<T>. Instead use Init with byte array parameter.");
                    throw new Exception("Wrong type for field.");
            }

        }

        public void Init(string name, byte[] data, FieldType type)
        {
            int expectedSize = 0;
            switch (type)
            {
                case FieldType.field_int:
                    expectedSize = sizeof(int);
                    break;
                case FieldType.field_short:
                    expectedSize = sizeof(short);
                    break;
                case FieldType.field_long:
                    expectedSize = sizeof(long);
                    break;
                case FieldType.field_float:
                    expectedSize = sizeof(float);
                    break;
                case FieldType.field_double:
                    expectedSize = sizeof(double);
                    break;
                case FieldType.field_string:
                    expectedSize = data.Length;
                    break;
                default:
                    throw new Exception("Unsupported field type.");
            }

            if (data.Length != expectedSize)
            {
                throw new ArgumentException("Invalid data size for the specified FieldType.");
            }

            _type = type;
            _name = name;
            _bufferSize = expectedSize;
            _buffer = data;
        }

        public T Read<T>()
        {
            if (_buffer.Length == 0)
                throw new Exception("Trying to read empty ByteField buffer.");

            FieldType targetFieldType = FieldTypeMapper.GetFieldType(typeof(T));

            if (targetFieldType != _type)
                throw new Exception("Trying to read incorrect value.");

            switch (targetFieldType)
            {
                case FieldType.field_int:
                    return (T)(object)BitConverter.ToInt32(_buffer, 0);
                case FieldType.field_short:
                    return (T)(object)BitConverter.ToInt16(_buffer, 0);
                case FieldType.field_long:
                    return (T)(object)BitConverter.ToInt64(_buffer, 0);
                case FieldType.field_double:
                    return (T)(object)BitConverter.ToDouble(_buffer, 0);
                case FieldType.field_float:
                    return (T)(object)BitConverter.ToSingle(_buffer, 0);
                case FieldType.field_string:
                    int strLength = BitConverter.ToInt32(_buffer, 0);
                    string strValue = Encoding.UTF8.GetString(_buffer, sizeof(int), strLength);
                    return (T)(object)strValue;
                default:
                    throw new Exception($"Field type mismatch. Expected type {typeof(T)}, but actual type is {_type}.");
            }
        }
    }
}

