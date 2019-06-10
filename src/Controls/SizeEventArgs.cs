// <copyright file="SizeEventArgs.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.Drawing;

    public class SizeEventArgs : EventArgs
    {
        public SizeEventArgs(Size size)
        {
            Size = size;
        }

        public SizeEventArgs(int width, int height)
            : this(new Size(width, height))
        {
        }

        public Size Size
        {
            get;
            set;
        }
    }
}
