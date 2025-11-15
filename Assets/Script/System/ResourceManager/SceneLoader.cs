using Cysharp.Threading.Tasks;

namespace MyProject.Systems.Resource
{
    public partial class SceneLoader : SingletonMonoBehaviour<SceneLoader>
    {
        private Loader _sceneLoad = null;
        public static Loader SceneLoad => Instance._sceneLoad;

        /// <summary>
        /// 初期化（非同期）
        /// </summary>
        protected override async UniTask InitializeAsync()
        {
            await UniTask.CompletedTask;
            _sceneLoad = new Loader();
        }

        /// <summary>
        /// 解放
        /// </summary>
        protected override void Release()
        {
            _sceneLoad = null;
        }
    }
}