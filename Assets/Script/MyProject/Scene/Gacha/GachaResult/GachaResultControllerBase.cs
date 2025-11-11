using Cysharp.Threading.Tasks;
using MyProject.Common;
using MyProject.Gacha.Lottery;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaResultControllerBase : MonoBehaviour
    {
        protected enum State
        {
            Idle,
            Lottery,
            ViewUI
        }

        [SerializeField] protected GachaResultUIController _uIController = null;
        [SerializeField] protected int _defaultLotteryId = 1;
        [SerializeField] protected int _playCount = 1;

        protected GachaLotteryControllerBase.ItemInfo[] _items = null;

        protected StateMachine<State> _stateMachine = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize()
        {
            _stateMachine = new StateMachine<State>(State.Idle);
            _items = new GachaLotteryControllerBase.ItemInfo[_playCount];

            _uIController.Initialize((int)_playCount);
        }

        /// <summary>
        /// 初期化（非同期）
        /// </summary>
        public virtual async UniTask InitializeAsync()
        {
            await UniTask.CompletedTask;
        }

        /// <summary>
        /// 解放
        /// </summary>
        public virtual void Release()
        {
            _items = null;
            _stateMachine = null;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void UpdateSub()
        {
            _stateMachine?.Update();
        }

        /// <summary>
        /// 次のシーンへ移動
        /// </summary>
        /// <param name="nextScenePath">シーンのファイルパス</param>
        protected async UniTask NextSceneAsync()
        {
            await SceneLoader.SceneLoad.LoadSceneAsync("Scene/Gacha/GachaMenu");
        }
    }
}