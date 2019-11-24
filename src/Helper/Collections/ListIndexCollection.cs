// <copyright file="ListIndexCollection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ListIndexCollection : IndexCollection
    {
        public ListIndexCollection(IList<int> list)
        {
            List = list ?? throw new ArgumentNullException(nameof(list));
        }

        public override int Count
        {
            get
            {
                return List.Count;
            }
        }

        public override int MinIndex
        {
            get
            {
                return Enumerable.Min(List);
            }
        }

        public override int MaxIndex
        {
            get
            {
                return Enumerable.Max(List);
            }
        }

        private IList<int> List
        {
            get;
        }

        public override int this[int index]
        {
            get
            {
                return List[index];
            }
        }

        public override bool ContainsIndex(int index)
        {
            return List.Contains(index);
        }

        public override IndexCollection Move(int offset)
        {
            throw new NotSupportedException();
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return List.GetEnumerator();
        }
    }
}
