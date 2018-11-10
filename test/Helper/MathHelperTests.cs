// <copyright file="MathHelperTests.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Tests
{
    using System;
    using Xunit;
    using static MathHelper;

    public class MathHelperTests
    {
        private const float Infinity = Single.PositiveInfinity;
        private const float NaN = Single.NaN;
        private const float MaxValue = Single.MaxValue;
        private const float MinValue = Single.MinValue;
        private const float Epsilon = Single.Epsilon;

        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(0, 0, -Epsilon, false)]
        [InlineData(0, 1, 0, false)]
        [InlineData(1, 0, 0, false)]
        [InlineData(0, 1, 1, true)]
        [InlineData(1, 0, 1, true)]
        [InlineData(0, 0, NaN, false)]
        [InlineData(MaxValue, MaxValue, 0, true)]
        [InlineData(MinValue, MinValue, 0, true)]
        [InlineData(MinValue, MinValue, Infinity, true)]
        [InlineData(Infinity, -Infinity, Infinity, true)]
        [InlineData(Infinity, Infinity, 0, false)]
        [InlineData(-Infinity, -Infinity, 0, false)]
        [InlineData(NaN, NaN, 0, false)]
        [InlineData(NaN, NaN, NaN, false)]
        [InlineData(0, Epsilon, DefaultTolerance, true)]
        [InlineData(1E-8, -1E-8, 1E-7, true)]
        public void NearlyEquals(
            float left,
            float right,
            float tolerance,
            bool expectedResult)
        {
            var actualResult = MathHelper.NearlyEquals(
                left,
                right,
                tolerance);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(0, 0, -Epsilon, false)]
        [InlineData(0, 1, 0, true)]
        [InlineData(1, 0, 0, false)]
        [InlineData(0, 1, 1, true)]
        [InlineData(1, 0, 1, true)]
        [InlineData(0, 0, NaN, false)]
        [InlineData(MaxValue, MaxValue, 0, true)]
        [InlineData(MinValue, MinValue, 0, true)]
        [InlineData(MinValue, MinValue, Infinity, true)]
        [InlineData(Infinity, -Infinity, Infinity, true)]
        [InlineData(Infinity, Infinity, 0, false)]
        [InlineData(-Infinity, -Infinity, 0, false)]
        [InlineData(NaN, NaN, 0, false)]
        [InlineData(NaN, NaN, NaN, false)]
        [InlineData(0, Epsilon, DefaultTolerance, true)]
        [InlineData(1E-8, -1E-8, 1E-7, true)]
        public void LessThanOrNearlyEqualTo(
            float left,
            float right,
            float tolerance,
            bool expectedResult)
        {
            var actualResult = MathHelper.LessThanOrNearlyEqualTo(
                left,
                right,
                tolerance);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0, 0, 0, true)]
        [InlineData(0, 0, -Epsilon, false)]
        [InlineData(0, 1, 0, false)]
        [InlineData(1, 0, 0, true)]
        [InlineData(0, 1, 1, true)]
        [InlineData(1, 0, 1, true)]
        [InlineData(0, 0, NaN, false)]
        [InlineData(MaxValue, MaxValue, 0, true)]
        [InlineData(MinValue, MinValue, 0, true)]
        [InlineData(MinValue, MinValue, Infinity, true)]
        [InlineData(Infinity, -Infinity, Infinity, true)]
        [InlineData(Infinity, Infinity, 0, false)]
        [InlineData(-Infinity, -Infinity, 0, false)]
        [InlineData(NaN, NaN, 0, false)]
        [InlineData(NaN, NaN, NaN, false)]
        [InlineData(0, Epsilon, DefaultTolerance, true)]
        [InlineData(1E-8, -1E-8, 1E-7, true)]
        public void GreaterThanOrNearlyEqualTo(
            float left,
            float right,
            float tolerance,
            bool expectedResult)
        {
            var actualResult = MathHelper.GreaterThanOrNearlyEqualTo(
                left,
                right,
                tolerance);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1E-7, 0, 1E-7, 0)]
        [InlineData(-1E-7, 0, 1E-7, 0)]
        [InlineData(1E-7, 0, -1E-7, 1E-7)]
        [InlineData(1E-8, 0, 1E-7, 0)]
        [InlineData(1E-6, 0, 1E-7, 1E-6)]
        [InlineData(MaxValue, 0, MaxValue, 0)]
        [InlineData(Infinity, 0, MaxValue, Infinity)]
        [InlineData(Infinity, 0, Infinity, 0)]
        [InlineData(Infinity, 0, -Infinity, Infinity)]
        [InlineData(-Infinity, 0, Infinity, 0)]
        [InlineData(-Infinity, Infinity, Infinity, Infinity)]
        [InlineData(0, NaN, Infinity, 0)]
        [InlineData(0, 1, NaN, 0)]
        [InlineData(NaN, 0, Infinity, NaN)]
        [InlineData(NaN, NaN, NaN, NaN)]
        public void SnapToLimit(
            float value,
            float limit,
            float tolerance,
            float expectedResult)
        {
            var actualResult = MathHelper.SnapToLimit(
                value,
                limit,
                tolerance);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0, 0, 0, 0, 0)]
        [InlineData(0, 0, 1, 0, 0)]
        [InlineData(0, 1, 2, 0, 1)]
        [InlineData(2, 0, 1, 0, 1)]
        [InlineData(1, 0, 2, 0, 1)]
        [InlineData(0, 1, -1, 0, NaN)]
        [InlineData(0, 1, -1, Infinity, NaN)]
        [InlineData(1E-7, 0, 1, 1E-7, 0)]
        [InlineData(-1E-7, -1, 0, 1E-7, 0)]
        [InlineData(NaN, 0, 0, Infinity, NaN)]
        [InlineData(0, NaN, Infinity, Infinity, Infinity)]
        [InlineData(0, NaN, NaN, Infinity, 0)]
        public void Clamp(
            float value,
            float min,
            float max,
            float tolerance,
            float expectedResult)
        {
            var actualResult = MathHelper.Clamp(value, min, max, tolerance);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0, 0, 0, null)]
        [InlineData(0, 0, 0, new float[] { 0 })]
        [InlineData(1, 0, 1, null)]
        [InlineData(5, 0, 2, new float[] { 5, 2, 3, -1, 5, 2 })]
        [InlineData(NaN, 0, Infinity, new float[] { NaN, -Infinity })]
        public void Max(
            float expectedResult,
            float value1,
            float value2,
            float[] values)
        {
            var actualResult = MathHelper.Max(value1, value2, values);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData(0, 0, 0, null)]
        [InlineData(0, 0, 0, new float[] { 0 })]
        [InlineData(0, 0, 1, null)]
        [InlineData(-1, 0, 2, new float[] { 5, 2, 3, -1, 5, 2 })]
        [InlineData(NaN, 0, Infinity, new float[] { NaN, -Infinity })]
        public void Min(
            float expectedResult,
            float value1,
            float value2,
            float[] values)
        {
            var actualResult = MathHelper.Min(value1, value2, values);

            Assert.Equal(expectedResult, actualResult);
        }
    }
}
