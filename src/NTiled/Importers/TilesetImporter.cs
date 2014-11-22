// ﻿
// Copyright (c) 2014 Patrik Svensson
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

namespace NTiled.Importers
{
    internal static class TilesetImporter
    {
        public static void ImportTilesets(XDocument document, string basePath)
        {
            ImportTilesets(document, tilesetSource => XDocument.Load(Path.Combine(basePath, tilesetSource)));
        }

        public static void ImportTilesets(XDocument document, Func<string, XDocument> resolver)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            foreach (var tilesetElement in document.Root.GetElements("tileset"))
            {
                var tilesetSource = tilesetElement.ReadAttribute("source", string.Empty);
                if (!string.IsNullOrEmpty(tilesetSource))
                {
                    var xDocument = resolver(tilesetSource);
                    if (xDocument.Root == null)
                    {
                        throw new TiledException("External tileset reference is invalid.");
                    }

                    tilesetElement.SetAttributeValue("name", xDocument.Root.ReadAttribute("name", string.Empty));
                    tilesetElement.SetAttributeValue("tilewidth", xDocument.Root.ReadAttribute("tilewidth", 0));
                    tilesetElement.SetAttributeValue("tileheight", xDocument.Root.ReadAttribute("tileheight", 0));
                    tilesetElement.Add(xDocument.Root.DescendantNodes());
                }
            }
        }
    }
}
