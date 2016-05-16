using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class PackOfCard
    {
        private Queue<Card> card;

        //возможность взять карту
        public bool State
        {
            get
            {
                return card.Count != 0;
            }
        }
        public PackOfCard(Card[] card)
        {
            this.card = new Queue<Card>(card);
        }
        //Массив информации о картах вида {R5,G1,G2,G3,G4,G5,Y1}
        public PackOfCard(string[] infoCard)
        {

            this.card = new Queue<Card>();
            foreach (string info in infoCard)
            {
                card.Enqueue(new Card(info));
            }

        }
        public OnHandCard TakeCard()
        {
            if (card.Count != 0)
            {
                return new OnHandCard(card.Dequeue());
            }
            else
            {
                return null;
            }
        }
    }
}
