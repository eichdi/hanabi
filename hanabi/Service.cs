using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hanabi.GameLogic;
namespace hanabi
{
    class Service
    {
        protected static int id = 0;


        public static int GetID
        {
            get
            {
                id++;
                return id;
            }
        }

		public static int GetRank(string command) {
			int result = int.Parse(command.Split()[2]);
			if (result >= 1 && result <= 5)
				return int.Parse(command.Split()[2]);
			else
				throw new ArgumentException("Неизвестная комманда. Попробуйте еще раз");
		}

        public static string GetColor(string command)
        {
			//// какашка которая делает так что в команде нет двух цветов
			//if(!((command.Contains(Card.Red) && (command.Contains(Card.Blue) || command.Contains(Card.Green) || command.Contains(Card.White) || command.Contains(Card.Yellow))) ||
			//   (command.Contains(Card.Blue) && (command.Contains(Card.Red) || command.Contains(Card.Green) || command.Contains(Card.White) || command.Contains(Card.Yellow))) ||
			//   (command.Contains(Card.Green) && (command.Contains(Card.Red) || command.Contains(Card.Blue) || command.Contains(Card.White) || command.Contains(Card.Yellow))) ||
			//   (command.Contains(Card.White) && (command.Contains(Card.Red) || command.Contains(Card.Blue) || command.Contains(Card.Green) || command.Contains(Card.Yellow))) ||
			//   (command.Contains(Card.Yellow) && (command.Contains(Card.Red) || command.Contains(Card.Blue) || command.Contains(Card.Green) || command.Contains(Card.White)))))
			//{
			//    if(command.Contains(Card.Red))
			//        return Card.Red;
			//    if (command.Contains(Card.Blue))
			//        return Card.Blue;
			//    if (command.Contains(Card.Green))
			//        return Card.Green;
			//    if (command.Contains(Card.White))
			//        return Card.White;
			//    if (command.Contains(Card.White))
			//        return Card.White;
			//}
			//return null;
			string result = command.Split()[2];
			if (result == "r" || result == "b" || result == "g" || result == "w" || result == "y")
				return result;
			else
				throw new ArgumentException("Неизвестная комманда. Попробуйте еще раз");
		}

        public static string ConvertColor(string color)
        {
			switch (color.ToLower()) {
				case "r":
					return Card.Red;
				case "y":
					return Card.Yellow;
				case "b":
					return Card.Blue;
				case "w":
					return Card.White;
				case "g":
					return Card.Green;
				default:
					throw new ArgumentException();
			}
            //return null;

        }

        public static string ConvertColor(char color)
        {
            return ConvertColor(color.ToString());
        }

        public static string ConvertCard(CollectCardOnHand cards)
        {
            return ConvertCard(cards.Card);
        }

        public static string ConvertCard(Card[] cards)
        {
            string result = "";
            foreach (Card item in cards)
            {
                result += item.ToString() + " ";
            }
            return result;
        }

        public static int GetCard(string nameCommand, string command)
        {
            return int.Parse(command.Substring(nameCommand.Length));
        }

        public static string[] GetCards(string nameCommand, string command)
        {
            return command.Substring(nameCommand.Length).Split(' ');
        }

        public static int[] GetIndexCards(string command)
        {
			string[] sresult;
			if (command.Contains("tell color"))
				sresult = command.Substring("tell color ".Length).Split();
			else if (command.Contains("tell rank"))
				sresult = command.Substring("tell rank ".Length).Split();
			else
				sresult = new string[] { "empty" };

			int[] result = new int[sresult.Length - 1];
			for (int i = 1; i < sresult.Length; i++) {  //sresult[0] contains color or keyword "empty"
				result[i - 1] = int.Parse(sresult[i]);
			}
			return result;
		}
    }
}
