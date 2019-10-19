using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Bomberman.TmxParser
{
    class MapParser
    {

        XmlDocument doc;
        string mapWidth;
        string mapHeight;
        string tileWidth;
        string tileHeight;
        List<TileSet> tileSets;
        int xmlCounter = 0;


        
       public MapParser(string path)
        {
            doc = new XmlDocument();
            tileSets = new List<TileSet>();
            doc.Load(@"Map.tmx");
            mapWidth = doc.DocumentElement.GetAttribute("width");
            mapHeight = doc.DocumentElement.GetAttribute("height");
            tileWidth = doc.DocumentElement.GetAttribute("tilewidth");
            tileHeight = doc.DocumentElement.GetAttribute("tileheight");

        }


        public void parseTilesets()
        {
            foreach(XmlNode node in doc.DocumentElement)
            {
                if(node.Name == "tileset")
                {
                    int firstgid = Int32.Parse(node.Attributes[0].InnerText);
                    string source = node.Attributes[1].InnerText;
                    Console.WriteLine(source);
                    tileSets.Add(new TileSet(firstgid, source));
                }
            }
        }




    }
}
