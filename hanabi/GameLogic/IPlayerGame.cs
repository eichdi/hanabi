using System;
namespace hanabi.GameLogic
{
    interface IPlayerGame
    {
        Player CurrentPlayer { get; }
        int GamedCards { get; }
        string GetTableCard();
        Player NextPlayer { get; }
        bool State { get; }
    }
}
