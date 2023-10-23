using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

// TO DO:
// [DontSerialize] - specified attributes to not serialize
// [nullAllowed] - example: public int? value;

namespace PerformanceTests.Test
{
    public static class PacketSerializer
    {
        public static T Deserialize<T>(byte[] data) where T : class, new()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();
            T obj = new T();
            
            using (MemoryStream MemStream = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(MemStream))
            {
                foreach (PropertyInfo property in properties)
                {
                    // Check is field a null.
                    if(reader.ReadByte() != 1)
                        SetObjectPropertyValue(property, reader, obj);
                }

                return obj;
            }
        }

        public static byte[] Serialize<T>(T obj) where T : class, new()
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            using (MemoryStream MemStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(MemStream))
            {
                foreach (PropertyInfo property in properties)
                {
                    WriteField(writer, property, obj);  
                }

                return MemStream.ToArray();
            }
        }

        private static void WriteField<T>(BinaryWriter writer, PropertyInfo property, T obj)
        {
            if (property == null)
            {
                throw new ArgumentNullException($"Property = {property} cannot be null.");
            }
            
            object? value = property.GetValue(obj);

            // if value is null set write "1".
            writer.Write((byte)(value == null ? 1 : 0));

            if (value == null) {
                return;
            }

            Type type = property.PropertyType;

            if (type.IsClass) { // Subclass
                writer.Write(PacketSerializer.CalculateTotalSize(value));
                writer.Write(PacketSerializer.Serialize(value));
                return;
            }
            else if (type == typeof(int)) {
                writer.Write((int)value);
                return;
            }
            else if (type == typeof(short)) {
                writer.Write((short)value);
                return;
            }
            else if (type == typeof(long)) {
                writer.Write((long)value);
                return;
            }
            else if (type == typeof(ushort)) {
                writer.Write((ushort)value);
                return;
            }
            else if (type == typeof(uint)) {
                writer.Write((uint)value);
                return;
            }
            else if (type == typeof(ulong)) {
                writer.Write((ulong)value);
                return;
            }
            else if (type == typeof(float)) {
                writer.Write((float)value);
                return;
            }
            else if (type == typeof(double)) {
                writer.Write((double)value);
                return;
            }
            else if (type == typeof(string)) {
                string strValue = (string)value;
                byte[] utf8Bytes = Encoding.UTF8.GetBytes(strValue);
                writer.Write(utf8Bytes.Length);
                writer.Write(utf8Bytes);
                return;
            }
            else if (type == typeof(bool)) {
                writer.Write((bool)value);
                return;
            }
            else {
                throw new InvalidOperationException($"Unsupported serialization type: {type}");
            }
        }

        private static void SetObjectPropertyValue<T>(PropertyInfo property, BinaryReader reader, T obj)
        {
            Type type = property.PropertyType;
            
            if (type.IsClass)
            {
                int size = reader.ReadInt32();
                byte[] data = reader.ReadBytes(size);
                
                //property.SetValue(obj, PacketSerializer.Deserialize<>(data));
                property.SetValue(obj, PacketSerializer.DeserializeSubClass(data, type));
            }
            else if (type == typeof(int))
            {
                property.SetValue(obj, reader.ReadInt32());
                return;
            }
            else if (type == typeof(short))
            {
                property.SetValue(obj, reader.ReadInt16());
                return;
            }
            else if (type == typeof(long))
            {
                property.SetValue(obj, reader.ReadInt64());
                return;
            }
            else if (type == typeof(uint))
            {
                property.SetValue(obj, reader.ReadUInt32());
                return;
            }
            else if (type == typeof(ushort))
            {
                property.SetValue(obj, reader.ReadUInt16());
                return;
            }
            else if (type == typeof(ulong))
            {
                property.SetValue(obj, reader.ReadUInt64());
                return;
            }
            else if (type == typeof(float))
            {
                property.SetValue(obj, reader.ReadSingle());
                return;
            }
            else if (type == typeof(double))
            {
                property.SetValue(obj, reader.ReadDouble());
                return;
            }
            else if (type == typeof(string))
            {
                int strLength = reader.ReadInt32();
                byte[] utf8Bytes = reader.ReadBytes(strLength);
                string strValue = Encoding.UTF8.GetString(utf8Bytes);
                property.SetValue(obj, strValue);
                return;
            }
            else if (type == typeof(bool))
            {
                property.SetValue(obj, reader.ReadBoolean());
                return;
            }
            else if (type == typeof(Type))
            {

                return;
            }
            else {
                throw new InvalidOperationException($"Unsupported serialization type: {type}");
            }
        }

        private static object DeserializeSubClass(byte[] data, Type type)
        {
           /* if(!type.IsGenericType && !(type.GetGenericTypeDefinition() == typeof(Nullable<>))) {
                throw new InvalidOperationException($"type = {type} does not meet the generic type.");  
            }*/

            //Type genericArgumentType = Activator.CreateInstance(type).GetType();

#pragma warning disable CS8602
            MethodInfo deserializeMethod = typeof(PacketSerializer)
            .GetMethod("Deserialize")
            .MakeGenericMethod(type);
#pragma warning restore CS8602

            object? deserializedValue = deserializeMethod.Invoke(null, new object[] { data });

            if (deserializedValue == null)
                throw new InvalidOperationException($"Value = {deserializedValue} was null.");

            return deserializedValue;
        }

        public static int CalculateTotalSize <T>(T obj) where T: class, new()
        {
            int totalSize = 0;

            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                totalSize += sizeof(int); // null check parameter
                object? value = property.GetValue(obj);
               
                if(value != null)
                {
                    Type type = property.PropertyType;

                    if (type.IsClass) { 
                        totalSize += CalculateTotalSize(value);
                    }
                    else if (type == typeof(int)) {
                        totalSize += sizeof(int);
                    }
                    else if (type == typeof(short)) {
                        totalSize += sizeof(short);
                    }
                    else if (type == typeof(long)) {
                        totalSize += sizeof(long);
                    }
                    else if (type == typeof(ushort)) {
                        totalSize += sizeof(ushort);
                    }
                    else if (type == typeof(uint)) {
                        totalSize += sizeof(uint);
                    }
                    else if (type == typeof(ulong)) {
                        totalSize += sizeof(ulong);
                    }
                    else if (type == typeof(float)) {
                        totalSize += sizeof(float);
                    }
                    else if (type == typeof(double)) {
                        totalSize += sizeof(double);
                    }
                    else if (type == typeof(string)) {
                        string strValue = (string)value;
                        totalSize += sizeof(int) + Encoding.UTF8.GetByteCount(strValue); // Rozmiar stringa (int) + bajty tekstu
                    }
                    else if (type == typeof(bool)) {
                        totalSize += sizeof(bool);
                    }
                    else {
                        throw new InvalidOperationException($"Unsupported serialization type: {type}");
                    }
                }
            }
            return totalSize;
        }
    }
}
