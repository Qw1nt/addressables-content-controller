using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Qw1nt.Runtime.AddressablesContentController.Common
{
    public readonly struct ContentOperation : IEqualityComparer<ContentOperation>, IEquatable<ContentOperation>
    {
        public ContentOperation(string key, OperationType operationType, AsyncOperationHandle handle)
        {
            Key = key;
            OperationType = operationType;
            Handle = handle;
        }

        internal string Key
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        internal OperationType OperationType
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        internal AsyncOperationHandle Handle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ContentOperation<T> Convert<T>() where T : class
        {
#if UNITY_EDITOR
            // TODO описание ошибки

            if (Handle.IsValid() == false)
                throw new ArgumentException("");
#endif

            return new ContentOperation<T>(Key, OperationType, Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            return HashCode.Combine(Key, (int)OperationType, Handle);
        }
        
        // TODO Изменить
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ContentOperation x, ContentOperation y)
        {
            return x.Key == y.Key;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetHashCode(ContentOperation obj)
        {
            return HashCode.Combine(obj.Key, (int) obj.OperationType, obj.Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ContentOperation other)
        {
            return Key == other.Key && OperationType == other.OperationType && Handle.Equals(other.Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
        {
            return obj is ContentOperation other && Equals(other);
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

        internal string Key
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        internal OperationType OperationType
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        internal AsyncOperationHandle<T> Handle
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContentOperation Convert()
        {
            return new ContentOperation(Key, OperationType, Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetResult()
        {
            return Handle.Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ContentOperation(ContentOperation<T> operation)
        {
            return new ContentOperation(operation.Key, operation.OperationType, operation.Handle);
        }
    }
}