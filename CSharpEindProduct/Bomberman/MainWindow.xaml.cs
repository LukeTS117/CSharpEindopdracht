using Bomberman.TmxParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TiledSharp;


namespace Bomberman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MapParser mapParser = new MapParser(@"C:\git\First Repo\CSharpEindopdracht\CSharpEindProduct\Bomberman\Map.tmx");

            mapParser.parseTilesets();
            
            /*
            TmxMap map = new TmxMap("Map.tmx");
            List<coordinate> coords = new List<coordinate>();
            Button button = new Button();

            

            uniGrid.Columns = map.Width;
            uniGrid.Rows = map.Height;
            uniGrid.Height = map.Height;
            uniGrid.Width = map.Width;


            Console.WriteLine(map.Width);

            for (var x = 0; x < uniGrid.Width; x++)
            {
                for (var y = 0; y < uniGrid.Height; y++)
                {
                    coords.Add(new coordinate(x, y));
                }
            }
            Console.WriteLine(coords.Count);

            for (var i = 0; i < coords.Count; i++)
            {

                BitmapImage bitImg = new BitmapImage();
                bitImg.BeginInit();
                bitImg.UriSource = new Uri(map.Tilesets[0].Image.Source, UriKind.Relative);
                bitImg.EndInit();

                Image img = new Image();
                img.Source = bitImg;


                uniGrid.Children.Add(img);
                

            }*/


        }

        public struct coordinate
        {
           public int X;
           public int Y;

           public coordinate(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
