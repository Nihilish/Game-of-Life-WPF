using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        // The grid the game uses to keep track of cells state
        private Cell[,] coreGrid = new Cell[40,40];
        // Timer used for iteration loop
        DispatcherTimer timer = new DispatcherTimer();
        // Adds support for 'drag and drop' functionality
        private bool isMouseDown = false;
        // This handles user saved presets
        private Dictionary<string, Cell[,]> savedPresets = new Dictionary<string, Cell[,]>();

        public MainWindow()
        {
            InitializeComponent();

            FillGridWithCells();

            timer.Tick += Iterate;
            timer.Interval = TimeSpan.FromMilliseconds(50);

        }

        /// <summary>
        /// Event triggered by dragging over a cell. Cycles the dragged over cell between dead and alive
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DragOverCell(object sender, RoutedEventArgs e)
        {

            if (isMouseDown)
            {
                
                Cell c = sender as Cell;
                c.DeadAliveSwitch();
            }
        }

        /// <summary>
        /// Same interaction logic as DragOverCell but upon clicking a cell
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClickCell(object sender, MouseButtonEventArgs e)
        {
            Cell c = sender as Cell;
            c.DeadAliveSwitch();
        }

        /// <summary>
        /// Initializes the grid with 'dead' empty cells
        /// </summary>
        private void FillGridWithCells()
        {

            Grid grid = CellGrid;

            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 40; y++)
                {

                    Cell c = new Cell(x, y);
                    
                    // Adds the two event handlers needed for cells to draw and click
                    c.MouseEnter += DragOverCell;
                    c.MouseLeftButtonDown += ClickCell;
                    
                    // Dynamically fills the grid
                    Grid.SetRow(c, x);
                    Grid.SetColumn(c, y);
                    grid.Children.Add(c);
                    coreGrid[x, y] = c;


                }
            }


        }

        /// <summary>
        /// Applies basic interaction logic for cell development
        /// If a cell has 2 or 3 live neighbors, survives to next iteration
        /// If a dead cell has exactly 3 live neighbors, it becomes alive
        /// Every other cell dies by next iteration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Iterate(object sender, EventArgs e)
        {

            // Cells that will undergo changes over next iteration
            // Dead cells in this list will become alive and vice-versa
            ArrayList changedCells = new ArrayList();
            
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 40; y++)
                {
                    
                    Cell c = coreGrid[x, y];

                    // Count live neighbors
                    int liveNeighbors = 0;

                    for (int i = x-1; i <= x+1; i++)
                    {
                        for (int j = y-1; j <= y+1; j++)
                        {
                            if(i < 0 || i >= 40 || j < 0 || j >= 40) continue;
                            if (i == x && j == y) continue;

                            if (coreGrid[i, j].isAlive) liveNeighbors++;

                        }
                    }

                    //If the cell is still alive
                    if (c.isAlive)
                    {

                        if (liveNeighbors == 2 || liveNeighbors == 3)
                        {
                            //Do nothing
                        }
                        
                        //Kill the cell
                        else
                        {
                            changedCells.Add(c);
                        }
                    }
                    
                    //If it's dead
                    else
                    {
                        if (liveNeighbors == 3)
                        {
                            changedCells.Add(c);
                        }
                    }

                }
            }

            //Apply changes to cells
            for (int i = 0; i < changedCells.Count; i++)
            {
                Cell temp = changedCells[i] as Cell;
                temp.DeadAliveSwitch();
            }

        }

        /// <summary>
        /// TODO: Use to load back any presets saved by the user
        /// </summary>
        /// <param name="newGrid"> Grid to be loaded </param>
        private void RefillGrid(Cell[,] newGrid)
        {
            Grid g = CellGrid;
            g.Children.Clear();
            

            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 40; y++)
                {
                    Cell c = newGrid[x, y].Clone() as Cell;
                    coreGrid[x, y] = c;
                    Grid.SetRow(c, x);
                    Grid.SetColumn(c, y);
                    g.Children.Add(c);
                }
            }

        }

        /// <summary>
        /// Starts the timer loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        /// <summary>
        /// Stops the timer loop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stop(object sender, RoutedEventArgs e)
        {
            timer.Stop();
        }

        /// <summary>
        /// Resets the grid back to its blank state and stops the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Reset(object sender, RoutedEventArgs e)
        {
            CellGrid.Children.Clear();
            timer.Stop();
            FillGridWithCells();
        }

        /// <summary>
        /// When the mouse right click is pressed, allow for 'drawing'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellGrid_OnPreviewRightMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!isMouseDown) isMouseDown = true;
        }

        /// <summary>
        /// When the right click is released, disable 'drawing' functionality
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellGrid_OnPreviewRightMouseUp(object sender, MouseButtonEventArgs e)
        {
            isMouseDown = false;
        }

        /// <summary>
        /// If the mouse pointer exits the grid, remove the 'drawing' feature
        /// This prevents the mouse pointer from still being in a
        /// false positive mousedown state upon re-entry
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CellGrid_OnMouseLeave(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        /// <summary>
        /// Saves the state of the game grid for further use
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SavePreset(object sender, RoutedEventArgs e)
        {

            timer.Stop();

            string nameOfPreset;

            SaveGridPopUp popUp = new SaveGridPopUp {Owner = this};

            if (popUp.ShowDialog() == true)
            {
                nameOfPreset = popUp.ChosenName;

                // Copies the current grid
                Cell[,] gridCopy = new Cell[40,40];

                for (int x = 0; x < 40; x++)
                {
                    for (int y = 0; y < 40; y++)
                    {
                        Cell cellCopy = coreGrid[x, y].Clone() as Cell;
                        gridCopy[x, y] = cellCopy;

                    }
                }

                savedPresets.Add(nameOfPreset, gridCopy);

            }
        }

        /// <summary>
        /// Loads a given state
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadGrid(object sender, RoutedEventArgs e)
        {
            
            timer.Stop();
            
            // Holds all the names given to the user presets
            ArrayList savedPresetsNames = new ArrayList();

            foreach (var entry in savedPresets)
            {
                savedPresetsNames.Add(entry.Key);
            }

            // Creates the Preset Load utility
            LoadGridPopUp lgp = new LoadGridPopUp(savedPresetsNames) {Owner = this};

            // Displays the Load Pop Up
            if (lgp.ShowDialog() == true)
            {
                string choice = lgp.ChosenPreset;

                Cell[,] loadedGrid = savedPresets[choice];

                RefillGrid(loadedGrid);

            }

        }
    }
}
