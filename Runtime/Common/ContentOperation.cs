using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Qw1nt.Runtime.AddressablesContentController.Common
{
    public readonly struct ContentOperation : IEqualityComparer<ContentOperation>
    {
        public ContentOperation(string key, OperationType operationType, AsyncOperationHandle handle)
        {
            Key = key;
            OperationType = operationType;
            Handle = handle;
        }

        internal string Key { get; }

        internal OperationType OperationType { get; }

        internal AsyncOperationHandle Handle { get; }

        internal ContentOperation<T> Convert<T>() where T : class
        {
#if UNITY_EDITOR
            // TODO описание ошибки

            if (Handle.IsValid() == false)
                throw new ArgumentException("");
#endif

            return new ContentOperation<T>(Key, OperationType, Handle);
        }

        // TODO Изменить
        public bool Equals(ContentOperation x, ContentOperation y)
        {
            return x.Key == y.Key;
        }

        public int GetHashCode(ContentOperation obj)
        {
            return HashCode.Combine(obj.Key, (int) obj.OperationType, obj.Handle);
        }
    }

    public readonly struct ContentOperation<T> where T : class
    {
        public ContentOperation(string key, OperationType operationType, AsyncOperationHandle result)
        {
            Key = key;
            OperationType = operationType;
            Handle = result.Convert<T>();
        }

        internal string Key { get; }

        internal OperationType OperationType { get; }

        internal AsyncOperationHandle<T> Handle { get; }

        public ContentOperation Convert()
        {
            return new ContentOperation(Key, OperationType, Handle);
        }

        public T GetResult()
        {
            return Handle.Result;
        }

        public static implicit operator ContentOperation(ContentOperation<T> operation)
        {
            return new ContentOperation(operation.Key, operation.OperationType, operation.Handle);
        }
    }
}