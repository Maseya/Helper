// <copyright file="HashListIndexCollection.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static ThrowHelper;

    public class HashListIndexCollection : IndexCollection
    {
        public HashListIndexCollection(IEnumerable<int> collection)
        {
            List = new List<int>(collection);
            if (List.Count == 0)
            {
                throw EmptyCollectionArgumentException(nameof(collection));
            }

            List.Sort();
            HashSet = new HashSet<int>(List);
        }

        public override int MinIndex
        {
            get
            {
                return List[0];
            }
        }

        public override int MaxIndex
        {
            get
            {
                return List[Count - 1];
            }
        }

        public override int Count
        {
            get
            {
                return List.Count;
            }
        }

        private List<int> List
        {
            get;
        }

        private HashSet<int> HashSet
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
            return HashSet.Contains(index);
        }

        public override IEnumerator<int> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public override IndexCollection Move(int amount)
        {
            return new HashListIndexCollection(
                List.Select(i => i + amount));
        }
    }
}
