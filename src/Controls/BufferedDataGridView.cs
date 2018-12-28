// <copyright file="BufferedDataGridView.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System.Windows.Forms;

    /// <summary>
    /// Provides an implementation of <see cref="DataGridView"/> that is
    /// double-buffered and user-drawn.
    /// </summary>
    /// <remarks>
    /// Use <see cref="BufferedDataGridView"/> over <see cref=" DataGridView"/>
    /// in situations where you need to display images in a grid, otherwise you
    /// can get flicker from drawing them.
    /// </remarks>
    public class BufferedDataGridView : DataGridView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// BufferedDataGridView"/> class.
        /// </summary>
        public BufferedDataGridView()
        {
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
        }
    }
}
