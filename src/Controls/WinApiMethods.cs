// <copyright file="WinApiMethods.cs" company="Public Domain">
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
    using System.Windows.Forms;
    using static SafeNativeMethods;
    using static UnsafeNativeMethods;

    /// <summary>
    /// Provides static methods and properties for WinAPI functions in
    /// a more .NET-friendly format.
    /// </summary>
    public static class WinApiMethods
    {
        /// <summary>
        /// Extended window style.
        /// </summary>
        private const int GwlExStyle = -20;

        /// <summary>
        /// Window style.
        /// </summary>
        private const int GwlStyle = -16;

        /// <summary>
        /// The window has a thin border.
        /// </summary>
        private const int WsBorder = 0x800000;

        /// <summary>
        /// The window has a border with a sunken edge.
        /// </summary>
        private const int WsExClientEdge = 0x200;

        /// <summary>
        /// The amount of border padding for captioned windows, in
        /// pixels.
        /// </summary>
        private const int SmCenterXPaddedBorder = 92;

        /// <summary>
        /// Gets the amount of border padding for captioned windows, in
        /// pixels.
        /// </summary>
        public static int PaddedBorderWidth
        {
            get
            {
                return GetSystemMetrics(SmCenterXPaddedBorder);
            }
        }

        /// <summary>
        /// Gets the amount of border padding for captioned windows, in
        /// pixels.
        /// </summary>
        public static Size PaddedBorderSize
        {
            get
            {
                return new Size(PaddedBorderWidth, PaddedBorderWidth);
            }
        }

        /// <summary>
        /// Gets the <see cref="BorderStyle"/> of an <see cref="
        /// IWin32Window"/>.
        /// </summary>
        /// <param name="window">
        /// The <see cref="IWin32Window"/> to get the <see cref="
        /// BorderStyle"/> from.
        /// </param>
        /// <returns>
        /// The <see cref="BorderStyle"/> of the window referenced by
        /// <see cref="IWin32Window.Handle"/> of <paramref name="
        /// window"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        public static BorderStyle GetBorderStyle(IWin32Window window)
        {
            // If the window has a 3D border extended-window style, this
            // supersedes that standard window-style border attribute,
            // so we test this first and return the 3D border style.
            return IsBorderFixed3D(window)
                ? BorderStyle.Fixed3D
                : IsBorderFixedSingle(window)
                    ? BorderStyle.FixedSingle
                    : BorderStyle.None;
        }

        /// <summary>
        /// Sets the <see cref="BorderStyle"/> of an <see cref="
        /// IWin32Window"/>.
        /// </summary>
        /// <param name="window">
        /// The <see cref="IWin32Window"/> to set the <see cref="
        /// IWin32Window"/> to.
        /// </param>
        /// <param name="borderStyle">
        /// The <see cref="BorderStyle"/> to set to the window
        /// referenced by <see cref="IWin32Window.Handle"/> of
        /// <paramref name="window"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="borderStyle"/> is not one of the values of
        /// <see cref="BorderStyle"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        public static void SetBorderStyle(
            IWin32Window window,
            BorderStyle borderStyle)
        {
            (var styleMask, var exStyleMask) = GetStyleMasks();

            var style = GetWindowStyle(window) & ~styleMask;
            var exstyle = GetWindowExStyle(window) & ~exStyleMask;

            SetWindowStyle(window, style);
            SetWindowExStyle(window, exstyle);

            (int styleMask, int exStyleMask) GetStyleMasks()
            {
                switch (borderStyle)
                {
                case BorderStyle.Fixed3D:
                    return (WsBorder, 0);

                case BorderStyle.FixedSingle:
                    return (0, WsExClientEdge);

                case BorderStyle.None:
                    return (WsBorder, WsExClientEdge);

                default:
                    throw new InvalidEnumArgumentException(
                        nameof(borderStyle),
                        (int)borderStyle,
                        typeof(BorderStyle));
                }
            }
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
        /// <remarks>
        /// Do not call this method in any constructors that override
        /// the <see cref="Control"/> class. It causes undefined
        /// behavior that is hard to track.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        public static Rectangle GetWindowRectangle(
            IWin32Window window)
        {
            return GetWindowRect(window);
        }

        /// <summary>
        /// Gets the thickness, in pixels, of a given window.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> to get the
        /// screen coordinates from.
        /// </param>
        /// <returns>
        /// The size of the border of <paramref name="window"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        public static Size GetBorderSize(IWin32Window window)
        {
            var borderStyle = GetBorderStyle(window);
            return GetBorderSize(borderStyle);
        }

        /// <summary>
        /// Gets the thickness, in pixels, of a window border determined
        /// by its <see cref="BorderStyle"/> value.
        /// </summary>
        /// <param name="borderStyle">
        /// The <see cref="BorderStyle"/> of an implementation of
        /// <see cref="IWin32Window"/> to get the border size of.
        /// </param>
        /// <returns>
        /// The thickness, in pixels, of an implementation of
        /// <see cref="IWin32Window"/> whose <see cref="BorderStyle"/>
        /// property is <paramref name="borderStyle"/>.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="borderStyle"/> is not one of the values of
        /// <see cref="BorderStyle"/>.
        /// </exception>
        public static Size GetBorderSize(BorderStyle borderStyle)
        {
            switch (borderStyle)
            {
            case BorderStyle.None:
                return Size.Empty;

            case BorderStyle.FixedSingle:
                return SystemInformation.BorderSize;

            case BorderStyle.Fixed3D:
                return SystemInformation.Border3DSize;

            default:
                throw new InvalidEnumArgumentException(
                    nameof(borderStyle),
                    (int)borderStyle,
                    typeof(BorderStyle));
            }
        }

        /// <summary>
        /// Gets the <see cref="Padding"/> of the border of an
        /// implementation of <see cref="IWin32Window"/>.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> to get the
        /// screen coordinates from.
        /// </param>
        /// <returns>
        /// A <see cref="Padding"/> value that describes the border
        /// of <paramref name="window"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        public static Padding GetBorderPadding(IWin32Window window)
        {
            var borderSize = GetBorderSize(window);
            return new Padding(
                borderSize.Width,
                borderSize.Height,
                borderSize.Width,
                borderSize.Height);
        }

        /// <summary>
        /// Gets the <see cref="Padding"/> that describes the
        /// difference in edge offsets of two <see cref="Rectangle"/>
        /// structures.
        /// </summary>
        /// <param name="large">
        /// The larger <see cref="Rectangle"/> that represents the
        /// exterior of a border.
        /// </param>
        /// <param name="small">
        /// The smaller <see cref="Rectangle"/> that represents the
        /// interior of a border.
        /// </param>
        /// <returns>
        /// A <see cref="Padding"/> whose <see cref="Padding.Left"/>,
        /// <see cref="Padding.Top"/>, <see cref="Padding.Right"/>, and
        /// <see cref="Padding.Bottom"/> values are the respective
        /// differences of the values of <see cref="Rectangle.Left"/>,
        /// <see cref="Rectangle.Top"/>, <see cref="Rectangle.Right"/>,
        /// and <see cref="Rectangle.Bottom"/> of <paramref name="
        /// small"/> subtracted from <paramref name="large"/>.
        /// </returns>
        public static Padding GetPadding(
            Rectangle large,
            Rectangle small)
        {
            return new Padding(
                small.Left - large.Left,
                small.Top - large.Top,
                large.Right - small.Right,
                large.Bottom - small.Bottom);
        }

        /// <summary>
        /// Inflates a <see cref="Rectangle"/> by a specified <see
        /// cref="Padding"/> amount.
        /// </summary>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> to inflate.
        /// </param>
        /// <param name="padding">
        /// The <see cref="Padding"/> amount to increase each side of
        /// <paramref name="rectangle"/> by.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="rectangle"/> whose
        /// <see cref="Rectangle.Left"/>, <see cref="Rectangle.Top"/>,
        /// <see cref="Rectangle.Right"/>, and <see cref="Rectangle.
        /// Bottom"/> values are increased, respectively, by the values
        /// of <see cref="Padding.Left"/>, <see cref="Padding.Top"/>,
        /// <see cref="Padding.Right"/>, and <see cref="Padding.
        /// Bottom"/> of <paramref name="padding"/>.
        /// </returns>
        public static Rectangle InflateRectangle(
            Rectangle rectangle,
            Padding padding)
        {
            return Rectangle.FromLTRB(
                rectangle.Left - padding.Left,
                rectangle.Top - padding.Top,
                rectangle.Right + padding.Right,
                rectangle.Bottom + padding.Bottom);
        }

        /// <summary>
        /// Deflates a <see cref="Rectangle"/> by a specified <see
        /// cref="Padding"/> amount.
        /// </summary>
        /// <param name="rectangle">
        /// The <see cref="Rectangle"/> to deflate.
        /// </param>
        /// <param name="padding">
        /// The <see cref="Padding"/> amount to reduce each side of
        /// <paramref name="rectangle"/> by.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="rectangle"/> whose
        /// <see cref="Rectangle.Left"/>, <see cref="Rectangle.Top"/>,
        /// <see cref="Rectangle.Right"/>, and <see cref="Rectangle.
        /// Bottom"/> values are reduced, respectively, by the values of
        /// <see cref="Padding.Left"/>, <see cref="Padding.Top"/>,
        /// <see cref="Padding.Right"/>, and <see cref="Padding.
        /// Bottom"/> of <paramref name="padding"/>.
        /// </returns>
        public static Rectangle DeflateRectangle(
            Rectangle rectangle,
            Padding padding)
        {
            return Rectangle.FromLTRB(
                rectangle.Left + padding.Left,
                rectangle.Top + padding.Top,
                rectangle.Right - padding.Right,
                rectangle.Bottom - padding.Bottom);
        }

        /// <summary>
        /// Inflates a <see cref="Size"/> by a specified <see cref="
        /// Padding"/> amount.
        /// </summary>
        /// <param name="size">
        /// The <see cref="Size"/> to inflate.
        /// </param>
        /// <param name="padding">
        /// The <see cref="Padding"/> amount to increase <paramref
        /// name="size"/> by.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="size"/> whose
        /// <see cref="Size.Width"/> and <see cref="Size.Height"/>
        /// values are increased, respectively, by the values
        /// of <see cref="Padding.Horizontal"/> and <see cref="Padding.
        /// Vertical"/> of <paramref name="padding"/>.
        /// </returns>
        public static Size InflateSize(Size size, Padding padding)
        {
            return new Size(
                size.Width + padding.Horizontal,
                size.Height + padding.Vertical);
        }

        /// <summary>
        /// Deflates a <see cref="Size"/> by a specified <see cref="
        /// Padding"/> amount.
        /// </summary>
        /// <param name="size">
        /// The <see cref="Size"/> to deflate.
        /// </param>
        /// <param name="padding">
        /// The <see cref="Padding"/> amount to decrease <paramref
        /// name="size"/> by.
        /// </param>
        /// <returns>
        /// A copy of <paramref name="size"/> whose
        /// <see cref="Size.Width"/> and <see cref="Size.Height"/>
        /// values are decreased, respectively, by the values
        /// of <see cref="Padding.Horizontal"/> and <see cref="Padding.
        /// Vertical"/> of <paramref name="padding"/>.
        /// </returns>
        public static Size DeflateSize(Size size, Padding padding)
        {
            return new Size(
                size.Width - padding.Horizontal,
                size.Height - padding.Vertical);
        }

        /// <summary>
        /// Gets a value indicating whether a window has a Fixed3D
        /// border style.
        /// </summary>
        /// <param name="window">
        /// The <see cref="IWin32Window"/> to inspect the Fixed3D border
        /// style of.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="window"/> has
        /// sunken-edge 3D border; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        private static bool IsBorderFixed3D(IWin32Window window)
        {
            return (GetWindowExStyle(window) & WsExClientEdge) != 0;
        }

        /// <summary>
        /// Gets a value indicating whether a window has a FixedSingle
        /// border style.
        /// </summary>
        /// <param name="window">
        /// The <see cref="IWin32Window"/> to inspect the FixedSingle
        /// border style of.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="window"/> has
        /// flat-styled border; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        private static bool IsBorderFixedSingle(IWin32Window window)
        {
            return (GetWindowStyle(window) & WsBorder) != 0;
        }

        /// <summary>
        /// Gets the window style of a window.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> to get the
        /// screen coordinates from.
        /// </param>
        /// <returns>
        /// The window style of <paramref name="window"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        private static int GetWindowStyle(IWin32Window window)
        {
            return GetWindowLong(window, GwlStyle);
        }

        /// <summary>
        /// Gets the extended window style of a window.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> to get the
        /// screen coordinates from.
        /// </param>
        /// <returns>
        /// The extended window style of <paramref name="window"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        private static int GetWindowExStyle(IWin32Window window)
        {
            return GetWindowLong(window, GwlExStyle);
        }

        /// <summary>
        /// Sets the window style of a window.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> to get the
        /// screen coordinates from.
        /// </param>
        /// <param name="value">
        /// The window style value to set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        private static void SetWindowStyle(
            IWin32Window window,
            int value)
        {
            SetWindowLong(window, GwlStyle, value);
        }

        /// <summary>
        /// Sets the extended window style of a window.
        /// </summary>
        /// <param name="window">
        /// An implementation of <see cref="IWin32Window"/> to get the
        /// screen coordinates from.
        /// </param>
        /// <param name="value">
        /// The window ex style value to set.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="window"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// A native WinAPI call returned an error.
        /// </exception>
        private static void SetWindowExStyle(
            IWin32Window window,
            int value)
        {
            SetWindowLong(window, GwlExStyle, value);
        }
    }
}
