using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



/**
 * This class handles the game board for the BattleShip game. Within it is a 10x10 grid that uses 'O' character for misses
 * and 'X' character for hits. The board can also display 'S' for ship locations if hacks are enabled. This contains
 * the functional logic for various parts regarding ship placement as well.
 */

namespace bfrazzinBattleship
{

    public class Gameboard
    {
        public const int gridSize = 10;
        private char[,] board = new char[gridSize, gridSize];
        public Ship[] shipArray = new Ship[6];
        public Boolean hacks = false;
        int shipCount = 6;

        // Creates the board
        public char[,] Board
        {
            get
            {
                return board;
            }
            set
            {
                board = value;
            }
        }


        /// <summary>
        /// Determine where to place one ship.
        /// This determines where the computer will save each ship's location.
        /// </summary>
        /// <param name="ship"></param>
        public void PlaceShip(Ship ship)
        {
            int[] startingPosition = { 0, 0 };
            char shipDirection = ' ';
            Boolean validPlacement = false;
            
            // loop until placement of ship is validated
            while (!validPlacement)
            {
                // randomly generate starting location
                startingPosition = ChooseStartingLocation();
                
                // randomly generate what direction the ship extends out in
                shipDirection = ChooseDirection();
                
                // validate placement is within board and not overlapping another ship
                validPlacement = ValidateShipPlacement(ship, shipDirection, startingPosition);

            }

            // now that the ship is validated, set the orientation
            ship.shipOrientation = shipDirection;
            // set the starting position
            ship.startingPosition = startingPosition;
            // draw the ship
            DrawShip(ship);
        }

        /// <summary>
        /// Creates the gameboard by filling the grid with spaces and 
        /// also places each ship in the array.
        /// </summary>
        public void CreateBoard()
        {
            // Fill out the array
            CreateShips();

            // Fill the grid with spaces
            FillGrid();

            // Place each ship in the array
            for (int i = 0; i < shipArray.Length; i++)
            {
                PlaceShip(shipArray[i]);
            }
        }

        /// <summary>
        /// Fill out the values of the ships
        /// </summary>
        private void CreateShips()
        {
            shipArray[0] = new Ship("Destroyer", 2);
            shipArray[1] = new Ship("Destroyer 2", 2);
            shipArray[2] = new Ship("Submarine", 3);
            shipArray[3] = new Ship("Submarine 2", 3);
            shipArray[4] = new Ship("Battleship", 4);
            shipArray[5] = new Ship("Carrier", 5);
        }

        /// <summary>
        /// Used to detect if all the ships have been sunk.
        /// This is used in the game class to determine end of game.
        /// </summary>
        /// <returns>true or false is returned</returns>
        public Boolean allShipsSunk()
        {
            // if all ships are sunk
            if(shipCount == 0)
            {
                // reset the shipCount value in case of new game
                shipCount = shipArray.Length;
                // tell program yes, all ships gone
                return true;
                
            }
            // tell program all ships are not gone
            return false;
        }


        /// <summary>
        /// Chooses a direction - vertical or horizontal - based on a random number between 0 and 10
        /// If the number is even, it chooses vertical, otherwise horizontal.
        /// </summary>
        /// <returns>The ship orientation direction is returned</returns>
        private char ChooseDirection()
        {
            // create random number
            Random rand = new Random();
            int num = rand.Next(0, 10);

            // mod 2 for even or odd
            int randomDirection = num % 2;

            // return result
            return num == 0 ? 'V' : 'H';
        }

        /// <summary>
        /// Choses a random starting location (coordinate) for the ship.
        /// </summary>
        /// <returns>
        /// The starting location is returned
        /// </returns>
        private int[] ChooseStartingLocation()
        {
           
            Random rand = new Random();
            // choose what row and column
            int row = rand.Next(0, gridSize);
            int col = rand.Next(0, gridSize);

            // return randomly generated starting location
            return new int[] { row, col };
        }


        /// <summary>
        /// Validates where the ship will be placed by checking each box that was chosen for a particular ship
        /// If any box is occupied, it returns false. If the entire selection of boxes is clear, it returns true/
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="shipDirection"></param>
        /// <param name="startingLocation"></param>
        /// <returns></returns>
        public Boolean ValidateShipPlacement(Ship ship, char shipDirection, int[] startingLocation)
        {
            // Set bool value
            Boolean validPlacement = true;

            // for horizontal orientation of the passed in ship. Also check the boxes that extend from that starting point
            // in simpler terms, check each generated cell that the ship is trying to exist in
            if(shipDirection == 'H' && startingLocation[1] + ship.shipLength - 1 < board.GetLength(1))
            {
                for(int i = 0; i < ship.shipLength && validPlacement; i++)
                {
                    if(board[startingLocation[0], startingLocation[1] + i] == 'S')
                    {
                        validPlacement = false;
                    }
                }
            }

            // for vertical orientation of the passed in ship. Also check the boxes that extend from that starting point
            // in simpler terms, check each generated cell that the ship is trying to exist in
            else if (shipDirection == 'V' && startingLocation[0] + ship.shipLength - 1 < board.GetLength(0))
            {
                for (int i = 0; i < ship.shipLength && validPlacement; i++)
                {
                    if (board[startingLocation[0] + i, startingLocation[1]] == 'S')
                    {
                        validPlacement = false;
                    }
                }
            }
            else
            {
                validPlacement = false;
            }

            return validPlacement;
        }

        /// <summary>
        /// Draws one ship. It takes the information from the passed in ship (orientation and starting positions)
        /// and figures out how to draw it. If it is vertical, then it draws a vertical line from startingpos cell 0 to startingpos
        /// cell 1. Same for horizontal (else case)
        /// </summary>
        /// <param name="ship">ship to be drawn onto the board</param>
        public void DrawShip(Ship ship)
        {
            // iterate for length of ship amount times
            for (int i = 0; i < ship.shipLength; i++)
            {
                // check orientation to know which way to draw
                if (ship.shipOrientation == 'V')
                {
                    // draw ship vertically
                    board[ship.StartingPostion[0] + i, ship.StartingPostion[1]] = 'S';
                }
                // check orientation to know which way to draw
                else
                {
                    // draw ship horizontally
                    board[ship.StartingPostion[0], ship.StartingPostion[1] + i] = 'S';
                }
            }
            
            
        }

        /// <summary>
        /// Searches for ships on the (completed) board. First determines the orientation of the ship (h/v)
        /// Then searches for exactly where the ship is(aka what cells it occupies)
        /// </summary>
        public Ship SearchForShips(int row, int col)
        {        
            for (int i = 0; i < shipArray.Length; i++)
            {
                // if vertically oriented
                if (shipArray[i].shipOrientation == 'V')
                {
                    if((row >= shipArray[i].StartingPostion[0] && row <= shipArray[i].StartingPostion[0] + 
                        shipArray[i].ShipLength - 1) && col == shipArray[i].StartingPostion[1])
                    {
                        return shipArray[i];
                    }
                }
                // if horizontally oriented
                else
                {
                    if((col >= shipArray[i].StartingPostion[1] && col <= shipArray[i].StartingPostion[1] + 
                        shipArray[i].ShipLength - 1) && row == shipArray[i].StartingPostion[0])
                    {
                        return shipArray[i];
                    }
                }
            }
            return null;
            
        }
        

        /// <summary>
        /// Keeps track of each of the ship's health. If health is zero then the system
        /// can now recognize that ship is destroyed
        /// </summary>
        public void HealthTracker()
        {
            for(int i = 0; i < shipArray.Length; i++)
            {
                // checks the health of each ship in the array
                if (shipArray[i].Health == 0)
                {
                    // flag to check if the destroyed ship message has been printed
                    if(!shipArray[i].messagePrinted)
                    {
                        // decrement the total amount of ships
                        // this is for the game over check
                        shipCount--;
                        // tell user what ship they just sunk
                        Console.WriteLine("\nYou just sunk the " + shipArray[i].Name + "!\n");
                        // show user what ships are remaining
                        DisplayRemainingShips();
                        // flag
                        shipArray[i].messagePrinted = true;
                    }
                }
            }
        }
        
        /// <summary>
        /// This simply just prints out to the user what ships are remaining.
        /// </summary>
        public void DisplayRemainingShips()
        {
            for(int i = 0; i < shipArray.Length; i++)
            {
                // print the ships that arent sunk
                if (shipArray[i].Health > 0)
                {
                    Console.WriteLine("The " + shipArray[i].Name + " is still standing. ");
                }
            }
            Console.WriteLine();
        }

        /// <summary>
        /// This is a simple set of instructions to tell the user about the game and how to play
        /// </summary>
        public void Instructions()
        {
            Console.WriteLine("Welcome to CS3020 BattleShip!\n");
            Console.WriteLine("Here are your instructions for BattleShip!\n");
            Console.WriteLine("This game is simply a version of BattleShip that is to be played" +
                " versus the computer.\nThe computer cannot fight back, " +
                "therefore you are only attacking their ships.");
            Console.WriteLine("There are only a few things you need to know:");
            Console.WriteLine("First, each round you must choose a location to attack by entering " +
                "in the coordinates in the \ntraditional format of" +
                " a letter followed by a number (Example: A1)." +
                "\nAny other input is invalid and will not be recognized by the computer.");
            Console.WriteLine("That being said, if you cannot win (or give up), you may type \"EH\" (stands for Enable Hacks)" +
                "\ninto your next turn input. This will enable hacks, " +
                "which will show you \nexactly where the computer has placed its ships. \nIf you wish to disable them " +
                "simply type in the command again. \n\nHave fun!");
        }

        /// <summary>
        /// SetChar we did in class that sets the passed in character at the location
        /// </summary>
        public void SetChar(int row, int col, char aChar)
        {
            if (row < board.GetLength(0) && col < board.GetLength(1))
            {
                board[row, col] = aChar;
            }
            else
            {
                Console.WriteLine($"Invalid Location: {row}, {col}");
            }
        }

        /// <summary>
        /// Gets a character at the specified location
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public char GetChar(int row, int col)
        {
            return board[row, col];
        }

        /// <summary>
        /// Fillgrid we did in class that fills each box with a space
        /// </summary>
        public void FillGrid()
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    board[row, col] = ' ';
                }
            }
        }

        /// <summary>
        /// Fills a grid with a specified Character.
        /// </summary>
        /// <param name="aChar">character to place</param>
        public void FillGrid(char aChar)
        {
            for (int row = 0; row < board.GetLength(0); row++)
            {
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    board[row, col] = aChar;
                }
            }
        }

        /// <summary>
        /// Displays to the user the appropriate board/board information
        /// If hacks are on, it shows where each ship is
        /// </summary>
        public void Display()
        {
            char rowChar = 'A';
            // iterate through the 2d array
            for (int row = 0; row < board.GetLength(0); row++)
            {
                // Formatting
                DrawLine();
                Console.Write(rowChar + " ");
                rowChar++;
                for (int col = 0; col < board.GetLength(1); col++)
                {
                    // If hacks are enabled show ships
                    if (hacks)
                    {
                        Console.Write($"| {board[row, col]} ");
                    }
                    else
                    {
                        if (board[row, col] == 'S')
                        {
                            Console.Write($"|   ");
                        }
                        else
                        {
                            Console.Write($"| {board[row, col]} ");
                        }
                    }

                }
                Console.WriteLine("|");
            }

            // Formatting
            DrawLine();
            DrawColumnNumbers();
        }

        /// <summary>
        /// Draws a line for the game board
        /// </summary>
        private void DrawLine()
        {
            Console.Write("  ");
            for (int col = 0; col < board.GetLength(1) * 4 + 1; col++)
            {
                Console.Write($"-");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Draws column numbers for the game board
        /// </summary>
        private void DrawColumnNumbers()
        {
            Console.Write("   ");
            for (int col = 0; col < board.GetLength(1); col++)
            {
                Console.Write($" {col + 1}  ");
            }
            Console.WriteLine();
        }


    }//CLASS
}