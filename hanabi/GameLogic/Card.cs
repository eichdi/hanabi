using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class Card
    {

        //public const string Red = "Red";
        //public const string Yellow = "Yellow";
        //public const string Blue = "Blue";
        //public const string White = "White";
        //public const string Green = "Green";        
        public const string Red = "R";
        public const string Yellow = "Y";
        public const string Blue = "B";
        public const string White = "W";
        public const string Green = "G";
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
