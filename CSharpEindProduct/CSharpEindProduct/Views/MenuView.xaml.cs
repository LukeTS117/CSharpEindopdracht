
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

namespace CSharpEindProduct.Views
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class MenuView : Page
    {

        
        public MenuView()
        {
            InitializeComponent();
        }

        private void TextGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.Background = Brushes.Red;
        }

        private void TextLostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.Background = Brushes.Red;
        }

        private void TextBlock_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           this.NavigationService.Navigate(new CreditsView());
            Console.WriteLine("MouseDown");
        }

        private void Button_Start_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new Username());
        }
    }
}
