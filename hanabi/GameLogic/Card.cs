using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class Card
    {
        public static string Red = "Red";
        public static string Yellow = "Yellow";
        public static string Blue = "Blue";
        public static string White = "White";
        public static string Green = "Green";
        private string color;
        private int rank;
        public string Color
        {
            get
            {
                return color;
            }
        }
        public int Rank
        {
            get
            {
                return rank;
            }
        }



        //Подается информация в виде цвет и вес карты. Пример - R1
        public Card(string info)
        {
            try
            {
                this.color = Service.ConvertColor(info[0]);
                this.rank = int.Parse(info[1].ToString());
            }
            catch
            {
                throw new ArgumentException();
            }

        }
        public Card(string color, int rank)
        {
            this.color = color;
            this.rank = rank;
        }
        public override string ToString()
        {
            return this.Color[0].ToString() + this.Rank.ToString();
        }
    }
}
