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
using System.IO;

namespace CSharpEindProduct.Views
{
    /// <summary>
    /// Interaction logic for Username.xaml
    /// </summary>
    public partial class Username : Page
    {
        public Username()
        {
            InitializeComponent();
        }

        private void Button_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void UserNameTextBoX_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (UserNameTextBox.Text.Length != 0)
                {
                    using (StreamReader reader = File.OpenText("ForbiddenWords.txt"))
                    {
                        string s;
                        bool isForbiddenWord = false;
                        while ((s = reader.ReadLine()) != null)
                        {
                            if (UserNameTextBox.Text.Equals(s.ToLower()))
                            {
                                LabelUsername.Text = "That is a forbidden word!";
                                isForbiddenWord = true;
                                break;
                            }
                        }

                        if (!isForbiddenWord)
                        {
                            Client client = new Client(UserNameTextBox.Text);
                            this.NavigationService.Navigate(new Lobby(client));
                        }
                        


                    }
                }

                else
                {
                    LabelUsername.Text = "You must choose a name!";
                }
            }
        }
    }
}
