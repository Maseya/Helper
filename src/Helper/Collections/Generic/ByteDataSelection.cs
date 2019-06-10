// <copyright file="ByteDataSelection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections.Generic
{
    using System;
    using System.Collections.Generic;

    public class ByteDataSelection<T> : ListSelection
    {
        public ByteDataSelection(
            int startOffset,
            IListSelection baseSelection,
            IByteDataConverter<T> converter)
        {
            StartOffset = startOffset;
            BaseSelection = baseSelection
                ?? throw new ArgumentNullException(nameof(baseSelection));

            Converter = converter
                ?? throw new ArgumentNullException(nameof(converter));
        }

        public IListSelection BaseSelection
        {
            get;
        }

        public IByteDataConverter<T> Converter
        {
            get;
        }

        public int StartOffset
        {
            get;
        }

        public override int MinIndex
        {
            get
            {
                return StartOffset + (BaseSelection.MinIndex * SizeOfItem);
            }
        }

        public override int MaxIndex
        {
            get
            {
                return StartOffset
                    + (((BaseSelection.MaxIndex + 1) * SizeOfItem) - 1);
            }
        }

        public override int Count
        {
            get
            {
                return BaseSelection.Count * SizeOfItem;
            }
        }

        private int SizeOfItem
        {
            get
            {
                return Converter.SizeOfItem;
            }
        }

        public override int this[int index]
        {
            get
            {
                index -= StartOffset;
                var baseIndex = index / SizeOfItem;
                return BaseSelection[baseIndex] + (index % SizeOfItem);
            }
        }

        public override ListSelection Move(int offset)
        {
            return new ByteDataSelection<T>(
                StartOffset + offset,
                BaseSelection,
                Converter);
        }

        public override bool ContainsIndex(int index)
        {
            return BaseSelection.ContainsIndex(
                (index - StartOffset) / SizeOfItem);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            foreach (var baseIndex in BaseSelection)
            {
                yield return StartOffset + (baseIndex * SizeOfItem);
            }
        }
    }
}
