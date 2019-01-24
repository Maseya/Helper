// <copyright file="LinkedTrackBar.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.Windows.Forms;

    public class LinkedTrackBar : TrackBar, IIntegerComponent
    {
        private IIntegerComponent _integerComponent;

        public IIntegerComponent IntegerComponent
        {
            get
            {
                return _integerComponent;
            }

            set
            {
                if (this == value)
                {
                    throw new ArgumentException();
                }

                if (IntegerComponent == value)
                {
                    return;
                }

                if (IntegerComponent != null)
                {
                    IntegerComponent.ValueChanged -=
                        IntegerComponent_ValueChanged;
                }

                _integerComponent = value;
                if (IntegerComponent != null)
                {
                    IntegerComponent.ValueChanged +=
                        IntegerComponent_ValueChanged;
                }
            }
        }

        protected override void OnValueChanged(EventArgs e)
        {
            if (IntegerComponent != null)
            {
                IntegerComponent.Value = Value;
            }

            base.OnValueChanged(e);
        }

        private void IntegerComponent_ValueChanged(object sender, EventArgs e)
        {
            var value = IntegerComponent.Value;
            if (value >= Minimum && value <= Maximum)
            {
                Value = value;
            }
        }
    }
}
