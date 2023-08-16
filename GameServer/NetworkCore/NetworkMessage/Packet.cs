using NetworkCore.NetworkMessage.old;
using NetworkCore.Packets;
using NetworkCore.Packets.Attributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public class Packet
    {
        public Type PacketType { get; }
        private List<PacketField> Fields = new List<PacketField>();

        public Packet(Type packetType)
        {
            PacketType = packetType;
        }

        public Packet(Packet packet)
        {
            PacketType = packet.PacketType;
            Fields = new List<PacketField>();
            
            foreach(PacketField field in packet.Fields)
            {
                Fields.Add(field);
            }
        }

        public Packet(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                int totalSize = reader.ReadInt32();

                int typeSize = reader.ReadInt32();
                PacketType = DeserializeType(reader.ReadBytes(typeSize));

                Console.WriteLine("test");

                // each field
                while (stream.Position < stream.Length)
                {
                    int fieldLength = reader.ReadInt32();
                    Fields.Add(PacketField.Deserialize(reader.ReadBytes(fieldLength)));
                }

                Console.WriteLine("test");
            }
            
        }

        public byte[] SerializePacket()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    byte[] serializedType = SerializeType(PacketType);
                    int serializedTypeLength = serializedType.Length;


                    int totalSize = 0;
                    totalSize += serializedTypeLength;
                    totalSize += sizeof(int) * 2;

                    foreach (var field in Fields)
                    {
                        totalSize += sizeof(int); 
                        totalSize += field.CalculateTotalSize();
                    }

                    writer.Write(totalSize);
                 
                    writer.Write(serializedType.Length);
                    writer.Write(serializedType);

                    foreach (var field in Fields)
                    {
                        writer.Write(field.CalculateTotalSize()); // int
                        writer.Write(field.Serialize());
                    }

                    return stream.ToArray();
                }
            }
        }

        private protected void Write<T>(string fieldName, T value)
        {
            byte[] buffer = null;

            if (typeof(T) == typeof(short))
                buffer = BitConverter.GetBytes(Convert.ToInt16(value));

            else if (typeof(T) == typeof(int))
                buffer = BitConverter.GetBytes(Convert.ToInt32(value));

            else if (typeof(T) == typeof(long))
                buffer = BitConverter.GetBytes(Convert.ToInt64(value));

            else if (typeof(T) == typeof(float))
                buffer = BitConverter.GetBytes(Convert.ToSingle(value));

            else if (typeof(T) == typeof(double))
                buffer = BitConverter.GetBytes(Convert.ToDouble(value));

            else if (typeof(T) == typeof(string))
                buffer = SerializeString(Convert.ToString(value));

            if (buffer != null)
                Fields.Add(new PacketField (SerializeType(typeof(T)), SerializeString(fieldName), buffer ));

        }

        private protected T Read<T>(string fieldName)
        {
            byte[] strBuffer = SerializeString(fieldName);
            // or deserialize field.Name

            foreach (var field in Fields)
            {
                if (field.Name.SequenceEqual(strBuffer))
                {
                    Type fieldType = DeserializeType(field.FieldType);
                    Console.WriteLine("asfa");

                    if (fieldType == typeof(short))
                        return (T)(object)BitConverter.ToInt16(field.Buffer, 0);

                    else if (fieldType == typeof(int))
                        return (T)(object)BitConverter.ToInt32(field.Buffer, 0);

                    else if (fieldType == typeof(long))
                        return (T)(object)BitConverter.ToInt64(field.Buffer, 0);

                    else if (fieldType == typeof(float))
                        return (T)(object)BitConverter.ToSingle(field.Buffer, 0);

                    else if (fieldType == typeof(double))
                        return (T)(object)BitConverter.ToDouble(field.Buffer, 0);

                    else if (fieldType == typeof(string))
                        return (T)(object)deserializeString(field.Buffer);

                }
            }

            throw new Exception("Cannot find specific fieldName in dictionary.");
        }

        private protected bool TryRead<T>(string fieldName, out T result)
        {
            try
            {
                result = Read<T>(fieldName);
                return true;
            }
            catch (Exception)
            {
                result = default(T);
                return false;
            }
        }

        public void Clear()
        {
            Fields.Clear();
        }

        public virtual string ToString()
        {
            return $"type = {PacketType}, ";
        }

        private protected byte[] SerializeString(string str)
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(str);
            byte[] lengthBytes = BitConverter.GetBytes(strBytes.Length);

            byte[] result = new byte[lengthBytes.Length + strBytes.Length];
            lengthBytes.CopyTo(result, 0);
            strBytes.CopyTo(result, lengthBytes.Length);
            return result;
        }

        private protected string deserializeString(byte[] buffer)
        {
            int stringLength = BitConverter.ToInt32(buffer, 0);
            return Encoding.UTF8.GetString(buffer, sizeof(int), stringLength);
        }

        private protected byte[] SerializeType(Type type)
        {
            return SerializeString(type.FullName);
        }

        private protected Type DeserializeType(byte[] buffer)
        {
            return Type.GetType(deserializeString(buffer));
        }
    }
}
