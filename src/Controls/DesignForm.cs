// <copyright file="DesignForm.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Drawing;
    using System.Security;
    using System.Windows.Forms;
    using static System.ComponentModel.DesignerSerializationVisibility;
    using WM = WindowMessages;

    /// <summary>
    /// Provides an empty <see cref="Form"/> with configurations optimized for
    /// design and sizing.
    /// </summary>
    /// <remarks>
    /// This class contains information about the window's border padding as
    /// well as custom events to modifying the sizing rectangle during resize
    /// events.
    /// </remarks>
    public class DesignForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesignForm"/> class.
        /// </summary>
        public DesignForm()
        {
            KeyPreview = true;

            ProcedureOverrides =
                new ReadOnlyDictionary<int, PreprocessMessageCallback>(
                    new Dictionary<int, PreprocessMessageCallback>()
                    {
                        { WM.Size, AdjustSizeFromSizing },
                        { WM.Sizing, AdjustRectangleFromSizing },
                    });
        }

        /// <summary>
        /// Preprocess the window rectangle before applying it during a resize
        /// operation.
        /// </summary>
        [Browsable(true)]
        [Description(
            "Preprocess the window rectangle before applying " +
            "it during a resize operation.")]
        public event EventHandler<RectangleEventArgs> AdjustWindowBounds;

        /// <summary>
        /// Preprocess the window size before applying it during a resize
        /// operation.
        /// </summary>
        [Browsable(true)]
        [Description(
            "Preprocess the window size before applying it " +
            "during a resize operation.")]
        public event EventHandler<RectangleEventArgs> AdjustWindowSize;

        /// <summary>
        /// Gets the vertical and horizontal thickness of the border around
        /// this form.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Size FormBorderSize
        {
            get
            {
                return GetFormBorderSize(this);
            }
        }

        /// <summary>
        /// Gets the height of the caption area of this <see cref="
        /// DesignForm"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int CaptionHeight
        {
            get
            {
                return GetCaptionHeight(this);
            }
        }

        /// <summary>
        /// Gets the padding information of the border around this <see
        /// cref="DesignForm"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Padding BorderPadding
        {
            get
            {
                return GetFormBorderPadding(this);
            }
        }

        /// <summary>
        /// Gets the padding information of the border and caption area of this
        /// <see cref="DesignForm"/> that surrounds its client area.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Padding WindowPadding
        {
            get
            {
                var padding = BorderPadding;
                padding.Top += CaptionHeight;
                return padding;
            }
        }

        /// <summary>
        /// Gets the dimensions of this <see cref="DesignForm"/> in screen
        /// coordinates.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Rectangle AbsoluteCoordinates
        {
            get
            {
                return WinApiMethods.GetWindowRectangle(this);
            }
        }

        /// <summary>
        /// Gets the input control keys to override if no others are specified.
        /// </summary>
        /// <remarks>
        /// These fallback keys are overridden because it is often desired to
        /// use keyboard navigation.
        /// </remarks>
        internal static ICollection<Keys> FallbackOverrideInputKeys
        {
            get
            {
                return DesignControl.FallbackOverrideInputKeys;
            }
        }

        /// <summary>
        /// Gets a dictionary of <see cref="PreprocessMessageCallback"/>
        /// delegates to call for given <see cref="Message.Msg"/> keys.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        private IReadOnlyDictionary<int, PreprocessMessageCallback>
            ProcedureOverrides
        {
            get;
        }

        /// <summary>
        /// Gets the vertical and horizontal thickness of the border of a <see
        /// cref="Form"/>.
        /// </summary>
        /// <param name="form">
        /// The <see cref="Form"/> to get the border size of.
        /// </param>
        /// <returns>
        /// The vertical and horizontal thickness, represented in a <see
        /// cref="Size"/> structure, of <paramref name="form"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="form"/> is <see langword="null"/>.
        /// </exception>
        public static Size GetFormBorderSize(Form form)
        {
            if (form is null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            return GetFormBorderSize(form.FormBorderStyle);
        }

        /// <summary>
        /// Gets the vertical and horizontal thickness of the border of a
        /// window given its <see cref="FormBorderStyle"/>.
        /// </summary>
        /// <param name="formBorderStyle">
        /// A <see cref="Form.FormBorderStyle"/> value of the window to get the
        /// border size from.
        /// </param>
        /// <returns>
        /// The vertical and horizontal thickness, represented in a <see
        /// cref="Size"/> structure, of the form border padding of a window
        /// described with <paramref name="formBorderStyle"/>.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="formBorderStyle"/> is not one of the values of <see
        /// cref="FormBorderStyle"/>.
        /// </exception>
        public static Size GetFormBorderSize(
            FormBorderStyle formBorderStyle)
        {
            switch (formBorderStyle)
            {
            case FormBorderStyle.None:
                return Size.Empty;

            case FormBorderStyle.FixedSingle:
            case FormBorderStyle.FixedDialog:
            case FormBorderStyle.Sizable:
                return SystemInformation.FrameBorderSize
                    + WinApiMethods.PaddedBorderSize;

            case FormBorderStyle.FixedToolWindow:
            case FormBorderStyle.SizableToolWindow:
                return SystemInformation.FixedFrameBorderSize
                    + WinApiMethods.PaddedBorderSize;

            case FormBorderStyle.Fixed3D:
                return SystemInformation.FrameBorderSize
                    + SystemInformation.Border3DSize
                    + WinApiMethods.PaddedBorderSize;

            default:
                throw new InvalidEnumArgumentException(
                    nameof(formBorderStyle),
                    (int)formBorderStyle,
                    typeof(FormBorderStyle));
            }
        }

        /// <summary>
        /// Gets the padding information of the border around a <see
        /// cref="Form"/>.
        /// </summary>
        /// <param name="form">
        /// The <see cref="Form"/> to get the border padding from.
        /// </param>
        /// <returns>
        /// A <see cref="Padding"/> structure whose edges are the thicknesses
        /// of each border edge of <paramref name="form"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="form"/> is <see langword="null"/>.
        /// </exception>
        public static Padding GetFormBorderPadding(Form form)
        {
            var borderSize = GetFormBorderSize(form);
            return new Padding(
                borderSize.Width,
                borderSize.Height,
                borderSize.Width,
                borderSize.Height);
        }

        /// <summary>
        /// Gets the height, in pixels, of the caption area of a <see
        /// cref="Form"/>.
        /// </summary>
        /// <param name="form">
        /// The <see cref="Form"/> to get the caption height of.
        /// </param>
        /// <returns>
        /// The caption height of <paramref name="form"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="form"/> is <see langword="null"/>.
        /// </exception>
        public static int GetCaptionHeight(Form form)
        {
            if (form is null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            return GetCaptionHeight(form.FormBorderStyle);
        }

        /// <summary>
        /// Gets the height, in pixels, of the caption area of a window given
        /// its <see cref="FormBorderStyle"/>.
        /// </summary>
        /// <param name="formBorderStyle">
        /// The <see cref="FormBorderStyle"/> of the window to get the caption
        /// height of.
        /// </param>
        /// <returns>
        /// The height, in pixels, of the caption or title area of a window
        /// corresponding to a <see cref="FormBorderStyle"/> of <paramref
        /// name="formBorderStyle"/>.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="formBorderStyle"/> is not one of the values of <see
        /// cref="FormBorderStyle"/>.
        /// </exception>
        public static int GetCaptionHeight(
            FormBorderStyle formBorderStyle)
        {
            switch (formBorderStyle)
            {
            case FormBorderStyle.None:
                return 0;

            case FormBorderStyle.FixedSingle:
            case FormBorderStyle.Fixed3D:
            case FormBorderStyle.FixedDialog:
            case FormBorderStyle.Sizable:
                return SystemInformation.CaptionHeight;

            case FormBorderStyle.FixedToolWindow:
            case FormBorderStyle.SizableToolWindow:
                return SystemInformation.ToolWindowCaptionHeight;

            default:
                throw new InvalidEnumArgumentException(
                    nameof(formBorderStyle),
                    (int)formBorderStyle,
                    typeof(FormBorderStyle));
            }
        }

        /// <summary>
        /// Determines whether the specified key is a regular input key or a
        /// special key that requires preprocessing.
        /// </summary>
        /// <param name="keyData">
        /// The key to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the key is regular input key; otherwise
        /// <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Arrows keys and their combinations with the modifier keys are now
        /// considered input keys in a <see cref=" DesignForm"/>.
        /// </remarks>
        protected override bool IsInputKey(Keys keyData)
        {
            return FallbackOverrideInputKeys.Contains(keyData)
                ? true
                : base.IsInputKey(keyData);
        }

        /// <summary>
        /// Processes a dialog key.
        /// </summary>
        /// <param name="keyData">
        /// The key to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the key is a dialog key; otherwise <see
        /// langword="false"/>.
        /// </returns>
        /// <remarks>
        /// Arrows keys and their combinations with the modifier keys are no
        /// longer considered dialog keys in a <see cref=" DesignForm"/>.
        /// </remarks>
        [SecuritySafeCritical]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            return FallbackOverrideInputKeys.Contains(keyData)
                ? false
                : base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Sends the specified message to the default window procedure.
        /// </summary>
        /// <param name="m">
        /// The windows <see cref="Message"/> to process.
        /// </param>
        [SecuritySafeCritical]
        protected override void DefWndProc(ref Message m)
        {
            // Why have an ugly, large, O(n) switch tree to preprocess messages
            // when you can do a pretty O(1) dictionary instead?
            if (TryGetPreprocessMessage(m, out var preprocessMessage))
            {
                preprocessMessage(ref m);
            }

            base.DefWndProc(ref m);
        }

        /// <summary>
        /// Gets the <see cref="PreprocessMessageCallback"/> that is associated
        /// with the specified <see cref="Message"/>.
        /// </summary>
        /// <param name="message">
        /// The key to locate.
        /// </param>
        /// <param name="preprocessMessage">
        /// When this method returns, the <see cref="
        /// PreprocessMessageCallback"/> associated with <paramref
        /// name="message"/>, if the key is found; otherwise <see
        /// langword="null"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="message"/> is associated
        /// with a <see cref="PreprocessMessageCallback"/>; otherwise <see
        /// langword="false"/>.
        /// </returns>
        protected virtual bool TryGetPreprocessMessage(
            Message message,
            out PreprocessMessageCallback preprocessMessage)
        {
            return ProcedureOverrides.TryGetValue(
                message.Msg,
                out preprocessMessage);
        }

        /// <summary>
        /// Raises the <see cref="AdjustWindowBounds"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="RectangleEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnAdjustWindowBounds(RectangleEventArgs e)
        {
            AdjustWindowBounds?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="AdjustWindowSize"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="RectangleEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnAdjustWindowSize(RectangleEventArgs e)
        {
            AdjustWindowSize?.Invoke(this, e);
        }

        /// <summary>
        /// Converts an <see cref="IntPtr"/> to a <see cref="Size"/> struct
        /// using the sequential data layout.
        /// </summary>
        /// <param name="value">
        /// The <see cref="IntPtr"/> to read.
        /// </param>
        /// <returns>
        /// A <see cref="Size"/> whose data is sequentially identical to
        /// <paramref name="value"/>.
        /// </returns>
        private static Size IntPtrToSize(IntPtr value)
        {
            return new Size((int)value & 0xFFFF, (int)value >> 0x10);
        }

        /// <summary>
        /// Converts an <see cref="Size"/> to an <see cref="IntPtr"/> struct
        /// using the sequential data layout.
        /// </summary>
        /// <param name="size">
        /// The <see cref="Size"/> to read.
        /// </param>
        /// <returns>
        /// A <see cref="IntPtr"/> whose data is sequentially identical to
        /// <paramref name="size"/>.
        /// </returns>
        private static IntPtr SizeToIntPtr(Size size)
        {
            return (IntPtr)(
                (size.Width * 0xFFFF) |
                ((size.Height & 0xFFFF) << 0x10));
        }

        /// <summary>
        /// Hook the WM_SIZE event and allow preprocessing of the size
        /// structure before using it in the window message loop.
        /// </summary>
        /// <param name="m">
        /// The <see cref="Message"/> to modify.
        /// </param>
        private void AdjustSizeFromSizing(ref Message m)
        {
            // Valid sizing processes require the message WParam be zero.
            // Nonzero values specify special circumstances like minimizing the
            // window.
            if (m.WParam != IntPtr.Zero)
            {
                return;
            }

            // Get the window size from the client size.
            var windowSize = WinApiMethods.InflateSize(
                IntPtrToSize(m.LParam),
                WindowPadding);

            // Let the user modify the size in any ways they like.
            var e = new RectangleEventArgs(size: windowSize);
            OnAdjustWindowSize(e);

            // Get the client size from the window size.
            var adjustedClientSize = WinApiMethods.DeflateSize(
                e.Size,
                WindowPadding);

            // Set the message LParam to the new client size.
            m.LParam = SizeToIntPtr(adjustedClientSize);
        }

        /// <summary>
        /// Hook the WM_SIZING event and allow preprocessing of the sizing
        /// rectangle before using it in the window message loop.
        /// </summary>
        /// <param name="m">
        /// The <see cref="Message"/> to modify.
        /// </param>
        private unsafe void AdjustRectangleFromSizing(ref Message m)
        {
            var windowBounds = (WinApiRectangle*)m.LParam;

            var e = new RectangleEventArgs(*windowBounds);
            OnAdjustWindowBounds(e);

            *windowBounds = e.Rectangle;
        }
    }
}
