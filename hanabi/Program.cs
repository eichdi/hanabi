﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace hanabi
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
                if(hasInfoRank == true)
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
    class InfoPlayer
    {
        public CollectCardOnHand cardPlayer;
        public Player player;
        public InfoPlayer(Player player, PackOfCard pack)
        {
            //раздаем карты
            OnHandCard[] card = new OnHandCard[5];
            for (int i = 0; i < 5; i++)
            {
                card[i] = pack.TakeCard();
            }
            cardPlayer = new CollectCardOnHand(card);
            this.player = player;

        }
    }
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
            int tempRank = rank -1;
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
    class Game
    {
        private int risk;
        private int gamedCards;
        private InfoPlayer firstPlayer;
        private InfoPlayer secondPlayer;
        private PackOfCard pack;
        private GameTable table;
        public Player CurrentPlayer
        {
            get
            {
                return firstPlayer.player;
            }
        }
        public Player NextPlayer
        {
            get
            {
                return secondPlayer.player;
            }
        }
        //true игра продолжается
        private bool state;
        public int GamedCards
        {
            get
            {
                return gamedCards;
            }
        }
        public bool State
        {
            get
            {
                return state && pack.State;
            }
        }
        public int Risk
        {
            get
            {
                return risk;
            }
        }
        public string GetTableCard()
        {
            return table.GetTableCard();
        }
        private void EndGame()
        {
            state = false;
        }
        public Game(Player player1, Player player2, PackOfCard pack, GameTable table = null)
        {
            risk = 0;
            gamedCards = 0;
            state = true;
            this.firstPlayer = new InfoPlayer(player1, pack);
            this.secondPlayer = new InfoPlayer(player2, pack);
            if (table == null)
            {
                this.table = new GameTable();
            }
            this.pack = pack;
        }
        //1
        public void PlayCard(Player player, int index)
        {
            if (CanPlay(player))
            {
                //проверяем является ли карта первого игрока по индексу рискованной в зависимости еще от стола
                bool risked = firstPlayer.cardPlayer.Card[index].RiskCard(table);
                if (risked)
                {
                    bool a;
                    a = risked;
                }
                //кладем на стол карту игрока по индексу и за место нее ставим карту из колоды
                if (!table.PutCard(firstPlayer.cardPlayer.TakeCard(index, pack.TakeCard())))
                {
                    EndGame();
                }
                else
                {
                    //если риск оправдан, то увеличиваем количество рискованных ходов
                    if (risked)
                    {
                        risk++;
                    }
                    gamedCards++;
                }
                DoNextPlayer();
            }

        }
        //2
        public void DropCard(Player player, int index)
        {
            if (CanPlay(player))
            {
                table.DropCard(firstPlayer.cardPlayer.TakeCard(index, pack.TakeCard()));
                DoNextPlayer();
            }
        }
        //3
        public void TellColor(Player player, string color, int[] index)
        {
            if (CanPlay(player))
            {
                for (int i = 0; i < index.Length; i++)
                {
                    //обращаемся к картам следующего игрока по индексу и говорим ему цвет
                    if (!secondPlayer.cardPlayer.Card[index[i]].CheckColor(color))
                    {
                        EndGame();
                    }
                }
            }
            DoNextPlayer();
        }
        //4
        public void TellRank(Player player, int rank, int[] index)
        {
            if (CanPlay(player))
            {
                for (int i = 0; i < index.Length; i++)
                {
                    //обращаемся к картам следующего игрока по индексу и говорим ему ранк
                    if (!secondPlayer.cardPlayer.Card[index[i]].CheckRank(rank))
                    {
                        EndGame();
                    }
                }
            }
            DoNextPlayer();
        }

        private bool CanPlay(Player player)
        {
            if (player.ID == firstPlayer.player.ID && state)
            {
                return true;
            }
            return false;
        }
        private void DoNextPlayer()
        {
            InfoPlayer temp = firstPlayer;
            firstPlayer = secondPlayer;
            secondPlayer = temp;
        }
        public CollectCardOnHand GetOpponentCard(Player player)
        {
            if (CanPlay(player))
            {
                return secondPlayer.cardPlayer;
            }
            else
            {
                return null;
            }
        }
        public string GetRiskCurrentPlayerCard()
        {
            return this.firstPlayer.cardPlayer.GetRisk(table);
        }
        public string GetRiskNextPlayerCard()
        {
            return this.secondPlayer.cardPlayer.GetRisk(table);
        }

        public string GetCurrentPlayerCard()
        {
            return Service.ConvertCard(firstPlayer.cardPlayer);
        }
        public string GetNextPlayerCard()
        {
            return Service.ConvertCard(secondPlayer.cardPlayer);
        }


    }
    class Player
    {
        //Для возможной игры через сеть
        private int id;
        public Game game;
        public int ID
        {
            get
            {
                return id;
            }
        }
        public Player(Game game)
        {
            this.game = game;
            //уникальное значение
            this.id = Service.GetID;
        }
        public string GetOpponentCard()
        {
            return Service.ConvertCard(game.GetOpponentCard(this));
        }
        public void PlayCard(int index)
        {
            game.PlayCard(this, index);
        }
        public void Drop(int index)
        {
            game.DropCard(this, index);
        }
        public void TellColor(string color, int[] index)
        {
            game.TellColor(this, color, index);
        }
        public void TellRank(int rank, int[] index)
        {
            game.TellRank(this, rank, index);
        }
    }
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

    class Command
    {
        public Game game;
        private int turn;
        private int level;
        public bool State
        {
            get
            {
                return game != null && game.State;
            }
        }
        public void StartGame(string[] card)
        {
            turn = 0;
            Player player1 = new Player(null);
            Player player2 = new Player(null);
            PackOfCard pack = new PackOfCard(card);
            game = new Game(player1, player2, pack);
            player1.game = game;
            player2.game = game;
        }
        public void PlayCard(string sindex)
        {
            try
            {
                int index = int.Parse(sindex);
                game.CurrentPlayer.PlayCard(index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void DropCard(string sindex)
        {
            try
            {
                int index = int.Parse(sindex);
                game.CurrentPlayer.Drop(index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void TellColor(string color, string[] cards)
        {
            try
            {
                int[] index = new int[cards.Length];
                for (int i = 0; i < cards.Length; i++)
                {
                    index[i] = int.Parse(cards[i]);
                }
                game.CurrentPlayer.TellColor(color, index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }
        public void TellRank(string srank, string[] cards)
        {
            try
            {
                int rank = int.Parse(srank);
                int[] index = new int[cards.Length];
                for (int i = 0; i < cards.Length; i++)
                {
                    index[i] = int.Parse(cards[i]);
                }
                game.CurrentPlayer.TellRank(rank, index);
            }
            catch
            {
                throw new Exception("input exception");
            }
        }

        public Command(int level)
        {
            this.level = level;
        }


        public bool DoFunc(string command)
        {
            if (true)
            {
                turn++;
                if (command.Contains("Start new game with deck "))
                {
                    StartGame(Service.GetCards("Start new game with deck ", command));
                    return true;
                }
                if (command.Contains("Play card "))
                {
                    PlayCard(Service.GetCard("Play card ", command));
                    return true;
                }
                if (command.Contains("Tell rank "))
                {
                    string[] analize = command.Split(' ');
                    TellRank(analize[2], Service.GetCards(("Tell rank " + analize[2] + " for cards "), command));
                    return true;
                }
                if (command.Contains("Tell color "))
                {
                    string[] analize = command.Split(' ');
                    TellColor(analize[2], Service.GetCards(("Tell color " + analize[2] + " for cards "), command));
                    return true;
                }
                if (command.Contains("Drop card "))
                {
                    DropCard(Service.GetCard("Drop card ", command));
                    return true;
                }
            }
            return false;
        }

        public string GetResultGame()
        {
            if (this.State == false)
            {
                if (level == 2)
                {
                    return "Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() + ", with risk: " + game.Risk.ToString();
                }
                else
                {
                    return "Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() + ", with risk: " + "0";
                }
            }
            else return null;
        }
        public string GetResult(string command)
        {
            if (this.game != null)
            {
                string result = "";
                result += "\n Turn: " + this.turn.ToString() + ", cards: " + game.GamedCards.ToString() + ", with risk: " + game.Risk.ToString() + ", state: " + this.State.ToString();
                result += "\n" + "  Current player: " + this.game.GetCurrentPlayerCard();
                result += "\n" + "                  " + this.game.GetRiskCurrentPlayerCard();
                result += "\n" + "     Next player: " + this.game.GetNextPlayerCard();
                result += "\n" + "                  " + this.game.GetRiskNextPlayerCard();
                result += "\n" + "           Table: " + this.game.GetTableCard();
                result += "\n" + command;
                return result;
            }
            else
            {
                return command;
            }
        }



    }
    class Program
    {

        //static void Main(string[] args)
        //{

        //    StreamReader sr = new StreamReader(args[0]);
        //    StreamWriter sw = new StreamWriter(args[1]);
        //    StreamWriter log = new StreamWriter(args[2]);
        //    Command com;
        //    try
        //    {
        //        com = new Command(int.Parse(args[0][0].ToString()));
        //    }
        //    catch
        //    {

        //        throw new ArgumentException();
        //    }
        //    string result;
        //    do
        //    {
        //        string scommand = sr.ReadLine();
        //        log.Write(com.GetResult(scommand));
        //        com.DoFunc(scommand);
        //        result = com.GetResultGame();
        //        if (result != null)
        //        {
        //            sw.WriteLine(result);
        //        }
        //    } while (!sr.EndOfStream);
        //    log.Flush();
        //    log.Close();
        //    sw.Flush();
        //    sw.Close();
        //    sr.Close();


        //}
        static void Main(string[] args)
        {
            int level = 2;
            Command com = new Command(level);
            string result;

            string scommand = Console.ReadLine();
            while (scommand != null)
            {
                com.DoFunc(scommand);
                result = com.GetResultGame();
                if (result != null)
                {
                    Console.WriteLine(result);
                }
                scommand = Console.ReadLine();
            }
        }
    }
}
