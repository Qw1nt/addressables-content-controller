using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Runtime.Shared.AddressablesContentController.Common;
using Runtime.Shared.AddressablesContentController.Extensions;
using Runtime.Shared.AddressablesContentController.Interfaces;
using Runtime.Shared.AddressablesContentController.SceneManagement;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Runtime.Shared.AddressablesContentController.Core
{
    public sealed class ContentController
    {
        internal const int DefaultInstancesCapacity = 3;

        private readonly IOperationsTracker _operationsTracker = new OperationsTracker();
        private readonly Cache _cache = new();
        private readonly PersistenceStorage _persistenceStorage = new();

        private SceneInstance _activeScene;

        public ContentController()
        {
            Scene = new SceneManipulator(_operationsTracker);
        }

        public SceneManipulator Scene { get; }

        public static ContentController Default
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Instances[0];
        }

        internal Cache Cache
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _cache;
        }

        internal PersistenceStorage Persistence
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _persistenceStorage;
        }

        internal static List<ContentController> Instances { get; } = new(DefaultInstancesCapacity);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask<ContentOperation<GameObject>> InstantiateAsync(AssetReference assetReference)
        {
            var instantiateOperation = assetReference.InstantiateAsync();
            await instantiateOperation;

            var resultOperation = new ContentOperation<GameObject>(assetReference.AssetGUID, OperationType.Instancing,
                instantiateOperation);

            _operationsTracker.Track(resultOperation);

            return resultOperation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask<ContentOperation<GameObject>> InstantiateAsync(string key)
        {
            var operationHandle = Addressables.InstantiateAsync(key);
            await operationHandle;

            var resultOperation = new ContentOperation<GameObject>(key, OperationType.Instancing, operationHandle);
            _operationsTracker.Track(resultOperation);

            return resultOperation;
        }

        public async UniTask<T> InstantiateAsync<T>(AssetReference reference) where T : Component
        {
            var instance = await InstantiateAsync(reference);
            return instance.GetResult().GetComponent<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask<ContentOperation<T>> LoadAsync<T>(AssetReference assetReference) where T : class
        {
            if (_cache.Has(assetReference) == true)
                return _cache.GetOperation<T>(assetReference);

            var operation = assetReference.LoadAssetAsync<T>();
            await operation;

            var resultOperation = new ContentOperation<T>(assetReference.AssetGUID, OperationType.Loading, operation);
            _operationsTracker.Track(resultOperation);

            return resultOperation;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public async UniTask<ContentOperation<T>> LoadAsync<T>(string path) where T : class
        {
            if (_cache.Has(path) == true)
                return _cache.GetOperation<T>(path);

            var operation = Addressables.LoadAssetAsync<T>(path);
            await operation;

            var resultOperation = new ContentOperation<T>(path, OperationType.Loading, operation);
            _operationsTracker.Track(resultOperation);

            return resultOperation;
        }

        public async UniTask<ContentOperation<IList<T>>> LoadLabelAsync<T>(AssetLabelReference labelReference)
            where T : class
        {
            var operation = Addressables.LoadAssetsAsync<T>(labelReference, null);
            await operation;

            var resultOperation =
                new ContentOperation<IList<T>>(labelReference.labelString, OperationType.Loading, operation);
            _operationsTracker.Track(resultOperation);

            return resultOperation;
        }

        public async UniTask<T> LoadInPersistenceAsync<T>(AssetReference reference) where T : class
        {
            if (_persistenceStorage.TryGet<T>(out var result) == true)
                return result.GetResult();

            var operation = reference.LoadAssetAsync<T>();
            await operation;

            var contentOperation = new ContentOperation(reference.AssetGUID, OperationType.Loading, operation);
            _persistenceStorage.Add(contentOperation.Convert<T>());

            return contentOperation.Unpack<T>();
        }

        public async UniTask<IReadOnlyList<T>> LoadLabelInPersistenceAsync<T>(AssetLabelReference reference)
            where T : class
        {
            if (_persistenceStorage.TryGetSet<T>(out var result) == true)
                return (IReadOnlyList<T>) result;

            var operation = Addressables.LoadAssetsAsync<T>(reference, null);
            await operation;

            var contentOperation = new ContentOperation<IList<T>>(reference.labelString, OperationType.Loading, operation);
            _persistenceStorage.AddSet(contentOperation);

            return (IReadOnlyList<T>) contentOperation.GetResult();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(AssetReference reference)
        {
            if (_cache.Has(reference) == false)
                return;

            _cache.Release(reference);
        }
    }
}