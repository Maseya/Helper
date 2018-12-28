// <copyright file="ExceptionMessageBox.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.Windows.Forms;
    using Helper;

    /// <summary>
    /// Displays message boxes for showing and handling a caught <see
    /// cref="Exception"/> to the user.
    /// </summary>
    public class ExceptionMessageBox : IExceptionHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="
        /// ExceptionMessageBox"/> class with the specified window owner and
        /// dialog caption.
        /// </summary>
        /// <param name="owner">
        /// An implementation of <see cref="IWin32Window"/> that will open the
        /// modal dialog box.
        /// </param>
        /// <param name="caption">
        /// The text to display in the title bar of the message box.
        /// </param>
        public ExceptionMessageBox(
            IWin32Window owner = null,
            string caption = "")
        {
            Owner = owner;
            Caption = caption;
        }

        /// <summary>
        /// Gets or sets an implementation of <see cref="IWin32Window"/> that
        /// will open the modal dialog box.
        /// </summary>
        public IWin32Window Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the text to display in the title bar of the message
        /// box.
        /// </summary>
        public string Caption
        {
            get;
            set;
        }

        /// <summary>
        /// Displays a message box that shows <see cref="Exception"/> info to
        /// the user.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to show.
        /// </param>
        /// <param name="owner">
        /// An implementation of <see cref="IWin32Window"/> that will open the
        /// modal dialog box.
        /// </param>
        /// <param name="caption">
        /// The text to display in the title bar of the message box.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex"/> is <see langword="null"/>.
        /// </exception>
        public static void Show(
            Exception ex,
            IWin32Window owner = null,
            string caption = "")
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            RtlAwareMessageBox.Show(
                ex.Message,
                owner,
                caption,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Displays a message box that shows <see cref="Exception"/> info to
        /// the user and prompts the user to retry the exceptional action.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to show.
        /// </param>
        /// <param name="owner">
        /// An implementation of <see cref="IWin32Window"/> that will open the
        /// modal dialog box.
        /// </param>
        /// <param name="caption">
        /// The text to display in the title bar of the message box.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the user selects to retry the process
        /// that threw the exception; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex"/> is <see langword="null"/>.
        /// </exception>
        public static bool ShowAndRetry(
            Exception ex,
            IWin32Window owner = null,
            string caption = "")
        {
            if (ex is null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            var dialogResult = RtlAwareMessageBox.Show(
                ex.Message,
                owner,
                caption,
                MessageBoxButtons.RetryCancel,
                MessageBoxIcon.Warning);

            return dialogResult == DialogResult.Retry;
        }

        /// <summary>
        /// Displays a message box that shows <see cref="Exception"/> info to
        /// the user.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to show.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex"/> is <see langword="null"/>.
        /// </exception>
        public void ShowException(Exception ex)
        {
            Show(ex, Owner, Caption);
        }

        /// <summary>
        /// Displays a message box that shows <see cref="Exception"/> info to
        /// the user and prompts the user to retry the exceptional action
        /// again.
        /// </summary>
        /// <param name="ex">
        /// The <see cref="Exception"/> to show.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the user selects Retry, otherwise <see
        /// langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="ex"/> is <see langword="null"/>.
        /// </exception>
        public bool ShowExceptionAndRetry(Exception ex)
        {
            return ShowAndRetry(ex, Owner, Caption);
        }
    }
}
