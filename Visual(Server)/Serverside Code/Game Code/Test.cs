using PlayerIO.GameLibrary;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyExample
{
    class Test : IFunction
    {
        public void Execute(Player player, Message message, GameCode game)
        {
            Console.WriteLine("test");
            Console.WriteLine($"{message.GetInt(1)}  {message.GetString(0)}");
            game.Broadcast("TEST", "playerTest");
        }
    }
}
