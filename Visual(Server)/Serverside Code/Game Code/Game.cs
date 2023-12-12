using System;
using System.Collections.Generic;
using AssemblyExample;
using PlayerIO.GameLibrary;

namespace Server
{
    public class Player : BasePlayer
    {
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
            _msgPossible.Add("PlayerJoined", new PlayerJoin());
           _msgPossible.Add("MovePlayer", new MoveBall());
           _msgPossible.Add("PlayerTurn", new SwitchTurn());
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
                    pl.Send("PlayerJoined", player.Id);
                    player.Send("PlayerJoined", pl.Id);
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
        }
    }
}