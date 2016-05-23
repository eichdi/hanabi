using System;
namespace hanabi.GameLogic
{
    interface IPlayerGame
    {
        Player GetOpponent(Player player);
        bool CheckPlayer(Player player);
        Player CurrentPlayer { get; }
        int GamedCards { get; }
        CollectCardOnHand GetOpponentCard(Player player);
        string GetTableCard();
        Player NextPlayer { get; }
        bool State { get; }
    }
}
