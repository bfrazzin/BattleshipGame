using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace bfrazzinBattleship
{
    public class Game
    {
        // create game board
        Gameboard board = new Gameboard();
      
        // run the game. This is to be called in the program.cs
        public void Run()
        {
            // safely intialize variables
            Boolean run = true;
            Boolean gameOver = false;
            string userResponse = "";
            int responseInt = 0;

            // loop while the program is told to run
            while (run)
            {
                // make sure hacks aren't shown in the case of a new (2nd or more) game.
                board.hacks = false;
                // print instructions
                board.Instructions();
                // create the board
                board.CreateBoard();
                // display the board
                board.Display();
                
                // loop while the game is not over
                // game being over means all ships are sunk
                while (!gameOver)
                {
                    // handles what the player can do for their turn
                    PlayerTurn();
                    // display updated board
                    board.Display();
                    // check health of ships
                    board.HealthTracker();
                    // check if the game should be over. if allShipsSunk returns true then its over
                    gameOver = board.allShipsSunk();
                }
                // print info to user
                Console.WriteLine("You win!");
                Console.WriteLine("Would you like to play another game? Enter 1 to play again, otherwise press 0 exit.");

                // read their input and decide to play new game or not
                responseInt = int.Parse(Console.ReadLine());
                if(responseInt == 1)
                {
                    run = true;
                    gameOver = false;
                }
                else
                {
                    run = false;
                    gameOver = true;
                }
            }
        }
        
        /// <summary>
        /// Handles the player's turn information
        /// </summary>
        public void PlayerTurn()
        {
            // safely intialize variables
            int row = 0;
            int col = 0;
            Boolean valid = true;
            string target = "";

            // enter do while
            do
            {
                Console.WriteLine("What coordinate should be attacked?");
                // bool for validity of choice check below
                valid = true;
                // read in input
                target = Console.ReadLine().ToUpper();
                
                // case for wanting hacks enabled
                if (target.Equals("EH"))
                {
                    board.hacks = !board.hacks;
                }
                // did not choose hacks this turn
                else
                {
                    // set row to ascii equivalent of 1-9 for user choice
                    row = target[0] - 65;
                    // set column value for user choice
                    col = int.Parse(target.Substring(1)) - 1;

                    // checks if spot is occupied
                    if (row >= 0 && row < board.Board.GetLength(0) && col >= 0 && col < board.Board.GetLength(1))
                    {
                        // check if user desired attack has already been conducted. aka checks if the cell is X or O. If it 
                        // is a valid choice (not X or O), return true
                        valid = board.GetChar(row, col) != 'X' && board.GetChar(row, col) != 'O';

                        // if not valid (aka cell is X or O)
                        if (!valid)
                        {
                            // print error
                            Console.WriteLine("\nYour chosen location has already been attacked!\n");
                        }
                    }
                    else
                    {
                        valid = false;
                    }
                }
            } while (!valid);

            // if hacks aren't chosen
            if(!target.Equals("EH"))
            {
                // search the board for where the ships are
                Ship ship = board.SearchForShips(row, col);
                // if a ship exists at the chosen point
                if(ship != null)
                {
                    // switch cell to X
                    board.SetChar(row, col, 'X');
                    // decrement the ship's health
                    ship.Health--;
                    // print successful hit
                    Console.WriteLine("\nTarget Hit. Great choice.\n");
                }
                // miss
                else
                {
                    board.SetChar(row, col, 'O');
                    Console.WriteLine("\nTarget Miss... We'll get 'em next time.\n");
                }
     
            }

        }
    }
}