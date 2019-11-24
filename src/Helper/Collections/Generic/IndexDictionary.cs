// <copyright file="IndexDictionary.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using static ThrowHelper;

    public sealed class IndexDictionary<T> :
        IIndexDictionary<T>,
        IReadOnlyDictionary<int, T>
        where T : unmanaged
    {
        public IndexDictionary(
            IIndexCollection selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            Selection = selection.Copy();
            BaseDictionary = new Dictionary<int, T>(Selection.Count);
            foreach (var index in Selection)
            {
                BaseDictionary.Add(index, default);
            }
        }

        public IndexDictionary(
            IIndexCollection selection,
            IReadOnlyList<T> data)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (selection.MinIndex < 0 || selection.MaxIndex >= data.Count)
            {
                throw IndexBoundsArgumentException(nameof(selection));
            }

            Selection = selection.Copy();
            BaseDictionary = new Dictionary<int, T>(Selection.Count);
            foreach (var index in Selection)
            {
                BaseDictionary.Add(index, data[index]);
            }
        }

        public IndexDictionary(
            IIndexCollection selection,
            byte[] sourceArray,
            IByteDataConverter<T> converter)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (sourceArray is null)
            {
                throw new ArgumentNullException(nameof(sourceArray));
            }

            if (converter is null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            if (selection.MinIndex < 0)
            {
                throw IndexBoundsArgumentException(nameof(selection));
            }

            var sizeOfT = converter.SizeOfItem;
            var lastByteIndex = selection.MaxIndex + sizeOfT - 1;
            if (lastByteIndex >= sourceArray.Length)
            {
                throw IndexBoundsArgumentException(nameof(selection));
            }

            Selection = selection.Copy();
            Converter = converter;
            BaseDictionary = new Dictionary<int, T>(Selection.Count);
            foreach (var index in Selection)
            {
                var item = converter.GetItem(sourceArray, index);
                BaseDictionary.Add(index, item);
            }
        }

        private IndexDictionary(
            IIndexCollection selection,
            IDictionary<int, T> dictionary)
        {
            Selection = selection.Copy();
            BaseDictionary = new Dictionary<int, T>(dictionary);
        }

        public int Count
        {
            get
            {
                return Selection.Count;
            }
        }

        public bool IsConvertedByteData
        {
            get
            {
                return Converter != null;
            }
        }

        public IIndexCollection Selection
        {
            get;
        }

        public ICollection<T> Values
        {
            get
            {
                return BaseDictionary.Values;
            }
        }

        public IByteDataConverter<T> Converter
        {
            get;
        }

        ICollection<int> IDictionary<int, T>.Keys
        {
            get
            {
                return new KeyCollection(Selection);
            }
        }

        IEnumerable<int> IReadOnlyDictionary<int, T>.Keys
        {
            get
            {
                return Selection;
            }
        }

        ICollection<T> IDictionary<int, T>.Values
        {
            get
            {
                return Values;
            }
        }

        IEnumerable<T> IReadOnlyDictionary<int, T>.Values
        {
            get
            {
                return Values;
            }
        }

        bool ICollection<KeyValuePair<int, T>>.IsReadOnly
        {
            get
            {
                return false;
            }
        }

        private Dictionary<int, T> BaseDictionary
        {
            get;
        }

        public T this[int key]
        {
            get
            {
                return BaseDictionary[key];
            }

            set
            {
                BaseDictionary[key] = value;
            }
        }

        public IndexDictionary<T> Copy()
        {
            return new IndexDictionary<T>(Selection, BaseDictionary);
        }

        public bool ContainsKey(int key)
        {
            return Selection.ContainsIndex(key);
        }

        public IEnumerator<KeyValuePair<int, T>> GetEnumerator()
        {
            foreach (var key in Selection)
            {
                yield return new KeyValuePair<int, T>(
                    key,
                    BaseDictionary[key]);
            }
        }

        public bool TryGetValue(int key, out T value)
        {
            return BaseDictionary.TryGetValue(key, out value);
        }

        IIndexDictionary<T> IIndexDictionary<T>.Copy()
        {
            return Copy();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void IDictionary<int, T>.Add(int key, T value)
        {
            throw new NotSupportedException();
        }

        bool IDictionary<int, T>.Remove(int key)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<int, T>>.Remove(
            KeyValuePair<int, T> item)
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<int, T>>.Add(KeyValuePair<int, T> item)
        {
            throw new NotSupportedException();
        }

        bool ICollection<KeyValuePair<int, T>>.Contains(
            KeyValuePair<int, T> item)
        {
            return (BaseDictionary as ICollection<KeyValuePair<int, T>>).
                Contains(item);
        }

        void ICollection<KeyValuePair<int, T>>.Clear()
        {
            throw new NotSupportedException();
        }

        void ICollection<KeyValuePair<int, T>>.CopyTo(
            KeyValuePair<int, T>[] array, int arrayIndex)
        {
            (BaseDictionary as ICollection<KeyValuePair<int, T>>).CopyTo(
                array,
                arrayIndex);
        }

        public void WriteToBytes(byte[] destinationArray)
        {
            WriteToBytes(destinationArray, 0);
        }

        public void WriteToBytes(byte[] destinationArray, int startOffset)
        {
            if (!IsConvertedByteData)
            {
                throw new InvalidOperationException();
            }

            if (destinationArray is null)
            {
                throw new ArgumentNullException(nameof(destinationArray));
            }

            if (startOffset + Selection.MinIndex < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(startOffset));
            }

            var sizeOfT = Converter.SizeOfItem;
            var lastOffset = Selection.MaxIndex + sizeOfT - 1;
            if (lastOffset + startOffset >= destinationArray.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(startOffset));
            }

            foreach (var kvp in this)
            {
                var index = kvp.Key;
                var bytes = Converter.GetBytes(kvp.Value);
                Array.Copy(
                    bytes,
                    0,
                    destinationArray,
                    index,
                    sizeOfT);
            }
        }

        private class KeyCollection : ICollection<int>, ICollection
        {
            public KeyCollection(IIndexCollection selection)
            {
                Selection = selection;
            }

            public int Count
            {
                get
                {
                    return Selection.Count;
                }
            }

            bool ICollection<int>.IsReadOnly
            {
                get
                {
                    return true;
                }
            }

            bool ICollection.IsSynchronized
            {
                get
                {
                    return false;
                }
            }

            object ICollection.SyncRoot
            {
                get;
            }

            private IIndexCollection Selection
            {
                get;
            }

            public bool Contains(int key)
            {
                return Selection.ContainsIndex(key);
            }

            public IEnumerator<int> GetEnumerator()
            {
                return Selection.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            void ICollection<int>.CopyTo(int[] array, int arrayIndex)
            {
                for (var i = 0; i < Selection.Count; i++)
                {
                    array[arrayIndex + i] = Selection[i];
                }
            }

            void ICollection.CopyTo(Array array, int index)
            {
                for (var i = 0; i < Selection.Count; i++)
                {
                    array.SetValue(i, index + i);
                }
            }

            void ICollection<int>.Add(int item)
            {
                throw new NotSupportedException();
            }

            void ICollection<int>.Clear()
            {
                throw new NotSupportedException();
            }

            bool ICollection<int>.Remove(int item)
            {
                throw new NotSupportedException();
            }
        }
    }
}
