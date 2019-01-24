// <copyright file="WinApiRectangle.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using static Helper.StringHelper;

    /// <summary>
    /// A rectangle structure whose data layout is consistent with the
    /// RECTANGLE struct used in the Windows API.
    /// </summary>
    /// <remarks>
    /// This data structure can be passed to WinAPI methods because its
    /// internal data storage is the same as the RECTANGLE struct that Windows
    /// API uses for sizing procedures.
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct WinApiRectangle : IEquatable<WinApiRectangle>
    {
        /// <summary>
        /// Represents a <see cref="WinApiRectangle"/> at the origin with no
        /// size.
        /// </summary>
        public static readonly WinApiRectangle Empty = default;

        /// <summary>
        /// Initializes a new instance of the <see cref=" WinApiRectangle"/>
        /// struct using the specified edge locations.
        /// </summary>
        /// <param name="left">
        /// The X-coordinate of the left edge of this <see cref="
        /// WinApiRectangle"/> structure.
        /// </param>
        /// <param name="top">
        /// The Y-coordinate of the top edge of this <see cref="
        /// WinApiRectangle"/> structure.
        /// </param>
        /// <param name="right">
        /// The X-coordinate of the right edge of this <see cref="
        /// WinApiRectangle"/> structure.
        /// </param>
        /// <param name="bottom">
        /// The Y-coordinate of the bottom edge of this <see cref="
        /// WinApiRectangle"/> structure.
        /// </param>
        public WinApiRectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Gets or sets the X-coordinate of the left edge of this <see
        /// cref="WinApiRectangle"/> structure.
        /// </summary>
        public int Left
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the top edge of this <see
        /// cref="WinApiRectangle"/> structure.
        /// </summary>
        public int Top
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the X-coordinate of the right edge of this <see
        /// cref="WinApiRectangle"/> structure.
        /// </summary>
        public int Right
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the bottom edge of this <see
        /// cref="WinApiRectangle"/> structure.
        /// </summary>
        public int Bottom
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the X-coordinate of the upper-left corner of this <see
        /// cref="WinApiRectangle"/> structure.
        /// </summary>
        public int X
        {
            get
            {
                return Left;
            }

            set
            {
                Left = value;
            }
        }

        /// <summary>
        /// Gets or sets the Y-coordinate of the upper-left corner of this <see
        /// cref="WinApiRectangle"/> structure.
        /// </summary>
        public int Y
        {
            get
            {
                return Top;
            }

            set
            {
                Top = value;
            }
        }

        /// <summary>
        /// Gets or sets the coordinates of the upper-left corner of this <see
        /// cref="WinApiRectangle"/> structure.
        /// </summary>
        public Point Location
        {
            get
            {
                return new Point(X, Y);
            }

            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets the width of this <see cref="WinApiRectangle"/>
        /// structure.
        /// </summary>
        public int Width
        {
            get
            {
                return Right - Left;
            }

            set
            {
                Right = value + Left;
            }
        }

        /// <summary>
        /// Gets or sets the height of this <see cref=" WinApiRectangle"/>
        /// structure.
        /// </summary>
        public int Height
        {
            get
            {
                return Bottom - Top;
            }

            set
            {
                Bottom = value + Top;
            }
        }

        /// <summary>
        /// Gets or sets the size of this <see cref="WinApiRectangle"/>
        /// structure.
        /// </summary>
        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }

            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }

        /// <summary>
        /// Creates a <see cref="WinApiRectangle"/> structure whose <see
        /// cref="Left"/>, <see cref="Top"/>, <see cref="Right"/>, and <see
        /// cref="Bottom"/> properties are equal to the properties of the same
        /// name of a <see cref="Rectangle"/> structure.
        /// </summary>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="WinApiRectangle"/> whose <see cref="Left"/>, <see
        /// cref="Top"/>, <see cref="Right"/>, and <see cref=" Bottom"/>
        /// properties are the same as the properties of the same name of
        /// <paramref name="rectangle"/>.
        /// </returns>
        public static implicit operator WinApiRectangle(
            Rectangle rectangle)
        {
            return new WinApiRectangle(
                rectangle.Left,
                rectangle.Top,
                rectangle.Right,
                rectangle.Bottom);
        }

        /// <summary>
        /// Creates a <see cref="Rectangle"/> whose <see cref="
        /// Rectangle.Location"/> and <see cref="Rectangle.Size"/> properties
        /// are the same as the <see cref="Location"/> and <see cref="Size"/>
        /// properties of a <see cref=" WinApiRectangle"/>.
        /// </summary>
        /// <param name="rectangle">
        /// The <see cref="WinApiRectangle"/> to convert.
        /// </param>
        /// <returns>
        /// A <see cref="Rectangle"/> whose <see cref="Rectangle. Location"/>
        /// and <see cref="Rectangle.Size"/> properties are the same as the
        /// <see cref="Location"/> and <see cref=" Size"/> properties of
        /// <paramref name="rectangle"/>.
        /// </returns>
        public static implicit operator Rectangle(
            WinApiRectangle rectangle)
        {
            return Rectangle.FromLTRB(
                rectangle.Left,
                rectangle.Top,
                rectangle.Right,
                rectangle.Bottom);
        }

        /// <summary>
        /// Compares two <see cref="WinApiRectangle"/> values. The result
        /// specifies whether the values of the <see cref=" Left"/>, <see
        /// cref="Top"/>, <see cref="Right"/>, and <see cref="Bottom"/>
        /// properties of the two <see cref=" WinApiRectangle"/> are equal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="WinApiRectangle"/> to compare to <paramref
        /// name="right"/>.
        /// </param>
        /// <param name="right">
        /// A <see cref="WinApiRectangle"/> to compare to <paramref
        /// name="left"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Left"/>, <see
        /// cref="Top"/>, <see cref="Right"/>, and <see cref="Bottom"/> values
        /// of <paramref name="left"/> and <paramref name=" right"/> are equal;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator ==(
            WinApiRectangle left,
            WinApiRectangle right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two <see cref="WinApiRectangle"/> values. The result
        /// specifies whether the values of the <see cref=" Left"/>, <see
        /// cref="Top"/>, <see cref="Right"/>, or <see cref="Bottom"/>
        /// properties of the two <see cref=" WinApiRectangle"/> are unequal.
        /// </summary>
        /// <param name="left">
        /// A <see cref="WinApiRectangle"/> to compare to <paramref
        /// name="right"/>.
        /// </param>
        /// <param name="right">
        /// A <see cref="WinApiRectangle"/> to compare to <paramref
        /// name="left"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <see cref="Left"/>, <see
        /// cref="Top"/>, <see cref="Right"/>, and <see cref="Bottom"/> values
        /// of <paramref name="left"/> or <paramref name=" right"/> are
        /// unequal; otherwise, <see langword="false"/>.
        /// </returns>
        public static bool operator !=(
            WinApiRectangle left,
            WinApiRectangle right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified <see cref="WinApiRectangle"/> value.
        /// </summary>
        /// <param name="other">
        /// A <see cref="WinApiRectangle"/> value to compare to this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="other"/> has the same
        /// values as this instance; otherwise <see langword=" false"/>.
        /// </returns>
        public bool Equals(WinApiRectangle other)
        {
            return
                Left.Equals(other.Left) &&
                Top.Equals(other.Top) &&
                Right.Equals(other.Right) &&
                Bottom.Equals(other.Bottom);
        }

        /// <summary>
        /// Returns a value indicating whether this instance is equal to a
        /// specified object.
        /// </summary>
        /// <param name="obj">
        /// An object to compare with this instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="obj"/> is an instance of
        /// <see cref="WinApiRectangle"/> and equals the value of this
        /// instance; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj is WinApiRectangle other
                ? Equals(other)
                : false;
        }

        /// <summary>
        /// Returns the hash code for this <see cref=" WinApiRectangle"/>
        /// structure.
        /// </summary>
        /// <returns>
        /// An integer that represents the hash code for this rectangle.
        /// </returns>
        /// <remarks>
        /// The hash code for this <see cref="WinApiRectangle"/> is the same
        /// value as the hash code for its <see cref="Rectangle"/> equivalent.
        /// </remarks>
        public override int GetHashCode()
        {
            return ((Rectangle)this).GetHashCode();
        }

        /// <summary>
        /// Converts the attributes of this <see cref=" WinApiRectangle"/> to a
        /// human-readable string.
        /// </summary>
        /// <returns>
        /// The string representation of the <see cref="WinApiRectangle"/>.
        /// </returns>
        public override string ToString()
        {
            return GetString(
                "{{L:{0},T:{1},R:{2},B:{3}}}",
                Left,
                Top,
                Right,
                Bottom);
        }
    }
}
