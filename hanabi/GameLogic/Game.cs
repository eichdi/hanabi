using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class Game
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
                return firstPlayer.player;
            }
        }
        public Player NextPlayer
        {
            get
            {
                return secondPlayer.player;
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
            this.firstPlayer.player = player1;
            this.secondPlayer.player = player2;
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
                bool risked = firstPlayer.cardPlayer.Card[index].RiskCard(table);
                if (risked)
                {
                    bool a;
                    a = risked;
                }
                //кладем на стол карту игрока по индексу и за место нее ставим карту из колоды
                if (!table.PutCard(firstPlayer.cardPlayer.TakeCard(index, pack.TakeCard())))
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
                table.DropCard(firstPlayer.cardPlayer.TakeCard(index, pack.TakeCard()));
                DoNextPlayer();
            }
        }
        //3
        public void TellColor(Player player, string color, int[] index)
        {
            if (CanPlay(player))
            {
                for (int i = 0; i < index.Length; i++)
                {
                    //обращаемся к картам следующего игрока по индексу и говорим ему цвет
                    if (!secondPlayer.cardPlayer.Card[index[i]].CheckColor(color))
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
                    if (!secondPlayer.cardPlayer.Card[index[i]].CheckRank(rank))
                    {
                        EndGame();
                    }
                }
            }
            DoNextPlayer();
        }

        //Возможность игры для игрока
        private bool CanPlay(Player player)
        {
            if (player.ID == firstPlayer.player.ID && state)
            {
                return true;
            }
            return false;
        }

        public bool CheckPlayer(Player player)
        {
            return (firstPlayer.player == player || secondPlayer.player == player);
        }


        private void DoNextPlayer()
        {
            InfoPlayer temp = firstPlayer;
            firstPlayer = secondPlayer;
            secondPlayer = temp;
        }

        public CollectCardOnHand GetOpponentCard(Player player)
        {
            if(player == firstPlayer.player)
                return secondPlayer.cardPlayer;
            if (player == secondPlayer.player)
                return firstPlayer.cardPlayer;
            return null;

        }



        //возможный функционал
        public string GetRiskCurrentPlayerCard()
        {
            return this.firstPlayer.cardPlayer.GetRisk(table);
        }
        public string GetRiskNextPlayerCard()
        {
            return this.secondPlayer.cardPlayer.GetRisk(table);
        }
        public string GetCurrentPlayerCard()
        {
            return Service.ConvertCard(firstPlayer.cardPlayer);
        }
        public string GetNextPlayerCard()
        {
            return Service.ConvertCard(secondPlayer.cardPlayer);
        }


    }
}
