using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for LoadGridPopUp.xaml
    /// </summary>
    public partial class LoadGridPopUp : Window
    {

        // Presets saved by the users, passed in to constructor
        private readonly ArrayList savedPresets;

        // Property for data binding
        public ArrayList SavedPresets
        {
            get
            {
                return savedPresets;
            }
        }

        // Selected item from the UI's combobox
        public string ChosenPreset { get; set; }
        
        /// <summary>
        /// Initializes the pop up
        /// </summary>
        /// <param name="savedPresets">arraylist of string representations of user saved presets</param>
        public LoadGridPopUp(ArrayList savedPresets)

        {
            this.savedPresets = savedPresets;
            InitializeComponent();
            this.DataContext = this;
        }

        /// <summary>
        /// Default constructor, technically not used
        /// </summary>
        public LoadGridPopUp()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Closes the dialog and returns true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
