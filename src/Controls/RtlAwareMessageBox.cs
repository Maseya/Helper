// <copyright file="RtlAwareMessageBox.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.Globalization;
    using System.Windows.Forms;

    /// <summary>
    /// Wraps the <see cref="MessageBox"/> class with the addition of automatic
    /// right-to-left detection.
    /// </summary>
    /// <remarks>
    /// The original <see cref="MessageBox"/> class allowed users to create
    /// dialog boxes without specifying a <see cref=" MessageBoxOptions"/>
    /// parameter. This would create dialog boxes that would not match the
    /// globalization parameters of the current UI culture.
    /// </remarks>
    public static class RtlAwareMessageBox
    {
        /// <summary>
        /// Specifies a <see cref="MessageBoxOptions"/> value for a message box
        /// whose text is right-aligned and displayed in right-to-left reading
        /// order.
        /// </summary>
        public const MessageBoxOptions RightToLeftMessageBoxOptions =
            MessageBoxOptions.RightAlign |
            MessageBoxOptions.RtlReading;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TextInfo"/> of <see
        /// cref="CultureInfo.CurrentUICulture"/> represents a writing system
        /// where text flows from right to left.
        /// </summary>
        public static bool IsCurrentUICultureRightToLeft
        {
            get
            {
                return CultureInfo.CurrentUICulture.TextInfo.
                    IsRightToLeft;
            }
        }

        /// <summary>
        /// Displays a right-to-left aware message box with the specified text,
        /// caption, buttons, icon, default button, and options.
        /// </summary>
        /// <param name="text">
        /// The text to display in the message box.
        /// </param>
        /// <param name="owner">
        /// An implementation of <see cref="IWin32Window"/> that will open the
        /// modal dialog box.
        /// </param>
        /// <param name="caption">
        /// The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        /// One of the <see cref="MessageBoxButtons"/> values that specifies
        /// which buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        /// One of the <see cref="MessageBoxIcon"/> values that specifies which
        /// icon to display in the message box.
        /// </param>
        /// <param name="defaultButton">
        /// One of the <see cref="MessageBoxDefaultButton"/> values that
        /// specifies which icon to display in the message box.
        /// </param>
        /// <param name="options">
        /// One of the <see cref="MessageBoxOptions"/> values that specifies
        /// which display and association options will be used for the message
        /// box. Right-to-left options will automatically be added if <paramref
        /// name="owner"/> is right-to-left enabled.
        /// </param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        /// <inheritdoc cref="MessageBox.Show(IWin32Window, String, String, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton, MessageBoxOptions)" select=" exception"/>
        ///
        public static DialogResult Show(
            string text,
            IWin32Window owner = null,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton =
                MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0)
        {
            return MessageBox.Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(owner, options));
        }

        /// <summary>
        /// Displays a right-to-left aware message box with the specified text,
        /// caption, buttons, icon, default button, options, and Help button.
        /// </summary>
        /// <param name="text">
        /// The text to display in the message box.
        /// </param>
        /// <param name="displayHelpButton">
        /// <see langword="true"/> to show the Help button; otherwise, <see
        /// langword="false"/>.
        /// </param>
        /// <param name="caption">
        /// The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        /// One of the <see cref="MessageBoxButtons"/> values that specifies
        /// which buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        /// One of the <see cref="MessageBoxIcon"/> values that specifies which
        /// icon to display in the message box.
        /// </param>
        /// <param name="defaultButton">
        /// One of the <see cref="MessageBoxDefaultButton"/> values that
        /// specifies which icon to display in the message box.
        /// </param>
        /// <param name="options">
        /// One of the <see cref="MessageBoxOptions"/> values that specifies
        /// which display and association options will be used for the message
        /// box. Right-to-left options will automatically be added if the UI
        /// culture is right-to-left enabled.
        /// </param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        /// <inheritdoc cref="MessageBox.Show(String, String, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton, MessageBoxOptions, Boolean)" select="exception"/>
        ///
        public static DialogResult Show(
            string text,
            bool displayHelpButton,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton =
                MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0)
        {
            return MessageBox.Show(
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(null, options),
                displayHelpButton);
        }

        /// <summary>
        /// Displays a right-to-left aware message box with the specified text,
        /// caption, buttons, icon, default button, options, and Help button,
        /// using the specific Help file and Help keyword.
        /// </summary>
        /// <param name="text">
        /// The text to display in the message box.
        /// </param>
        /// <param name="helpFilePath">
        /// The path and name of the Help file to display when the user clicks
        /// on the Help button.
        /// </param>
        /// <param name="owner">
        /// An implementation of <see cref="IWin32Window"/> that will open the
        /// modal dialog box.
        /// </param>
        /// <param name="caption">
        /// The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        /// One of the <see cref="MessageBoxButtons"/> values that specifies
        /// which buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        /// One of the <see cref="MessageBoxIcon"/> values that specifies which
        /// icon to display in the message box.
        /// </param>
        /// <param name="defaultButton">
        /// One of the <see cref="MessageBoxDefaultButton"/> values that
        /// specifies which icon to display in the message box.
        /// </param>
        /// <param name="options">
        /// One of the <see cref="MessageBoxOptions"/> values that specifies
        /// which display and association options will be used for the message
        /// box. Right-to-left options will automatically be added if <paramref
        /// name="owner"/> is right-to-left enabled.
        /// </param>
        /// <param name="keyword">
        /// The Help keyword to display when the user clicks the Help button.
        /// </param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        /// <inheritdoc cref="MessageBox.Show(IWin32Window, String, String, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton, MessageBoxOptions, String, String)" select="exception"/>
        ///
        public static DialogResult Show(
            string text,
            string helpFilePath,
            IWin32Window owner = null,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton =
                MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0,
            string keyword = "")
        {
            return MessageBox.Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(owner, options),
                helpFilePath,
                keyword);
        }

        /// <summary>
        /// Displays a right-to-left aware message box with the specified text,
        /// caption, buttons, icon, default button, options, and Help button,
        /// using the specified Help file, <see cref="HelpNavigator"/>, and
        /// Help topic.
        /// </summary>
        /// <param name="text">
        /// The text to display in the message box.
        /// </param>
        /// <param name="helpFilePath">
        /// The path and name of the Help file to display when the user clicks
        /// on the Help button.
        /// </param>
        /// <param name="owner">
        /// An implementation of <see cref="IWin32Window"/> that will open the
        /// modal dialog box.
        /// </param>
        /// <param name="caption">
        /// The text to display in the title bar of the message box.
        /// </param>
        /// <param name="buttons">
        /// One of the <see cref="MessageBoxButtons"/> values that specifies
        /// which buttons to display in the message box.
        /// </param>
        /// <param name="icon">
        /// One of the <see cref="MessageBoxIcon"/> values that specifies which
        /// icon to display in the message box.
        /// </param>
        /// <param name="defaultButton">
        /// One of the <see cref="MessageBoxDefaultButton"/> values that
        /// specifies which icon to display in the message box.
        /// </param>
        /// <param name="options">
        /// One of the <see cref="MessageBoxOptions"/> values that specifies
        /// which display and association options will be used for the message
        /// box. Right-to-left options will automatically be added if <paramref
        /// name="owner"/> is right-to-left enabled.
        /// </param>
        /// <param name="navigator">
        /// One of the <see cref="HelpNavigator"/> values.
        /// </param>
        /// <param name="param">
        /// The numeric ID of the Help topic to display when the user clicks
        /// the Help button.
        /// </param>
        /// <returns>
        /// One of the <see cref="DialogResult"/> values.
        /// </returns>
        /// <inheritdoc cref="MessageBox.Show(IWin32Window, String, String, MessageBoxButtons, MessageBoxIcon, MessageBoxDefaultButton, MessageBoxOptions, String, HelpNavigator, Object)" select="exception"/>
        ///
        public static DialogResult Show(
            string text,
            string helpFilePath,
            IWin32Window owner = null,
            string caption = "",
            MessageBoxButtons buttons = MessageBoxButtons.OK,
            MessageBoxIcon icon = MessageBoxIcon.None,
            MessageBoxDefaultButton defaultButton =
                MessageBoxDefaultButton.Button1,
            MessageBoxOptions options = 0,
            HelpNavigator navigator = HelpNavigator.TableOfContents,
            object param = null)
        {
            return MessageBox.Show(
                owner,
                text,
                caption,
                buttons,
                icon,
                defaultButton,
                RightToLeftAwareOptions(owner, options),
                helpFilePath,
                navigator,
                param);
        }

        /// <summary>
        /// Returns a <see cref="MessageBoxOptions"/> value that will enable
        /// right-to-left flags if the owner control is right-to-left enabled.
        /// </summary>
        /// <param name="owner">
        /// The window to inspect whether right-to-left is enabled.
        /// </param>
        /// <param name="options">
        /// One of the <see cref="MessageBoxOptions"/> values that specifies
        /// which display and association options will be used for the message
        /// box. Right-to-left options will automatically be added if <paramref
        /// name="owner"/> is right-to-left enabled.
        /// </param>
        /// <returns>
        /// <paramref name="options"/> is <paramref name="owner"/> is not
        /// right-to-left enabled; otherwise, <paramref name=" options"/> with
        /// <see cref="MessageBoxOptions.RightAlign"/> and <see
        /// cref="MessageBoxOptions.RightAlign"/> enabled.
        /// </returns>
        public static MessageBoxOptions RightToLeftAwareOptions(
            IWin32Window owner,
            MessageBoxOptions options)
        {
            return IsWindowRightToLeft(owner)
                ? options | RightToLeftMessageBoxOptions
                : options;
        }

        /// <summary>
        /// Returns a value indicating whether a window's elements are aligned
        /// to support locales using right-to-left fonts.
        /// </summary>
        /// <param name="owner">
        /// The window to inspect whether right-to-left is enabled.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if <paramref name="owner"/> is a control
        /// that is right-to-left enabled; otherwise <see langword="false"/>.
        /// </returns>
        /// <remarks>
        /// If a <see cref="Control"/> cannot be constructed from <paramref
        /// name="owner"/>, then the return value is defaulted to <see
        /// cref="IsCurrentUICultureRightToLeft"/>. If <see
        /// cref="Control.RightToLeft"/> of <paramref name=" owner"/> is set to
        /// <see cref="RightToLeft.Inherit"/>, then the value of <see
        /// cref="Control.Parent"/> is used instead. If <paramref
        /// name="owner"/> has no parent, then <see cref="
        /// IsCurrentUICultureRightToLeft"/> is defaulted to again.
        /// </remarks>
        public static bool IsWindowRightToLeft(IWin32Window owner)
        {
            return owner is Control control
                ? control.RightToLeft == RightToLeft.Yes
                : IsCurrentUICultureRightToLeft;
        }
    }
}
