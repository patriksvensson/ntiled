﻿// ﻿
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
using System.Collections.Generic;
using System.Drawing;

namespace NTiled
{
    /// <summary>
    /// Represents an object group.
    /// </summary>
    public sealed class TiledObjectGroup : TiledLayer
    {
        private List<TiledObject> _objects;

        /// <summary>
        /// Gets or sets the color used to display the objects in this group..
        /// </summary>
        /// <value>
        /// The color used to display the objects in this group..
        /// </value>
        public Color Color { get; set; }

        /// <summary>
        /// Gets the objects in the layer.
        /// </summary>
        /// <value>
        /// The objects in the layer.
        /// </value>
        public List<TiledObject> Objects
        {
            get { return _objects; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledObjectGroup"/> class.
        /// </summary>
        public TiledObjectGroup()
        {
            _objects = new List<TiledObject>();
        }
    }
}