#ifndef __PACKET_TYPES_H__
#define __PACKET_TYPES_H__

#include <cstdint>

enum class PacketType : uint8_t // 1 bajt - zakres od 0 do 255
{
    player_move = 10,
    player_level_up = 11,
    player_teleport = 12,
    player_shoot = 13,
    enemy_shoot = 20,
    monster_pos = 30,
};

#endif