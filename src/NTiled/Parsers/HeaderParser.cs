using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NTiled.Parsers
{
    internal static class HeaderParser
    {
        public static void ReadHeader(TiledMap map, XElement root)
        {
            map.Version = Version.Parse(root.ReadAttribute("version", "1.0"));
            map.Orientation = root.ReadAttribute("orientation", string.Empty);
            map.Width = root.ReadAttribute("width", 0);
            map.Height = root.ReadAttribute("height", 0);
            map.TileWidth = root.ReadAttribute("tilewidth", 0);
            map.TileHeight = root.ReadAttribute("tileheight", 0);
            map.BackgroundColor = ColorTranslator.FromHtml(root.ReadAttribute("backgroundcolor", string.Empty));
        }
    }
}
