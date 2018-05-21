using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace AnkiEditor
{

    public class SmartCollection<T> : ObservableCollection<T>
    {
        private bool _raiseEvents = true;

        public void Sort<T2>(Func<T, T2> keySelect) where T2 : IComparable
        {
            _raiseEvents = false;
            List<T> sorted = this.OrderBy(keySelect).ToList();
            for (int i = 0; i < sorted.Count; i++)
                Move(IndexOf(sorted[i]), i);
            _raiseEvents = true;

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)); // Does the action matter?
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            if (_raiseEvents) base.MoveItem(oldIndex, newIndex);
            else
            {
                T removedItem = this[oldIndex];

                RemoveItem(oldIndex);
                InsertItem(newIndex, removedItem);
            }
        }

        protected override void InsertItem(int index, T item)
        {
            if (_raiseEvents) base.InsertItem(index, item);
            else
            {
                // Hacky way to call base.base.InsertItem to prevent raising change events
                // ReSharper disable once PossibleNullReferenceException
                var ptr = typeof(Collection<T>).GetMethod(nameof(InsertItem), BindingFlags.Instance | BindingFlags.NonPublic).MethodHandle
                    .GetFunctionPointer();
                var baseInsertItem = (Action<int, T>)Activator.CreateInstance(typeof(Action<int, T>), this, ptr);
                baseInsertItem(index, item);
            }

        }

        protected override void RemoveItem(int index)
        {
            if (_raiseEvents) base.RemoveItem(index);
            else
            {
                // Hacky way to call base.base.DeleteItem to prevent raising change events
                // ReSharper disable once PossibleNullReferenceException
                var ptr = typeof(Collection<T>).GetMethod(nameof(RemoveItem), BindingFlags.Instance | BindingFlags.NonPublic).MethodHandle
                    .GetFunctionPointer();
                var baseRemoveItem = (Action<int>)Activator.CreateInstance(typeof(Action<int>), this, ptr);
                baseRemoveItem(index);
            }

        }
    }
}

