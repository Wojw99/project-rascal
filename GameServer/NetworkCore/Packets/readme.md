I have problem with casting base class object (Packet) to derived class object (for example PlayerStatePacket),
so I am using a copy constructor. In that way we must be carefull. So before overload copy contructor in
"OnPacketReceived" method, always check is packet that came typeof of specified packet that you want.
Also do it anywhere else.

I don't know what is the cause of this problem, but I think the problem is in Packet Serialization methods.