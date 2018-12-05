// <copyright file="PreprocessMessageCallback.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System.Windows.Forms;

    /// <summary>
    /// References a method to be called that will preprocess a
    /// <see cref="Message"/> before it is used in <see cref="Control.
    /// WndProc(ref Message)"/> or <see cref="Control.
    /// DefWndProc(ref Message)"/>.
    /// </summary>
    /// <param name="m">
    /// The <see cref="Message"/> to preprocess.
    /// </param>
    public delegate void PreprocessMessageCallback(ref Message m);
}
