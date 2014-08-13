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
    /// Represents a layer.
    /// </summary>
    public abstract class TiledLayer : IHasProperties
    {
        private readonly TiledPropertyCollection _properties;

        /// <summary>
        /// Gets or sets the name of the layer.
        /// </summary>
        /// <value>The name of the layer.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the horizontal offset of the layer in tiles.
        /// </summary>
        /// <value>The horizontal offset of the layer in tiles.</value>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the vertical offset of the layer in tiles.
        /// </summary>
        /// <value>The vertical offset of the layer in tiles.</value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the layer in tiles.
        /// </summary>
        /// <value>The width of the layer in tiles.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the layer in tiles.
        /// </summary>
        /// <value>The height of the layer in tiles.</value>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the opacity of the layer.
        /// </summary>
        /// <value>The opacity as a value between 0.0 and 1.0.</value>
        public float Opacity { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether the layer is visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the layer is visible; otherwise, <c>false</c>.
        /// </value>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets the layer properties.
        /// </summary>
        /// <value>The layer properties.</value>
        public TiledPropertyCollection Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledLayer"/> class.
        /// </summary>
        protected TiledLayer()
        {
            _properties = new TiledPropertyCollection();

            Name = string.Empty;
            Width = 0;
            Height = 0;
            Opacity = 1f;
            Visible = true;
        }
    }
}
