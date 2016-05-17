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
        public static PackOfCard GetRandomCard()
        {
            //пока так, написать функцию для того чтобы генерить infocard
            string[] infocard = "Y4 R4 Y1 W4 W3 W4 W5 W2 R1 Y1 B1 G3 G2 B5 R2 G3 Y4 R3 B2 B2 W1 W2 W1 B3 Y3 G4 G1 Y5 G2 R5 R1 Y1 R2 Y3 G4 Y2 B4 R3 R4 W1 G5 B3 B1 G1 G1 B1 B4 Y2 W3 R1".Split(' ');
            List<Card> cardList = new List<Card>();
            foreach (var item in infocard)
            {
                cardList.Add(new Card(item));
            }
            return new PackOfCard(cardList.ToArray());
        }
    }
}
