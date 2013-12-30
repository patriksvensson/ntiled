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
namespace NTiled
{
    /// <summary>
    /// Represents a tileset.
    /// </summary>
    public sealed class TiledTileset
    {
        private readonly TiledTileCollection _tiles;

        /// <summary>
        /// Gets or sets the first tile index in the tileset.
        /// </summary>
        /// <value>The first tile index in the tileset.</value>
        public int FirstId { get; set; }

        /// <summary>
        /// Gets or sets the name of the tileset.
        /// </summary>
        /// <value>The name of the tileset.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the tile width in pixels.
        /// </summary>
        /// <value>The tile width in pixels.</value>
        public int TileWidth { get; set; }

        /// <summary>
        /// Gets or sets the tile height in pixels.
        /// </summary>
        /// <value>The tile height in pixels.</value>
        public int TileHeight { get; set; }

        /// <summary>
        /// Gets or sets the spacing.
        /// </summary>
        /// <value>The spacing.</value>
        public int Spacing { get; set; }
        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        /// <value>The margin.</value>
        public int Margin { get; set; }

        /// <summary>
        /// Gets or sets the horizontal offset in tiles.
        /// </summary>
        /// <value>The horizontal offset in tiles.</value>
        public int OffsetX { get; set; }

        /// <summary>
        /// Gets or sets the vertical offset in tiles.
        /// </summary>
        /// <value>The vertical offset in tiles.</value>
        public int OffsetY { get; set; }

        /// <summary>
        /// Gets or sets the tileset image.
        /// </summary>
        /// <value>The tileset image.</value>
        public TiledImage Image { get; set; }

        /// <summary>
        /// Gets the tiles.
        /// </summary>
        /// <value>The tiles.</value>
        public TiledTileCollection Tiles
        {
            get { return _tiles; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledTileset"/> class.
        /// </summary>
        public TiledTileset()
        {
            _tiles = new TiledTileCollection();
        }
    }
}