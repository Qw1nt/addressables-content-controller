using System;
using System.Collections.Generic;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace Qw1nt.Runtime.Shared.AddressablesContentController.SceneManagement
{
    public readonly struct PreloadSceneOperation : IEqualityComparer<PreloadSceneOperation>
    {
        public PreloadSceneOperation(SceneData data, AsyncOperationHandle<SceneInstance> handle)
        {
            Data = data;
            Handle = handle;
        }

        public SceneData Data { get; }

        public AsyncOperationHandle<SceneInstance> Handle { get; }

        public static PreloadSceneOperation Null => new();

        public static bool operator ==(PreloadSceneOperation left, PreloadSceneOperation right)
        {
            return left.Data?.Key == right.Data?.Key;
        }

        public static bool operator !=(PreloadSceneOperation left, PreloadSceneOperation right)
        {
            return !(left == right);
        }

        public bool Equals(PreloadSceneOperation x, PreloadSceneOperation y)
        {
            return x.Data?.Key == y.Data?.Key;
        }

        public int GetHashCode(PreloadSceneOperation obj)
        {
            return obj == Null ? 0 : HashCode.Combine(obj.Data.Key);
        }

        public bool Equals(PreloadSceneOperation other)
        {
            return Data?.Key == other.Data?.Key;
        }

        public override bool Equals(object obj)
        {
            return obj is PreloadSceneOperation other && Equals(other);
        }

        public override int GetHashCode()
        {
            return this == Null ? 0 : HashCode.Combine(Data.Key);
        }
    }
}