// <copyright file="ColorValueControl.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Helper;

    /// <summary>
    /// Provides a control that draws a <see cref="Color"/> to its surface.
    /// </summary>
    public class ColorValueControl : DesignControl
    {
        /// <summary>
        /// The represented <see cref="Color"/> of this <see cref="
        /// ColorValueControl"/>.
        /// </summary>
        private Color _selectedColor;

        /// <summary>
        /// Occurs when <see cref="SelectedColor"/> changes.
        /// </summary>
        [Category("Editor")]
        [Description("Occurs when the selected color value of the " +
            "control changes.")]
        public event EventHandler ColorValueChanged;

        /// <summary>
        /// Gets or sets the represented <see cref="Color"/> of this <see
        /// cref="ColorValueControl"/>.
        /// </summary>
        [Category("Appearance")]
        [DefaultValue("Black")]
        [Description("The selected color for the control.")]
        public Color SelectedColor
        {
            get
            {
                return _selectedColor;
            }

            set
            {
                if (SelectedColor == value)
                {
                    return;
                }

                _selectedColor = value;
                OnColorValueChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Gets the visual color that will be drawn to this <see cref="
        /// ColorValueControl"/>.
        /// </summary>
        private Color DrawColor
        {
            get
            {
                if (Enabled)
                {
                    return SelectedColor;
                }

                var colorF = (ColorF)SelectedColor;
                var gray = colorF.Grayscale();
                return (Color)gray;
            }
        }

        /// <summary>
        /// Redraws the control and raises the <see cref="Control.
        /// EnabledChanged"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            Invalidate();
            base.OnEnabledChanged(e);
        }

        /// <summary>
        /// Redraws the control and raises the <see cref=" ColorValueChanged"/>
        /// event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnColorValueChanged(EventArgs e)
        {
            Invalidate();
            ColorValueChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Calls <see cref="DrawColorValue(Graphics)"/> and raises the <see
        /// cref="Control.Paint"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="PaintEventArgs"/> that contains the event data.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="e"/> is <see langword="null"/>.
        /// </exception>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (e is null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            DrawColorValue(e.Graphics);

            base.OnPaint(e);
        }

        /// <summary>
        /// Fills this <see cref="ColorValueChanged"/> client area with the
        /// <see cref="Color"/> determined by <see cref=" SelectedColor"/>.
        /// </summary>
        /// <param name="graphics">
        /// The drawing surface to fill <see cref="SelectedColor"/> onto.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="graphics"/> is <see langword="null"/>.
        /// </exception>
        protected virtual void DrawColorValue(Graphics graphics)
        {
            if (graphics is null)
            {
                throw new ArgumentNullException(nameof(graphics));
            }

            using (var brush = new SolidBrush(DrawColor))
            {
                graphics.FillRectangle(brush, ClientRectangle);
            }
        }
    }
}
