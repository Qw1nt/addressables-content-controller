using System;
using System.Runtime.CompilerServices;
using Qw1nt.Runtime.AddressablesContentController.Common;
using Qw1nt.Runtime.AddressablesContentController.Core;
using Qw1nt.Runtime.Shared.AddressablesContentController.SceneManagement;

namespace Qw1nt.Runtime.AddressablesContentController.Extensions
{
    public static class OperationObserverExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OperationObserver OnUpdate(this OperationObserver observer, Action<float> onUpdate)
        {
            observer.SetUpdateCallback(onUpdate);
            return observer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static OperationObserver OnComplete(this OperationObserver observer, Action onComplete)
        {
            observer.SetCompleteCallback(onComplete);
            return observer;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PreloadScene(this OperationObserver observer, SceneData sceneData)
        {
            ContentController.Default.Scene.Preloader.Schedule(sceneData, observer);
        }
    }
}