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
using System.IO;
using System.Xml.Linq;
using NTiled.Importers;
using NTiled.Parsers;

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
            var root = document.GetDocumentRoot("map");
            var map = new TiledMap();

            // Read the different aspects of the map.
            HeaderParser.ReadHeader(map, root);
            PropertyParser.ReadProperties(map, root);
            TilesetParser.ReadTilesets(map, root);
            LayerParser.ReadLayers(map, root);

            return map;
        }

        /// <summary>
        /// Parses the specified map.
        /// </summary>
        /// <param name="filename">The filename of the external tile set.</param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        public TiledMap Read(string filename)
        {
            if (filename == null)
            {
                throw new ArgumentNullException("filename");
            }

            var document = XDocument.Load(filename);
            var basePath = new FileInfo(filename).DirectoryName;
            
            // Import external files into the main document
            TilesetImporter.ImportTilesets(document, basePath);

            return Read(document);
        }
    }
}
