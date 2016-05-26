using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class InfoPlayer
    {
        public CollectCardOnHand CardPlayer {
			get; set;
		}

        public Player Player {
			get; set;
		}

        public InfoPlayer(Player player, PackOfCard pack)
        {
            //раздаем карты
            OnHandCard[] card = new OnHandCard[5];
            for (int i = 0; i < 5; i++)
            {
                card[i] = pack.TakeCard();
            }
            CardPlayer = new CollectCardOnHand(card);
            this.Player = player;

        }
    }
}
