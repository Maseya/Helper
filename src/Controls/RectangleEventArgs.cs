// <copyright file="RectangleEventArgs.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.Drawing;

    /// <summary>
    /// Provides data for sizing events in <see cref="DesignControl"/> and <see
    /// cref="DesignForm"/>.
    /// </summary>
    /// <remarks>
    /// This class can also be used to modify just <see cref="Size"/> or <see
    /// cref="Point"/> locations by using the appropriate constructor.
    /// </remarks>
    public class RectangleEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref=" RectangleEventArgs"/>
        /// class from the given location and size.
        /// </summary>
        /// <param name="location">
        /// The locations of the design item.
        /// </param>
        /// <param name="size">
        /// The size of the design item.
        /// </param>
        public RectangleEventArgs(
            Point location = default,
            Size size = default)
        {
            Location = location;
            Size = size;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref=" RectangleEventArgs"/>
        /// class from the given sizing rectangle.
        /// </summary>
        /// <param name="rectangle">
        /// The sizing rectangle of the design item.
        /// </param>
        public RectangleEventArgs(Rectangle rectangle)
        {
            Location = rectangle.Location;
            Size = rectangle.Size;
        }

        /// <summary>
        /// Gets or sets the sizing rectangle of the design item.
        /// </summary>
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(Location, Size);
            }

            set
            {
                Location = value.Location;
                Size = value.Size;
            }
        }

        /// <summary>
        /// Gets or sets the location of the design item.
        /// </summary>
        public Point Location
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the size of the design item.
        /// </summary>
        public Size Size
        {
            get;
            set;
        }
    }
}
