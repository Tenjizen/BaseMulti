using System;
using System.Collections.Generic;
using AssemblyExample;
using PlayerIO.GameLibrary;

namespace Server
{
    public class Player : BasePlayer
    {
        public float posx = 0;
        public float posz = 0;
    }

    [RoomType("Server")]
    public class GameCode : Game<Player>
    {

        private Dictionary<string, IFunction> _msgPossible = new Dictionary<string, IFunction>();
        // This method is called when an instance of your the game is created
        public override void GameStarted()
        {
            // anything you write to the Console will show up in the 
            // output window of the development server
            Console.WriteLine("Game is started: " + RoomId);

            AddMessagePossible();
        }

        private void AddMessagePossible()
        {
            _msgPossible.Add("TEST", new Test());
            _msgPossible.Add("TESTMOVE", new TestS2C());
        }


        // This method is called when the last player leaves the room, and it's closed down.
        public override void GameClosed()
        {
            Console.WriteLine("RoomId: " + RoomId);
        }

        // This method is called whenever a player joins the game
        public override void UserJoined(Player player)
        {
            foreach (Player pl in Players)
            {
                if (pl.ConnectUserId != player.ConnectUserId)
                {
                    pl.Send("PlayerJoined", player.ConnectUserId, 0, 0);
                    player.Send("PlayerJoined", pl.ConnectUserId, pl.posx, pl.posz);
                }
            }
        }

        // This method is called when a player leaves the game
        public override void UserLeft(Player player)
        {
            Broadcast("PlayerLeft", player.ConnectUserId);
        }

        // This method is called when a player sends a message into the server code
        public override void GotMessage(Player player, Message message)
        {
            if (!_msgPossible.TryGetValue(message.Type, out IFunction func))
            {
                Console.WriteLine("no message with that type");
                return;
            }


            func.Execute(player,message,this);

            //    switch (message.Type)
            //{
            //    // called when a player clicks on the ground
            //    case "MOVE":
            //        int oldPosX = message.GetInt(0);
            //        int oldPosY = message.GetInt(1);
            //        int newPosX = message.GetInt(2);
            //        int newPosY = message.GetInt(3);
            //        Console.WriteLine($"old {oldPosX}:{oldPosY} - new {newPosX}:{newPosY}");
            //        Broadcast("MOVE", player.ConnectUserId, oldPosX, oldPosY, newPosX, newPosY);
            //        break;

            //    case "TEST":
            //        Console.WriteLine($"{message.GetInt(1)}  {message.GetString(0)}");
            //        break;
            //}
        }
    }
}