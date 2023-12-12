using PlayerIO.GameLibrary;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyExample
{
    class SwitchTurn : IFunction
    {
        public void Execute(Player player, Message message, GameCode game)
        {
            int index = message.GetInt(0);
            game.Broadcast("PlayerTurn", index % game.PlayerCount+1);
        }
    }
}
