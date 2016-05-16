using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class Player
    {
        //Для возможной игры через сеть
        private int id;
        public Game game;
        public int ID
        {
            get
            {
                return id;
            }
        }
        public Player(Game game)
        {
            this.game = game;
            //уникальное значение
            this.id = Service.GetID;
        }
        public string GetOpponentCard()
        {
            return Service.ConvertCard(game.GetOpponentCard(this));
        }
        public void PlayCard(int index)
        {
            game.PlayCard(this, index);
        }
        public void Drop(int index)
        {
            game.DropCard(this, index);
        }
        public void TellColor(string color, int[] index)
        {
            game.TellColor(this, color, index);
        }
        public void TellRank(int rank, int[] index)
        {
            game.TellRank(this, rank, index);
        }
    }

}
