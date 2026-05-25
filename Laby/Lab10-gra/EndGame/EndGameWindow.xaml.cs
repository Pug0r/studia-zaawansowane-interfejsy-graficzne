using System.Windows;

namespace Lab10_gra.EndGame
{
    public partial class EndGameWindow : Window
    {
        public EndGameWindow(EndGameViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void ExitGame_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
