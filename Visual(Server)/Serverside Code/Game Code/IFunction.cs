using PlayerIO.GameLibrary;
using Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyExample
{
    public interface IFunction
    {
        void Execute(Player player, Message message,GameCode game);
    }
}
