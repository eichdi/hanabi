﻿using System;
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
        public Game(Player player1, Player player2, PackOfCard pack, GameTable table = null)
        {
            risk = 0;
            gamedCards = 0;
            state = true;
            this.firstPlayer = new InfoPlayer(player1, pack);
            this.secondPlayer = new InfoPlayer(player2, pack);
            if (table == null)
            {
                this.table = new GameTable();
            }
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

        private bool CanPlay(Player player)
        {
            if (player.ID == firstPlayer.player.ID && state)
            {
                return true;
            }
            return false;
        }
        private void DoNextPlayer()
        {
            InfoPlayer temp = firstPlayer;
            firstPlayer = secondPlayer;
            secondPlayer = temp;
        }
        public CollectCardOnHand GetOpponentCard(Player player)
        {
            if (CanPlay(player))
            {
                return secondPlayer.cardPlayer;
            }
            else
            {
                return null;
            }
        }
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
