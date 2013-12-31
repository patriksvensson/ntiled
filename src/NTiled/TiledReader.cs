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
            var root = document.GetDocumentRoot("map");

            // Read the map information.
            map.Version = Version.Parse(root.ReadAttribute("version", "1.0"));
            map.Orientation = root.ReadAttribute("orientation", string.Empty);
            map.Width = root.ReadAttribute("width", 0);
            map.Height = root.ReadAttribute("height", 0);
            map.TileWidth = root.ReadAttribute("tilewidth", 0);
            map.TileHeight = root.ReadAttribute("tileheight", 0);
            map.BackgroundColor = ColorTranslator.FromHtml(root.ReadAttribute("backgroundcolor", string.Empty));

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
            var element = root.GetElement("properties");
            if (element != null)
            {
                foreach (var property in element.GetElements("property"))
                {
                    var key = property.ReadAttribute("name", string.Empty);
                    var value = property.ReadAttribute("value", string.Empty);
                    properties.Add(key, value);
                }
            }
            return properties;
        }

        private static void ReadTilesets(TiledMap map, XElement root)
        {
            foreach (var tilesetElement in root.GetElements("tileset"))
            {
                var tileset = new TiledTileset();

                // Read mandatory attributes.
                tileset.FirstId = tilesetElement.ReadAttribute("firstgid", 0);
                tileset.Name = tilesetElement.ReadAttribute("name", string.Empty);
                tileset.TileWidth = tilesetElement.ReadAttribute("tilewidth", 0);
                tileset.TileHeight = tilesetElement.ReadAttribute("tileheight", 0);

                // Read optional attributes.
                tileset.Spacing = tilesetElement.ReadAttribute("spacing", 0);
                tileset.Margin = tilesetElement.ReadAttribute("margin", 0);

                // Read the tileset image.
                tileset.Image = ReadImage(tilesetElement);

                // Read the tileset offset.
                var offset = ReadTileOffset(tilesetElement);
                tileset.OffsetX = offset.Item1;
                tileset.OffsetY = offset.Item2;

                // Read tile metadata.
                var tileMetadata = ReadTilesetMetadata(tilesetElement);
                if (tileMetadata.Count > 0)
                {
                    tileset.Tiles.AddRange(tileMetadata);
                }

                // Add the tileset to the map.
                map.Tilesets.Add(tileset);
            }
        }

        private static List<TiledTile> ReadTilesetMetadata(XElement root)
        {
            List<TiledTile> tiles = new List<TiledTile>();
            IEnumerable<XElement> tileElements = root.Elements("tile");
            foreach (XElement tileElement in tileElements)
            {
                TiledTile tile = new TiledTile();
                tile.Id = tileElement.ReadAttribute("id", 0);

                var properties = ReadProperties(tileElement);
                if (properties.Count > 0)
                {
                    foreach (var property in properties)
                    {
                        tile.Properties.Add(property.Key, property.Value);
                    }
                }

                tiles.Add(tile);
            }
            return tiles;
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
                    ReadTileObject(layer, objectElement);
                }
            }

            map.Layers.Add(layer);
        }

        private static void ReadTileObject(TiledObjectGroup layer, XElement root)
        {
            var tileObject = new TiledTileObject();

            // Read generic object information.
            ReadGenericObjectInformation(tileObject, root);

            // Read tile specific stuff.
            tileObject.Tile = root.ReadAttribute("gid", 0);

            layer.Objects.Add(tileObject);
        }

        private static void ReadGenericObjectInformation(TiledObject obj, XElement root)
        {
            obj.Name = root.ReadAttribute("name", string.Empty);
            obj.Type = root.ReadAttribute("type", string.Empty);
            obj.X = root.ReadAttribute("x", 0);
            obj.Y = root.ReadAttribute("y", 0);
            obj.Width = root.ReadAttribute("width", 0);
            obj.Height = root.ReadAttribute("height", 0);

            // Read object properties.
            var properties = ReadProperties(root);
            foreach (var property in properties)
            {
                obj.Properties.Add(property.Key, property.Value);
            }
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
            var properties = ReadProperties(root);
            foreach (var property in properties)
            {
                layer.Properties.Add(property.Key, property.Value);
            }
        }

        private static Tuple<int, int> ReadTileOffset(XElement root)
        {
            var element = root.GetElement("tileoffset");
            if (element != null)
            {
                int x = element.ReadAttribute("x", 0);
                int y = element.ReadAttribute("y", 0);
                return new Tuple<int, int>(x, y);
            }
            return new Tuple<int, int>(0, 0);
        }

        private static TiledImage ReadImage(XElement root)
        {
            var imageElement = root.GetElement("image");
            if (imageElement != null)
            {
                var image = new TiledImage();
                image.Source = imageElement.ReadAttribute("source", string.Empty);
                image.Width = imageElement.ReadAttribute("width", 0);
                image.Height = imageElement.ReadAttribute("height", 0);
                return image;
            }
            return null;
        }
    }
}
