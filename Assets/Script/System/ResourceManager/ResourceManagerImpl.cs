using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


namespace MyProject.Systems.Resource
{
    public partial class ResourceManager
    {
        public class Loader
        {
            private Dictionary<string, AsyncOperationHandle> _loadedAssets = null;

            private List<string> _loadingAddress = null;

            public Loader()
            {
                _loadedAssets = new Dictionary<string, AsyncOperationHandle>();
                _loadingAddress = new List<string>();
            }

            /// <summary>
            /// 解放
            /// </summary>
            public void Release()
            {
            }

            /// <summary>
            /// アセット読み込み（非同期）
            /// </summary>
            /// <typeparam name="T">型</typeparam>
            /// <param name="path">パス</param>
            public async UniTask LoadAssetAsync<T>(string path) where T : Object
            {
                if (!_loadingAddress.Contains(path) && !_loadedAssets.ContainsKey(path))
                {
                    _loadingAddress.Add(path);
                    var handle = Addressables.LoadAssetAsync<T>(path);
                    await handle.Task;

                    _loadingAddress.Remove(path);
                    _loadedAssets.Add(path, handle);
                }
            }

            /// <summary>
            /// アセットを解放
            /// </summary>
            /// <param name="path">パス</param>
            public void UnloadAssets(string path)
            {
                if (_loadedAssets.ContainsKey(path))
                {
                    Addressables.Release(_loadedAssets[path]);
                    _loadedAssets.Remove(path);
                }
            }

            /// <summary>
            /// 全てのアセットを解放
            /// </summary>
            public void UnloadAllAssets()
            {
                foreach (var assets in _loadedAssets)
                {
                    UnloadAssets(assets.Key);
                }
            }

            /// <summary>
            /// アセットを生成して取得
            /// </summary>
            /// <typeparam name="T">型</typeparam>
            /// <param name="path">パス</param>
            /// <returns>アセット</returns>
            public T GetAsset<T>(string path) where T : Object
            {
                if (_loadedAssets.ContainsKey(path))
                {
                    return Instantiate((T)_loadedAssets[path].Result);
                }

                return null;
            }

            /// <summary>
            /// 読み込み中か？
            /// </summary>
            /// <param name="path">パス</param>
            /// <returns>true: 読み込み中, false: 読み込み終了</returns>
            public bool IsLoading(string path)
            {
                return _loadingAddress.Contains(path);
            }

            public bool IsLoading()
            {
                return _loadingAddress.Count > 0;
            }
        }
    }
}