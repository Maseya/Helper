// <copyright file="IndexCollection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public abstract class IndexCollection :
        IIndexCollection
    {
        public abstract int MinIndex
        {
            get;
        }

        public abstract int MaxIndex
        {
            get;
        }

        public abstract int Count
        {
            get;
        }

        public abstract int this[int index]
        {
            get;
        }

        public abstract IndexCollection Move(int offset);

        public IndexCollection Copy()
        {
            return Move(0);
        }

        public abstract bool ContainsIndex(int index);

        public IEnumerable<T> EnumerateValues<T>(IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (MinIndex < 0 || MaxIndex >= list.Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var index in this)
            {
                yield return list[index];
            }
        }

        public IEnumerable<(int index, T value)> EnumerateIndexValues<T>(
            IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            if (MinIndex < 0 || MaxIndex >= list.Count)
            {
                throw new InvalidOperationException();
            }

            foreach (var index in this)
            {
                yield return (index, list[index]);
            }
        }

        public int[] ToArray()
        {
            var result = new int[Count];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = this[i];
            }

            return result;
        }

        public Dictionary<int, T> GetValueDictionary<T>(IReadOnlyList<T> list)
        {
            if (list is null)
            {
                throw new ArgumentNullException(nameof(list));
            }

            var result = new Dictionary<int, T>(list.Count);
            foreach (var (index, value) in EnumerateIndexValues(list))
            {
                result.Add(index, value);
            }

            return result;
        }

        public abstract IEnumerator<int> GetEnumerator();

        IIndexCollection IIndexCollection.Move(int offset)
        {
            return Move(offset);
        }

        IIndexCollection IIndexCollection.Copy()
        {
            return Copy();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IIndexCollection IIndexCollection.ToByteSelection<T>(
            int startOffset,
            IByteDataConverter<T> converter)
        {
            return ToByteSelection(startOffset, converter);
        }

        public HashListIndexCollection ToByteSelection<T>(
            int startOffset,
            IByteDataConverter<T> converter)
        {
            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            var sizeOfT = converter.SizeOfItem;
            return new HashListIndexCollection(
                Enumerable.Select(this, GetOffset));

            int GetOffset(int index)
            {
                return converter.GetOffset(startOffset, index);
            }
        }
    }
}
