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
using LibClient;

namespace CSharpEindProduct.Views
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Page
    {

        Client client;

        public Lobby(Client client)
        {
            this.client = client;
            InitializeComponent();

        }

        private void AddSessionMouseDown(object sender, MouseButtonEventArgs e)
        {
            client.SendMessage(ServerClient.ServerClient.Tag.cns, " ");
        }
    }
}
