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
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml.Linq;
using NTiled.Utilties;

namespace NTiled
{
    /// <summary>
    /// Class for reading Tiled maps.
    /// </summary>
    public sealed class TiledReader
    {
        /// <summary>
        /// Parses the specified map.
        /// </summary>
        /// <param name="document">The map.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public TiledMap Read(XDocument document)
        {
            var map = new TiledMap();

            // Get the document root.
            var root = XmlHelper.GetDocumentRoot(document, "map");

            // Read the map information.
            map.Version = Version.Parse(XmlHelper.ReadAttribute(root, "version", "1.0"));
            map.Orientation = XmlHelper.ReadAttribute(root, "orientation", string.Empty);
            map.Width = XmlHelper.ReadAttribute(root, "width", 0);
            map.Height = XmlHelper.ReadAttribute(root, "height", 0);
            map.TileWidth = XmlHelper.ReadAttribute(root, "tilewidth", 0);
            map.TileHeight = XmlHelper.ReadAttribute(root, "tileheight", 0);
            map.BackgroundColor = ColorTranslator.FromHtml(XmlHelper.ReadAttribute(root, "backgroundcolor", string.Empty));

            // Read the map's properties.
            var properties = ReadProperties(root);
            foreach (var property in properties)
            {
                map.Properties.Add(property.Key, property.Value);
            }

            // Read tilesets.
            ReadTilesets(map, root);

            // Read layers.
            ReadLayers(map, root);

            return map;
        }

        private static IDictionary<string, string> ReadProperties(XElement root)
        {
            var properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var element = XmlHelper.GetElement(root, "properties");
            if (element != null)
            {
                foreach (var property in XmlHelper.GetElements(element, "property"))
                {
                    var key = XmlHelper.ReadAttribute(property, "name", string.Empty);
                    var value = XmlHelper.ReadAttribute(property, "value", string.Empty);
                    properties.Add(key, value);
                }
            }
            return properties;
        }

        private static void ReadTilesets(TiledMap map, XElement root)
        {
            foreach (var tilesetElement in XmlHelper.GetElements(root, "tileset"))
            {
                var tileset = new TiledTileset();

                // Read mandatory attributes.
                tileset.FirstId = XmlHelper.ReadAttribute(tilesetElement, "firstgid", 0);
                tileset.Name = XmlHelper.ReadAttribute(tilesetElement, "name", string.Empty);
                tileset.TileWidth = XmlHelper.ReadAttribute(tilesetElement, "tilewidth", 0);
                tileset.TileHeight = XmlHelper.ReadAttribute(tilesetElement, "tileheight", 0);

                // Read optional attributes.
                tileset.Spacing = XmlHelper.ReadAttribute(tilesetElement, "spacing", 0);
                tileset.Margin = XmlHelper.ReadAttribute(tilesetElement, "margin", 0);

                // Read the tileset image.
                tileset.Image = ReadImage(tilesetElement);

                // Read the tileset offset.
                var offset = ReadTileOffset(tilesetElement);
                tileset.OffsetX = offset.Item1;
                tileset.OffsetY = offset.Item2;

                // Add the tileset to the map.
                map.Tilesets.Add(tileset);
            }
        }

        private static void ReadLayers(TiledMap map, XElement root)
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
            }
        }

        private static void ReadTileLayer(TiledMap map, XElement root)
        {
            TiledTileLayer layer = new TiledTileLayer();

            // Read generic layer information.
            layer.Name = XmlHelper.ReadAttribute(root, "name", string.Empty);
            layer.X = XmlHelper.ReadAttribute(root, "x", 0);
            layer.Y = XmlHelper.ReadAttribute(root, "y", 0);
            layer.Width = XmlHelper.ReadAttribute(root, "width", 0);
            layer.Height = XmlHelper.ReadAttribute(root, "height", 0);
            layer.Opacity = XmlHelper.ReadAttribute(root, "opacity", 1f);
            layer.Visible = XmlHelper.ReadAttribute(root, "visible", true);

            // Read layer properties.
            var properties = ReadProperties(root);
            foreach (var property in properties)
            {
                layer.Properties.Add(property.Key, property.Value);
            }

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
            string encoding = XmlHelper.ReadAttribute<string>(root, "encoding", null);
            string compression = XmlHelper.ReadAttribute<string>(root, "compression", null);

            bool canUnencode = encoding != null && encoding.Equals("base64", StringComparison.OrdinalIgnoreCase);
            bool canDecompress = compression != null && compression.Equals("gzip", StringComparison.OrdinalIgnoreCase);

            if (canUnencode && canDecompress)
            {
                string content = root.Value;
                using (Stream unencoded = new MemoryStream(Convert.FromBase64String(content), false))
                using (GZipStream uncompressed = new GZipStream(unencoded, CompressionMode.Decompress, false))
                using (BinaryReader reader = new BinaryReader(uncompressed))
                {
                    var tiles = new TiledTile[layer.Width * layer.Height];
                    for (int i = 0; i < layer.Width * layer.Height; i++)
                    {
                        tiles[i] = new TiledTile { Index = reader.ReadInt32() };
                    }
                    layer.SetTileData(tiles);
                }
            }
        }

        private static Tuple<int, int> ReadTileOffset(XElement root)
        {
            var element = XmlHelper.GetElement(root, "tileoffset");
            if (element != null)
            {
                int x = XmlHelper.ReadAttribute(element, "x", 0);
                int y = XmlHelper.ReadAttribute(element, "y", 0);
                return new Tuple<int, int>(x, y);
            }
            return new Tuple<int, int>(0, 0);
        }

        private static TiledImage ReadImage(XElement root)
        {
            var imageElement = XmlHelper.GetElement(root, "image");
            if (imageElement != null)
            {
                var image = new TiledImage();
                image.Source = XmlHelper.ReadAttribute(imageElement, "source", string.Empty);
                image.Width = XmlHelper.ReadAttribute(imageElement, "width", 0);
                image.Height = XmlHelper.ReadAttribute(imageElement, "height", 0);
                return image;
            }
            return null;
        }
    }
}
