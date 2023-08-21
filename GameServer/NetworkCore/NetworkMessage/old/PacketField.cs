/*using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace NetworkCore.NetworkMessage.old
{
    public struct PacketField
    {
        public byte[] FieldType { get; set; }

        public byte[] Name { get; set; }

        public byte[] Buffer { get; set; }

        public byte FieldTypeLength { get; set; }

        public byte NameLength { get; set; }

        public int BufferLength { get; set; }

        public PacketField(byte[] fieldType, byte[] name, byte[] buffer)
        {
            FieldType = fieldType;
            Name = name;
            Buffer = buffer;
            FieldTypeLength = (byte)fieldType.Length;
            NameLength = (byte)name.Length;
            BufferLength = buffer.Length;
        }

        public byte[] Serialize()
        {
            using (MemoryStream stream = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(FieldTypeLength);
                writer.Write(FieldType);

                writer.Write(NameLength);
                writer.Write(Name);

                writer.Write(BufferLength);
                writer.Write(Buffer);

                return stream.ToArray();
            }
        }

        public static PacketField Deserialize(byte[] data)
        {
            PacketField field = new PacketField();

            using (MemoryStream stream = new MemoryStream(data))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                field.FieldTypeLength = reader.ReadByte();
                field.FieldType = reader.ReadBytes(field.FieldTypeLength);

                field.NameLength = reader.ReadByte();
                field.Name = reader.ReadBytes(field.NameLength);

                field.BufferLength = reader.ReadInt32();
                field.Buffer = reader.ReadBytes(field.BufferLength);
            }

            return field;
        }

        public int CalculateTotalSize()
        {
            // Including serialization lengths and sizes of length values (2 * sizeof(byte)), and for buffer length sizeof(int).
            return FieldType.Length + Name.Length + Buffer.Length + sizeof(byte) * 2 + sizeof(int);
        }



    }
}
*/