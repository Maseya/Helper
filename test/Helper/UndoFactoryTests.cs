// <copyright file="UndoFactoryTests.cs" company="Public Domain">
//     Copyright (c) 2019 Nelson Garcia. All rights reserved. Licensed under
//     GNU Affero General Public License. See LICENSE in project root for full
//     license information, or visit https://www.gnu.org/licenses/#AGPL
// </copyright>

namespace Maseya.Helper.Tests
{
    using System;
    using Xunit;

    public class UndoFactoryTests
    {
        [Fact]
        public void UndoThrowsOnNull()
        {
            var history = new UndoFactory();

            Assert.Throws<ArgumentNullException>(
                "undo",
                Add);

            void Add()
            {
                history.Add(null, () => { });
            }
        }

        [Fact]
        public void RedoThrowsOnNull()
        {
            var history = new UndoFactory();

            Assert.Throws<ArgumentNullException>(
                "redo",
                Add);

            void Add()
            {
                history.Add(() => { }, null);
            }
        }

        [Fact]
        public void UndoRedoTestState()
        {
            var history = new UndoFactory();
            var state = 0;
            Test(0, false, false, 0, 0);

            Set(1);
            Test(1, true, false, 1, 1);

            Set(0);
            Test(0, true, false, 2, 2);

            Set(2);
            Test(2, true, false, 3, 3);

            Undo();
            Test(0, true, true, 3, 2);

            Undo();
            Test(1, true, true, 3, 1);

            Undo();
            Test(0, false, true, 3, 0);

            Redo();
            Test(1, true, true, 3, 1);

            Redo();
            Test(0, true, true, 3, 2);

            Redo();
            Test(2, true, false, 3, 3);

            Redo();
            Test(2, true, false, 3, 3);

            Undo();
            Test(0, true, true, 3, 2);

            Undo();
            Test(1, true, true, 3, 1);

            Set(-2);
            Test(-2, true, false, 2, 2);

            void Set(int value)
            {
                var old_value = state;
                state = value;
                history.Add(
                    () => state = old_value,
                    () => state = value);
            }

            void Undo()
            {
                history.Undo();
            }

            void Redo()
            {
                history.Redo();
            }

            void Test(
                int expectedState,
                bool expectedCanUndo,
                bool expectedCanRedo,
                int expectedCount,
                int expectedIndex)
            {
                Assert.Equal(expectedCanUndo, history.CanUndo);
                Assert.Equal(expectedCanRedo, history.CanRedo);
                Assert.Equal(expectedState, state);
                Assert.Equal(expectedCount, history.Count);
                Assert.Equal(expectedIndex, history.Index);
            }
        }
    }
}
