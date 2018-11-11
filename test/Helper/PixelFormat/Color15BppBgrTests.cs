// <copyright file="Color15BppBgrTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.PixelFormat.Tests
{
    using System.Drawing;
    using Xunit;

    public class Color15BppBgrTests
    {
        [Theory]
        [InlineData(0, 0, 0)]
        [InlineData(0, 1, 0x100)]
        [InlineData(1, 0, 1)]
        [InlineData(0xFF, 0xFF, 0xFFFF)]
        public void ConstructorLowHigh(
            byte low,
            byte high,
            int expectedValue)
        {
            var actualColor = new Color15BppBgr(low, high);
            var actualValue = actualColor.Value;
            var expectedColor = (Color15BppBgr)expectedValue;
            var expectedHashCode = expectedColor.GetHashCode();
            var actualHashCode = actualColor.GetHashCode();

            Assert.Equal(expectedValue, actualValue);

            Assert.True(expectedColor == actualColor);
            Assert.False(expectedColor != actualColor);

            Assert.True(expectedColor.Equals(actualColor));
            Assert.Equal(expectedHashCode, actualHashCode);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 0, 1)]
        [InlineData(0, 1, 0, 0x20)]
        [InlineData(0, 0, 1, 0x400)]
        [InlineData(1, 1, 1, 0x421)]
        [InlineData(0x1F, 0, 0, 0x1F)]
        [InlineData(0, 0x1F, 0, 0x3E0)]
        [InlineData(0, 0, 0x1F, 0x7C00)]
        [InlineData(0x1F, 0x1F, 0x1F, 0x7FFF)]
        [InlineData(0x20, 0x20, 0x20, 0)]
        [InlineData(0xFF, 0xFF, 0xFF, 0x7FFF)]
        public void ConstructorRGB(
            int red,
            int green,
            int blue,
            int expectedValue)
        {
            var actualColor = new Color15BppBgr(red, green, blue);
            var actualValue = actualColor.Value;
            var expectedColor = (Color15BppBgr)expectedValue;
            var expectedHashCode = expectedColor.GetHashCode();
            var actualHashCode = actualColor.GetHashCode();

            Assert.Equal(expectedValue, actualValue);

            Assert.True(expectedColor == actualColor);
            Assert.False(expectedColor != actualColor);

            Assert.True(expectedColor.Equals(actualColor));
            Assert.Equal(expectedHashCode, actualHashCode);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 8, 0, 0)]
        [InlineData(0, 1, 0, 0, 8, 0)]
        [InlineData(0, 0, 1, 0, 0, 8)]
        [InlineData(1, 2, 3, 8, 16, 24)]
        [InlineData(0x1F, 0x1F, 0xFF, 0xF8, 0xF8, 0xF8)]
        public void ToColor(
            int red,
            int green,
            int blue,
            byte expectedRed,
            byte expectedGreen,
            byte expectedBlue)
        {
            var color15 = new Color15BppBgr(red, green, blue);
            var actualColor = (Color)color15;
            var actualRed = actualColor.R;
            var actualGreen = actualColor.G;
            var actualBlue = actualColor.B;

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
        public void FromColor(
            byte red,
            byte green,
            byte blue,
            int expectedRed,
            int expectedGreen,
            int expectedBlue)
        {
            var color = Color.FromArgb(red, green, blue);
            var actualColor = (Color15BppBgr)color;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 8, 0, 0)]
        [InlineData(0, 1, 0, 0, 8, 0)]
        [InlineData(0, 0, 1, 0, 0, 8)]
        [InlineData(1, 2, 3, 8, 16, 24)]
        [InlineData(0x1F, 0x1F, 0xFF, 0xF8, 0xF8, 0xF8)]
        public void ToColor24(
            int red,
            int green,
            int blue,
            byte expectedRed,
            byte expectedGreen,
            byte expectedBlue)
        {
            var color15 = new Color15BppBgr(red, green, blue);
            var actualColor = (Color24BppRgb)color15;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 8, 0, 0)]
        [InlineData(0, 1, 0, 0, 8, 0)]
        [InlineData(0, 0, 1, 0, 0, 8)]
        [InlineData(1, 2, 3, 8, 16, 24)]
        [InlineData(0x1F, 0x1F, 0xFF, 0xF8, 0xF8, 0xF8)]
        public void ToColor32(
            int red,
            int green,
            int blue,
            byte expectedRed,
            byte expectedGreen,
            byte expectedBlue)
        {
            var color15 = new Color15BppBgr(red, green, blue);
            var actualColor = (Color32BppArgb)color15;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        // This is pretty tedious to test with floating point
        // arithmetic.
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        public void ToColorF(
            int red,
            int green,
            int blue,
            float expectedRed,
            float expectedGreen,
            float expectedBlue)
        {
            var color15 = new Color15BppBgr(red, green, blue);
            var actualColor = (ColorF)color15;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedRed, actualRed, 10);
            Assert.Equal(expectedGreen, actualGreen, 10);
            Assert.Equal(expectedBlue, actualBlue, 10);
        }

        /// <summary>
        /// Assert that conversion from <see cref="Color15BppBgr"/> to
        /// <see cref="ColorF"/> back to <see cref="Color15BppBgr"/>
        /// doesn't create any rounding errors.
        /// </summary>
        [Fact]
        public void ColorFConsistency()
        {
            for (var i = 0; i < 0x7FFF; i++)
            {
                var expectedColor15 = (Color15BppBgr)i;
                var colorF = (ColorF)expectedColor15;
                var actualColor15 = (Color15BppBgr)colorF;

                Assert.Equal(expectedColor15, actualColor15);
            }
        }
    }
}
