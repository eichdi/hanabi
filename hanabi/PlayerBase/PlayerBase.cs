using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hanabi.GameLogic;

namespace hanabi.PlayerBase
{
    class PlayerBase
    {
        private List<Player> playerList;

        public PlayerBase()
        {
            playerList = new List<Player>();
        }
        public Player GetPlayer(long id)
        {
            foreach (Player player in playerList)
            {
                if (id == player.ID)
                {
                    return player;
                }
            }
            return new Player(id);
        }
    }
}
