using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hanabi.GameLogic;
using Telegram.Bot;
using hanabi.PlayerBase;

namespace hanabi.Controller
{
    class Command
    {
        private int turn;
        private Player waitPlayer;
        private Api bot;
        private IPlayerBase playerBase;
        public string Instruction = "123";

        public void StartGame(long id)
        {
            Player player = playerBase.GetPlayer(id);

            if (waitPlayer != null && waitPlayer.ID != id)
            {
                var game = new Game(player, waitPlayer);
                bot.SendTextMessage(player.ID, "Игра началась \n " + Instruction);
                ShowInfo(player);
                bot.SendTextMessage(waitPlayer.ID, "Игра началась \n" + Instruction);
                ShowInfo(waitPlayer);
                waitPlayer = null;
            }
            else
            {
                waitPlayer = player;
                bot.SendTextMessage(player.ID, "Подождите пока мы не найдем второго игрока");
            }
        }

        private void ShowInfo(Player player)
        {
            if (player.State)
            {
                bot.SendTextMessage(player.ID, player.GetOpponentCard() + player.GetTableCard());   
            }
            else
            {
                bot.SendTextMessage(player.ID, "Игра закончена!");
            }
        }

        private void ShowInfo(long id)
        {
            ShowInfo(playerBase.GetPlayer(id));
        }

        public void PlayCard(string sindex, long id)
        {
            try
            {
                int index = int.Parse(sindex);
                //game.CurrentPlayer.PlayCard(index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void DropCard(string sindex, long id)
        {
            try
            {
                int index = int.Parse(sindex);
                //game.CurrentPlayer.Drop(index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void TellColor(string color, string[] cards, long id)
        {
            try
            {
                int[] index = new int[cards.Length];
                for (int i = 0; i < cards.Length; i++)
                {
                    index[i] = int.Parse(cards[i]);
                }
                //game.CurrentPlayer.TellColor(color, index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void TellRank(string srank, string[] cards, long id)
        {
            try
            {
                int rank = int.Parse(srank);
                int[] index = new int[cards.Length];
                for (int i = 0; i < cards.Length; i++)
                {
                    index[i] = int.Parse(cards[i]);
                }
                //game.CurrentPlayer.TellRank(rank, index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }

        public Command(string token)
        {
            //bot = new Api(token);
            bot = new Api("225115203:AAH_vGJDopLajGzNSK16YkQLjGBCZzVUT10");
            playerBase = new PlayerBase.PlayerBase();
            bot.StopReceiving();
            bot.StartReceiving();
            ListenMessege().Wait();
        }

        public async Task ListenMessege()
        {
            var me = bot.GetMe();
            Console.WriteLine("I listen!");
            var offset = 0;

            while (true)
            {
                var updates = await bot.GetUpdates();
                foreach (var update in updates)
                {
                    if (update.Message.Type == Telegram.Bot.Types.MessageType.TextMessage)
                    {
                        MakeMove(update.Message.Text, update.Message.Chat.Id);
                    }

                    offset = update.Id + 1;
                }
            }

        }


        public bool MakeMove(string command, long id)
        {
            turn++;
            if (command.Contains("Start"))
            {
				StartGame(id);
                return true;
            }
            if (command.Contains("Play card "))
            {
                PlayCard(Service.GetCard("Play card ", command),id);
                return true;
            }
            if (command.Contains("Tell rank "))
            {
                string[] analize = command.Split(' ');
                TellRank(analize[2], Service.GetCards(("Tell rank " + analize[2] + " for cards "), command),id);
                return true;
            }
            if (command.Contains("Tell color "))
            {
                string[] analize = command.Split(' ');
                TellColor(analize[2], Service.GetCards(("Tell color " + analize[2] + " for cards "), command),id);
                return true;
            }
            if (command.Contains("Drop card "))
            {
                DropCard(Service.GetCard("Drop card ", command),id);
                return true;
            }
            return false;
        }

        public string GetResultGame()
        {
            //if (this.State == false)
            //{
            //    return "Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() ;
            //}
            //else return null;
            return null;
        }
        public string GetResult(string command)
        {
            //if (this.game != null)
            //{
            //    string result = "";
            //    result += "\n Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() + ", with risk: " + game.Risk.ToString() + ", state: " + this.State.ToString();
            //    result += "\n" + "  Current player: " + this.game.GetCurrentPlayerCard();
            //    result += "\n" + "                  " + this.game.GetRiskCurrentPlayerCard();
            //    result += "\n" + "     Next player: " + this.game.GetNextPlayerCard();
            //    result += "\n" + "                  " + this.game.GetRiskNextPlayerCard();
            //    result += "\n" + "           Table: " + this.game.GetTableCard();
            //    result += "\n" + command;
            //    return result;
            //}
            //else
            //{
            //    return command;
            //}
            return null;
        }
    }
}
