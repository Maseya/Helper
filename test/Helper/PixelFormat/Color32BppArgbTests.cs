// <copyright file="Color32BppArgbTests.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.PixelFormat.Tests
{
    using System.Drawing;
    using Xunit;

    public class Color32BppArgbTests
    {
        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 0, 0x10000)]
        [InlineData(0, 1, 0, 0x100)]
        [InlineData(0, 0, 1, 1)]
        [InlineData(1, 1, 1, 0x10101)]
        [InlineData(255, 255, 255, 0xFFFFFF)]
        public void ConstructorRGB(
            int red,
            int green,
            int blue,
            int expectedValue)
        {
            var actualColor = new Color32BppArgb(red, green, blue);
            var actualValue = actualColor.Value;
            unchecked
            { expectedValue |= (int)0xFF000000; }
            var expectedColor = (Color32BppArgb)expectedValue;
            var expectedHashCode = expectedColor.GetHashCode();
            var actualHashCode = actualColor.GetHashCode();

            Assert.Equal(expectedValue, actualValue);

            Assert.True(expectedColor == actualColor);
            Assert.False(expectedColor != actualColor);

            Assert.True(expectedColor.Equals(actualColor));
            Assert.Equal(expectedHashCode, actualHashCode);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 0, 1, 0, 0, 0)]
        [InlineData(0, 1, 0, 0, 0, 1, 0, 0)]
        [InlineData(0, 0, 1, 0, 0, 0, 1, 0)]
        [InlineData(0, 0, 0, 1, 0, 0, 0, 1)]
        [InlineData(1, 2, 3, 4, 1, 2, 3, 4)]
        [InlineData(0x11F, 0x1F, 0xFF, 0x7F, 0x1F, 0x1F, 0xFF, 0x7F)]
        public void ToColor(
            int alpha,
            int red,
            int green,
            int blue,
            byte expectedAlpha,
            byte expectedRed,
            byte expectedGreen,
            byte expectedBlue)
        {
            var color32 = new Color32BppArgb(alpha, red, green, blue);
            var actualColor = (Color)color32;
            var actualAlpha = actualColor.A;
            var actualRed = actualColor.R;
            var actualGreen = actualColor.G;
            var actualBlue = actualColor.B;

            Assert.Equal(expectedAlpha, actualAlpha);
            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, 1, 1, 1, 1)]
        [InlineData(7, 7, 7, 7, 7, 7, 7, 7)]
        [InlineData(7, 8, 9, 6, 7, 8, 9, 6)]
        [InlineData(10, 20, 30, 40, 10, 20, 30, 40)]
        [InlineData(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF)]
        public void FromColor(
            byte alpha,
            byte red,
            byte green,
            byte blue,
            int expectedAlpha,
            int expectedRed,
            int expectedGreen,
            int expectedBlue)
        {
            var color = Color.FromArgb(alpha, red, green, blue);
            var actualColor = (Color32BppArgb)color;
            var actualAlpha = actualColor.Alpha;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedAlpha, actualAlpha);
            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 0, 0, 0)]
        [InlineData(7, 7, 7, 0, 0, 0)]
        [InlineData(7, 8, 9, 0, 1, 1)]
        [InlineData(10, 20, 30, 1, 2, 3)]
        [InlineData(0xFF, 0xFF, 0xFF, 0x1F, 0x1F, 0x1F)]
        public void ToColor15(
            byte red,
            byte green,
            byte blue,
            int expectedRed,
            int expectedGreen,
            int expectedBlue)
        {
            var color32 = new Color32BppArgb(red, green, blue);
            var actualColor = (Color15BppBgr)color32;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        // This is pretty tedious to test with floating point arithmetic.
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0, 0, 0)]
        [InlineData(255, 255, 255, 255, 1, 1, 1, 1)]
        public void ToColorF(
            int alpha,
            int red,
            int green,
            int blue,
            float expectedAlpha,
            float expectedRed,
            float expectedGreen,
            float expectedBlue)
        {
            var color32 = new Color32BppArgb(alpha, red, green, blue);
            var actualColor = (ColorF)color32;
            var actualAlpha = actualColor.Alpha;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedAlpha, actualAlpha, 10);
            Assert.Equal(expectedRed, actualRed, 10);
            Assert.Equal(expectedGreen, actualGreen, 10);
            Assert.Equal(expectedBlue, actualBlue, 10);
        }

        /// <summary>
        /// Assert that conversion from <see cref="Color32BppArgb"/> to <see
        /// cref="ColorF"/> back to <see cref="Color32BppArgb"/> doesn't create
        /// any rounding errors.
        /// </summary>
        [Fact]
        public void ColorFConsistency()
        {
            for (var i = 0; i < 0xFF; i++)
            {
                var expectedColor32 = new Color32BppArgb(i, i, i, i);
                var colorF = (ColorF)expectedColor32;
                var actualColor32 = (Color32BppArgb)colorF;

                Assert.Equal(expectedColor32, actualColor32);
            }
        }
    }
}
