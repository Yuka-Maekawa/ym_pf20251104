using Cysharp.Threading.Tasks;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Common.UI
{
    public partial class FadeManager
    {
        public class FadeAnimation
        {
            private static readonly string _fadeControllerPath = "UI/Common/Fade";

            private GameObject _gameObject = null;
            private FadeController _fadeController = null;

            /// <summary>
            /// 初期化（非同期）
            /// </summary>
            /// <param name="transform">親Transform</param>
            public async UniTask InitializeAsync(Transform transform)
            {
                await ResourceManager.Global.LoadAssetAsync<GameObject>(_fadeControllerPath);
                _gameObject = ResourceManager.Global.GetAsset<GameObject>(_fadeControllerPath, transform);

                _fadeController = _gameObject.GetComponent<FadeController>();
            }

            /// <summary>
            /// 解放
            /// </summary>
            public void Release()
            {
                _fadeController = null;

                if (_gameObject != null)
                {
                    Destroy(_gameObject);
                    _gameObject = null;
                }
            }

            /// <summary>
            /// フェードイン（非同期）
            /// </summary>
            public async UniTask PlayFadeInAsync()
            {
                await _fadeController.PlayFadeInAsync();
            }

            /// <summary>
            /// フェードアウト（非同期）
            /// </summary>
            public async UniTask PlayFadeOutAsync()
            {
                await _fadeController.PlayFadeOutAsync();
            }
        }
    }
}