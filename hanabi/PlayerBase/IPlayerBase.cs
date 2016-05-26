using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hanabi.GameLogic;

namespace hanabi.PlayerBase
{
    interface IPlayerBase
    {
        Player GetPlayer(long id);
        
    }
}
