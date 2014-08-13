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
using System.Xml.Linq;

namespace NTiled.Parsers
{
    internal static class TilesetParser
    {
        public static void ReadTilesets(TiledMap map, XElement root)
        {
            foreach (var tilesetElement in root.GetElements("tileset"))
            {
                var tileset = new TiledTileset();

                // Read mandatory attributes.
                tileset.FirstId = tilesetElement.ReadAttribute("firstgid", 0);
                tileset.Name = tilesetElement.ReadAttribute("name", String.Empty);
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
            var tiles = new List<TiledTile>();
            var tileElements = root.Elements("tile");
            foreach (var tileElement in tileElements)
            {
                var tile = new TiledTile();
                tile.Id = tileElement.ReadAttribute("id", 0);

                PropertyParser.ReadProperties(tile, tileElement);

                tiles.Add(tile);
            }
            return tiles;
        }

        private static Tuple<int, int> ReadTileOffset(XElement root)
        {
            var element = root.GetElement("tileoffset");
            if (element != null)
            {
                var x = element.ReadAttribute("x", 0);
                var y = element.ReadAttribute("y", 0);
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
                image.Source = imageElement.ReadAttribute("source", String.Empty);
                image.Width = imageElement.ReadAttribute("width", 0);
                image.Height = imageElement.ReadAttribute("height", 0);
                return image;
            }
            return null;
        }
    }
}
