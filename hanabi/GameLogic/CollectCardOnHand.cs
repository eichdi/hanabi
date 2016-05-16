using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class CollectCardOnHand
    {
        private List<OnHandCard> card;
        public OnHandCard[] Card
        {
            get
            {
                return card.ToArray();
            }
        }

        public string GetRisk(GameTable table)
        {
            string result = "";
            foreach (OnHandCard card in this.card)
            {
                result += card.GetRisk(table) + " ";
            }
            return result;
        }
        public CollectCardOnHand(OnHandCard[] card)
        {
            //создаем копию, для того чтобы другая часть программы не имела ссылку на поле
            this.card = card.ToList();
        }
        public OnHandCard TakeCard(int index, OnHandCard introPack)
        {
            OnHandCard temp = this.card[index];
            card.RemoveAt(index);
            this.card.Add(introPack);
            return temp;
        }

    }
}
