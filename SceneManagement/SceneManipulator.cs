using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Shared.AddressablesContentController.Interfaces;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Runtime.Shared.AddressablesContentController.SceneManagement
{
    public class SceneManipulator : SceneManipulatorBase<ScenePreloader>
    {
        private readonly IOperationsTracker _operationsTracker;
        private SceneInstance _activeScene;

        internal SceneManipulator(IOperationsTracker operationsTracker) : base(new ScenePreloader(PreloadOperations, operationsTracker))
        {
            _operationsTracker = operationsTracker;
        }

        private static Dictionary<string, AsyncOperationHandle<SceneInstance>> PreloadOperations { get; } = new(4);

        public override async UniTask Load(SceneData sceneData)
        {
            if (Preloader.IsScheduled(sceneData) == true)
                await Preloader.Load(sceneData, _activeScene);
            else
                await LoadInternal(sceneData);
        }

        private async UniTask LoadInternal(SceneData sceneData)
        {
            var operation = await sceneData.AssetReference.LoadSceneAsync(sceneData.LoadMode, false);

            if (string.IsNullOrEmpty(_activeScene.Scene.name) == false)
                await Addressables.UnloadSceneAsync(_activeScene);

            _activeScene = operation;
            _operationsTracker.Purge();
            
            await operation.ActivateAsync();
        }
    }
}