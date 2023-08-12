using System;
using System.Collections.Generic;
using System.Text;

namespace NetworkCore.NetworkMessage
{
    public enum PacketType
    {
        packet_player_move = 10,
        packet_player_level_up = 11,
        packet_player_teleport = 12,
        packet_player_shoot = 13,
        packet_enemy_shoot = 20,
        packet_monster_pos = 30,
        packet_test_packet = 31,
        packet_global_player_position = 32
    }
}
