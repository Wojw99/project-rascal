using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public enum FieldType
    {
        field_null = 0,
        field_int = 1,
        field_short = 2,
        field_long = 3,
        field_double = 4,
        field_float = 5,
        field_string = 6,
    }
}
