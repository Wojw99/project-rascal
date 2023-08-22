using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public abstract class PacketBase
    {
        public PacketType TypeId { get; set; }

        public bool IsResponse { get; set; }

        public PacketBase(PacketType typeId, bool isResponse) { TypeId = typeId; IsResponse = isResponse; }

        public PacketBase(byte[] data)
        {
            using (MemoryStream MemStream = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(MemStream))
            {
                int totalSize = reader.ReadInt32();
                TypeId = (PacketType)reader.ReadByte();
                IsResponse = reader.ReadBoolean();

                foreach (var property in GetType().GetProperties())
                {
                    var serializationAttribute = property.GetCustomAttribute<SerializationAttribute>();

                    if (serializationAttribute != null)
                    {
                        byte serializationTypeByte = reader.ReadByte();
                        SerializationType serializationType = (SerializationType)serializationTypeByte;

                        switch (serializationType)
                        {
                            case SerializationType.type_null:
                                reader.ReadByte();
                                property.SetValue(this, null);
                                break;
                            case SerializationType.type_Int16:
                                property.SetValue(this, reader.ReadInt16());
                                break;
                            case SerializationType.type_Int32:
                                property.SetValue(this, reader.ReadInt32());
                                break;
                            case SerializationType.type_Int64:
                                property.SetValue(this, reader.ReadInt64());
                                break;
                            case SerializationType.type_UInt16:
                                property.SetValue(this, reader.ReadUInt16());
                                break;
                            case SerializationType.type_UInt32:
                                property.SetValue(this, reader.ReadUInt32());
                                break;
                            case SerializationType.type_UInt64:
                                property.SetValue(this, reader.ReadUInt64());
                                break;
                            case SerializationType.type_float:
                                property.SetValue(this, reader.ReadSingle());
                                break;
                            case SerializationType.type_double:
                                property.SetValue(this, reader.ReadDouble());
                                break;
                            case SerializationType.type_string:
                                int strLength = reader.ReadInt32();
                                byte[] utf8Bytes = reader.ReadBytes(strLength);
                                string strValue = Encoding.UTF8.GetString(utf8Bytes);
                                property.SetValue(this, strValue);
                                break;
                            case SerializationType.type_bool:
                                property.SetValue(this, reader.ReadBoolean());
                                break;
                            default:
                                throw new InvalidOperationException($"Unsupported serialization type: {serializationType}");
                        }
                    }
                }
            }
        }

        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream())
            {

                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write(CalculateTotalSize()); // int32
                    writer.Write((byte)TypeId);
                    writer.Write(IsResponse);

                    foreach (var property in GetType().GetProperties())
                    {
                        var serializationAttribute = property.GetCustomAttribute<SerializationAttribute>();

                        if (serializationAttribute != null)
                        {
                            Object value = property.GetValue(this);
                            if(value == null)
                            {
                                writer.Write((byte)SerializationType.type_null);
                                writer.Write((byte)0x0);
                            }
                            else
                            {
                                writer.Write((byte)serializationAttribute.Type);
                                switch (serializationAttribute.Type)
                                {
                                    //case SerializationType.type_null:
                                      //  writer.Write((byte)0x0);
                                        //break;
                                    case SerializationType.type_Int16:
                                        writer.Write((short)value);
                                        break;
                                    case SerializationType.type_Int32:
                                        writer.Write((int)value);
                                        break;
                                    case SerializationType.type_Int64:
                                        writer.Write((long)value);
                                        break;
                                    case SerializationType.type_UInt16:
                                        writer.Write((ushort)value);
                                        break;
                                    case SerializationType.type_UInt32:
                                        writer.Write((uint)value);
                                        break;
                                    case SerializationType.type_UInt64:
                                        writer.Write((ulong)value);
                                        break;
                                    case SerializationType.type_float:
                                        writer.Write((float)value);
                                        break;
                                    case SerializationType.type_double:
                                        writer.Write((double)value);
                                        break;
                                    case SerializationType.type_string:
                                        string strValue = (string)value;
                                        byte[] utf8Bytes = Encoding.UTF8.GetBytes(strValue);
                                        // saving length and value 
                                        writer.Write(utf8Bytes.Length);
                                        writer.Write(utf8Bytes);
                                        break;
                                    case SerializationType.type_bool:
                                        writer.Write((bool)value);
                                        break;
                                    default:
                                        throw new InvalidOperationException($"Unsupported serialization type: {(byte)serializationAttribute.Type}");
                                }
                            }
                        }
                    }
                }
                return stream.ToArray();
            }
        }

        public int CalculateTotalSize()
        {
            int totalSize = sizeof(byte); // TypeId
            totalSize += sizeof(int);
            totalSize += sizeof(bool); // IsResponse

            foreach (var property in GetType().GetProperties())
            {
                var serializationAttribute = property.GetCustomAttribute<SerializationAttribute>();

                if (serializationAttribute != null)
                {
                    totalSize += sizeof(byte); // Serialization type byte

                    Object value = property.GetValue(this);
                    
                    if (value == null)
                    {
                        totalSize += sizeof(byte);
                    }
                    else
                    {
                        switch (serializationAttribute.Type)
                        {
                            case SerializationType.type_Int16:
                                totalSize += sizeof(short);
                                break;
                            case SerializationType.type_Int32:
                                totalSize += sizeof(int);
                                break;
                            case SerializationType.type_Int64:
                                totalSize += sizeof(long);
                                break;
                            case SerializationType.type_UInt16:
                                totalSize += sizeof(ushort);
                                break;
                            case SerializationType.type_UInt32:
                                totalSize += sizeof(uint);
                                break;
                            case SerializationType.type_UInt64:
                                totalSize += sizeof(ulong);
                                break;
                            case SerializationType.type_float:
                                totalSize += sizeof(float);
                                break;
                            case SerializationType.type_double:
                                totalSize += sizeof(double);
                                break;
                            case SerializationType.type_string:
                                string strValue = (string)property.GetValue(this);
                                totalSize += sizeof(int) + Encoding.UTF8.GetByteCount(strValue); // Rozmiar stringa (int) + bajty tekstu
                                break;
                            case SerializationType.type_bool:
                                totalSize += sizeof(bool);
                                break;
                            default:
                                throw new InvalidOperationException($"Unsupported serialization type: {(byte)serializationAttribute.Type}");
                        }
                    }

                }
            }
            return totalSize;
        }

    }
}
