using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hanabi.GameLogic
{
    class GameTable
    {
        private Dictionary<string, Card> heap;
        private int amount;
        public GameTable()
        {
            heap = new Dictionary<string, Card>();
            //нулевые карты не имеющие веса
            heap.Add(Card.Blue, new Card(Card.Blue, 0));
            heap.Add(Card.Green, new Card(Card.Green, 0));
            heap.Add(Card.Red, new Card(Card.Red, 0));
            heap.Add(Card.White, new Card(Card.White, 0));
            heap.Add(Card.Yellow, new Card(Card.Yellow, 0));
            heap.Add("drop", null);
            amount = 0;

        }
        public bool PutCard(Card card)
        {
            if (amount < 25)
            {
                if (card != null)
                {
                    if ((heap[card.Color].Rank + 1) == card.Rank)
                    {
                        heap[card.Color] = card;
                        amount++;
                        return true;
                    }
                    else
                    {
                        //возвратит false, игра закончится
                        heap["drop"] = card;
                    }
                }
            }
            return false;
        }
        public bool DropCard(Card card)
        {
            if (card != null)
            {
                heap["drop"] = card;
                return true;
            }
            return false;
        }

        public Dictionary<string, Card> GetHeap()
        {
            return new Dictionary<string, Card>(heap);
        }
        public bool CanPutRank(int rank)
        {
            int tempRank = rank - 1;
            return
                   heap[Card.Blue].Rank == tempRank
                && heap[Card.Red].Rank == tempRank
                && heap[Card.Green].Rank == tempRank
                && heap[Card.White].Rank == tempRank
                && heap[Card.Yellow].Rank == tempRank;

        }
        public string GetTableCard()
        {
            return heap[Card.Red].ToString() + " "
                + heap[Card.Green].ToString() + " "
                + heap[Card.Blue].ToString() + " "
                + heap[Card.White].ToString() + " "
                + heap[Card.Yellow].ToString();
        }
    }
}
