using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Bomberman.TmxParser
{
    class TileSet
    {
        public int firstgid;
        public int lastgid;
        public string name;
        public int tileWidth;
        public string source;
        public int tileHeight;
        public int imageWidth;
        public int imageHeight;
        public Bitmap bitmapData;
        public int tileAmountWidth;
        XmlDocument tsxFile;

        public TileSet(int firstgid, string source)
        {
            this.firstgid = firstgid;
            this.source = source;
            tsxFile.Load(source);            
        }

        
    }
}
