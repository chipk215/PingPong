using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PingPong.Managers;
using PingPong.Models;

namespace UnitTests
{
    [TestClass]
    public class GameManagerTest
    {
        // Verify algorithm which determines server works correctly
        [TestMethod]
        public void PlayerOneCrushServerTest()
        {
            var manager = new GameManager();

            // Set initial server to side one
            manager.Game.InitialServer = PlayerId.SideOne;
            Assert.AreEqual(PlayerId.SideOne, manager.GetNextToServe());

            // award 1st point Score 1-0
            var point = new Point {PlayerToAward = PlayerId.SideOne};
            manager.AwardPoint(point);
            // SideOne serves 2nd point
            Assert.AreEqual(PlayerId.SideOne,manager.GetNextToServe());

            // award 2nd point  Score 2-0
            manager.AwardPoint(new Point { PlayerToAward = PlayerId.SideOne });
            // SideTwo serves 3rd point
            Assert.AreEqual(PlayerId.SideTwo, manager.GetNextToServe());

            // award 3nd point  Score 3-0
            manager.AwardPoint(new Point { PlayerToAward = PlayerId.SideOne });
            // SideTwo serves 4rd point
            Assert.AreEqual(PlayerId.SideTwo, manager.GetNextToServe());

            // award 4th point  Score 4-0
            manager.AwardPoint(new Point { PlayerToAward = PlayerId.SideOne });
            // SideOne serves 4th point
            Assert.AreEqual(PlayerId.SideOne, manager.GetNextToServe());

            // award 5th point  Score 5-0
            manager.AwardPoint(new Point { PlayerToAward = PlayerId.SideOne });
            // SideOne serves 5th point
            Assert.AreEqual(PlayerId.SideOne, manager.GetNextToServe());

            // award points 6-10
            for (int i = 6; i < 11; i++)
            {
                manager.AwardPoint(new Point {PlayerToAward = PlayerId.SideOne});
            }

            // initial receiver should server 11th point
            Assert.AreEqual(PlayerId.SideTwo, manager.GetNextToServe());

        }

        //Verify correct server is identified for 1st deuce point
        [TestMethod]
        public void FirstDeucePoint()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)PlayerId.SideOne].Score = 11;
            manager.Game.Players[(int)PlayerId.SideTwo].Score = 11;

            //23rd point should be served by Initial Receiver
            Assert.AreNotEqual<PlayerId>(manager.Game.InitialServer,manager.GetNextToServe());

        }


        //Verify correct server is identified for 1st add point
        [TestMethod]
        public void FirstAddPoint()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)PlayerId.SideOne].Score = 12;
            manager.Game.Players[(int)PlayerId.SideTwo].Score = 11;

            //24th point should be served by Initial Server
            Assert.AreEqual<PlayerId>(manager.Game.InitialServer, manager.GetNextToServe());
        }


        //Verify correct server is identified for 2nd deuce point
        [TestMethod]
        public void SecondDeucePoint()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)PlayerId.SideOne].Score = 12;
            manager.Game.Players[(int)PlayerId.SideTwo].Score = 12;

            //23rd point should be served by Initial Receiver
            Assert.AreNotEqual<PlayerId>(manager.Game.InitialServer, manager.GetNextToServe());

        }

        //Verify correct server is identified for 2nd add point
        [TestMethod]
        public void SecondAddPoint()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)PlayerId.SideOne].Score = 12;
            manager.Game.Players[(int)PlayerId.SideTwo].Score = 13;

            //24th point should be served by Initial Server
            Assert.AreEqual<PlayerId>(manager.Game.InitialServer, manager.GetNextToServe());
        }

        //Ensure exceptioin is thrown for invalid score adjustment
        [TestMethod]
        [ExpectedException(typeof (System.ArgumentOutOfRangeException))]
        public void InvalidScoreTest()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)PlayerId.SideOne].Score = 12;
            manager.Game.Players[(int)PlayerId.SideTwo].Score = 14;

            // attempt to make score 12-15
            manager.AwardPoint(new Point{PlayerToAward = PlayerId.SideTwo});

        }


        //Verify game state changes to Complete when player achieves 11th point with 2 point margin
        [TestMethod]
        public void EndOfGameTest()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)PlayerId.SideOne].Score = 10;
            manager.Game.Players[(int)PlayerId.SideTwo].Score = 7;

            Assert.AreEqual(manager.Game.GameState, GameState.InProgress);
            manager.AwardPoint(new Point { PlayerToAward = PlayerId.SideOne });

            Assert.AreEqual(manager.Game.GameState,GameState.Complete);
        }

        //Verify game state remains in progress when player achieves 11th point with 1 point margin
        [TestMethod]
        public void ContinueGameTest()
        {
            var manager = new GameManager();
            manager.Game.Players[(int)PlayerId.SideOne].Score = 10;
            manager.Game.Players[(int)PlayerId.SideTwo].Score = 10;

            Assert.AreEqual(manager.Game.GameState, GameState.InProgress);
            manager.AwardPoint(new Point { PlayerToAward = PlayerId.SideOne });

            Assert.AreEqual(manager.Game.GameState, GameState.InProgress);
        }
    }
}
