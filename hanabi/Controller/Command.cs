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
        private Player waitPlayer;
        private Api bot;
        private IPlayerBase playerBase;
        // TODO: Написать инструкцию к игре
        public string Instruction;

        public void StartGame(long id)
        {
            Player player = playerBase.GetPlayer(id);

            if (waitPlayer != null && waitPlayer.ID != id)
            {
                var game = new Game(player, waitPlayer);

                Respond(player.ID, "Игра началась \n " + Instruction).Wait();
                Respond(waitPlayer.ID, "Игра началась \n " + Instruction).Wait();
                waitPlayer = null;
            }
            else
            {
                waitPlayer = player;
                Respond(player.ID, "Подождите пока мы не найдем второго игрока").Wait();
            }
        }





        private void ShowInfo(Player player)
        {
            //Условие при котором ходит наш игрок
            // TODO: Не правильно показывается карты нанешнего игрока, может проблема в  GetKnownCards
            if (player.State)
            {
                Respond(player.ID, "Карты второго игрока: " + player.GetOpponentCard() + "\nКарты на столе: " + player.GetTableCard() +
                    "\nВаши карты:" + player.GetKnownCards()).Wait();
                Respond(player.ID, "Ваш ход").Wait();

                Respond(player.GetOpponent().ID, "Карты второго игрока: " + player.GetOpponent().GetOpponentCard() + "\nКарты на столе: " + player.GetOpponent().GetTableCard() +
                "\nВаши карты:" + player.GetOpponent().GetKnownCards()).Wait();
                Respond(player.GetOpponent().ID, "Ход вашего оппонента").Wait();
            }
            else
            {
                if (player.PlayerGame != null)
                {
                    if (player.PlayerGame.State)
                    {
                        Respond(player.ID, "Карты второго игрока: " + player.GetOpponentCard() + "\nКарты на столе: " + player.GetTableCard() +
                        "\nВаши карты:" + player.GetKnownCards()).Wait();
                        Respond(player.ID, "Ход вашего оппонента").Wait();

                        Respond(player.GetOpponent().ID, "Карты второго игрока: " + player.GetOpponent().GetOpponentCard() + "\nКарты на столе: " + player.GetOpponent().GetTableCard() +
                        "\nВаши карты:" + player.GetOpponent().GetKnownCards()).Wait();
                        Respond(player.GetOpponent().ID, "Ваш ход").Wait();
                    }
                    else //Случай при котором игра закончена 
                    {
                        Respond(player.ID, "Игра закончена!" + Instruction).Wait();
                        Respond(player.GetOpponent().ID, "Игра закончена!" + Instruction).Wait();
                        player.GetOpponent().ExitGame();
                        player.ExitGame();
                    }
                }
                //Условие при котором игра не закончена и не ходит наш игрок
            }
        }


        public void PlayCard(int index, Player player)
        {
            player.PlayCard(index);
        }

        public void DropCard(int index, Player player)
        {
            player.Drop(index);
        }

        public void TellColor(string color, int[] cards, Player player)
        {
            player.TellColor(color, cards);
        }

        public void TellRank(int rank, int[] cards, Player player)
        {
            player.TellRank(rank, cards);
        }

        public Command(string token)
        {

            Instruction = new System.IO.StreamReader("instruction.txt", Encoding.UTF8).ReadToEnd();
            bot = new Api(token);
            playerBase = new PlayerBase.PlayerBase();
            bot.StopReceiving();
            bot.StartReceiving();
            ListenMessege().Wait();
        }


        public async Task Respond(long chatId, string text)
        {
            Console.WriteLine(chatId.ToString() + " send this - " + text);
            await bot.SendTextMessage(chatId, text);
        }



        public async Task ListenMessege()
        {
            //var me = bot.GetMe();
            Console.WriteLine("Ready to work");
            var offset = 0;

            while (true)
            {
                var updates = await bot.GetUpdates(offset);

                foreach (var update in updates)
                {
                    if (update.Message.Type == Telegram.Bot.Types.MessageType.TextMessage)
                    {
                        Console.WriteLine(update.Message.Text + " from " + update.Message.From.FirstName + " " + update.Message.Chat.Id);
                        try
                        {

                            MakeMove(update.Message.Text, update.Message.Chat.Id);

                        }
                        catch (Exception e)
                        {
                            Respond(update.Message.Chat.Id, e.Message).Wait();
                        }
                    }

                    offset = update.Id + 1;
                }
            }
        }


        public bool MakeMove(string command, long id)
        {
            command = command.ToLower();
            Player player = playerBase.GetPlayer(id);
            if (command == "start")
            {
                StartGame(id);
            }
            else if (player.State)
            {
                if (command.Contains("play card"))
                {
                    PlayCard(Service.GetCard("play card", command), player);
                }
                else if (command.Contains("tell rank"))
                {
                    int rank = Service.GetRank(command);
                    TellRank(rank, Service.GetIndexCards(command), player);
                }
                else if (command.Contains("tell color"))
                {
                    string color = Service.GetColor(command);
                    TellColor(color, Service.GetIndexCards(command), player);
                }
                else if (command.Contains("drop"))
                {
                    DropCard(Service.GetCard("drop", command), player);
                }
                else
                {
                    Respond(player.ID, "Неизвестная комманда. Попробуйте еще раз").Wait();
                }
            }
            else
                throw new ArgumentException("Неизвестная комманда. Попробуйте еще раз");
            ShowInfo(player);
            return true;
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
