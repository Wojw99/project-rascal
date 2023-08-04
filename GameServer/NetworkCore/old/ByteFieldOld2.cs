/*using NetworkCore;
using System.IO;
using System.Text;
using System;

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
        public byte[] _buffer { get; private set; }

        public ByteField()
        {
            _type = FieldType.field_null;
            _name = "";
            _buffer = new byte[0];
        }

        public void Init<T>(string name, T value)
        {
            _type = FieldTypeMapper.GetFieldType(typeof(T));
            _name = name;

            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    switch (_type)
                    {
                        case FieldType.field_int:
                            writer.Write((int)(object)value);
                            break;
                        case FieldType.field_short:
                            writer.Write((short)(object)value);
                            break;
                        case FieldType.field_long:
                            writer.Write((long)(object)value);
                            break;
                        case FieldType.field_double:
                            writer.Write((double)(object)value);
                            break;
                        case FieldType.field_float:
                            writer.Write((float)(object)value);
                            break;
                        case FieldType.field_string:
                            string strValue = (string)(object)value;
                            byte[] strBytes = Encoding.UTF8.GetBytes(strValue);
                            writer.Write(strBytes.Length);
                            writer.Write(strBytes);
                            break;
                        default:
                            throw new Exception("Wrong type for field.");
                    }

                    _buffer = stream.ToArray();
                }
            }
        }

        public T Read<T>()
        {
            if (_buffer.Length == 0)
                throw new Exception("Trying to read empty ByteField buffer.");

            FieldType targetFieldType = FieldTypeMapper.GetFieldType(typeof(T));

            if (targetFieldType != _type)
                throw new Exception("Trying to read incorrect value.");

            using (MemoryStream stream = new MemoryStream(_buffer))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    switch (targetFieldType)
                    {
                        case FieldType.field_int:
                            return (T)(object)reader.ReadInt32();
                        case FieldType.field_short:
                            return (T)(object)reader.ReadInt16();
                        case FieldType.field_long:
                            return (T)(object)reader.ReadInt64();
                        case FieldType.field_double:
                            return (T)(object)reader.ReadDouble();
                        case FieldType.field_float:
                            return (T)(object)reader.ReadSingle();
                        case FieldType.field_string:
                            int strLength = reader.ReadInt32();
                            byte[] strBytes = reader.ReadBytes(strLength);
                            return (T)(object)Encoding.UTF8.GetString(strBytes);
                        default:
                            throw new Exception($"Field type mismatch. Expected type {typeof(T)}, but actual type is {_type}.");
                    }
                }
            }
        }
    }
}
*/