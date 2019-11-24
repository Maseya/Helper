// <copyright file="SelectionBuilder.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    public class SelectionBuilder
    {
        public SelectionBuilder()
        {
            Collection = Enumerable.Empty<int>();
        }

        private IEnumerable<int> Collection
        {
            get;
            set;
        }

        public void Add(IIndexCollection selection)
        {
            Collection = Collection.Union(selection);
        }

        public void AddLinearSelection(int index, int count)
        {
            Add(new IndexRangeCollection(index, count));
        }

        public void AddBoxSelection(int index, Size size, int gridWidth)
        {
            Add(new BoxIndexCollection(index, size, gridWidth));
        }

        public void AddBoxSelection(
            int index,
            int width,
            int height,
            int gridWidth)
        {
            Add(new BoxIndexCollection(index, width, height, gridWidth));
        }

        public void AddIndex(int index)
        {
            Collection.Union(new int[] { index });
        }

        public void Remove(IIndexCollection selection)
        {
            Collection.Except(selection);
        }

        public void Clear()
        {
            Collection = Enumerable.Empty<int>();
        }

        public HashListIndexCollection CreateSelection()
        {
            return CreateSelection(true);
        }

        public HashListIndexCollection CreateSelection(bool clear)
        {
            var result = new HashListIndexCollection(Collection);
            if (clear)
            {
                Clear();
            }

            return result;
        }

        public List<int> Current()
        {
            return new List<int>(Collection);
        }
    }
}
