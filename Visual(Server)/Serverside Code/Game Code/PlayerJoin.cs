using PlayerIO.GameLibrary;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyExample
{
    class PlayerJoin : IFunction
    {
        public void Execute(Player player, Message message, GameCode game)
        {
            player.Send("PlayerSetId",player.Id);
            player.Send("PlayerJoined",player.Id);
        }
    }
}
