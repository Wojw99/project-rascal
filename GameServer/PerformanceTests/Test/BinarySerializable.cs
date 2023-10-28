using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTests.Test
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public class BinarySerializable : Attribute
    {
        public SerializationType Type { get; } 

        public BinarySerializable()
        {
            this.Type = SerializationType.Default;
        }

        public BinarySerializable(SerializationType Type)
        {
            this.Type = Type;
        }
    }

    public enum SerializationType
    {
        Default = 0x1,
        Collection =0x2,

    }
}
