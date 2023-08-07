using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public enum MessageType
    {
        Request = 0x1,
        Response = 0x2,
        //Async = 0x4,
    }
}
