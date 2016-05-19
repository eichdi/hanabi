using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hanabi.GameLogic;

namespace hanabi.Controller
{
    class Command
    {
        public Game game;
        private int turn;
        private int level;
        public bool State
        {
            get
            {
                return game != null && game.State;
            }
        }
        public void StartGame(string[] card, long id1, long id2)
        {
            turn = 0;
            Player player1 = new Player(id1);
            Player player2 = new Player(id2);
            PackOfCard pack = new PackOfCard(card);
            game = new Game(player1, player2, pack);
        }


        public void PlayCard(string sindex)
        {
            try
            {
                int index = int.Parse(sindex);
                game.CurrentPlayer.PlayCard(index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void DropCard(string sindex)
        {
            try
            {
                int index = int.Parse(sindex);
                game.CurrentPlayer.Drop(index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void TellColor(string color, string[] cards)
        {
            try
            {
                int[] index = new int[cards.Length];
                for (int i = 0; i < cards.Length; i++)
                {
                    index[i] = int.Parse(cards[i]);
                }
                game.CurrentPlayer.TellColor(color, index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void TellRank(string srank, string[] cards)
        {
            try
            {
                int rank = int.Parse(srank);
                int[] index = new int[cards.Length];
                for (int i = 0; i < cards.Length; i++)
                {
                    index[i] = int.Parse(cards[i]);
                }
                game.CurrentPlayer.TellRank(rank, index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }

        public Command(int level)
        {
            this.level = level;
        }


        public bool DoFunc(string command)
        {
            if (true)
            {
                turn++;
                if (command.Contains("Start new game with deck "))
                {
                    //временное решение
                    // TODO : пересобрать эту часть кода
                    StartGame(Service.GetCards("Start new game with deck ", command),0,1);
                    return true;
                }
                if (command.Contains("Play card "))
                {
                    PlayCard(Service.GetCard("Play card ", command));
                    return true;
                }
                if (command.Contains("Tell rank "))
                {
                    string[] analize = command.Split(' ');
                    TellRank(analize[2], Service.GetCards(("Tell rank " + analize[2] + " for cards "), command));
                    return true;
                }
                if (command.Contains("Tell color "))
                {
                    string[] analize = command.Split(' ');
                    TellColor(analize[2], Service.GetCards(("Tell color " + analize[2] + " for cards "), command));
                    return true;
                }
                if (command.Contains("Drop card "))
                {
                    DropCard(Service.GetCard("Drop card ", command));
                    return true;
                }
            }
            return false;
        }

        public string GetResultGame()
        {
            if (this.State == false)
            {
                if (level == 2)
                {
                    return "Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() + ", with risk: " + game.Risk.ToString();
                }
                else
                {
                    return "Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() + ", with risk: " + "0";
                }
            }
            else return null;
        }
        public string GetResult(string command)
        {
            if (this.game != null)
            {
                string result = "";
                result += "\n Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() + ", with risk: " + game.Risk.ToString() + ", state: " + this.State.ToString();
                result += "\n" + "  Current player: " + this.game.GetCurrentPlayerCard();
                result += "\n" + "                  " + this.game.GetRiskCurrentPlayerCard();
                result += "\n" + "     Next player: " + this.game.GetNextPlayerCard();
                result += "\n" + "                  " + this.game.GetRiskNextPlayerCard();
                result += "\n" + "           Table: " + this.game.GetTableCard();
                result += "\n" + command;
                return result;
            }
            else
            {
                return command;
            }
        }



    }
}
