﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class Player
    {
        private long id;
        private Game game;
        public bool State
        {
            get
            {
                return game != null && game.State;
            }
        }
        public long ID
        {
            get
            {
                return id;
            }
        }
        public Player(Game game, long ID)
        {
            this.game = game;
            //уникальное значение
            this.id = ID;
        }
        public Player(long ID)
        {
            this.id = ID;
        }
        public bool SetGame(Game game)
        {
            return game.CheckPlayer(this);
        }
        public string GetOpponentCard()
        {
            return Service.ConvertCard(game.GetOpponentCard(this));
        }
        public string GetTableCard()
        {
            return game.GetTableCard();
        }

        //так то пользователь должен помнить свои карты сам
        //public string GetMyCard() { 
        //
        //}


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
