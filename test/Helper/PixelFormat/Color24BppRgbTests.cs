// <copyright file="Color24BppRgbTests.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.PixelFormat.Tests
{
    using System.Drawing;
    using Xunit;

    public class Color24BppRgbTests
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
            var actualColor = new Color24BppRgb(red, green, blue);
            var actualValue = actualColor.Value;
            var expectedColor = (Color24BppRgb)expectedValue;
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
        [InlineData(1, 0, 0, 1, 0, 0)]
        [InlineData(0, 1, 0, 0, 1, 0)]
        [InlineData(0, 0, 1, 0, 0, 1)]
        [InlineData(1, 2, 3, 1, 2, 3)]
        [InlineData(0x1F, 0x1F, 0xFF, 0x1F, 0x1F, 0xFF)]
        public void ToColor(
            int red,
            int green,
            int blue,
            byte expectedRed,
            byte expectedGreen,
            byte expectedBlue)
        {
            var color24 = new Color24BppRgb(red, green, blue);
            var actualColor = (Color)color24;
            var actualRed = actualColor.R;
            var actualGreen = actualColor.G;
            var actualBlue = actualColor.B;

            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 1, 1, 1, 1, 1)]
        [InlineData(7, 7, 7, 7, 7, 7)]
        [InlineData(7, 8, 9, 7, 8, 9)]
        [InlineData(10, 20, 30, 10, 20, 30)]
        [InlineData(0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF)]
        public void FromColor(
            byte red,
            byte green,
            byte blue,
            int expectedRed,
            int expectedGreen,
            int expectedBlue)
        {
            var color = Color.FromArgb(red, green, blue);
            var actualColor = (Color24BppRgb)color;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

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
            var color24 = new Color24BppRgb(red, green, blue);
            var actualColor = (Color15BppBgr)color24;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(1, 0, 0, 1, 0, 0)]
        [InlineData(0, 1, 0, 0, 1, 0)]
        [InlineData(0, 0, 1, 0, 0, 1)]
        [InlineData(1, 2, 3, 1, 2, 3)]
        [InlineData(0x1F, 0x1F, 0xFF, 0x1F, 0x1F, 0xFF)]
        public void ToColor32(
            int red,
            int green,
            int blue,
            byte expectedRed,
            byte expectedGreen,
            byte expectedBlue)
        {
            var color24 = new Color24BppRgb(red, green, blue);
            var actualColor = (Color32BppArgb)color24;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedRed, actualRed);
            Assert.Equal(expectedGreen, actualGreen);
            Assert.Equal(expectedBlue, actualBlue);
        }

        // This is pretty tedious to test with floating point arithmetic.
        [Theory]
        [InlineData(0, 0, 0, 0, 0, 0)]
        [InlineData(255, 255, 255, 1, 1, 1)]
        public void ToColorF(
            int red,
            int green,
            int blue,
            float expectedRed,
            float expectedGreen,
            float expectedBlue)
        {
            var color24 = new Color24BppRgb(red, green, blue);
            var actualColor = (ColorF)color24;
            var actualRed = actualColor.Red;
            var actualGreen = actualColor.Green;
            var actualBlue = actualColor.Blue;

            Assert.Equal(expectedRed, actualRed, 10);
            Assert.Equal(expectedGreen, actualGreen, 10);
            Assert.Equal(expectedBlue, actualBlue, 10);
        }

        /// <summary>
        /// Assert that conversion from <see cref="Color24BppRgb"/> to <see
        /// cref="ColorF"/> back to <see cref="Color24BppRgb"/> doesn't create
        /// any rounding errors.
        /// </summary>
        [Fact]
        public void ColorFConsistency()
        {
            for (var i = 0; i < 0xFF; i++)
            {
                var expectedColor24 = new Color24BppRgb(i, i, i);
                var colorF = (ColorF)expectedColor24;
                var actualColor24 = (Color24BppRgb)colorF;

                Assert.Equal(expectedColor24, actualColor24);
            }
        }
    }
}
