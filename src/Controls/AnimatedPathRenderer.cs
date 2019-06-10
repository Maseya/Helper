// <copyright file="AnimatedPathRenderer.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
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
    /// Implements methods and properties to draw an animated dashed line
    /// across a <see cref="GraphicsPath"/>.
    /// </summary>
    public class AnimatedPathRenderer : Component, IPathRenderer
    {
        /// <summary>
        /// The length of the first dashed line.
        /// </summary>
        private int _length1;

        /// <summary>
        /// The length of second dashed line.
        /// </summary>
        private int _length2;

        private Color _color1;

        private Color _color2;

        private int _interval;

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// AnimatedPathRenderer"/> class.
        /// </summary>
        public AnimatedPathRenderer()
        {
            _length1 = 1;
            _length2 = 1;
            _color1 = Color.Black;
            _color2 = Color.White;
            Timer = new Timer()
            {
                Interval = 1000,
            };

            Timer.Tick += Timer_Tick;

            if (Site is null || !Site.DesignMode)
            {
                Timer.Start();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// AnimatedPathRenderer"/> class with the specified <see cref="
        /// IContainer"/>.
        /// </summary>
        /// <param name="container">
        /// An <see cref="IContainer"/> that represents the container for this
        /// <see cref="AnimatedPathRenderer"/>.
        /// </param>
        public AnimatedPathRenderer(IContainer container)
            : this()
        {
            if (container is null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Add(this);
        }

        public event EventHandler Length1Changed;

        public event EventHandler Length2Changed;

        public event EventHandler Color1Changed;

        public event EventHandler Color2Changed;

        public event EventHandler IntervalChanged;

        public event EventHandler Redraw;

        /// <summary>
        /// Occurs every <see cref="Interval"/> milliseconds when this <see
        /// cref="AnimatedPathRenderer"/> is constructed.
        /// </summary>
        [Category("Animator")]
        [Description("Occurs when the specified timer interval has elapsed.")]
        public event EventHandler Tick;

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
                if (Length1 == value)
                {
                    return;
                }

                _length1 = value > 0
                    ? value
                    : throw ValueNotGreaterThan(nameof(value), value);

                OnLength1Changed(EventArgs.Empty);
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
                if (Length2 == value)
                {
                    return;
                }

                _length2 = value > 0
                    ? value
                    : throw ValueNotGreaterThan(nameof(value), value);

                OnLength2Changed(EventArgs.Empty);
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
        /// Gets or sets the color of the second dashed line.
        /// </summary>
        [Category("Animator")]
        [DefaultValue(typeof(Color), "White")]
        [Description("The color of the second dashed line.")]
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
        /// Gets or sets the time, in milliseconds, before the next update of
        /// the line animation.
        /// </summary>
        [Category("Animator")]
        [DefaultValue(1000)]
        [Description("The time, in milliseconds, before the next" +
            "update of the line animation.")]
        public int Interval
        {
            get
            {
                return _interval;
            }

            set
            {
                if (Interval == value)
                {
                    return;
                }

                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }

                _interval = value;
                if (Interval == 0)
                {
                    Timer.Enabled = false;
                }
                else
                {
                    Timer.Interval = value;
                    if (!Timer.Enabled && (Site is null || !Site.DesignMode))
                    {
                        Timer.Start();
                    }
                }

                OnIntervalChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets or sets the initial offset of the first dashed line.
        /// </summary>
        private int Offset
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
        /// Gets the <see cref="Pen.DashPattern"/> that will be used for the
        /// first <see cref="Pen"/>.
        /// </summary>
        private float[] DashPattern1
        {
            get
            {
                return new float[] { Length1, Length2 };
            }
        }

        /// <summary>
        /// Gets the <see cref="Pen.DashPattern"/> that will be used for the
        /// second <see cref="Pen"/>.
        /// </summary>
        private float[] DashPattern2
        {
            get
            {
                return new float[] { Length2, Length1 };
            }
        }

        /// <summary>
        /// Draws a <see cref="GraphicsPath"/> to a <see cref=" Graphics"/>
        /// using an animated dashed line described by this <see
        /// cref="AnimatedPathRenderer"/>.
        /// </summary>
        /// <param name="graphics">
        /// The <see cref="Graphics"/> to draw to.
        /// </param>
        /// <param name="path">
        /// The <see cref="GraphicsPath"/> to draw to <paramref name="
        /// graphics"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graphics"/> or <paramref name="path"/> is <see
        /// langword="null"/>.
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
            Offset++;
            Tick?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnLength1Changed(EventArgs e)
        {
            Length1Changed?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnLength2Changed(EventArgs e)
        {
            Length2Changed?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
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

        protected virtual void OnIntervalChanged(EventArgs e)
        {
            IntervalChanged?.Invoke(this, e);
            OnRedraw(EventArgs.Empty);
        }

        protected virtual void OnRedraw(EventArgs e)
        {
            Redraw?.Invoke(this, e);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="
        /// AnimatedPathRenderer"/> and optionally releases the managed
        /// resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to released both managed and unmanaged
        /// resources; <see langword="false"/> to release only unmanaged
        /// resources.
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
                graphics.DrawPath(pen, path);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            OnTick(e);
        }
    }
}
