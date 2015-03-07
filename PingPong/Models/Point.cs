using System;

namespace PingPong.Models
{
    public class Point
    {
        public Guid GameId { get; set; }

        public PlayerId PlayerToAward { get; set; }

        public ulong Ticks { get; set; }
    }
}
