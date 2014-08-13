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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;

namespace NTiled.Tests.Fixtures
{
    public sealed class ResourceFixture
    {
        private readonly IDictionary<string, XDocument> _cache;
        private readonly object _lock;

        public ResourceFixture()
        {
            _cache = new Dictionary<string, XDocument>(StringComparer.OrdinalIgnoreCase);
            _lock = new object();
        }

        public XDocument ReadMapDocument(string filename)
        {
            lock (_lock)
            {
                if (!_cache.ContainsKey(filename))
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var resourceName = string.Concat("NTiled.Tests.Data.", filename);

                    using (var stream = assembly.GetManifestResourceStream(resourceName))
                    {
                        if (stream == null)
                        {
                            const string format = "Could not read manifest resource stream for '{0}'.";
                            throw new InvalidOperationException(string.Format(format, resourceName));
                        }
                        using (var reader = new StreamReader(stream))
                        {
                            _cache.Add(filename, XDocument.Parse(reader.ReadToEnd()));
                        }
                    }
                }
                return _cache[filename];
            }
        }
    }
}
