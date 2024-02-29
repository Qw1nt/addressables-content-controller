using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Qw1nt.Runtime.Shared.AddressablesContentController.SceneManagement
{
    [CreateAssetMenu(menuName = "Content Controller/Scene Data")]
    public class SceneData : ScriptableObject
    {
        [SerializeField] private AssetReference _assetReference;
        [SerializeField] private LoadSceneMode _loadMode;
        [SerializeField] private string _key;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_assetReference != null)
                _key = _assetReference?.editorAsset.name;
        }
#endif

        public AssetReference AssetReference => _assetReference;

        public LoadSceneMode LoadMode => _loadMode;
        
        public string Key => _key;
    }
}