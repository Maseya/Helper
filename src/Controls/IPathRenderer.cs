// <copyright file="IPathRenderer.cs" company="Public Domain">
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

    public interface IPathRenderer : IComponent
    {
        event EventHandler Redraw;

        void DrawPath(Graphics graphics, GraphicsPath graphicsPath);
    }
}
