using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hanabi.GameLogic;
using Telegram.Bot;
using hanabi.PlayerBase;

namespace hanabi.Controller {
	class Command {
		private Player waitPlayer;
		private Api bot;
		private IPlayerBase playerBase;
		public string Instruction = "123";

		public void StartGame(long id) {
			Player player = playerBase.GetPlayer(id);

			if (waitPlayer != null && waitPlayer.ID != id) {
				var game = new Game(player, waitPlayer);

				Respond(player.ID, "Игра началась \n " + Instruction).Wait();
				ShowInfo(player);

				Respond(waitPlayer.ID, "Игра началась \n " + Instruction).Wait();
				ShowInfo(waitPlayer);

				waitPlayer = null;
			} else {
				waitPlayer = player;
				Respond(player.ID, "Подождите пока мы не найдем второго игрока").Wait();
			}
		}

		private void ShowInfo(Player player) {
			if (player.State) {
				//string knownCards = player.
				Respond(player.ID, "Карты второго игрока: " + player.GetOpponentCard() + "\nКарты на столе: " + player.GetTableCard() +
					"\nВаши карты:" + player.GetKnownCards()).Wait();
			} else {
				Respond(player.ID, "Игра закончена!").Wait();
			}
		}

		private void ShowInfo(long id) {
			ShowInfo(playerBase.GetPlayer(id));
		}

		public void PlayCard(int index, long id) {
			playerBase.GetPlayer(id).PlayCard(index);
			//game.CurrentPlayer.PlayCard(index);
		}

		public void DropCard(int index, long id) {
			playerBase.GetPlayer(id).Drop(index);
			//game.CurrentPlayer.Drop(index);
		}
		public void TellColor(string color, int[] cards, long id) {
			playerBase.GetPlayer(id).TellColor(color, cards);
			//game.CurrentPlayer.TellColor(color, index);
		}
		public void TellRank(int rank, int[] cards, long id) {
			playerBase.GetPlayer(id).TellRank(rank, cards);
			//game.CurrentPlayer.TellRank(rank, index);
		}

		public Command(string token) {
			bot = new Api(token);
			playerBase = new PlayerBase.PlayerBase();
			bot.StopReceiving();
			bot.StartReceiving();
			ListenMessege().Wait();
		}

		public async Task Respond(long chatId, string text) {
			Console.WriteLine(chatId.ToString() + " send this - " + text);
			await bot.SendTextMessage(chatId, text);
		}
		public async Task ListenMessege() {
			//var me = bot.GetMe();
			Console.WriteLine("Ready to work");

			while (true) {
				var updates = await bot.GetUpdates();
				foreach (var update in updates) {
					if (update.Message.Type == Telegram.Bot.Types.MessageType.TextMessage) {
						Console.WriteLine(update.Message.Text + " from " + update.Message.From.FirstName + " " + update.Message.Chat.Id);
						try {

							MakeMove(update.Message.Text, update.Message.Chat.Id);
							
						} catch (Exception e) {
							await Respond(update.Message.Chat.Id, e.Message);
						}
					}
				}
			}
		}


		public bool MakeMove(string command, long id) {
			command = command.ToLower();
			if (command == "start") {
				StartGame(id);
			} else if (command.Contains("play card")) {
				PlayCard(Service.GetCard("play card", command), id);
			} else if (command.Contains("tell rank")) {
				int rank = Service.GetRank(command);
				TellRank(rank, Service.GetIndexCards(command), id);
			} else if (command.Contains("tell color")) {
				string color = Service.GetColor(command);
				TellColor(color, Service.GetIndexCards(command), id);
			} else if (command.Contains("drop")) {
				DropCard(Service.GetCard("drop", command), id);
			} else
				throw new ArgumentException("Неизвестная комманда. Попробуйте еще раз");
			Player player = playerBase.GetPlayer(id);
			ShowInfo(player);
			ShowInfo(player.GetOpponent());
			return true;
		}

		public string GetResultGame() {
			//if (this.State == false)
			//{
			//    return "Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() ;
			//}
			//else return null;
			return null;
		}
		public string GetResult(string command) {
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
