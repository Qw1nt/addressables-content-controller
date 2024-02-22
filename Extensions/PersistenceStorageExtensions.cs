using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Runtime.Shared.AddressablesContentController.Common;
using Runtime.Shared.AddressablesContentController.Core;

namespace Runtime.Shared.AddressablesContentController.Extensions
{
    public static class PersistenceStorageExtensions
    {
        public static T GetFromPersistence<T>(this ContentController contentController) where T : class
        {
            return contentController.Persistence.Get<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetFromPersistence<T>(this ContentController contentController, out T result)
            where T : class
        {
            var has = contentController.Persistence.TryGet<T>(out var operation);
            result = has == true ? operation.GetResult() : default;

            return has;
        }

        public static bool TryGetSetFromPersistence<T>(this ContentController contentController, out IList<T> result)
            where T : class
        {
            var has = contentController.Persistence.TryGetSet(out result);
            return has;
        }
        
        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetShared<T>(this ContentController contentController) where T : class
        {
            return contentController.Persistence.Get<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IList<T> GetSharedSet<T>(this ContentController contentController) where T : class
        {
            return contentController.Persistence.GetSet<T>();
        }*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T MakePersistence<T>(this ContentOperation<T> operation) where T : class
        {
            return ContentController.Default.Persistence.Add(operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<T> MakePersistence<T>(this UniTask<ContentOperation<T>> operation) where T : class
        {
            var operationResult = await operation;
            return ContentController.Default.Persistence.Add(operationResult);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<IList<T>> MakeSharedSet<T>(this UniTask<ContentOperation<IList<T>>> operation)
            where T : class
        {
            var operationResult = await operation;
            return ContentController.Default.Persistence.AddSet(operationResult);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IReadOnlyList<T> AsReadonly<T>(this IList<T> set)
        {
            return (IReadOnlyList<T>) set;
        }

        public static IList<T> Duplicate<T>(this IList<T> set)
        {
            return new List<T>(set);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<IReadOnlyList<T>> AsReadonly<T>(this UniTask<IList<T>> operation) where T : class
        {
            var set = await operation;
            return (IReadOnlyList<T>) set;
        }
    }
}