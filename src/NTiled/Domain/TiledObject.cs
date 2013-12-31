using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTiled
{
    /// <summary>
    /// Represents an object in an object group.
    /// </summary>
    public abstract class TiledObject
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
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the y coordinate of the object in pixels.
        /// </summary>
        /// <value>The y coordinate of the object in pixels.</value>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the width of the object in pixels.
        /// </summary>
        /// <value>The width of the object in pixels.</value>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the height of the object in pixels.
        /// </summary>
        /// <value>The height of the object in pixels.</value>
        public int Height { get; set; }

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
        public TiledObject()
        {
            _properties = new TiledPropertyCollection();
        }
    }
}
