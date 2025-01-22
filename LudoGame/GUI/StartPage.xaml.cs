using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LudoGame.GUI
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        public static int NUM_PLAYERS = 2;
        public StartPage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NUM_PLAYERS = 2;
            NavigationService.Navigate(new Uri("GUI/Board.xaml", UriKind.Relative));
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NUM_PLAYERS = 3 ;
            NavigationService.Navigate(new Uri("GUI/Board.xaml", UriKind.Relative));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NUM_PLAYERS = 4;
            NavigationService.Navigate(new Uri("GUI/Board.xaml", UriKind.Relative));
        }
    }
}
