using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTiled
{
    /// <summary>
    /// Represents a tile object.
    /// </summary>
    public sealed class TiledTileObject : TiledObject
    {
        /// <summary>
        /// Gets or sets an reference to a tile.
        /// </summary>
        /// <value>An reference to a tile.</value>
        public int Tile { get; set; }
    }
}
