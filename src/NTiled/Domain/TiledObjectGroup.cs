using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
