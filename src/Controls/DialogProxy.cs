// <copyright file="DialogProxy.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    /// <summary>
    /// Provides a simplified representation of forms that are intended
    /// to only be handled as dialog windows. This is an abstract class.
    /// </summary>
    /// <remarks>
    /// Many windows <see cref="Form"/> classes are intended to be use
    /// as modal dialog boxes. These classes usually do not intend to
    /// make public the many properties and methods that a form exposes.
    /// This class is therefore used to make public only the essential
    /// parameters than an application developer intends to make usable.
    /// The base <see cref="Form"/> is kept internal so inheritors can
    /// select which properties, methods, and events should be visible.
    /// </remarks>
    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    public abstract class DialogProxy :
        MarshalByRefObject,
        IComponent,
        IDisposable
    {
        /// <summary>
        /// Occurs when the user clicks  the Help button in the dialog
        /// box.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Microsoft.Design",
            "CA1009:DeclareEventHandlersCorrectly",
            Justification = "Legacy name choice by Microsoft.")]
        public event HelpEventHandler HelpRequested;

        /// <summary>
        /// Occurs when the component is disposed by a call to the
        /// <see cref="Dispose()"/> method.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Gets the <see cref="Form"/> to use for modal dialog
        /// operations.
        /// </summary>
        protected Form BaseForm
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Help button is
        /// displayed in the dialog box.
        /// </summary>
        public bool ShowHelp
        {
            get
            {
                return BaseForm.HelpButton;
            }

            set
            {
                BaseForm.HelpButton = value;
            }
        }

        /// <summary>
        /// Gets or sets the dialog box title.
        /// </summary>
        public string Title
        {
            get
            {
                return BaseForm.Text;
            }

            set
            {
                BaseForm.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets an object that contains data about the control.
        /// </summary>
        public object Tag
        {
            get
            {
                return BaseForm.Tag;
            }

            set
            {
                BaseForm.Tag = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ISite"/> of the <see cref="
        /// Component"/>.
        /// </summary>
        public ISite Site
        {
            get
            {
                return BaseForm.Site;
            }

            set
            {
                BaseForm.Site = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogProxy"/>
        /// class.
        /// </summary>
        /// <param name="baseForm">
        /// The <see cref="Form"/> to use for modal dialog operations.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseForm"/> is <see langword="null"/>.
        /// </exception>
        protected DialogProxy(Form baseForm)
        {
            BaseForm = baseForm ??
                throw new ArgumentNullException(nameof(baseForm));

            BaseForm.HelpRequested += (sender, e) => OnHelpRequested(e);
            BaseForm.Disposed += (sender, e) => OnDisposed(e);
        }

        /// <summary>
        /// Runs a common dialog box with a specified owner or default
        /// if none is given.
        /// </summary>
        /// <param name="owner">
        /// An <see cref="IWin32Window"/> that represents the top-level
        /// windows the will own the modal dialog box, or <see
        /// langword="null"/> to specify the currently active window of
        /// your application.
        /// </param>
        /// <returns>
        /// The <see cref="DialogResult"/> that this dialog box returns
        /// when it closes.
        /// </returns>
        public DialogResult ShowDialog(IWin32Window owner = null)
        {
            return BaseForm.ShowDialog(owner);
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Component"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Raises the <see cref="HelpRequested"/> event.
        /// </summary>
        /// <param name="e">
        /// A <see cref="HelpEventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnHelpRequested(HelpEventArgs e)
        {
            HelpRequested?.Invoke(this, e);
        }

        /// <summary>
        /// Raises the <see cref="Disposed"/> event.
        /// </summary>
        /// <param name="e">
        /// An <see cref="EventArgs"/> that contains the event data.
        /// </param>
        protected virtual void OnDisposed(EventArgs e)
        {
            Disposed?.Invoke(this, e);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="
        /// DialogProxy"/> and optionally releases the managed
        /// resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to released both managed and
        /// unmanaged resources; <see langword="false"/> to release only
        /// unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                BaseForm.Dispose();
            }
        }
    }
}
