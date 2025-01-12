using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Qw1nt.Runtime.AddressablesContentController.Common;
using Qw1nt.Runtime.AddressablesContentController.Extensions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Qw1nt.Runtime.AddressablesContentController.Core
{
    internal class Cache
    {
        private readonly Dictionary<string, ContentOperation> _operations = new(32);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(AssetReference reference)
        {
            return _operations.ContainsKey(reference.AssetGUID);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Has(string key)
        {
            return _operations.ContainsKey(key);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(string key, ContentOperation operation)
        {
            _operations.Add(key, operation);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AsyncOperationHandle<T> Get<T>(string key)
        {
            var operation = _operations[key];
            return operation.Handle.Convert<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public AsyncOperationHandle<T> Get<T>(AssetReference reference)
        {
            var operation = _operations[reference.AssetGUID];
            return operation.Handle.Convert<T>();
        }

        public ContentOperation<T> GetOperation<T>(AssetReference assetReference) where T : class
        {
            var key = assetReference.AssetGUID;
            return GetOperation<T>(key);
        }      
        
        public ContentOperation<T> GetOperation<T>(string key) where T : class
        {
            if (_operations.ContainsKey(key) == false)
                throw new ArgumentException();

            return _operations[key].Convert<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool SafeGet<T>(AssetReference reference, out AsyncOperationHandle<T> handle)
        {
            handle = default;
            var operation = _operations[reference.AssetGUID];

            if (operation.Handle.IsValid() == false)
            {
                _operations.Remove(reference.AssetGUID);
                return false;
            }

            handle = operation.Handle.Convert<T>();
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(AssetReference assetReference)
        {
            var guid = assetReference.AssetGUID;

            if (_operations.ContainsKey(guid) == false)
                return;

            _operations[guid].Release();
            _operations.Remove(guid);
        }   
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Release(string assetKey)
        {
            _operations[assetKey].Release();
            _operations.Remove(assetKey);
        }
    }
}