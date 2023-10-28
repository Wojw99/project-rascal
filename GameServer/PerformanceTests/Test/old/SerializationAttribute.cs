/*using System;
using System.Collections.Generic;
using System.Text;

namespace PerformanceTests.Test
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class SerializationAttribute : Attribute
    {
        public SerializationType Type { get; }

        public SerializationAttribute(SerializationType Type)
        {
            this.Type = Type;
        }
    }

    public enum SerializationType
    {
        type_null = 0x0,
        type_Int16 = 0x1,
        type_Int32 = 0x2,
        type_Int64 = 0x3,
        type_UInt16 = 0x4,
        type_UInt32 = 0x5,
        type_UInt64 = 0x6,
        type_float = 0x7,
        type_double = 0x8,
        type_string = 0x9,
        type_bool = 0x10,
        type_list = 0x11,
        type_subPacket = 0x12,
        type_subPacketList = 0x13,
    }
}
*/