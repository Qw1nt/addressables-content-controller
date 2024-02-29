using System;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Qw1nt.Runtime.AddressablesContentController.Common;
using Qw1nt.Runtime.AddressablesContentController.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Qw1nt.Runtime.AddressablesContentController.Extensions
{
    public static class OperationsExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ContentOperation Cache<T>(this ContentOperation<T> operation) where T : class
        {
            var cache = ContentController.Default.Cache;

            if (cache.Has(operation.Key) == false)
                cache.Add(operation.Key, operation);

            return operation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<ContentOperation<T>> Cache<T>(this UniTask<ContentOperation<T>> operation)
            where T : class
        {
            var operationResult = await operation;

            if (ContentController.Default.Cache.Has(operationResult.Key) == false)
                operationResult.Cache();

            return operationResult;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Unpack<T>(this ContentOperation operation) where T : class
        {
            return operation.Convert<T>().Handle.Result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<T> Unpack<T>(this UniTask<ContentOperation<T>> operation) where T : class
        {
            var result = await operation;
            return result.GetResult();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryUnpack<T>(this ContentOperation operation, out T result) where T : class
        {
            result = null;

            if (operation.Handle is T == false)
                return false;

            result = operation.Handle.Convert<T>().Result;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<T> GetComponent<T>(this UniTask<ContentOperation<GameObject>> operation)
            where T : class
        {
            var operationResult = await operation;
            return operationResult.GetResult().GetComponent<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Release<T>(this ContentOperation<T> operation) where T : class
        {
            if (operation.Handle.IsValid() == false)
                return;

            ReleaseHandle(operation.OperationType, operation.Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Release(this ContentOperation operation)
        {
            if (operation.Handle.IsValid() == false)
                return;

            ReleaseHandle(operation.OperationType, operation.Handle);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ReleaseHandle(OperationType type, AsyncOperationHandle handle)
        {
            switch (type)
            {
                case OperationType.Instancing:

                    if (handle.IsValid() == true)
                        Addressables.ReleaseInstance(handle);

                    break;

                case OperationType.Loading:

                    if (handle.IsValid() == true)
                        Addressables.Release(handle);

                    break;

#if UNITY_EDITOR
                default:
                    throw new ArgumentOutOfRangeException();
#endif
            }
        }
    }
}