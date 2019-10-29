// <copyright file="CheckerPatternRenderer.cs" company="Public Domain">
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
    public class CheckerPatternRenderer : Component, IImageRenderer
    {
        private Color _color1;

        private Color _color2;

        private Size _size;

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// CheckerPatternRenderer"/> class.
        /// </summary>
        public CheckerPatternRenderer()
        {
            _color1 = Color.Black;
            _color2 = Color.White;
            _size = new Size(4, 4);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// CheckerPatternRenderer"/> class with the specified <see cref="
        /// IContainer"/>.
        /// </summary>
        /// <param name="container">
        /// An <see cref="IContainer"/> that represents the container for this
        /// <see cref="CheckerPatternRenderer"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="container"/> is <see langword="null"/>.
        /// </exception>
        public CheckerPatternRenderer(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }
        }

        public event EventHandler Color1Changed;

        public event EventHandler Color2Changed;

        public event EventHandler SizeChanged;

        public event EventHandler Redraw;

        /// <summary>
        /// Gets or sets the color of the first checkerboard square.
        /// </summary>
        [Category("Drawer")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("The color of the first checkerboard square.")]
        public Color Color1
        {
            get
            {
                return _color1;
            }

            set
            {
                if (Color1 == value)
                {
                    return;
                }

                _color1 = value;
                OnColor1Changed(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the color of the second checkerboard square.
        /// </summary>
        [Category("Drawer")]
        [DefaultValue(typeof(Color), "White")]
        [Description("The color of the second checkerboard square.")]
        public Color Color2
        {
            get
            {
                return _color2;
            }

            set
            {
                if (Color2 == value)
                {
                    return;
                }

                _color2 = value;
                OnColor2Changed(EventArgs.Empty);
            }
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
                return Size.Width;
            }

            set
            {
                if (Width == value)
                {
                    return;
                }

                _size.Width = value > 0
                    ? value
                    : throw ValueNotGreaterThan(nameof(value), value);

                OnSizeChanged(EventArgs.Empty);
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
                return Size.Height;
            }

            set
            {
                if (Height == value)
                {
                    return;
                }

                _size.Height = value > 0
                    ? value
                    : throw ValueNotGreaterThan(nameof(value), value);

                OnSizeChanged(EventArgs.Empty);
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
                return _size;
            }

            set
            {
                if (Size == value)
                {
                    return;
                }

                _size = value;
                OnSizeChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the width, in pixels, of the resulting <see cref=" Bitmap"/>
        /// created by <see cref="RenderImage"/>.
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
        /// created by <see cref="RenderImage"/>.
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
                    new Rectangle(0, Height, Width, Height),
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
        public Bitmap RenderImage()
        {
            // See MSDN rule "CA2000: Dispose objects before losing scope" for
            // an explanation on using variables `temp` and `result`.
            // Basically, we ensure the object is still Disposed even if an
            // Exception occurs.
            Bitmap temp = null;
            Bitmap result;
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

        Image IImageRenderer.RenderImage()
        {
            return RenderImage();
        }

        public void Draw(Graphics graphics)
        {
            if (graphics is null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            using (var image = RenderImage())
            {
                graphics.DrawImageUnscaled(image, Point.Empty);
            }
        }

        protected virtual void OnColor1Changed(EventArgs e)
        {
            Color1Changed?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnColor2Changed(EventArgs e)
        {
            Color2Changed?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnSizeChanged(EventArgs e)
        {
            SizeChanged?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnRedraw(EventArgs e)
        {
            Redraw?.Invoke(this, e);
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
