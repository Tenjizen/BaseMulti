using PlayerIO.GameLibrary;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyExample
{
    class MoveBall : IFunction
    {
        public void Execute(Player player, Message message, GameCode game)
        {
            float newPosX = message.GetFloat(0);
            float newPosY = message.GetFloat(1);
            float newPosZ = message.GetFloat(2);


            foreach (Player pl in game.Players)
            {
                if (pl.ConnectUserId != player.ConnectUserId)
                {
                    pl.Send("MovePlayer" , newPosX, newPosY, newPosZ, player.Id);
                }
            }
        }
    }
}
