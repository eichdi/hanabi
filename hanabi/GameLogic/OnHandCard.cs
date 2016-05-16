using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class OnHandCard : Card
    {
        private bool hasInfoColor = false;
        private bool hasInfoRank = false;

        public string GetRisk(GameTable table)
        {
            string result = "";
            if (hasInfoColor == true)
            {
                result += "1";
            }
            else
            {
                result += "0";
            }
            if (table.CanPutRank(this.Rank) && hasInfoRank)
            {
                //зная ранк и все карты на столе можно догадаться что ты можешь положить эту карту в любом случае
                result += "T";
            }
            else
            {
                if (hasInfoRank == true)
                {
                    result += "1";
                }
                else
                {
                    result += "0";
                }

            }

            return result;
        }
        public bool RiskCard(GameTable table)
        {
            bool IsRisk;
            IsRisk = hasInfoRank && hasInfoColor;
            IsRisk = IsRisk || hasInfoRank && table.CanPutRank(this.Rank);
            return !IsRisk;
        }
        public bool CheckColor(string color)
        {
            if (this.Color == color)
            {
                hasInfoColor = true;
                return true;
            }
            else
            {
                //неверное утверждение о цвете
                return false;
            }
        }
        public bool CheckRank(int rank)
        {
            if (this.Rank == rank)
            {

                hasInfoRank = true;
                return true;
            }
            else
            {
                //неверное утверждение о весе
                return false;
            }
        }
        public OnHandCard(Card card)
            : base(card.Color, card.Rank)
        {
        }
        public OnHandCard(string color, int rank)
            : base(color, rank)
        {
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
