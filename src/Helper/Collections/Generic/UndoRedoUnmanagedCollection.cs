// <copyright file="UndoRedoUnmanagedCollection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections.Generic
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public sealed class UndoRedoUnmanagedCollection<T> :
        IUnmanagedCollection<T>,
        IReadOnlyList<T>
        where T : unmanaged
    {
        public UndoRedoUnmanagedCollection()
        {
            History = new UndoFactory();
            BaseList = new UnamangedCollection<T>();
            BaseList.ContentsModified += (s, e) => OnContentsModified(
                EventArgs.Empty);
        }

        public UndoRedoUnmanagedCollection(int capacity)
        {
            History = new UndoFactory();
            BaseList = new UnamangedCollection<T>(capacity);
            BaseList.ContentsModified += (s, e) => OnContentsModified(
                EventArgs.Empty);
        }

        public UndoRedoUnmanagedCollection(IEnumerable<T> collection)
        {
            History = new UndoFactory();
            BaseList = new UnamangedCollection<T>(collection);
            BaseList.ContentsModified += (s, e) => OnContentsModified(
                EventArgs.Empty);
        }

        public event EventHandler ContentsModified;

        public bool CanRedo
        {
            get
            {
                return History.CanRedo;
            }
        }

        public bool CanUndo
        {
            get
            {
                return History.CanUndo;
            }
        }

        public int Capacity
        {
            get
            {
                return BaseList.Capacity;
            }

            set
            {
                BaseList.Capacity = value;
            }
        }

        public int Count
        {
            get
            {
                return BaseList.Count;
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return (BaseList as ICollection<T>).IsReadOnly;
            }
        }

        private UndoFactory History
        {
            get;
        }

        private UnamangedCollection<T> BaseList
        {
            get;
        }

        public T this[int index]
        {
            get
            {
                return BaseList[index];
            }

            set
            {
                var oldValue = BaseList[index];
                ModifyList(
                    list => list[index] = value,
                    list => list[index] = oldValue);
            }
        }

        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            InsertRange(Count, collection);
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            return new ReadOnlyCollection<T>(this);
        }

        public void Clear()
        {
            var items = new List<T>(BaseList);
            ModifyList(
                list => list.Clear(),
                list => list.AddRange(items));
        }

        public void ClearSelection(IIndexCollection selection)
        {
            TransformSelection(selection, item => default);
        }

        public bool Contains(T item)
        {
            return BaseList.Contains(item);
        }

        public void CopyTo(T[] array)
        {
            CopyTo(array, 0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            CopyTo(array, 0, arrayIndex, Count);
        }

        public void CopyTo(T[] array, int index, int arrayIndex, int length)
        {
            BaseList.CopyTo(array, index, arrayIndex, length);
        }

        public void CopyFrom(T[] array, int index)
        {
            if (array is null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            CopyFrom(array, index, 0, array.Length);
        }

        public void CopyFrom(T[] array, int index, int arrayIndex, int length)
        {
            var copy = new T[length];
            Array.Copy(
                sourceArray: array,
                sourceIndex: arrayIndex,
                destinationArray: copy,
                destinationIndex: 0,
                length: length);

            var oldValues = BaseList.ToArray(index, length);
            ModifyList(
                list => CopyFrom(copy, index, arrayIndex, length),
                list => CopyFrom(oldValues, index, arrayIndex, length));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return BaseList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return BaseList.IndexOf(item);
        }

        public int IndexOf(T item, int index)
        {
            return BaseList.IndexOf(item, index);
        }

        public int IndexOf(T item, int index, int count)
        {
            return BaseList.IndexOf(item, index, count);
        }

        public void Insert(int index, T item)
        {
            ModifyList(
                list => list.Insert(index, item),
                list => list.RemoveAt(index));
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            var items = new List<T>(collection);
            ModifyList(
                list => list.InsertRange(index, items),
                list => list.RemoveRange(index, items.Count));
        }

        public void InsertSelection(IIndexDictionary<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var selection = values.Selection.Copy();
            var data = values.Copy();
            ModifyList(
                list => list.InsertSelection(data),
                list => list.RemoveSelection(selection));
        }

        public void Redo()
        {
            History.Redo();
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index != -1)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            var item = BaseList[index];
            ModifyList(
                list => list.RemoveAt(index),
                list => list.Insert(index, item));
        }

        public void RemoveRange(int index, int count)
        {
            var items = BaseList.ToArray(index, count);
            ModifyList(
                list => list.RemoveRange(index, count),
                list => list.InsertRange(index, items));
        }

        public void RemoveSelection(IIndexCollection selection)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            var copy = selection.Copy();
            var values = new IndexDictionary<T>(selection, this);
            ModifyList(
                list => list.RemoveSelection(selection),
                list => list.InsertSelection(values));
        }

        public void SetRange(int index, IEnumerable<T> collection)
        {
            var items = new List<T>(collection);
            var oldValues = BaseList.ToArray(index, items.Count);
            ModifyList(
                list => list.SetRange(index, items),
                list => list.SetRange(index, oldValues));
        }

        public T[] ToArray()
        {
            return BaseList.ToArray();
        }

        public T[] ToArray(int index, int count)
        {
            return BaseList.ToArray(index, count);
        }

        public void TransformSelection(
            IIndexCollection selection,
            Func<T, T> transformItem)
        {
            if (selection is null)
            {
                throw new ArgumentNullException(nameof(selection));
            }

            if (transformItem is null)
            {
                throw new ArgumentNullException(nameof(transformItem));
            }

            var values = new IndexDictionary<T>(selection);
            foreach (var index in selection)
            {
                values[index] = transformItem(this[index]);
            }

            var oldValues = new IndexDictionary<T>(selection, BaseList);
            ModifyList(
                list => list.WriteSelection(values),
                list => list.WriteSelection(oldValues));
        }

        public void Undo()
        {
            History.Undo();
        }

        public void WriteSelection(IIndexDictionary<T> values)
        {
            if (values is null)
            {
                throw new ArgumentNullException(nameof(values));
            }

            var copy = values.Copy();
            var oldValues = new IndexDictionary<T>(copy.Selection, BaseList);
            ModifyList(
                list => list.WriteSelection(copy),
                list => list.WriteSelection(oldValues));
        }

        private void OnContentsModified(EventArgs e)
        {
            ContentsModified?.Invoke(this, e);
        }

        private void ModifyList(
            Action<UnamangedCollection<T>> redo,
            Action<UnamangedCollection<T>> undo)
        {
            History.Add(Modify(undo), Modify(redo));
            redo(BaseList);

            Action Modify(Action<UnamangedCollection<T>> action)
            {
                return () => action(BaseList);
            }
        }
    }
}
