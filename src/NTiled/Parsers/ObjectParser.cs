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
using System.Xml.Linq;

namespace NTiled.Parsers
{
    internal static class ObjectParser
    {
        public static void ReadTileObject(TiledObjectGroup layer, XElement root)
        {
            var obj = new TiledTileObject();

            // Read generic object information.
            ReadGenericObjectInformation(obj, root);

            // Read tile specific stuff.
            obj.Tile = root.ReadAttribute("gid", 0);

            layer.Objects.Add(obj);
        }

        public static void ReadRectangleObject(TiledObjectGroup layer, XElement root)
        {
            var obj = new TiledRectangleObject();

            // Read generic object information.
            ReadGenericObjectInformation(obj, root);

            layer.Objects.Add(obj);
        }

        private static void ReadGenericObjectInformation(TiledObject obj, XElement root)
        {
            obj.Name = root.ReadAttribute("name", string.Empty);
            obj.Type = root.ReadAttribute("type", string.Empty);
            obj.X = root.ReadAttribute("x", 0);
            obj.Y = root.ReadAttribute("y", 0);
            obj.Width = root.ReadAttribute("width", 0);
            obj.Height = root.ReadAttribute("height", 0);

            // Read object properties.
            PropertyParser.ReadProperties(obj, root);
        }
    }
}
