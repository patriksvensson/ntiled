using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NTiled.Parsers
{
    internal static class LayerParser
    {
        public static void ReadLayers(TiledMap map, XElement root)
        {
            string[] layerTypes = { "layer", "objectgroup" };
            var elements = root.Elements().Where(e => layerTypes.Contains(e.Name.LocalName, StringComparer.OrdinalIgnoreCase));
            foreach (XElement element in elements)
            {
                string layerType = element.Name.LocalName;
                if (layerType.Equals("layer", StringComparison.OrdinalIgnoreCase))
                {
                    ReadTileLayer(map, element);
                }
                else if (layerType.Equals("objectgroup", StringComparison.OrdinalIgnoreCase))
                {
                    ReadObjectGroup(map, element);
                }
            }
        }

        private static void ReadTileLayer(TiledMap map, XElement root)
        {
            TiledTileLayer layer = new TiledTileLayer();

            // Read generic layer information.
            ReadGenericLayerInformation(layer, root);

            // Read layer data.
            XElement dataElement = root.Element("data");
            if (dataElement != null)
            {
                ReadTileLayerData(layer, dataElement);
            }

            map.Layers.Add(layer);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private static void ReadTileLayerData(TiledTileLayer layer, XElement root)
        {
            string encoding = root.ReadAttribute<string>("encoding", null);
            string compression = root.ReadAttribute<string>("compression", null);

            bool canUnencode = encoding != null && encoding.Equals("base64", StringComparison.OrdinalIgnoreCase);
            bool canDecompress = compression != null && compression.Equals("gzip", StringComparison.OrdinalIgnoreCase);

            if (canUnencode && canDecompress)
            {
                string content = root.Value;
                using (Stream unencoded = new MemoryStream(Convert.FromBase64String(content), false))
                using (GZipStream uncompressed = new GZipStream(unencoded, CompressionMode.Decompress, false))
                using (BinaryReader reader = new BinaryReader(uncompressed))
                {
                    var tiles = new int[layer.Width * layer.Height];
                    for (int i = 0; i < layer.Width * layer.Height; i++)
                    {
                        tiles[i] = reader.ReadInt32();
                    }
                    layer.SetTileData(tiles);
                }
            }
        }

        private static void ReadObjectGroup(TiledMap map, XElement root)
        {
            var layer = new TiledObjectGroup();

            // Read generic layer information.
            ReadGenericLayerInformation(layer, root);

            // Read the used to display the objects in this group (if any).
            layer.Color = ColorTranslator.FromHtml(root.ReadAttribute("color", string.Empty));

            // Read all objects.
            foreach (var objectElement in root.GetElements("object"))
            {
                if (objectElement.HasAttribute("gid"))
                {
                    ObjectParser.ReadTileObject(layer, objectElement);
                }
            }

            map.Layers.Add(layer);
        }

        private static void ReadGenericLayerInformation(TiledLayer layer, XElement root)
        {
            layer.Name = root.ReadAttribute("name", string.Empty);
            layer.X = root.ReadAttribute("x", 0);
            layer.Y = root.ReadAttribute("y", 0);
            layer.Width = root.ReadAttribute("width", 0);
            layer.Height = root.ReadAttribute("height", 0);
            layer.Opacity = root.ReadAttribute("opacity", 1f);
            layer.Visible = root.ReadAttribute("visible", true);

            // Read layer properties.
            PropertyParser.ReadProperties(layer, root);
        }
    }
}
