/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests.Test.old
{
    public static class PacketSerializer2
    {
        public static T Deserialize<T>(byte[] data) where T : class, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            T obj = new T();

            using (MemoryStream MemStream = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(MemStream))
            {
                //int totalSize = reader.ReadInt32();

                foreach (PropertyInfo property in properties)
                {
                    SerializationAttribute attribute = property.GetCustomAttribute<SerializationAttribute>();

                    if (attribute != null)
                    {
                        //Object value = property.GetValue(obj);
                        SerializationType serializationType = (SerializationType)reader.ReadByte();
                        ReadSetValue(property, reader, serializationType, obj);
                    }
                }

                return obj;
            }
        }

        public static byte[] Serialize<T>(T obj) where T : class, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            using (MemoryStream MemStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(MemStream))
            {
                foreach (PropertyInfo property in properties)
                {
                    SerializationAttribute attribute = property.GetCustomAttribute<SerializationAttribute>();
                    if (attribute != null)
                    {
                        object value = property.GetValue(obj);

                        if (value == null)
                        {
                            writer.Write((byte)SerializationType.type_null);
                            writer.Write((byte)0x0);
                        }
                        else
                        {
                            writer.Write((byte)attribute.Type);
                            WriteField(writer, value, attribute.Type);
                        }
                    }
                }

                return MemStream.ToArray();
            }
        }

        private static void WriteField(BinaryWriter WriterRef, object value, SerializationType type)
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
                    WriteSubPacket(WriterRef, value);
                    break;

                case SerializationType.type_subPacketList:
                    //List<PacketBase> listValue = ((IEnumerable<PacketBase>)value).ToList();
                    //WriterRef.Write(listValue.Count); // saving the count of list elements

                    //foreach (var listItem in listValue)
                    //    WriteSubPacket(WriterRef, listItem);
                    break;

                default:
                    throw new InvalidOperationException($"Unsupported serialization type: {type}");
            }
        }

        private static void ReadSetValue(PropertyInfo property, BinaryReader readerRef, SerializationType type, object obj)
        {
            switch (type)
            {
                case SerializationType.type_null:
                    readerRef.ReadByte();
                    property.SetValue(obj, null);
                    break;

                case SerializationType.type_Int16:
                    property.SetValue(obj, readerRef.ReadInt16());
                    break;

                case SerializationType.type_Int32:
                    property.SetValue(obj, readerRef.ReadInt32());
                    break;

                case SerializationType.type_Int64:
                    property.SetValue(obj, readerRef.ReadInt64());
                    break;

                case SerializationType.type_UInt16:
                    property.SetValue(obj, readerRef.ReadUInt16());
                    break;

                case SerializationType.type_UInt32:
                    property.SetValue(obj, readerRef.ReadUInt32());
                    break;

                case SerializationType.type_UInt64:
                    property.SetValue(obj, readerRef.ReadUInt64());
                    break;

                case SerializationType.type_float:
                    property.SetValue(obj, readerRef.ReadSingle());
                    break;

                case SerializationType.type_double:
                    property.SetValue(obj, readerRef.ReadDouble());
                    break;

                case SerializationType.type_string:
                    int strLength = readerRef.ReadInt32();
                    byte[] utf8Bytes = readerRef.ReadBytes(strLength);
                    string strValue = Encoding.UTF8.GetString(utf8Bytes);
                    property.SetValue(obj, strValue);
                    break;

                case SerializationType.type_bool:
                    property.SetValue(obj, readerRef.ReadBoolean());
                    break;

                case SerializationType.type_subPacket:
                    property.SetValue(obj, ReadSubPacket(readerRef, obj));
                    break;

                case SerializationType.type_subPacketList:
                    *//* int ListCount = readerRef.ReadInt32();
                     List<PacketBase> packetList = new List<PacketBase>();

                     for (int i = 0; i < ListCount; i++)
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

                     property.SetValue(value, typedList); // Ustaw wartość właściwości*//*

                    break;

                default:
                    throw new InvalidOperationException($"Unsupported serialization type: {type}");
            }
        }

        private static void WriteSubPacket<T>(BinaryWriter writerRef, T packetClass) where T : class, new()
        {
            writerRef.Write(CalculateTotalSize(packetClass));
            byte[] testData = Serialize(packetClass);
            writerRef.Write(testData);
            Console.WriteLine("test");
        }

        private static T ReadSubPacket<T>(BinaryReader readerRef, T packetClass) where T : class, new()
        {

            int packetSize = BitConverter.ToInt32(readerRef.ReadBytes(4), 0);
            byte[] data = readerRef.ReadBytes(packetSize);
            return Deserialize<T>(data);
        }

        public static int CalculateTotalSize<T>(T packetClass) where T : class, new()
        {
            int totalSize = sizeof(byte); // TypeId
            totalSize += sizeof(int);
            totalSize += sizeof(bool); // IsResponse

            foreach (PropertyInfo property in packetClass.GetType().GetProperties())
            {
                SerializationAttribute serializationAttribute = property.GetCustomAttribute<SerializationAttribute>();

                if (serializationAttribute != null)
                {
                    totalSize += sizeof(byte); // Serialization type byte

                    object value = property.GetValue(packetClass);

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
                                string strValue = (string)property.GetValue(packetClass);
                                totalSize += sizeof(int) + Encoding.UTF8.GetByteCount(strValue); // Rozmiar stringa (int) + bajty tekstu
                                break;

                            case SerializationType.type_bool:
                                totalSize += sizeof(bool);
                                break;

                            case SerializationType.type_subPacket:
                                totalSize += CalculateTotalSize((T)value);
                                break;

                            case SerializationType.type_subPacketList:
                                *//*//List<PacketBase> listValue = (List<PacketBase>)value;
                                List<PacketBase> listValue = ((IEnumerable<PacketBase>)value).ToList();

                                totalSize += 4; // saved list count size (sizeof(int))

                                foreach (var listItem in listValue)
                                {
                                    totalSize += listItem.CalculateTotalSize();
                                }*//*
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



    // **********************************************************************************************************
    // **********************************************************************************************************
    // **********************************************************************************************************
    // **********************************************************************************************************
    // **********************************************************************************************************
    // **********************************************************************************************************
    // **********************************************************************************************************



}
*/