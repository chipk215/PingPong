using System;
using System.Collections.Generic;
using PingPong.Models;

namespace PingPong.Managers
{
    public class GameManager : IGameManager
    {

        //=============== Properties ===========================


        // Lazily instantiate a Game if one is not provided
        private Game _game;
        public Game Game
        {
            get
            {
                if (_game == null)
                {
                    _game = new Game
                    {
                        GameId = new Guid(),
                        InitialServer = ChooseInitialServer(),
                        Players = new Player[]
                        {
                            new Player{Name = "Player One", Score = 0, History = new List<Point>()},
                            new Player{Name = "Player Two", Score = 0 ,History = new List<Point>()}
                        },
                        GameState = GameState.InProgress
                    };
                    
                }
                return _game;
            }

            set { _game = value; }
        }


        //=============== Public Methods ===========================
        public void AwardPoint(Point point)
        {
            int playerAwarded = (int)point.PlayerToAward;
            Game.Players[playerAwarded].Score++;
            Game.Players[playerAwarded].History.Add(point);
            UpdateGameState();
        }

        public PlayerId GetNextToServe()
        {
            PlayerId server;
            PlayerId initialReceiver;

            // Determine who received first
            if (Game.InitialServer == PlayerId.SideOne)
            {
                initialReceiver = PlayerId.SideTwo;
            }
            else
            {
                initialReceiver = PlayerId.SideOne;
            }
            ushort playerOneScore = Game.Players[(int)PlayerId.SideOne].Score;
            ushort playerTwoScore = Game.Players[(int)PlayerId.SideTwo].Score;

            var pointNumber = playerOneScore + playerTwoScore + 1;


            if (pointNumber > 22)
            {
                server = IsOdd(pointNumber) ? initialReceiver : Game.InitialServer;
            }
            else  // score has not reached deuce
            {
                // check for odd point number
                if (pointNumber % 2 > 0)
                {
                    pointNumber++;
                }
                server = IsOdd(pointNumber / 2) ? Game.InitialServer : initialReceiver;
            }

            return server;
        }


        //=============== Private Methods ===========================

        private void UpdateGameState()
        {
            ushort playerOneScore = Game.Players[(int)PlayerId.SideOne].Score;
            ushort playerTwoScore = Game.Players[(int)PlayerId.SideTwo].Score;

            // throw exception if scores are invalid
            if ((playerOneScore > 10) && (playerTwoScore > 10) && (Math.Abs(playerOneScore - playerTwoScore) > 2))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (Math.Abs(playerOneScore - playerTwoScore) > 2)
            {
                if ((playerOneScore >= 11) || (playerTwoScore >= 11))
                {
                    Game.GameState = GameState.Complete;
                }
            }
        }


        //=============== Static Helpers ===========================
        static bool IsOdd(int value)
        {
            return value % 2 > 0;
        }

        static public PlayerId ChooseInitialServer()
        {
            var choice = PlayerId.SideOne;
            var coin = new Random();
            var toss= coin.Next(0, 1);
            if (toss == 1)
            {
                choice = PlayerId.SideTwo;
            }

            return choice;
        }


    }
}
