using NetworkCore.Packets;
using NetworkCore.Packets.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerApplication.GameService.Base
{
    public class Player
    {
        //public IPeer PlayerPeer { get; set; }
        public List<PlayerAttribute> attributes = new List<PlayerAttribute>();
        public int PlayerId;

        public Player(int playerId) 
        {
            PlayerId = playerId;
        }
    }
}
