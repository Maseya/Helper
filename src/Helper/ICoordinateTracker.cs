// <copyright file="ICoordinateTracker.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text;

    public interface ICoordinateTracker : IComponent
    {
        Point CurrentPosition
        {
            get;
        }

        Point PreviousPosition
        {
            get;
        }

        bool IsIdle
        {
            get;
        }
    }
}
