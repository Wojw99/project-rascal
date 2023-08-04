using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public static class PacketManager
    {
        public static byte[] serializePacket(Packet packet)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write((int)packet._type);

                    foreach (var field in packet._fields) {
                        writer.Write((byte)field._type);
                        writer.Write(field._name);
                        writer.Write(field._bufferSize);
                        writer.Write(field._buffer);
                    }
                }

                return stream.ToArray();
            }
        }

        public static Packet DeserializeByteData(byte[] data) 
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    PacketType packetType = (PacketType)reader.ReadInt32();

                    Packet packet = new Packet(packetType);

                    while (stream.Position < stream.Length) {
                        FieldType fieldType = (FieldType)reader.ReadByte();
                        string fieldName = reader.ReadString();
                        int bufferSize = reader.ReadInt32();

                        packet.WriteBytes(fieldName, reader.ReadBytes(bufferSize), fieldType);

                        /*switch(fieldType) {
                        case FieldType.field_int:
                            packet.WriteBytes(fieldName, reader.ReadBytes(4), fieldType);
                            break;
                        case FieldType.field_short:
                            packet.WriteBytes(fieldName, reader.ReadBytes(2), fieldType);
                            break;
                        case FieldType.field_long:
                            packet.WriteBytes(fieldName, reader.ReadBytes(8), fieldType);
                            break;
                        case FieldType.field_double:
                            packet.WriteBytes(fieldName, reader.ReadBytes(8), fieldType);
                            break;
                        case FieldType.field_float:
                            packet.WriteBytes(fieldName, reader.ReadBytes(4), fieldType);
                            break;
                        case FieldType.field_string:
                            int strLength = BitConverter.ToInt32(reader.ReadBytes(4), 0);
                            byte[] strData = reader.ReadBytes(strLength);
                            packet.WriteBytes(fieldName, strData, fieldType);
                            break;
                        default:
                            throw new Exception($"Unsupported field type: {fieldType}");
                        }*/
                    }

                    return packet;
                }
            }
        }
    }
}
