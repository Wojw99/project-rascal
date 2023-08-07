using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public static class PacketSerializationManager
    {
        public static byte[] serializePacket(Packet packet)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    // calculating the size of packet
                    int size = 0;
                    size += sizeof(PacketType);
                    foreach (var field in packet._fields)
                    {
                        size += sizeof(byte) * 2; 
                        size += Encoding.UTF8.GetByteCount(field._name);
                        size += sizeof(int);
                        size += field._buffer.Length;
                    }

                    writer.Write(size + sizeof(int));
                    writer.Write((int)packet._type);

                    foreach (var field in packet._fields)
                    {
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
                    int expectedSize = reader.ReadInt32(); // aktualnie nieuzywane, ale musi zostać
                    // odczytane aby działała deserializacja.
                    // Ewentualnie można by pominąć te 4 bajty.

                    PacketType packetType = (PacketType)reader.ReadInt32();

                    Packet packet = new Packet(packetType);

                    while (stream.Position < stream.Length) { 
                        FieldType fieldType = (FieldType)reader.ReadByte();
                        string fieldName = reader.ReadString();
                        int bufferSize = reader.ReadInt32();
                        packet.WriteBytes(fieldName, reader.ReadBytes(bufferSize), fieldType);
                    }

                    return packet;
                }
            }
        }
    }
}
