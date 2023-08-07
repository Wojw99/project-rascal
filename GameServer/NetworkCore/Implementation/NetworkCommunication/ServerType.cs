using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkCommunication
{
    public enum ServerType
    {
        default_server = 0x1,
        auth_server = 0x2,
        proxy_server = 0x3,
        world_server = 0x4,
        area_server = 0x5, // game server
        patch_server = 0x6
    }
}
