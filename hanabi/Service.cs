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
    }
}
