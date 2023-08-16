using NetworkCore.Packets.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkCore.NetworkData
{
    public class Enemy
    {
        public List<PlayerAttribute> attributes = new List<PlayerAttribute>();
        public int EnemyId;
    }
}
