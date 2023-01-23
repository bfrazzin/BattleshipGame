using System;

/// <summary>
/// Name: Blake Frazzini
/// Class: CS 3020
/// Project: Project1 Battleship
/// Date: 02/12/22
/// </summary>


namespace bfrazzinBattleship

{
    /// <summary>
    /// Contains the main
    /// Only has 2 commands as specified
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.Run();
        }
    }
}
