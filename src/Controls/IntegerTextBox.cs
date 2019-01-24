// <copyright file="IntegerTextBox.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Forms;
    using static Maseya.Helper.StringHelper;

    [DefaultEvent("ValueChanged")]
    [DefaultProperty("Value")]
    [Description("A text box that only accepts integer values.")]
    public class IntegerTextBox : TextBox, IIntegerComponent
    {
        private const int FallbackValue = 0;

        private const bool FallbackAllowHex = false;

        private const bool FallbackAllowNegative = false;

        private const CharacterCasing FallbackCharacterCasing =
            CharacterCasing.Upper;

        private bool _allowHex = FallbackAllowHex;

        private bool _allowNegative = FallbackAllowNegative;

        private int _value = FallbackValue;

        public IntegerTextBox()
        {
            Text = GetString(FallbackValue);
            CharacterCasing = FallbackCharacterCasing;
        }

        public event EventHandler ValueChanged;

        public event EventHandler TextParseFailed;

        public event EventHandler TextParseSucceeded;

        [Category("Editor")]
        [DefaultValue(FallbackAllowHex)]
        [Description("Determines whether the control reads " +
            "hexadecimal values or decimal.")]
        public bool AllowHex
        {
            get
            {
                return _allowHex;
            }

            set
            {
                _allowHex = value;
                SetValue(Value);
            }
        }

        [Category("Editor")]
        [DefaultValue(FallbackAllowNegative)]
        [Description("Determines whether negative numbers are valid input.")]
        public bool AllowNegative
        {
            get
            {
                return _allowNegative;
            }

            set
            {
                _allowNegative = value;
                SetValue(Value);
            }
        }

        [Category("Editor")]
        [DefaultValue(FallbackValue)]
        [Description("The value written to the text box.")]
        public int Value
        {
            get { return _value; }
            set { SetValue(value); }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        private NumberStyles NumberStyle
        {
            get
            {
                var hexSpecifier = AllowHex ?
                    NumberStyles.AllowHexSpecifier :
                    NumberStyles.AllowThousands;

                var allowNegative = AllowNegative ?
                    NumberStyles.AllowLeadingSign :
                    NumberStyles.None;

                var allowWhite = NumberStyles.AllowLeadingWhite
                    | NumberStyles.AllowTrailingWhite;

                return hexSpecifier | allowNegative | allowWhite;
            }
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            ValueChanged?.Invoke(this, e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            // Save original value.

            // Parse new value.
            if (Int32.TryParse(
                Text,
                NumberStyle,
                CultureInfo.CurrentUICulture,
                out var textValue))
            {
                // Only raise event if value changed.
                if (Value != textValue)
                {
                    Value = textValue;
                }

                OnTextParseSucceeded(e);
            }
            else
            {
                OnTextParseFailed(e);
            }

            base.OnTextChanged(e);
        }

        protected virtual void OnTextParseFailed(EventArgs e)
        {
            TextParseFailed?.Invoke(this, e);
        }

        protected virtual void OnTextParseSucceeded(EventArgs e)
        {
            TextParseSucceeded?.Invoke(this, e);
        }

        private void SetValue(int value)
        {
            // Make value positive if negative is not allowed.
            if (!AllowNegative)
            {
                value = Math.Abs(value);
            }

            // Set the value.
            _value = value;

            // Parse the value.
            Text = GetString(Value, AllowHex ? "X" : String.Empty);
            OnValueChanged(EventArgs.Empty);
        }
    }
}
