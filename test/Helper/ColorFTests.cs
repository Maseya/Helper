// <copyright file="ColorFTests.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Tests
{
    using System;
    using Xunit;

    public class ColorFTests
    {
        [Fact]
        public void FromArgbThrowNaNAlpha()
        {
            Assert.Throws<ArgumentException>(
                "alpha",
                () => ColorF.FromArgb(Single.NaN, 0, 0, 0));
        }

        [Fact]
        public void FromArgbThrowNaNRed()
        {
            Assert.Throws<ArgumentException>(
                "red",
                () => ColorF.FromArgb(0, Single.NaN, 0, 0));
        }

        [Fact]
        public void FromArgbThrowNaNGreen()
        {
            Assert.Throws<ArgumentException>(
                "green",
                () => ColorF.FromArgb(0, 0, Single.NaN, 0));
        }

        [Fact]
        public void FromArgbThrowNaNBlue()
        {
            Assert.Throws<ArgumentException>(
                "blue",
                () => ColorF.FromArgb(0, 0, 0, Single.NaN));
        }

        [Fact]
        public void FromCmyThrowNaNAlpha()
        {
            Assert.Throws<ArgumentException>(
                "alpha",
                () => ColorF.FromCmy(Single.NaN, 0, 0, 0));
        }

        [Fact]
        public void FromCmyThrowNaNCyan()
        {
            Assert.Throws<ArgumentException>(
                "cyan",
                () => ColorF.FromCmy(0, Single.NaN, 0, 0));
        }

        [Fact]
        public void FromCmyThrowNaNMagenta()
        {
            Assert.Throws<ArgumentException>(
                "magenta",
                () => ColorF.FromCmy(0, 0, Single.NaN, 0));
        }

        [Fact]
        public void FromCmyThrowNaNYellow()
        {
            Assert.Throws<ArgumentException>(
                "yellow",
                () => ColorF.FromCmy(0, 0, 0, Single.NaN));
        }

        [Fact]
        public void FromCmykThrowNaNAlpha()
        {
            Assert.Throws<ArgumentException>(
                "alpha",
                () => ColorF.FromCmyk(Single.NaN, 0, 0, 0, 0));
        }

        [Fact]
        public void FromCmykThrowNaNCyan()
        {
            Assert.Throws<ArgumentException>(
                "cyan",
                () => ColorF.FromCmyk(0, Single.NaN, 0, 0, 0));
        }

        [Fact]
        public void FromCmykThrowNaNMagenta()
        {
            Assert.Throws<ArgumentException>(
                "magenta",
                () => ColorF.FromCmyk(0, 0, Single.NaN, 0, 0));
        }

        [Fact]
        public void FromCmykThrowNaNYellow()
        {
            Assert.Throws<ArgumentException>(
                "yellow",
                () => ColorF.FromCmyk(0, 0, 0, Single.NaN, 0));
        }

        [Fact]
        public void FromCmykThrowNaNBlack()
        {
            Assert.Throws<ArgumentException>(
                "black",
                () => ColorF.FromCmyk(0, 0, 0, 0, Single.NaN));
        }

        [Fact]
        public void FromHcyThrowNaNAlpha()
        {
            Assert.Throws<ArgumentException>(
                "alpha",
                () => ColorF.FromHcy(Single.NaN, 0, 0, 0));
        }

        [Fact]
        public void FromHcyThrowNaNHue()
        {
            Assert.Throws<ArgumentException>(
                "hue",
                () => ColorF.FromHcy(0, Single.NaN, 0, 0));
        }

        [Fact]
        public void FromHcyThrowNaNChroma()
        {
            Assert.Throws<ArgumentException>(
                "chroma",
                () => ColorF.FromHcy(0, 0, Single.NaN, 0));
        }

        [Fact]
        public void FromHcyThrowNaNLuma()
        {
            Assert.Throws<ArgumentException>(
                "luma",
                () => ColorF.FromHcy(0, 0, 0, Single.NaN));
        }

        [Fact]
        public void FromHslThrowNaNAlpha()
        {
            Assert.Throws<ArgumentException>(
                "alpha",
                () => ColorF.FromHsl(Single.NaN, 0, 0, 0));
        }

        [Fact]
        public void FromHslThrowNaNHue()
        {
            Assert.Throws<ArgumentException>(
                "hue",
                () => ColorF.FromHsl(0, Single.NaN, 0, 0));
        }

        [Fact]
        public void FromHslThrowNaNChroma()
        {
            Assert.Throws<ArgumentException>(
                "saturation",
                () => ColorF.FromHsl(0, 0, Single.NaN, 0));
        }

        [Fact]
        public void FromHslThrowNaNLuma()
        {
            Assert.Throws<ArgumentException>(
                "lightness",
                () => ColorF.FromHsl(0, 0, 0, Single.NaN));
        }
    }
}
