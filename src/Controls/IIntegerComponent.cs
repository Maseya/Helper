// <copyright file="IIntegerComponent.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Controls
{
    using System;
    using System.ComponentModel;

    public interface IIntegerComponent : IComponent
    {
        event EventHandler ValueChanged;

        int Value { get; set; }
    }
}
