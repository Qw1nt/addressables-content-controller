using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using Qw1nt.Runtime.AddressablesContentController.Common;
using Qw1nt.Runtime.AddressablesContentController.Core;
using UnityEngine;
using UnityEngine.AddressableAssets;

#if CONTENT_CONTROLLER_VCONTAINER
using VContainer;
#endif

namespace Qw1nt.Runtime.AddressablesContentController.Extensions
{
    public static class ContentControllerExtensions
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        public static void InitializeDefaultController()
        {
            ContentController.Instances.Clear();
            ContentController.Instances.Capacity = ContentController.DefaultInstancesCapacity;

            ContentController.Instances.Add(new ContentController());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OperationObserver ObservableOperation(this ContentController controller)
        {
            return new OperationObserver();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask<ContentOperation<GameObject>> CreateInstance(this AssetReference reference)
        {
            return ContentController.Default.InstantiateAsync(reference);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<ContentOperation<GameObject>> CreateInstance(this AssetReference reference, Vector3 position, Quaternion rotation)
        {
            var operation = await ContentController.Default.InstantiateAsync(reference);
            
            var gameObject = operation.GetResult();
            gameObject.transform.position = position;
            gameObject.transform.rotation = rotation;

            return operation;
        }
        
#if CONTENT_CONTROLLER_VCONTAINER
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static async UniTask<ContentOperation<GameObject>> CreateInstance(this AssetReference reference, IObjectResolver resolver)
        {
            var instance = await ContentController.Default.InstantiateAsync(reference);
            resolver.InjectGameObject(instance.GetResult());
            return instance;
        }
#endif
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask<ContentOperation<IList<T>>> LoadLabel<T>(this AssetLabelReference labelReference)
            where T : class
        {
            return ContentController.Default.LoadLabelAsync<T>(labelReference);
        }   
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static UniTask<IReadOnlyList<T>> LoadLabelInPersistence<T>(this AssetLabelReference labelReference)
            where T : class
        {
            return ContentController.Default.LoadLabelInPersistenceAsync<T>(labelReference);
        }
    }
}