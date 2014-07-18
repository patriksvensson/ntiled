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
    /// Represents an object in an object group.
    /// </summary>
    public abstract class TiledObject : IHasProperties
    {
        private readonly TiledPropertyCollection _properties;

        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        /// <value>The name of the object.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>The type of the object.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the x coordinate of the object in pixels.
        /// </summary>
        /// <value>The x coordinate of the object in pixels.</value>
        public decimal X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate of the object in pixels.
        /// </summary>
        /// <value>The y coordinate of the object in pixels.</value>
        public decimal Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the object in pixels.
        /// </summary>
        /// <value>The width of the object in pixels.</value>
        public decimal Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the object in pixels.
        /// </summary>
        /// <value>The height of the object in pixels.</value>
        public decimal Height { get; set; }

        /// <summary>
        /// Gets the object properties.
        /// </summary>
        /// <value>
        /// The object properties.
        /// </value>
        public TiledPropertyCollection Properties
        {
            get { return _properties; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TiledObject"/> class.
        /// </summary>
        protected TiledObject()
        {
            _properties = new TiledPropertyCollection();
        }
    }
}
