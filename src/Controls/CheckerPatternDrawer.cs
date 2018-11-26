// <copyright file="CheckerPatternDrawer.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using static Helper.ThrowHelper;

    /// <summary>
    /// Implements methods and properties to draw a checkerboard pattern
    /// on a <see cref="Bitmap"/>.
    /// </summary>
    public class CheckerPatternDrawer : Component
    {
        /// <summary>
        /// The width of the checkerboard squares.
        /// </summary>
        private int _width;

        /// <summary>
        /// The height of the checkerboard squares.
        /// </summary>
        private int _height;

        /// <summary>
        /// Gets or sets the color of the first checkerboard square.
        /// </summary>
        [Category("Drawer")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("The color of the first checkerboard square.")]
        public Color Color1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the second checkerboard square.
        /// </summary>
        [Category("Drawer")]
        [DefaultValue(typeof(Color), "White")]
        [Description("The color of the second checkerboard square.")]
        public Color Color2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the checkerboard squares.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Property is set to value less than or equal to zero.
        /// </exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int Width
        {
            get
            {
                return _width;
            }

            set
            {
                if (value <= 0)
                {
                    throw ValueNotGreaterThan(
                        nameof(value),
                        value);
                }

                _width = value;
            }
        }

        /// <summary>
        /// Gets or sets the height of the checkerboard squares.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Property is set to value less than or equal to zero.
        /// </exception>
        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public int Height
        {
            get
            {
                return _height;
            }

            set
            {
                if (value <= 0)
                {
                    throw ValueNotGreaterThan(
                        nameof(value),
                        value);
                }

                _height = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the checkerboard squares.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Property is set to value less than or equal to zero.
        /// </exception>
        [Description("The size of the checkerboard squares.")]
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
        /// Initializes a new instance of the <see cref="
        /// CheckerPatternDrawer"/> class.
        /// </summary>
        public CheckerPatternDrawer()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// CheckerPatternDrawer"/> class with the specified <see cref="
        /// IContainer"/>.
        /// </summary>
        /// <param name="container">
        /// An <see cref="IContainer"/> that represents the container
        /// for this <see cref="CheckerPatternDrawer"/>.
        /// </param>
        public CheckerPatternDrawer(IContainer container)
            : this(Color.Black, Color.White, 4, 4, container)
        {
        }

        /// <summary>
        /// Initializes a new instances of the <see cref="
        /// CheckerPatternDrawer"/> class with the specified colors,
        /// size, and <see cref="IContainer"/>.
        /// </summary>
        /// <param name="color1">
        /// The color of the first checkerboard square.
        /// </param>
        /// <param name="color2">
        /// The color of the second checkerboard square.
        /// </param>
        /// <param name="width">
        /// The width of the checkerboard squares.
        /// </param>
        /// <param name="height">
        /// The height of the checkerboard squares.
        /// </param>
        /// <param name="container">
        /// An <see cref="IContainer"/> that represents the container
        /// for this <see cref="CheckerPatternDrawer"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> or <paramref name="height"/> is
        /// less than or equal to zero.
        /// </exception>
        public CheckerPatternDrawer(
            Color color1,
            Color color2,
            int width,
            int height,
            IContainer container = null)
        {
            if (width <= 0)
            {
                throw ValueNotGreaterThan(
                    nameof(width),
                    width);
            }

            if (height <= 0)
            {
                throw ValueNotLessThan(
                    nameof(height),
                    height);
            }

            Color1 = color1;
            Color2 = color2;
            Width = width;
            Height = height;

            container?.Add(this);
        }

        /// <summary>
        /// Create a <see cref="Bitmap"/> of a 2x2 checkerboard pattern.
        /// </summary>
        /// <returns>
        /// A <see cref="Bitmap"/> of a 2x2 checkerboard where there
        /// the top-left and bottom-right squares are <see cref="
        /// Color1"/> and the top-right and bottom-left squares are
        /// <see cref="Color2"/>. The size of each square is <see cref="
        /// Size"/>.
        /// </returns>
        public Bitmap CreateCheckerImage()
        {
            var bitmap = new Bitmap(Width * 2, Height * 2);

            // Squares for first color.
            var rectangles1 = new Rectangle[]
            {
                    new Rectangle(0, 0, Width, Height),
                    new Rectangle(Width, Height, Width, Height),
            };

            // Squares for second color.
            var rectangles2 = new Rectangle[]
            {
                    new Rectangle(Width, 0, Width, Height),
                    new Rectangle(Height, 0, Width, Height),
            };

            using (var graphics = Graphics.FromImage(bitmap))
            using (var brush1 = new SolidBrush(Color1))
            using (var brush2 = new SolidBrush(Color2))
            {
                graphics.FillRectangles(brush1, rectangles1);
                graphics.FillRectangles(brush2, rectangles2);
            }

            return bitmap;
        }
    }
}
