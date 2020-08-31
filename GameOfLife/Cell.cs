using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameOfLife
{
    /// <summary>
    /// Represents a single cell (Square) in the game
    /// </summary>
    class Cell : StackPanel, ICloneable
    {

        // Whether the cell is dead or alive. All cells start as dead
        public bool isAlive = false;
        // Position on the grid
        public int x, y;
        
        /// <summary>
        /// Initializes new Cell at position x,y
        /// </summary>
        /// <param name="x">Position along the x-axis</param>
        /// <param name="y">Position along the y-axis</param>
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
            Style = FindResource("CellView") as Style;

        }

        /// <summary>
        /// Switches the state of the cell between dead and alive
        /// </summary>
        public void DeadAliveSwitch()
        {
            isAlive = !isAlive;

            Background = (isAlive) ? Brushes.GreenYellow : Brushes.DarkSlateGray;

        }


        /// <summary>
        /// Allows for cloning cells
        /// Useful for making non-destructive copies
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            Cell clonedCell = new Cell(this.x, this.y) {Style = FindResource("CellView") as Style};
            if(isAlive) clonedCell.DeadAliveSwitch();
            return clonedCell;
        }
    }
}
