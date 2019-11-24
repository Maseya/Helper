// <copyright file="IIndexCollection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System.Collections.Generic;

    public interface IIndexCollection : IReadOnlyList<int>
    {
        int MinIndex
        {
            get;
        }

        int MaxIndex
        {
            get;
        }

        bool ContainsIndex(int index);

        int[] ToArray();

        IIndexCollection Copy();

        IIndexCollection Move(int offset);

        IEnumerable<(int index, T value)> EnumerateIndexValues<T>(
            IReadOnlyList<T> list);

        IIndexCollection ToByteSelection<T>(
            int startOffset,
            IByteDataConverter<T> converter);
    }
}
