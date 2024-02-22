using System;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Runtime.Shared.AddressablesContentController.Common
{
    public struct OperationObserver
    {
        private Action<float> _onUpdate;
        private Action _completeCallback;

        internal void SetUpdateCallback(Action<float> updateCallback)
        {
            _onUpdate = updateCallback;
        }

        internal void SetCompleteCallback(Action completeCallback)
        {
            _completeCallback = completeCallback;
        }
        
        internal async UniTaskVoid PassOperation(AsyncOperationHandle handle)
        {
            while (handle.IsDone == false)
            {
                _onUpdate.Invoke(handle.PercentComplete);
                await UniTask.Yield();
            }
            
            _completeCallback?.Invoke();
        }

        public bool IsNull()
        {
            return _onUpdate == null;
        }
    }
}