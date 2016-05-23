using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class Game : hanabi.GameLogic.IPlayerGame
    {
        private int risk;
        private int gamedCards;
        private InfoPlayer firstPlayer;
        private InfoPlayer secondPlayer;
        private PackOfCard pack;
        private GameTable table;
        public Player CurrentPlayer
        {
            get
            {
                return firstPlayer.Player;
            }
        }
        public Player NextPlayer
        {
            get
            {
                return secondPlayer.Player;
            }
        }

        //true игра продолжается
        private bool state;
        public int GamedCards
        {
            get
            {
                return gamedCards;
            }
        }

        //возможно ли продолжать игру

        public bool State
        {
            get
            {
                return state && pack.State;
            }
        }

        public int Risk
        {
            get
            {
                return risk;
            }
        }

        public string GetTableCard()
        {
            return table.GetTableCard();
        }
        private void EndGame()
        {
            state = false;
        }

        public bool SynchGame(Player player1, Player player2){
            this.firstPlayer.Player = player1;
            this.secondPlayer.Player = player2;
            return player1.SetGame(this) && player2.SetGame(this);
        }

        public Game(Player player1, Player player2, PackOfCard pack = null, GameTable table = null)
        {
            if(pack == null){
                pack = PackOfCard.GetRandomCard();
            }
            if (table == null)
            {
                table = new GameTable();
            }
            risk = 0;
            gamedCards = 0;
            state = true;
            this.firstPlayer = new InfoPlayer(player1, pack);
            this.secondPlayer = new InfoPlayer(player2, pack);
            if (!this.SynchGame(player1, player2))
            {
                throw new Exception();
            }
            this.table = table;
            this.pack = pack;
        }
        
        //1
        public void PlayCard(Player player, int index)
        {
            if (CanPlay(player))
            {
                //проверяем является ли карта первого игрока по индексу рискованной в зависимости еще от стола
                bool risked = firstPlayer.CardPlayer.Card[index].RiskCard(table);
                if (risked)
                {
                    bool a;
                    a = risked;
                }
                //кладем на стол карту игрока по индексу и за место нее ставим карту из колоды
                if (!table.PutCard(firstPlayer.CardPlayer.TakeCard(index, pack.TakeCard())))
                {
                    EndGame();
                }
                else
                {
                    //если риск оправдан, то увеличиваем количество рискованных ходов
                    if (risked)
                    {
                        risk++;
                    }
                    gamedCards++;
                }
                DoNextPlayer();
            }

        }
        //2
        public void DropCard(Player player, int index)
        {
            if (CanPlay(player))
            {
                table.DropCard(firstPlayer.CardPlayer.TakeCard(index, pack.TakeCard()));
                DoNextPlayer();
            }
        }
        //3
        public void TellColor(Player player, string color, int[] index)
        {
            color = color.ToUpper();
            if (CanPlay(player))
            {
                for (int i = 0; i < index.Length; i++)
                {
                    //обращаемся к картам следующего игрока по индексу и говорим ему цвет
                    if (!secondPlayer.CardPlayer.Card[index[i]].CheckColor(color))
                    {
                        EndGame();
                    }
                }
            }
            DoNextPlayer();
        }
        //4
        public void TellRank(Player player, int rank, int[] index)
        {
            if (CanPlay(player))
            {
                for (int i = 0; i < index.Length; i++)
                {
                    //обращаемся к картам следующего игрока по индексу и говорим ему ранк
                    if (!secondPlayer.CardPlayer.Card[index[i]].CheckRank(rank))
                    {
                        EndGame();
                    }
                }
            }
            DoNextPlayer();
        }

        //Возможность игры для игрока
        public bool CanPlay(Player player)
        {
            if (player.ID == firstPlayer.Player.ID && state)
            {
                return true;
            }
            return false;
        }

        public Player GetOpponent(Player player)
        {
            if (player.ID != firstPlayer.Player.ID && player.ID == secondPlayer.Player.ID)
                return firstPlayer.Player;
            if (player.ID != secondPlayer.Player.ID && player.ID == firstPlayer.Player.ID)
                return secondPlayer.Player;
            else
                throw new ArgumentException("Игрок не принадлежит этой игре");
        }

        public bool CheckPlayer(Player player)
        {
            return (firstPlayer.Player == player || secondPlayer.Player == player);
        }


        private void DoNextPlayer()
        {
            InfoPlayer temp = firstPlayer;
            firstPlayer = secondPlayer;
            secondPlayer = temp;
        }

        public CollectCardOnHand GetOpponentCard(Player player)
        {
            if(player == firstPlayer.Player)
                return secondPlayer.CardPlayer;
            if (player == secondPlayer.Player)
                return firstPlayer.CardPlayer;
            return null;

        }

		public string GetKnownCards(Player player) {
			if (player == firstPlayer.Player)
				return GetKnownCards(firstPlayer);
			else if (player == secondPlayer.Player)
				return GetKnownCards(secondPlayer);
			else
				throw new ArgumentException("Пользователь не играет в данный момент");
		}

		public string GetKnownCards(InfoPlayer player) {
			string result = "";
			for (int i = 0; i < 5; i++) {
				OnHandCard card = player.CardPlayer[i];
				if (card != null) {
					result += ' ';

					if (card.HasInfoColor)
						result += card.Color[0];
					else
						result += '*';

					if (card.HasInfoRank)
						result += card.Rank;
					else
						result += '*';
				}
			}
			return result;
		}

        //возможный функционал
        public string GetRiskCurrentPlayerCard()
        {
            return this.firstPlayer.CardPlayer.GetRisk(table);
        }
        public string GetRiskNextPlayerCard()
        {
            return this.secondPlayer.CardPlayer.GetRisk(table);
        }
        public string GetCurrentPlayerCard()
        {
            return Service.ConvertCard(firstPlayer.CardPlayer);
        }
        public string GetNextPlayerCard()
        {
            return Service.ConvertCard(secondPlayer.CardPlayer);
        }


    }
}
