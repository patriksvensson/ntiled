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
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using NTiled.Tests.Fixtures;
using Xunit;

namespace NTiled.Tests.Unit
{
    public class TiledReaderTests
    {
        public class TheReadMethod : IUseFixture<ResourceFixture>
        {
            private XDocument _map;

            public void SetFixture(ResourceFixture data)
            {
                _map = data.ReadMapDocument("_0._91.Correct.tmx");
            }

            [Fact]
            public void Should_Read_Map_Version()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(new Version(1, 0), result.Version);
            }

            [Fact]
            public void Should_Read_Map_Orientation()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal("orthogonal", result.Orientation);
            }

            [Fact]
            public void Should_Read_Map_Width()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(40, result.Width);
            }

            [Fact]
            public void Should_Read_Map_Height()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(30, result.Height);
            }

            [Fact]
            public void Should_Read_Map_Tile_Width()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(16, result.TileWidth);
            }

            [Fact]
            public void Should_Read_Map_Tile_Height()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(16, result.TileWidth);
            }

            [Fact]
            public void Should_Read_Map_Background_Color()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(Color.FromArgb(255, 170, 170, 127), result.BackgroundColor);
            }

            [Fact]
            public void Should_Read_Map_Properties()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(1, result.Properties.Count);
                Assert.Equal("Hello", result.Properties["MapProperty1"]);
            }

            [Fact]
            public void Should_Read_Tilesets()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(2, result.Tilesets.Count);
            }

            [Fact]
            public void Should_Read_Tileset_First_ID()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(1, result.Tilesets[0].FirstId);
            }

            [Fact]
            public void Should_Read_Tileset_Name()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal("Tileset", result.Tilesets[0].Name);
            }

            [Fact]
            public void Should_Read_Tileset_Tile_Width()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(16, result.Tilesets[0].TileWidth);
            }

            [Fact]
            public void Should_Read_Tileset_Tile_Height()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(16, result.Tilesets[0].TileHeight);
            }

            [Fact]
            public void Should_Read_Tileset_Margin()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(1, result.Tilesets[0].Margin);
            }

            [Fact]
            public void Should_Read_Tileset_Spacing()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(2, result.Tilesets[0].Spacing);
            }

            [Fact]
            public void Should_Read_Tileset_Image_Source()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal("graphics/tileset.png", result.Tilesets[0].Image.Source);
            }

            [Fact]
            public void Should_Read_Tileset_Image_Width()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(100, result.Tilesets[0].Image.Width);
            }

            [Fact]
            public void Should_Read_Tileset_Image_Height()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(50, result.Tilesets[0].Image.Height);
            }

            [Fact]
            public void Should_Read_Tileset_Offset()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(1, result.Tilesets[1].OffsetX);
                Assert.Equal(2, result.Tilesets[1].OffsetY);
            }

            [Fact]
            public void Should_Read_Tile_Layers()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(2, result.Layers.OfType<TiledTileLayer>().Count());
            }

            [Fact]
            public void Should_Read_Tile_Layer_Name()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal("Background", result.Layers[0].Name);
            }

            [Fact]
            public void Should_Read_Tile_Layer_X()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(1, result.Layers[0].X);
            }

            [Fact]
            public void Should_Read_Tile_Layer_Y()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(2, result.Layers[0].Y);
            }

            [Fact]
            public void Should_Read_Tile_Layer_Opacity()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(0.99f, result.Layers[0].Opacity);
            }

            [Fact]
            public void Should_Read_Tile_Layer_Visibility()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.True(result.Layers[0].Visible);
            }

            [Fact]
            public void Should_Read_Tile_Layer_Width()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(40, result.Layers[0].Width);
            }

            [Fact]
            public void Should_Read_Tile_Layer_Height()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(30, result.Layers[0].Height);
            }

            [Fact]
            public void Should_Read_Tile_Layer_Properties()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(1, result.Layers[0].Properties.Count);
                Assert.Equal("Test", result.Layers[0].Properties["LayerProperty1"]);
            }

            [Fact]
            public void Should_Read_Tile_Layer_Data_If_Compressed_With_GZip_And_Encoded_With_Base64()
            {
                // Given, When
                var result = new TiledReader().Read(_map);
                // Then
                Assert.Equal(1, ((TiledTileLayer)result.Layers[0]).Tiles.Count(x => x.Index != 0));
                Assert.Equal(2, ((TiledTileLayer)result.Layers[0]).Tiles.First(x => x.Index != 0).Index);
            }
        }
    }
}
