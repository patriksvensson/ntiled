// ﻿
// Copyright (c) 2013 Patrik Svensson
// 
// This file is part of NTiled.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using NTiled.Utilties;

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
            var layer = new TiledTileLayer();

            // Read generic layer information.
            ReadGenericLayerInformation(layer, root);

            // Read layer data.
            var dataElement = root.Element("data");
            if (dataElement != null)
            {
                ReadTileLayerData(layer, dataElement);
            }

            map.Layers.Add(layer);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        private static void ReadTileLayerData(TiledTileLayer layer, XElement root)
        {
            var encoding = root.ReadAttribute<string>("encoding", null);

            if (encoding != null)
            {
                if (encoding.Equals("csv", StringComparison.OrdinalIgnoreCase))
                    ReadCsvTileLayer(layer, root);
                else if (encoding.Equals("base64", StringComparison.OrdinalIgnoreCase))
                    ReadBase64TileLayer(layer, root);
            }
        }

        private static void ReadCsvTileLayer(TiledTileLayer layer, XElement root)
        {
            var value = root.Value;

            if (!String.IsNullOrEmpty(value))
            {
                var parsed = value.Split(',')
                                   .Select(s => Int32.Parse(s, NumberStyles.Integer | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingWhite, CultureInfo.InvariantCulture))
                                   .ToArray();

                layer.SetTileData(parsed);
            }
        }

        private static void ReadBase64TileLayer(TiledTileLayer layer, XElement root)
        {
            var compression = root.ReadAttribute("compression", string.Empty);
            var content = root.Value;

            using (var unencoded = new MemoryStream(Convert.FromBase64String(content), false))
            using (var uncompressed = unencoded.GetDecompressor(compression))
            using (var reader = new BinaryReader(uncompressed))
            {
                var tiles = new int[layer.Width * layer.Height];
                for (var i = 0; i < layer.Width * layer.Height; i++)
                {
                    tiles[i] = reader.ReadInt32();
                }
                layer.SetTileData(tiles);
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
                // Is this a tile object?
                if (objectElement.HasAttribute("gid"))
                {
                    ObjectParser.ReadTileObject(layer, objectElement);
                }
                else if (objectElement.HasElement("ellipse"))
                {
                    // TODO: Add support for ellipsis objects.
                }
                else if (objectElement.HasElement("polygon"))
                {
                    // TODO: Add support for polygon objects.
                }
                else if (objectElement.HasElement("polyline"))
                {
                    // TODO: Add support for polyline objects.
                }
                else
                {
                    ObjectParser.ReadRectangleObject(layer, objectElement);
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
