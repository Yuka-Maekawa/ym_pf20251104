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
            OpenWindow,
            ViewUI,
            BackWait
        }

        [SerializeField] protected GachaResultUIController _uIController = null;
        [SerializeField] protected int _defaultLotteryId = 1;
        [SerializeField] protected int _playCount = 1;

        private static readonly string _backScenePath = "Scene/Gacha/GachaMenu";

        protected GachaLotteryControllerBase.ItemInfo[] _items = null;

        protected StateMachine<State> _stateMachine = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize()
        {
            _stateMachine = new StateMachine<State>(State.Idle);
            _items = new GachaLotteryControllerBase.ItemInfo[_playCount];

            _uIController.Initialize(_playCount);
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
        /// ステート：UIを表示
        /// </summary>
        protected void StateViewUI()
        {
            if (_stateMachine.FirstTime)
            {
                StateViewUIAsync().Forget();
            }
        }

        /// <summary>
        /// UIを表示するステートの非同期処理
        /// </summary>
        private async UniTask StateViewUIAsync()
        {
            await _uIController.ViewAllItemAsync();
            _uIController.ViewBackSceneButton();
            _stateMachine.MoveState(State.BackWait);
        }

        /// <summary>
        /// メニューに戻る
        /// </summary>
        public void PushBackSceneButton()
        {
            PushBackSceneButtonAsync().Forget();
        }

        /// <summary>
        /// メニューに戻る（非同期）
        /// </summary>
        private async UniTask PushBackSceneButtonAsync()
        {
            if (_stateMachine.Current != State.BackWait) { return; }

            _stateMachine.MoveState(State.Idle);

            await _uIController.CloseAsync();
            await _uIController.ReleaseAsync();
            await NextSceneAsync();
        }

        /// <summary>
        /// 次のシーンへ移動
        /// </summary>
        /// <param name="nextScenePath">シーンのファイルパス</param>
        protected async UniTask NextSceneAsync()
        {
            await SceneLoader.SceneLoad.LoadSceneAsync(_backScenePath);
        }
    }
}