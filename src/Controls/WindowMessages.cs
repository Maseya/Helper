// <copyright file="WindowMessages.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System.Windows.Forms;

    /// <summary>
    /// Provides constants for WinAPI WinProc message values.
    /// </summary>
    internal static class WindowMessages
    {
        /// <summary>
        /// Posted to the window with the keyboard focus when a non-system key
        /// is pressed. A non-system key is a key that is pressed when the ALT
        /// key is not pressed.
        /// </summary>
        public const int KeyDown = 0x100;

        /// <summary>
        /// Posted to the window with the keyboard focus when a non-system key
        /// is released. A non-system key is a key that is pressed when the ALT
        /// key is not pressed, or a keyboard key that is pressed when a window
        /// has the keyboard focus.
        /// </summary>
        public const int KeyUp = 0x101;

        /// <summary>
        /// Posted to the window with the keyboard focus when the user presses
        /// the F10 key (which activates the menu bar) or holds down the ALT
        /// key and then presses another key. It also occurs when no window
        /// currently has the keyboard focus; in this case, the <see
        /// cref="SystemKeyDown"/> message is sent to the active window. The
        /// window that receives the message can distinguish between these two
        /// contexts by checking the context code in the <see
        /// cref="Message.LParam"/> parameter.
        /// </summary>
        public const int SystemKeyDown = 0x104;

        /// <summary>
        /// Posted to the window with the keyboard focus when the user releases
        /// a key that was pressed while the ALT key was held down. It also
        /// occurs when no window currently has the keyboard focus; in this
        /// case, the <see cref="SystemKeyUp"/> message is sent to the active
        /// window. The window that receives the message can distinguish
        /// between these two contexts by checking the context code in the <see
        /// cref=" Message.LParam"/> parameter.
        /// </summary>
        public const int SystemKeyUp = 0x105;

        /// <summary>
        /// Posted to a window when the cursor moves. If the mouse is not
        /// captured, the message is posted to the window that contains the
        /// cursor. Otherwise, the message is posted to the window that has
        /// captured the mouse.
        /// </summary>
        public const int MouseMove = 0x200;

        /// <summary>
        /// Posted to a window when the cursor hovers over the client area of
        /// the window for the period of time specified in a prior call to <see
        /// cref="Control.MouseMove"/>.
        /// </summary>
        public const int MouseHover = 0x2A1;

        /// <summary>
        /// Posted to a window when the cursor leaves the client area of the
        /// window specified in a prior call to <see cref="
        /// Control.MouseMove"/>.
        /// </summary>
        public const int MouseLeave = 0x2A3;

        /// <summary>
        /// Sent to a window after its size has changed.
        /// </summary>
        public const int Size = 0x005;

        /// <summary>
        /// Sent to a window that the user is resizing. By processing this
        /// message, an application can monitor the size and position of the
        /// drag rectangle and, if needed, change its size or position.
        /// </summary>
        public const int Sizing = 0x0214;
    }
}
