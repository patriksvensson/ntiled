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

namespace NTiled
{
    /// <summary>
    /// Representation of a Tiled map.
    /// </summary>
    public sealed class TiledMap : IHasProperties
    {
        private readonly TiledPropertyCollection _properties;
        private readonly TiledTilesetCollection _tilesets;
        private readonly TiledLayerCollection _layers;

        /// <summary>
        /// Gets or sets the map version.
        /// </summary>
        /// <value>The map version.</value>
        public Version Version { get; set; }

        /// <summary>
        /// Gets or sets the map orientation.
        /// </summary>
        /// <value>The map orientation.</value>
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or sets the map width in tiles.
        /// </summary>
        /// <value>The map width in tiles.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the map height in tiles.
        /// </summary>
        /// <value>The map height in tiles.</value>
        public int Height { get; set; }

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
        /// Gets or sets the color of the background.
        /// </summary>
        /// <value>The color of the background.</value>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets the map properties.
        /// </summary>
        /// <value>The map properties.</value>
        public TiledPropertyCollection Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Gets or sets the tilesets.
        /// </summary>
        /// <value>The tilesets.</value>
        public TiledTilesetCollection Tilesets
        {
            get { return _tilesets; }
        }

        /// <summary>
        /// Gets or sets the layers.
        /// </summary>
        /// <value>The layers.</value>
        public TiledLayerCollection Layers
        {
            get { return _layers; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledMap"/> class.
        /// </summary>
        public TiledMap()
        {
            _properties = new TiledPropertyCollection();
            _tilesets = new TiledTilesetCollection();
            _layers = new TiledLayerCollection();
        }
    }
}