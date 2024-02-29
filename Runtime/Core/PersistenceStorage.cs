using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Qw1nt.Runtime.AddressablesContentController.Common;
using Qw1nt.Runtime.AddressablesContentController.Extensions;

namespace Qw1nt.Runtime.AddressablesContentController.Core
{
    internal class PersistenceStorage
    {
        private readonly HashSet<ContentOperation> _internalStorage;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PersistenceStorage()
        {
            _internalStorage = new HashSet<ContentOperation>(15);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PersistenceStorage(int capacity)
        {
            _internalStorage = new HashSet<ContentOperation>(capacity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Get<T>() where T : class
        {
            foreach (var operation in _internalStorage)
            {
                if(operation.Handle.Result is T == false)
                    continue;

                return operation.Convert<T>().GetResult();
            }

            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IReadOnlyList<T> GetSet<T>()
        {
            return (IReadOnlyList<T>) Get<IList<T>>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGet<T>(out ContentOperation<T> result) where T : class
        {
            foreach (var value in _internalStorage)
            {
                if (value.Handle.Result is T == false)
                    continue;

                result = value.Convert<T>();
                return true;
            }

            result = default;
            return false;
        }

        public bool TryGetSet<T>(out IList<T> content) where T : class
        {
            var result = TryGet<IList<T>>(out var operation);
            content = result == true ? operation.GetResult() : default;

            return result;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Add<T>(ContentOperation<T> operation) where T : class
        {
#if UNITY_EDITOR
            if (operation.GetResult() is IEnumerable)
                throw new ArgumentException($"Error. You can't add set in default method. Use {nameof(AddSet)}");
#endif

            // TODO мб не лучшее решение
            if (TryGet<T>(out var stored) == true)
                return stored.GetResult();

            _internalStorage.Add(operation);
            return operation.GetResult();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IList<T> AddSet<T>(ContentOperation<IList<T>> set) where T : class
        {
            if (TryGet<IList<T>>(out var stored) == true)
                return stored.GetResult();

            _internalStorage.Add(set);
            return set.GetResult();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Remove<T>() where T : class
        {
            if (TryGet<T>(out var stored) == false)
                return;

            _internalStorage.Remove(stored);
            stored.Release();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveSet<T>() where T : class
        {
            if (TryGet<IList<T>>(out var stored) == false)
                return;

            _internalStorage.Remove(stored);
            stored.Release();
        }
    }
}