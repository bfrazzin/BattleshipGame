using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bfrazzinBattleship
{

    /// <summary>
    /// Handles everything about the ship and its characteristics
    /// </summary>
    public class Ship
    {
        // length of ship
        public int shipLength;
        // which way ship is oriented (vertical or horizontal)
        public char shipOrientation;
        // ship's health (same as its length)
        public int health;
        // where the first cell is at. The program takes this value and combines it with the
        // shipOrientation variable to determine which way to build the ship
        public int[] startingPosition;
        // simply the name of the ship
        public string name;
        // used to check if the sunk msg was printed
        public Boolean messagePrinted;


        // constructor
        public Ship(string name, int shipLength)
        {
            this.shipLength = shipLength;
            this.name = name;
            startingPosition = new int[2];
            health = shipLength;
            messagePrinted = false;
        }



        /// Getters and setters below



        public int ShipLength
        {
            get => shipLength;
        }

        public char ShipOrientation
        {
            get => shipOrientation;
            set => shipOrientation = value;
        }

        public int Health
        {
            get => health;
            set => health = value;
        }

        public int[] StartingPostion
        {
            get => startingPosition; 
            set => startingPosition = value;
        }

        public string Name
        {
            get => name;
        }
       
    }
}
