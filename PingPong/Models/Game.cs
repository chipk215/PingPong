using System;

namespace PingPong.Models
{

    public class Game
    {
        // Identifies Unique Game
        public Guid GameId { get; set; }

        // Represents Two Players
        public Player[] Players { get; set; }

        // Initial Server
        public PlayerId InitialServer { get; set; }

        // Game State
        public GameState GameState { get; set; }
        

    }
}
