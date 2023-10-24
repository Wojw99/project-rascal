using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;

// TO DO:
// [DontSerialize] - specified attributes to not serialize
// [nullAllowed] - example: public int? value;

namespace PerformanceTests.Test
{
    public static class PacketSerializer
    {
        public static byte[] Serialize<T>(T obj) where T : class, new()
        {
            using (MemoryStream MemStream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(MemStream))
            {
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    object? value = property.GetValue(obj);

                    // if value == null -> write 1
                    writer.Write((byte)(value == null ? 1 : 0));

                    if (value != null)
                        WriteField(writer, value);
                }
                return MemStream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] data) where T : class, new()
        {
            T obj = new T();
            using (MemoryStream MemStream = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(MemStream))
            {
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    // Check is field a null.
                    if(reader.ReadByte() != 1)
                    {
                        property.SetValue(obj, PacketSerializer.ReadField(reader, property.PropertyType));
                    }
                }

                return obj;
            }
        }

        private static void WriteField(BinaryWriter writer, object value)
        {
            Type type = value.GetType();

            if (type.IsGenericType) { // Subclass
                WriteGenericField(writer, value);
                return;
            }
            else if(type.IsClass)
            {
                WriteClass(writer, value);
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

        private static object ReadField(BinaryReader reader, Type type)
        {
            if (type.IsGenericType)
            {
                return ReadGenericField(reader, type);
            }
            else if(type.IsClass)
            {
                return ReadClass(reader, type);
            }
            else if (type == typeof(int))
            {
                return reader.ReadInt32();
            }
            else if (type == typeof(short))
            {
                return reader.ReadInt16();
            }
            else if (type == typeof(long))
            {
                return reader.ReadInt64();
            }
            else if (type == typeof(uint))
            {
                return reader.ReadUInt32();
            }
            else if (type == typeof(ushort))
            {
                return reader.ReadUInt16();
            }
            else if (type == typeof(ulong))
            {
                return reader.ReadUInt64();
            }
            else if (type == typeof(float))
            {
                return reader.ReadSingle();
            }
            else if (type == typeof(double))
            {
                return reader.ReadDouble();
            }
            else if (type == typeof(string))
            {
                int strLength = reader.ReadInt32();
                byte[] utf8Bytes = reader.ReadBytes(strLength);
                string strValue = Encoding.UTF8.GetString(utf8Bytes);
                return strValue;
            }
            else if (type == typeof(bool))
            {
                return reader.ReadBoolean();
            }
            else
            {
                throw new InvalidOperationException($"Unsupported serialization type: {type}");
            }
        }

        private static void WriteGenericField(BinaryWriter writer, object value)
        {
            if (value == null)
                throw new ArgumentNullException($"Argument 'value' = {value} was null.");

            Type genericType = value.GetType().GetGenericTypeDefinition();

            // idk it have sense
            if (!(genericType.IsGenericType))
                throw new ArgumentException($"Argument 'genericType' = {genericType} was not generic type.");

            if (genericType == typeof(List<>)) {
                var list = (IList)value;

                writer.Write(list.Count);

                foreach (object item in list) {
                    WriteField(writer, item);
                }
                return;
            }
            else {
                WriteClass(writer, value);
                return;
            }
        }

        private static object ReadGenericField(BinaryReader reader, Type type)
        {
            if (type == null)
                throw new ArgumentNullException($"Argument 'type' = {type} was null.");

            Type genericType = type.GetGenericTypeDefinition();

            if (!(type.IsGenericType))
                throw new ArgumentException($"Argument 'type' = {type} was not generic type.");

            if (genericType == typeof(List<>))
            {
                int listCount = reader.ReadInt32();
                Type elementType = type.GetGenericArguments()[0];
                IList list = (IList)Activator.CreateInstance(type);

                if (list == null)
                    throw new InvalidOperationException($"Cannot create a list.");

                for (int i = 0; i < listCount; i++)
                    list.Add(ReadField(reader, elementType));

                return list;
            }
            else
            {
                return ReadClass(reader, type);
            }
        }

        private static void WriteClass(BinaryWriter writer, object value)
        {
            if(!(value.GetType().IsClass))
                throw new ArgumentException($"Argument 'value' = {value} was not a class.");

            writer.Write(PacketSerializer.CalculateTotalSize(value));
            writer.Write(PacketSerializer.Serialize(value));
        }

        private static object ReadClass(BinaryReader reader, Type type)
        {
            if (!(type.IsClass))
                throw new ArgumentException($"Argument 'type' = {type} was not a class.");

            int size = reader.ReadInt32();
            byte[] data = reader.ReadBytes(size);

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

        private static int GetGenericTypeObjectSize(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException($"Argument 'obj' = {obj} was null.");

            Type genericType = obj.GetType().GetGenericTypeDefinition();

            // idk it have sense
            if (!(genericType.IsGenericType))
                throw new ArgumentException($"Argument 'genericType' = {genericType} was not generic type.");

            if (genericType == typeof(List<>))
            {
                var list = (IList)obj;
                int totalSize = 0;

                foreach(object item in list )
                {
                    totalSize += GetFieldSize(item);
                }

                return totalSize;
            }
            else
            {
                return CalculateTotalSize(obj);
            }
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

                    totalSize += GetFieldSize(value);
                }
            }
            return totalSize;
        }

        private static int GetFieldSize(object value)
        {
            if (value == null)
                throw new ArgumentNullException($"value = {value} was null.");

            int totalSize = 0;

            Type type = value.GetType();

            if (type.IsGenericType)
            {
                totalSize += GetGenericTypeObjectSize(value);
            }
            if (type.IsClass)
            {
                totalSize += CalculateTotalSize(value);
            }
            else if (type == typeof(int))
            {
                totalSize += sizeof(int);
            }
            else if (type == typeof(short))
            {
                totalSize += sizeof(short);
            }
            else if (type == typeof(long))
            {
                totalSize += sizeof(long);
            }
            else if (type == typeof(ushort))
            {
                totalSize += sizeof(ushort);
            }
            else if (type == typeof(uint))
            {
                totalSize += sizeof(uint);
            }
            else if (type == typeof(ulong))
            {
                totalSize += sizeof(ulong);
            }
            else if (type == typeof(float))
            {
                totalSize += sizeof(float);
            }
            else if (type == typeof(double))
            {
                totalSize += sizeof(double);
            }
            else if (type == typeof(string))
            {
                string strValue = (string)value;
                totalSize += sizeof(int) + Encoding.UTF8.GetByteCount(strValue); // Rozmiar stringa (int) + bajty tekstu
            }
            else if (type == typeof(bool))
            {
                totalSize += sizeof(bool);
            }
            else
            {
                throw new InvalidOperationException($"Unsupported serialization type: {type}");
            }

            return totalSize;
        }
    }
}
