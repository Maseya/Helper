// <copyright file="AnimatedLineDrawer.cs" company="Public Domain">
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
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;
    using static Helper.ThrowHelper;

    /// <summary>
    /// Implements methods and properties to draw an animated dashed
    /// line across a <see cref="GraphicsPath"/>.
    /// </summary>
    public class AnimatedLineDrawer : Component
    {
        /// <summary>
        /// The length of the first dashed line.
        /// </summary>
        private int _length1;

        /// <summary>
        /// The length of second dashed line.
        /// </summary>
        private int _length2;

        /// <summary>
        /// Occurs every <see cref="Interval"/> milliseconds when this
        /// <see cref="AnimatedLineDrawer"/> is constructed.
        /// </summary>
        [Category("Animator")]
        [Description("Occurs when the specified timer interval has" +
            "elapsed.")]
        public event EventHandler Tick;

        /// <summary>
        /// Gets or sets the initial offset of the first dashed line.
        /// </summary>
        private int Offset
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the length of the first dashed line.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Property is set to value less than zero.
        /// </exception>
        [Category("Animator")]
        [DefaultValue(1)]
        [Description("The length of the first dashed line.")]
        public int Length1
        {
            get
            {
                return _length1;
            }

            set
            {
                _length1 = value > 0
                    ? value
                    : throw ValueNotGreaterThan(nameof(value), value);
            }
        }

        /// <summary>
        /// Gets or sets the length of the second dashed line.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Property is set to value less than zero.
        /// </exception>
        [Category("Animator")]
        [DefaultValue(1)]
        [Description("The length of the second dashed line.")]
        public int Length2
        {
            get
            {
                return _length2;
            }

            set
            {
                _length2 = value > 0
                    ? value
                    : throw ValueNotGreaterThan(nameof(value), value);
            }
        }

        /// <summary>
        /// Gets or sets the color of the first dashed line.
        /// </summary>
        [Category("Animator")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("The color of the first dashed line.")]
        public Color Color1
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the second dashed line.
        /// </summary>
        [Category("Animator")]
        [DefaultValue(typeof(Color), "White")]
        [Description("The color of the second dashed line.")]
        public Color Color2
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the timer that determines that determines the animation
        /// frequency of the dashed lines.
        /// </summary>
        private Timer Timer
        {
            get;
        }

        /// <summary>
        /// Gets or sets the time, in milliseconds, before the next
        /// update of the line animation.
        /// </summary>
        [Category("Animator")]
        [DefaultValue(1000)]
        [Description("The time, in milliseconds, before the next" +
            "update of the line animation.")]
        public int Interval
        {
            get
            {
                return Timer.Interval;
            }

            set
            {
                Timer.Interval = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISite"/> of the <see cref="
        /// Component"/>.
        /// </summary>
        public override ISite Site
        {
            get
            {
                return Timer.Site;
            }

            set
            {
                Timer.Site = value;
            }
        }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the
        /// <see cref="AnimatedLineDrawer"/>.
        /// </summary>
        private object SyncRoot
        {
            get;
        }

        /// <summary>
        /// Gets the <see cref="Pen.DashPattern"/> that will be used
        /// for the first <see cref="Pen"/>.
        /// </summary>
        private float[] DashPattern1
        {
            get
            {
                return new float[] { Length1, Length2 };
            }
        }

        /// <summary>
        /// Gets the <see cref="Pen.DashPattern"/> that will be used
        /// for the second <see cref="Pen"/>.
        /// </summary>
        private float[] DashPattern2
        {
            get
            {
                return new float[] { Length2, Length1 };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// AnimatedLineDrawer"/> class.
        /// </summary>
        public AnimatedLineDrawer()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// AnimatedLineDrawer"/> class with the specified <see cref="
        /// IContainer"/>.
        /// </summary>
        /// <param name="container">
        /// An <see cref="IContainer"/> that represents the container
        /// for this <see cref="AnimatedLineDrawer"/>.
        /// </param>
        public AnimatedLineDrawer(IContainer container)
            : this(1, 1, Color.Black, Color.White, 1000, container)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// AnimatedLineDrawer"/> class with the specified lengths,
        /// colors, interval, and <see cref="IContainer"/>.
        /// </summary>
        /// <param name="length1">
        /// The length of the first dashed line.
        /// </param>
        /// <param name="length2">
        /// The length of the second dashed line.
        /// </param>
        /// <param name="color1">
        /// The color of the first dashed line.
        /// </param>
        /// <param name="color2">
        /// The color of the second dashed line.
        /// </param>
        /// <param name="interval">
        /// The time, in milliseconds, before the next update of the
        /// line animation.
        /// </param>
        /// <param name="container">
        /// An <see cref="IContainer"/> that represents the container
        /// for this <see cref="AnimatedLineDrawer"/>.
        /// </param>
        public AnimatedLineDrawer(
            int length1,
            int length2,
            Color color1,
            Color color2,
            int interval,
            IContainer container = null)
        {
            Length1 = length1;
            Length2 = length2;
            Color1 = color1;
            Color2 = color2;
            SyncRoot = new object();

            Timer = container is null
                ? new Timer()
                : new Timer(container);

            Timer.Interval = interval;
            Timer.Tick += (sender, e) =>
            {
                OnTick(e);
                Offset++;
            };

            Timer.Start();
        }

        /// <summary>
        /// Draws a <see cref="GraphicsPath"/> to a <see cref="
        /// Graphics"/> using an animated dashed line described by this
        /// <see cref="AnimatedLineDrawer"/>.
        /// </summary>
        /// <param name="graphics">
        /// The <see cref="Graphics"/> to draw to.
        /// </param>
        /// <param name="path">
        /// The <see cref="GraphicsPath"/> to draw to <paramref name="
        /// graphics"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graphics"/> or <paramref name="path"/> is
        /// <see langword="null"/>.
        /// </exception>
        public void DrawPath(Graphics graphics, GraphicsPath path)
        {
            if (graphics is null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            DrawPathInternal(
                graphics,
                path,
                Color1,
                Offset,
                DashPattern1);

            DrawPathInternal(
                graphics,
                path,
                Color2,
                Offset + Length1,
                DashPattern2);
        }

        /// <summary>
        /// Raises the <see cref="Tick"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnTick(EventArgs e)
        {
            Tick?.Invoke(this, e);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="
        /// AnimatedLineDrawer"/> and optionally releases the managed
        /// resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to released both managed and
        /// unmanaged resources; <see langword="false"/> to release only
        /// unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Timer.Dispose();
            }

            base.Dispose(disposing);
        }

        private void DrawPathInternal(
            Graphics graphics,
            GraphicsPath path,
            Color color,
            int dashOffset,
            float[] dashPattern)
        {
            using (var pen = new Pen(color))
            {
                pen.DashStyle = DashStyle.Custom;
                pen.DashOffset = dashOffset;
                pen.DashPattern = dashPattern;

                lock (SyncRoot)
                {
                    graphics.DrawPath(pen, path);
                }
            }
        }
    }
}
