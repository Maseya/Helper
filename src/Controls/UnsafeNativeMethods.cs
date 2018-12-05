// <copyright file="UnsafeNativeMethods.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Security;
    using System.Windows.Forms;

    /// <summary>
    /// Provides static methods and properties for WinAPI method calls
    /// that have an unsafe context.
    /// </summary>
    internal static class UnsafeNativeMethods
    {
        /// <summary>
        /// Retrieves information about the specified window. The
        /// function also retrieves the 32-bit value at the specified
        /// offset into the extra window memory.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window and, indirectly, the class to which
        /// the window belongs.
        /// </param>
        /// <param name="nIndex">
        /// The zero-based offset to the value to be retrieved.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the requested
        /// value. <para/> If the function fails, the return value is
        /// zero. To get extended error information, call <see cref="
        /// Marshal.GetLastWin32Error()"/>.<para/> If <see cref="
        /// SetWindowLong(IntPtr, Int32, Int32)"/> has not been called
        /// previously, <see cref="GetWindowLong(IntPtr, Int32)"/>
        /// returns zero for values in the extra window or class memory.
        /// </returns>
        /// <remarks>
        /// See WinAPI documentation for a comprehensive list of valid
        /// values for <paramref name="nIndex"/>.
        /// </remarks>
        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(
            IntPtr hWnd,
            int nIndex);

        /// <summary>
        /// Changes an attribute of the specified window. The function
        /// also sets the 32-bit value at the specified offset into the
        /// extra window memory.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window and, indirectly, the class to which
        /// the window belongs.
        /// </param>
        /// <param name="nIndex">
        /// The zero-based offset to the value to be retrieved.
        /// </param>
        /// <param name="dwNewLong">
        /// The replacement value.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the previous
        /// value of the specified 32-bit integer.<para/>If the function
        /// fails, the return value is zero. To get extended error
        /// information, call <see cref="Marshal.GetLastWin32Error()"/>.
        /// <para/>If the previous value of the specified 32-bit integer
        /// is zero, and the function succeeds, the return value is
        /// zero, but the function does not clear the last error
        /// information. This makes it difficult to determine success or
        /// failure. To deal with this, you should clear the last error
        /// information by calling <see cref="SetLastError(Int32)"/>
        /// with 0 before calling <see cref="SetWindowLong(IntPtr,
        /// Int32, Int32)"/>. Then, function failure will be indicated
        /// by a return value of zero and a <see cref="Marshal.
        /// GetLastWin32Error"/> result that is nonzero.
        /// </returns>
        /// <remarks>
        /// See WinAPI documentation for a comprehensive list of valid
        /// values for <paramref name="nIndex"/>.
        /// </remarks>
        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern int SetWindowLong(
            IntPtr hWnd,
            int nIndex,
            int dwNewLong);

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the
        /// specified window. The dimensions are given in screen
        /// coordinates that are relative to the upper-left corner of
        /// the screen.
        /// </summary>
        /// <param name="hWnd">
        /// A handle to the window.
        /// </param>
        /// <param name="lpRect">
        /// A pointer to <see cref="WinApiRectangle"/> structure that
        /// receives the screen coordinates of the upper-left and
        /// lower-right corners of the window.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// <para/>If the function fails, the return value is zero. To
        /// get extended error information, call <see cref="Marshal.
        /// GetLastWin32Error()"/>
        /// </returns>
        [SecurityCritical]
        [DllImport("user32.dll", SetLastError = true)]
        private static extern unsafe int GetWindowRect(
            IntPtr hWnd,
            WinApiRectangle* lpRect);

        /// <summary>
        /// Sets the last-error code for the calling thread.
        /// </summary>
        /// <param name="dwErrCode">
        /// The last-error code for the thread.
        /// </param>
        [SecurityCritical]
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern void SetLastError(int dwErrCode);

        /// <summary>
        /// Retrieves information about the specified window.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> to get the
        /// information from.
        /// </param>
        /// <param name="index">
        /// The zero-based offset to the value to be retrieved.
        /// </param>
        /// <returns>
        /// The required value from the window specified by the
        /// <see cref="IWin32Window.Handle"/> of <paramref name="
        /// window"/>.
        /// </returns>
        /// <remarks>
        /// See WinAPI documentation for a comprehensive list of valid
        /// values for <paramref name="index"/>.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        public static int GetWindowLong(IWin32Window window, int index)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var code = GetWindowLong(window.Handle, index);

            if (IsWinApiError(code))
            {
                throw new Win32Exception();
            }

            return code;
        }

        /// <summary>
        /// Changes an attribute of the specified window.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> whose
        /// attributes will be changed.
        /// </param>
        /// <param name="index">
        /// The zero-based offset to the value to be retrieved.
        /// </param>
        /// <param name="value">
        /// The replacement value.
        /// </param>
        /// <returns>
        /// The previous value of the attribute that was changed.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        internal static int SetWindowLong(
            IWin32Window window,
            int index,
            int value)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            SetLastError(0);
            var code = SetWindowLong(window.Handle, index, value);

            if (IsWinApiError(code))
            {
                throw new Win32Exception();
            }

            return code;
        }

        /// <summary>
        /// Retrieves the dimensions of the bounding rectangle of the
        /// specified window. The dimensions are given in screen
        /// coordinates that are relative to the upper-left corner of
        /// the screen.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> to get the
        /// screen coordinates from.
        /// </param>
        /// <returns>
        /// A <see cref="WinApiRectangle"/> structure that receives the
        /// screen coordinates of the upper-left and lower-right corners
        /// of the window described by <see cref="IWin32Window.Handle"/>
        /// of <paramref name="window"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        internal static WinApiRectangle GetWindowRect(
            IWin32Window window)
        {
            if (window is null)
            {
                throw new ArgumentNullException(nameof(window));
            }

            var rect = WinApiRectangle.Empty;
            int code;

            unsafe
            {
                code = GetWindowRect(window.Handle, &rect);
            }

            if (IsWinApiError(code))
            {
                throw new Win32Exception();
            }

            return rect;
        }

        /// <summary>
        /// Determines whether a WinAPI return value elicits an error
        /// has occurred.
        /// </summary>
        /// <param name="code">
        /// The value to inspect.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="code"/> is zero
        /// and <see cref="Marshal.GetLastWin32Error()"/> is non-zero;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        private static bool IsWinApiError(int code)
        {
            return code == 0
                ? Marshal.GetLastWin32Error() != 0
                : false;
        }
    }
}
