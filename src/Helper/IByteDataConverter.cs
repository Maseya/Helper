// <copyright file="IByteDataConverter.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper
{
    using System.Collections.Generic;

    public interface IByteDataConverter<T>
    {
        int SizeOfItem
        {
            get;
        }

        T GetItem(IEnumerable<byte> bytes);

        IEnumerable<byte> GetBytes(T item);
    }
}
