// <copyright file="UndoEventArgs.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System;

    public class UndoEventArgs : EventArgs
    {
        public UndoEventArgs(Action undo, Action redo)
        {
            Undo = undo ?? throw new ArgumentNullException(nameof(undo));
            Redo = redo ?? throw new ArgumentNullException(nameof(redo));
        }

        public Action Undo
        {
            get;
        }

        public Action Redo
        {
            get;
        }
    }
}
