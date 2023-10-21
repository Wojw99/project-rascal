using NetworkCore.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
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
                        ReadSetValue(property, reader, serializationType);
                    }
                }
            }
        }

        public static PacketBase Deserialize(PacketType packetType, byte[] receivedData)
        {
            return packetType switch
            {
                PacketType.LOGIN_REQUEST => new ClientLoginRequestPacket(receivedData),
                PacketType.LOGIN_RESPONSE => new ClientLoginResponsePacket(receivedData),
                PacketType.CHARACTER_LOAD_REQUEST => new CharacterLoadRequestPacket(receivedData),
                PacketType.CHARACTER_LOAD_RESPONSE => new CharacterLoadResponsePacket(receivedData),
                PacketType.CHARACTER_LOAD_SUCCES => new CharacterLoadSuccesPacket(receivedData),
                PacketType.ADVENTURER_LOAD_PACKET => new AdventurerLoadPacket(receivedData),
                PacketType.ADVENTURER_LOAD_COLLECTION_PACKET => new AdventurerLoadCollectionPacket(receivedData),
                PacketType.ATTRIBUTES_PACKET => new AttributesPacket(receivedData),
                PacketType.ATTRIBUTES_COLLECTION_PACKET => new AttributesCollectionPacket(receivedData),
                PacketType.ATTRIBUTES_UPDATE_PACKET => new AttributesUpdatePacket(receivedData),
                PacketType.ATTRIBUTES_COLLECTION_UPDATE_PACKET => new AttributesUpdateCollectionPacket(receivedData),
                PacketType.TRANSFORM_PACKET => new TransformPacket(receivedData),
                PacketType.TRANSFORM_COLLECTION_PACKET => new TransformCollectionPacket(receivedData),
                PacketType.CHARACTER_EXIT_PACKET => new AdventurerExitPacket(receivedData),
                PacketType.CLIENT_DISCONNECT => new ClientDisconnectPacket(receivedData),
                PacketType.PING_REQUEST => new PingRequestPacket(receivedData),
                PacketType.PING_RESPONSE => new PingResponsePacket(receivedData),
                _ => throw new ArgumentException("Unknown packet type"),
            };
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
                                WriteField(writer, value, serializationAttribute.Type);
                            }
                        }
                    }
                }
                return stream.ToArray();
            }
        }

        private void ReadSetValue(PropertyInfo property, BinaryReader readerRef, SerializationType type)
        {
            switch (type)
            {
                case SerializationType.type_null:
                    readerRef.ReadByte();
                    property.SetValue(this, null);
                    break;

                case SerializationType.type_Int16:
                    property.SetValue(this, readerRef.ReadInt16());
                    break;

                case SerializationType.type_Int32:
                    property.SetValue(this, readerRef.ReadInt32());
                    break;

                case SerializationType.type_Int64:
                    property.SetValue(this, readerRef.ReadInt64());
                    break;

                case SerializationType.type_UInt16:
                    property.SetValue(this, readerRef.ReadUInt16());
                    break;

                case SerializationType.type_UInt32:
                    property.SetValue(this, readerRef.ReadUInt32());
                    break;

                case SerializationType.type_UInt64:
                    property.SetValue(this, readerRef.ReadUInt64());
                    break;

                case SerializationType.type_float:
                    property.SetValue(this, readerRef.ReadSingle());
                    break;

                case SerializationType.type_double:
                    property.SetValue(this, readerRef.ReadDouble());
                    break;

                case SerializationType.type_string:
                    int strLength = readerRef.ReadInt32();
                    byte[] utf8Bytes = readerRef.ReadBytes(strLength);
                    string strValue = Encoding.UTF8.GetString(utf8Bytes);
                    property.SetValue(this, strValue);
                    break;

                case SerializationType.type_bool:
                    property.SetValue(this, readerRef.ReadBoolean());
                    break;

                case SerializationType.type_subPacket:
                    property.SetValue(this, ReadSubPacket(readerRef));
                    break;

                case SerializationType.type_subPacketList:
                    int ListCount = readerRef.ReadInt32();  
                    List<PacketBase> packetList = new List<PacketBase>();

                    for(int i = 0; i < ListCount; i++)
                    {
                        packetList.Add(ReadSubPacket(readerRef));
                    }

                    // @ reflection of list type @ maybe is better method 

                    Type listType = property.PropertyType; 
                    Type elementType = listType.GetGenericArguments()[0]; 

                    var typedList = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType)); 
                    foreach (var subPacket in packetList)
                    {
                        typedList.GetType().GetMethod("Add").Invoke(typedList, new object[] { subPacket }); 
                    }

                    property.SetValue(this, typedList); // Ustaw wartość właściwości

                    break;

                default:
                    throw new InvalidOperationException($"Unsupported serialization type: {type}");
            }
        }

        private void WriteField (BinaryWriter WriterRef, object value, SerializationType type)
        {
            switch (type)
            {
                case SerializationType.type_Int16:
                    WriterRef.Write((short)value);
                    break;

                case SerializationType.type_Int32:
                    WriterRef.Write((int)value);
                    break;

                case SerializationType.type_Int64:
                    WriterRef.Write((long)value);
                    break;

                case SerializationType.type_UInt16:
                    WriterRef.Write((ushort)value);
                    break;

                case SerializationType.type_UInt32:
                    WriterRef.Write((uint)value);
                    break;

                case SerializationType.type_UInt64:
                    WriterRef.Write((ulong)value);
                    break;

                case SerializationType.type_float:
                    WriterRef.Write((float)value);
                    break;

                case SerializationType.type_double:
                    WriterRef.Write((double)value);
                    break;

                case SerializationType.type_string:
                    string strValue = (string)value;
                    byte[] utf8Bytes = Encoding.UTF8.GetBytes(strValue);
                    // saving length and value 
                    WriterRef.Write(utf8Bytes.Length);
                    WriterRef.Write(utf8Bytes);
                    break;

                case SerializationType.type_bool:
                    WriterRef.Write((bool)value);
                    break;

                case SerializationType.type_subPacket:
                    WriteSubPacket(WriterRef, (PacketBase)value);
                    break;

                case SerializationType.type_subPacketList:
                    List<PacketBase> listValue = ((IEnumerable<PacketBase>)value).ToList();
                    WriterRef.Write(listValue.Count); // saving the count of list elements

                    foreach (var listItem in listValue)
                        WriteSubPacket(WriterRef, listItem);
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported serialization type: {type}");
            }
        }

        private void WriteSubPacket(BinaryWriter writerRef, PacketBase packet)
        {
            writerRef.Write(packet.Serialize());
        }

        private PacketBase ReadSubPacket(BinaryReader readerRef)
        {
            byte[] packetSizeBytes = readerRef.ReadBytes(4);
            byte packetType = readerRef.ReadByte();
            try
            {
                int packetSize = BitConverter.ToInt32(packetSizeBytes, 0);
                byte[] SlicedPacketData = readerRef.ReadBytes(packetSize - 4 - 1);
                byte[] packetData = new byte[packetSize + 1 + 4];

                Array.Copy(packetSizeBytes, 0, packetData, 0, 4);
                packetData[4] = packetType;
                Array.Copy(SlicedPacketData, 0, packetData, 5, SlicedPacketData.Length);

                return Deserialize((PacketType)packetType, packetData);

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            throw new Exception("asdf");
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

                            case SerializationType.type_subPacket:
                                totalSize += ((PacketBase)value).CalculateTotalSize();
                                break;

                            case SerializationType.type_subPacketList:
                                //List<PacketBase> listValue = (List<PacketBase>)value;
                                List<PacketBase> listValue = ((IEnumerable<PacketBase>)value).ToList();

                                totalSize += 4; // saved list count size (sizeof(int))

                                foreach (var listItem in listValue)
                                {
                                    totalSize += listItem.CalculateTotalSize();
                                }
                                break;

                            default:
                                throw new InvalidOperationException($"Unsupported serialization type: {(byte)serializationAttribute.Type}");
                        }
                    }

                }
            }
            return totalSize;
        }

        public virtual string GetInfo()
        {
            return CalculateTotalSize().ToString();
        }

    }
}
