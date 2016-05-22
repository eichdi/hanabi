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

        public static string GetColor(string command)
        {
            // какашка которая делает так что в команде нет двух цветов
            if(!((command.Contains(Card.Red) && (command.Contains(Card.Blue) || command.Contains(Card.Green) || command.Contains(Card.White) || command.Contains(Card.Yellow))) ||
               (command.Contains(Card.Blue) && (command.Contains(Card.Red) || command.Contains(Card.Green) || command.Contains(Card.White) || command.Contains(Card.Yellow))) ||
               (command.Contains(Card.Green) && (command.Contains(Card.Red) || command.Contains(Card.Blue) || command.Contains(Card.White) || command.Contains(Card.Yellow))) ||
               (command.Contains(Card.White) && (command.Contains(Card.Red) || command.Contains(Card.Blue) || command.Contains(Card.Green) || command.Contains(Card.Yellow))) ||
               (command.Contains(Card.Yellow) && (command.Contains(Card.Red) || command.Contains(Card.Blue) || command.Contains(Card.Green) || command.Contains(Card.White)))))
            {
                if(command.Contains(Card.Red))
                    return Card.Red;
                if (command.Contains(Card.Blue))
                    return Card.Blue;
                if (command.Contains(Card.Green))
                    return Card.Green;
                if (command.Contains(Card.White))
                    return Card.White;
                if (command.Contains(Card.White))
                    return Card.White;
            }
            return null;
        }

        public static string ConvertColor(string color)
        {
            switch (color)
            {
                case "R":
                    return Card.Red;
                case "Y":
                    return Card.Yellow;
                case "B":
                    return Card.Blue;
                case "W":
                    return Card.White;
                case "G":
                    return Card.Green;
            }
            throw new ArgumentException();
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
        public static string GetCard(string nameCommand, string command)
        {
            return command.Substring(nameCommand.Length);
        }
        public static string[] GetCards(string nameCommand, string command)
        {
            return command.Substring(nameCommand.Length).Split(' ');
        }
        public static string[] GetIndexCards(string command)
        {

            foreach (var word in command)
            {
                

            }
            return null; // TODO: GetIndexCards
        }
    }
}
