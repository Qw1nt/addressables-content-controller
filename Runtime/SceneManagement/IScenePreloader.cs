using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Qw1nt.Runtime.AddressablesContentController.Common;
using Qw1nt.Runtime.Shared.AddressablesContentController.Interfaces;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Qw1nt.Runtime.Shared.AddressablesContentController.SceneManagement
{
    public interface IScenePreloader : IDisposable
    {
        void Schedule(SceneData sceneData, OperationObserver observer = default);

        bool IsScheduled(SceneData sceneData);

        UniTask GetOperationAwaiter(SceneData sceneData);

        UniTask Load(SceneData sceneData, SceneInstance activeScene);
    }

    public readonly struct ScenePreloader : IScenePreloader
    {
        private readonly Dictionary<string, AsyncOperationHandle<SceneInstance>> _operationsBuffer;
        private readonly IOperationsTracker _operationsTracker;

        public ScenePreloader(Dictionary<string, AsyncOperationHandle<SceneInstance>> operationBuffer, IOperationsTracker tracker)
        {
            _operationsBuffer = operationBuffer;
            _operationsTracker = tracker;
        }

        public void Schedule(SceneData sceneData, OperationObserver observer = default)
        {
            if (_operationsBuffer.ContainsKey(sceneData.Key) == true)
                return;

            var operation = sceneData.AssetReference.LoadSceneAsync(sceneData.LoadMode, false);
            _operationsBuffer.Add(sceneData.Key, operation);

            if (observer.IsNull() == false)
                observer.PassOperation(operation).Forget();
        }

        public bool IsScheduled(SceneData sceneData)
        {
            return _operationsBuffer.ContainsKey(sceneData.Key);
        }

        public UniTask GetOperationAwaiter(SceneData sceneData)
        {
            return IsScheduled(sceneData) == false
                ? UniTask.CompletedTask
                : _operationsBuffer[sceneData.Key].ToUniTask();
        }

        public async UniTask Load(SceneData sceneData, SceneInstance activeScene)
        {
            var key = sceneData.Key;

            if (_operationsBuffer.ContainsKey(key) == false)
                return;

            if (_operationsBuffer.TryGetValue(key, out var operation) == false)
                return;

            _operationsBuffer.Remove(key);

            while (operation.IsDone == false)
                await UniTask.Yield();

            if (string.IsNullOrEmpty(activeScene.Scene.name) == false)
                await Addressables.UnloadSceneAsync(activeScene);
            
            _operationsTracker.Purge();
            operation.Result.ActivateAsync();
        }

        public void Dispose()
        {
            _operationsBuffer.Clear();
        }
    }
}