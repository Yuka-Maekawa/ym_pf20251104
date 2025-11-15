using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MyProject.Systems.Resource
{
    public partial class ResourceManager : SingletonMonoBehaviour<ResourceManager>
    {
        private static readonly System.Type _gameObjectType = typeof(GameObject);
        private static readonly System.Type _monoBehaviourType = typeof(MonoBehaviour);

        private Loader _globalInstance = null;
        private Loader _localInstance = null;

        public static Loader Global=> Instance._globalInstance;
        public static Loader Local => Instance._localInstance;

        /// <summary>
        /// 初期化
        /// </summary>
        protected override async UniTask InitializeAsync()
        {
            await Addressables.InitializeAsync(true).ToUniTask();

            _globalInstance = new Loader();
            _localInstance = new Loader();
        }

        /// <summary>
        /// 解放
        /// </summary>
        protected override void Release()
        {
            _localInstance.Release();
            _localInstance = null;

            _globalInstance.Release();
            _globalInstance = null;
        }

        /// <summary>
        /// インスタンスを生成する必要があるか？
        /// </summary>
        /// <param name="objType">オブジェクトの型</param>
        /// <returns>true: 必要, false: 不要</returns>
        private static bool IsNeedInstantiate(System.Type objType)
        {
            // 対象はプレハブ(GameObject)
            return _gameObjectType.IsAssignableFrom(objType) || _monoBehaviourType.IsAssignableFrom(objType);
        }
    }
}