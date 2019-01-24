// <copyright file="CheckerPatternDrawer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using static System.Drawing.Imaging.PixelFormat;
    using static Helper.ThrowHelper;

    /// <summary>
    /// Implements methods and properties to draw a checkerboard pattern on a
    /// <see cref="Bitmap"/>.
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
        /// An <see cref="IContainer"/> that represents the container for this
        /// <see cref="CheckerPatternDrawer"/>.
        /// </param>
        public CheckerPatternDrawer(IContainer container)
            : this(Color.Black, Color.White, 4, 4, container)
        {
        }

        /// <summary>
        /// Initializes a new instances of the <see cref="
        /// CheckerPatternDrawer"/> class with the specified colors, size, and
        /// <see cref="IContainer"/>.
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
        /// An <see cref="IContainer"/> that represents the container for this
        /// <see cref="CheckerPatternDrawer"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="width"/> or <paramref name="height"/> is less than
        /// or equal to zero.
        /// </exception>
        public CheckerPatternDrawer(
            Color color1,
            Color color2,
            int width,
            int height,
            IContainer container = null)
        {
            Width = width;
            Height = height;
            Color1 = color1;
            Color2 = color2;

            container?.Add(this);
        }

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
                _width = value > 0
                    ? value
                    : throw ValueNotGreaterThan(nameof(value), value);
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
                _height = value > 0
                    ? value
                    : throw ValueNotGreaterThan(nameof(value), value);
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
        /// Gets the width, in pixels, of the resulting <see cref=" Bitmap"/>
        /// created by <see cref="CreateCheckerImage"/>.
        /// </summary>
        private int ImageWidth
        {
            get
            {
                return Width * 2;
            }
        }

        /// <summary>
        /// Gets the height, in pixels, of the resulting <see cref=" Bitmap"/>
        /// created by <see cref="CreateCheckerImage"/>.
        /// </summary>
        private int ImageHeight
        {
            get
            {
                return Height * 2;
            }
        }

        /// <summary>
        /// Gets the checkerboard squares where <see cref="Color1"/> is filled
        /// in <see cref="FillCheckerRectangles(Image)"/>.
        /// </summary>
        private Rectangle[] Rectangles1
        {
            get
            {
                return new Rectangle[]
                {
                    new Rectangle(0, 0, Width, Height),
                    new Rectangle(Width, Height, Width, Height),
                };
            }
        }

        /// <summary>
        /// Gets the checkerboard squares where <see cref="Color2"/> is filled
        /// in <see cref="FillCheckerRectangles(Image)"/>.
        /// </summary>
        private Rectangle[] Rectangles2
        {
            get
            {
                return new Rectangle[]
                {
                    new Rectangle(Width, 0, Width, Height),
                    new Rectangle(Height, 0, Width, Height),
                };
            }
        }

        /// <summary>
        /// Create a <see cref="Bitmap"/> of a 2x2 checkerboard pattern.
        /// </summary>
        /// <returns>
        /// A <see cref="Bitmap"/> of a 2x2 checkerboard where there the
        /// top-left and bottom-right squares are <see cref=" Color1"/> and the
        /// top-right and bottom-left squares are <see cref="Color2"/>. The
        /// size of each square is <see cref=" Size"/>.
        /// </returns>
        public Bitmap CreateCheckerImage()
        {
            // See MSDN rule "CA2000: Dispose objects before losing scope" for
            // an explanation on using variables `temp` and `result`.
            // Basically, we ensure the object is still Disposed even if an
            // Exception occurs.
            Bitmap temp = null;
            Bitmap result = null;
            try
            {
                // Try to open the new `IDisposable` Bitmap object.
                temp = new Bitmap(
                    ImageWidth,
                    ImageHeight,
                    Format32bppArgb);

                // Try to add checkerboard squares to image.
                FillCheckerRectangles(temp);

                // Return `result` and make sure Bitmap is not Disposed by
                // `finally` statement if no Exceptions were raised.
                result = temp;
                temp = null;
                return result;
            }
            finally
            {
                // Ensure object is Disposed if an Exception was raised.
                temp?.Dispose();
            }
        }

        /// <summary>
        /// Fills the interiors of a series of rectangles with a specified <see
        /// cref="Color"/>.
        /// </summary>
        /// <param name="graphics">
        /// The <see cref="Graphics"/> objects to fill the rectangles in.
        /// </param>
        /// <param name="color">
        /// The <see cref="Color"/> to use for the <see cref=" SolidBrush"/> to
        /// fill the rectangles with.
        /// </param>
        /// <param name="rectangles">
        /// The rectangles to fill.
        /// </param>
        private static void FillRectangles(
            Graphics graphics,
            Color color,
            Rectangle[] rectangles)
        {
            using (var brush = new SolidBrush(color))
            {
                graphics.FillRectangles(brush, rectangles);
            }
        }

        /// <summary>
        /// Fill the checkerboard rectangles onto an <see cref=" Image"/>.
        /// </summary>
        /// <param name="image">
        /// The <see cref="Image"/> to fill the checkerboard rectangles onto.
        /// </param>
        private void FillCheckerRectangles(Image image)
        {
            using (var graphics = Graphics.FromImage(image))
            {
                FillRectangles(graphics, Color1, Rectangles1);
                FillRectangles(graphics, Color2, Rectangles2);
            }
        }
    }
}
