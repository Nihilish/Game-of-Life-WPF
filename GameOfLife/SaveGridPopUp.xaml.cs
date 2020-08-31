using System.Windows;
using System.Windows.Input;

namespace GameOfLife
{
    /// <summary>
    /// Interaction logic for SaveGridPopUp.xaml
    /// </summary>
    public partial class SaveGridPopUp : Window
    {
        public SaveGridPopUp()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Name input by user. Bound to the TextBox
        public string ChosenName { get; set; }

        /// <summary>
        /// Closes the dialog and saves the preset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        /// <summary>
        /// Adds support for pressing enter key to save preset
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnterToSave(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Save(sender, null);
            }
        }
    }
}