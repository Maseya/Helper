// <copyright file="DesignControl.cs" company="Public Domain">
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
    using static System.ComponentModel.EditorBrowsableState;
    using WM = WindowMessages;

    /// <summary>
    /// Provides an empty <see cref="UserControl"/> with configurations
    /// optimized for design and drawing uses.
    /// </summary>
    [DefaultEvent("Paint")]
    [Description("Provides a control to be used for design purposes.")]
    public class DesignControl : UserControl
    {
        /// <summary>
        /// Represents a location for any mouse cursor that is not inside of a
        /// <see cref="DesignControl"/> client area.
        /// </summary>
        public static readonly Point MouseOutOfRange = new Point(
            Int32.MinValue,
            Int32.MinValue);

        /// <summary>
        /// Represents the input control keys to override if no others are
        /// specified.
        /// </summary>
        /// <remarks>
        /// These fallback keys are overridden because it is often desired to
        /// use keyboard navigation.
        /// </remarks>
        internal static readonly ICollection<Keys>
            FallbackOverrideInputKeys = new HashSet<Keys>
        {
            Keys.Up,
            Keys.Up | Keys.Shift,
            Keys.Up | Keys.Control,
            Keys.Up | Keys.Shift | Keys.Control,
            Keys.Left,
            Keys.Left | Keys.Shift,
            Keys.Left | Keys.Control,
            Keys.Left | Keys.Shift | Keys.Control,
            Keys.Down,
            Keys.Down | Keys.Shift,
            Keys.Down | Keys.Control,
            Keys.Down | Keys.Shift | Keys.Control,
            Keys.Right,
            Keys.Right | Keys.Shift,
            Keys.Right | Keys.Control,
            Keys.Right | Keys.Shift | Keys.Control,
        };

        /// <summary>
        /// Initializes a new instance of the <see cref=" DesignControl"/>
        /// class.
        /// </summary>
        public DesignControl()
        {
            // These are basically required for any desired drawing to take
            // place in a UserControl
            var designFlags = ControlStyles.ResizeRedraw
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint;

            SetStyle(designFlags, true);

            // This is simply a style choice.
            BorderStyle = BorderStyle.FixedSingle;

            // Create a dictionary here rather than making a giant switch table
            // of DefWndProc overrides.
            ProcedureOverrides =
                new ReadOnlyDictionary<int, PreprocessMessageCallback>(
                    new Dictionary<int, PreprocessMessageCallback>()
                    {
                        { WM.KeyDown, UpdateKeyStateFromKeyDown },
                        { WM.SystemKeyDown, UpdateKeyStateFromKeyDown },
                        { WM.KeyUp, UpdateKeyStateFromKeyUp },
                        { WM.SystemKeyUp, UpdateKeyStateFromKeyUp },
                        { WM.MouseMove, UpdateMouseStateFromMouseMove },
                        { WM.MouseLeave, UpdateMouseStateFromMouseLeave },
                    });
        }

        /// <summary>
        /// Occurs when the mouse wheel moves while the <see cref="
        /// DesignControl"/> has focus.
        /// </summary>
        [Browsable(true)]
        [Category("Mouse")]
        [Description(
            "Occurs when the mouse wheel moves while the control " +
            "has focus.")]
        public new event MouseEventHandler MouseWheel
        {
            add
            {
                base.MouseWheel += value;
            }

            remove
            {
                base.MouseWheel -= value;
            }
        }

        /// <summary>
        /// Gets the current <see cref="Keys"/> being held down in any <see
        /// cref="DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static Keys CurrentKeys
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="Keys"/> that were held down in any <see
        /// cref="DesignControl"/> the last time any Key events were processed.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static Keys PreviousKeys
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the new <see cref="Keys"/> that were pressed in any <see
        /// cref="DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static Keys PressedKeys
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the old <see cref="Keys"/> that were released in any <see
        /// cref="DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static Keys ReleasedKeys
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether any keyboard Control is currently
        /// being held.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static bool ControlKeyHeld
        {
            get
            {
                return (ModifierKeys & Keys.Control) != Keys.None;
            }
        }

        /// <summary>
        /// Gets a value indicating whether any keyboard Shift is currently
        /// being held.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static bool ShiftKeyHeld
        {
            get
            {
                return (ModifierKeys & Keys.Shift) != Keys.None;
            }
        }

        /// <summary>
        /// Gets a value indicating whether any keyboard Alt is currently being
        /// held.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static bool AltKeyHeld
        {
            get
            {
                return (ModifierKeys & Keys.Alt) != Keys.None;
            }
        }

        /// <summary>
        /// Gets the mouse buttons that are currently held down in any <see
        /// cref="DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static MouseButtons CurrentMouseButtons
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the last value of <see cref="CurrentMouseButtons"/> before the
        /// last mouse up or down events occurred.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static MouseButtons PreviousMouseButtons
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the newly pressed mouse buttons in any <see cref="
        /// DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static MouseButtons ActiveMouseButtons
        {
            get
            {
                return CurrentMouseButtons & ~PreviousMouseButtons;
            }
        }

        /// <summary>
        /// Gets the newly released mouse buttons in any <see cref="
        /// DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public static MouseButtons ReleasedMouseButtons
        {
            get
            {
                return PreviousMouseButtons & ~CurrentMouseButtons;
            }
        }

        /// <summary>
        /// Gets or sets the height and width of the client area of the
        /// control.
        /// </summary>
        [Browsable(true)]
        [Description("The size of the client area of the form.")]
        [EditorBrowsable(Always)]
        public new Size ClientSize
        {
            get
            {
                return base.ClientSize;
            }

            set
            {
                base.ClientSize = value;
            }
        }

        /// <summary>
        /// Gets or sets the width of the client area of the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int ClientWidth
        {
            get
            {
                return ClientSize.Width;
            }

            set
            {
                ClientSize = new Size(value, ClientHeight);
            }
        }

        /// <summary>
        /// Gets or sets the height of the client area of the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public int ClientHeight
        {
            get
            {
                return ClientSize.Height;
            }

            set
            {
                ClientSize = new Size(ClientWidth, value);
            }
        }

        /// <summary>
        /// Gets the width and height of the border of this <see cref="
        /// DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Size BorderSize
        {
            get
            {
                switch (BorderStyle)
                {
                    case BorderStyle.FixedSingle:
                        return SystemInformation.BorderSize;

                    case BorderStyle.Fixed3D:
                        return SystemInformation.Border3DSize;

                    case BorderStyle.None:
                    default:
                        return Size.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the padding information of the border of this <see
        /// cref="DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Padding BorderPadding
        {
            get
            {
                var borderSize = BorderSize;
                return new Padding(
                    borderSize.Width,
                    borderSize.Height,
                    borderSize.Width,
                    borderSize.Height);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the mouse is currently idle over
        /// this <see cref="DesignControl"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public bool MouseIdle
        {
            get
            {
                return PreviousMousePosition == CurrentMousePosition;
            }
        }

        /// <summary>
        /// Gets the current client location of the mouse if it is in the
        /// client area of this <see cref="DesignControl"/>, or <see
        /// cref="MouseOutOfRange"/> if it is not.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Point CurrentMousePosition
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the previous value of <see cref=" CurrentMousePosition"/>
        /// before the last mouse move event occurred.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(Hidden)]
        public Point PreviousMousePosition
        {
            get;
            private set;
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
        /// Sends the specified message to the default window procedure.
        /// </summary>
        /// <param name="m">
        /// The windows <see cref="Message"/> to process.
        /// </param>
        [SecuritySafeCritical]
        protected override void DefWndProc(ref Message m)
        {
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
        /// considered input keys in a <see cref=" DesignControl"/>.
        /// </remarks>
        protected override bool IsInputKey(Keys keyData)
        {
            // We return true for any overridden keys that we have determined
            // should be input keys.
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
        /// longer considered dialog keys in a <see cref=" DesignControl"/>.
        /// </remarks>
        [SecuritySafeCritical]
        protected override bool ProcessDialogKey(Keys keyData)
        {
            return FallbackOverrideInputKeys.Contains(keyData)
                ? false
                : base.ProcessDialogKey(keyData);
        }

        /// <summary>
        /// Updates <see cref="PreviousKeys"/>, <see cref=" CurrentKeys"/>,
        /// <see cref="PressedKeys"/> and <see cref=" ReleasedKeys"/> from the
        /// WM_KEYDOWN message.
        /// </summary>
        /// <param name="m">
        /// The <see cref="Message"/> to preprocess.
        /// </param>
        private static void UpdateKeyStateFromKeyDown(ref Message m)
        {
            PreviousKeys = CurrentKeys;
            CurrentKeys = (Keys)m.WParam;
            PressedKeys = CurrentKeys & ~PreviousKeys;
            ReleasedKeys = Keys.None;
        }

        /// <summary>
        /// Updates <see cref="PreviousKeys"/>, <see cref=" CurrentKeys"/>,
        /// <see cref="PressedKeys"/> and <see cref=" ReleasedKeys"/> from the
        /// WM_KEYUP message.
        /// </summary>
        /// <param name="m">
        /// The <see cref="Message"/> to preprocess.
        /// </param>
        private static void UpdateKeyStateFromKeyUp(ref Message m)
        {
            PreviousKeys = CurrentKeys;
            ReleasedKeys = (Keys)m.WParam;
            CurrentKeys &= ~ReleasedKeys;
            PressedKeys = Keys.None;
        }

        /// <summary>
        /// Converts an <see cref="IntPtr"/> to a <see cref="Point"/> struct
        /// using the sequential data layout.
        /// </summary>
        /// <param name="value">
        /// The <see cref="IntPtr"/> to read.
        /// </param>
        /// <returns>
        /// A <see cref="Point"/> whose data is sequentially identical to
        /// <paramref name="value"/>.
        /// </returns>
        private static Point IntPtrToPoint(IntPtr value)
        {
            return new Point((int)value & 0xFFFF, (int)value >> 0x10);
        }

        /// <summary>
        /// Updates the <see cref="DesignControl"/> mouse states from
        /// WM_MOUSELEAVE.
        /// </summary>
        /// <param name="m">
        /// The <see cref="Message"/> to preprocess.
        /// </param>
        private void UpdateMouseStateFromMouseLeave(ref Message m)
        {
            PreviousMousePosition = CurrentMousePosition;
            CurrentMousePosition = MouseOutOfRange;

            PreviousMouseButtons = CurrentMouseButtons;
            CurrentMouseButtons = MouseButtons.None;
        }

        /// <summary>
        /// Updates the <see cref="DesignControl"/> mouse states from
        /// WM_MOUSEMOVE.
        /// </summary>
        /// <param name="m">
        /// The <see cref="Message"/> to preprocess.
        /// </param>
        private void UpdateMouseStateFromMouseMove(ref Message m)
        {
            PreviousMousePosition = CurrentMousePosition;
            CurrentMousePosition = IntPtrToPoint(m.LParam);

            PreviousMouseButtons = CurrentMouseButtons;
            CurrentMouseButtons = MouseButtons;
        }
    }
}
