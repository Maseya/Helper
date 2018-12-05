// <copyright file="SafeNativeMethods.cs" company="Public Domain">
//     Copyright (c) 2018 Nelson Garcia. All rights reserved. Licensed
//     under GNU Affero General Public License. See LICENSE in project
//     root for full license information, or visit
//     https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides static methods and properties for WinAPI method calls
    /// that have a safe context.
    /// </summary>
    internal static class SafeNativeMethods
    {
        /// <summary>
        /// Retrieves the specified system metric or system
        /// configuration setting.
        /// </summary>
        /// <param name="index">
        /// The system metric or configuration setting to be retrieved.
        /// </param>
        /// <returns>
        /// If the function succeeds, the return value is the requested
        /// system metric or configuration setting.<para/>If the
        /// function fails, the return value is 0.
        /// </returns>
        /// <remarks>
        /// See WinAPI documentation for a comprehensive list of valid
        /// values for <paramref name="index"/>.
        /// </remarks>
        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int index);
    }
}
