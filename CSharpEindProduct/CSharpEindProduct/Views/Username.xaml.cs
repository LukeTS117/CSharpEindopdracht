﻿using System;
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

            }
        }
    }
}
