using Cysharp.Threading.Tasks;

namespace MyProject.Common.UI
{
    public partial class FadeManager : SingletonMonoBehaviour<FadeManager>
    {
        private FadeAnimation _fade = null;
        public static FadeAnimation Fade => Instance._fade;

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        protected override async UniTask Initialize()
        {
            _fade = new FadeAnimation();
            await _fade.InitializeAsync(transform);
        }

        /// <summary>
        /// 解放
        /// </summary>
        protected override void Release()
        {
            _fade.Release();
        }
    }
}