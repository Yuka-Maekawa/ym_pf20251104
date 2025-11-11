using Cysharp.Threading.Tasks;
using MyProject.Common;
using MyProject.Gacha.Lottery;
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
        [SerializeField] protected uint _defaultLotteryId = 1;
        [SerializeField] protected uint _playCount = 1;

        protected GachaLotteryController _gachaLottery = null;
        protected GachaLotteryController.ItemInfo[] _items = null;

        protected StateMachine<State> _stateMachine = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public virtual void Initialize()
        {
            _stateMachine = new StateMachine<State>(State.Idle);
            _gachaLottery = new GachaLotteryController();
            _items = new GachaLotteryController.ItemInfo[_playCount];

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

            _uIController.ReleaseAsync().Forget();

            _gachaLottery?.Release();
            _gachaLottery = null;

            _stateMachine = null;
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void UpdateSub()
        {
            _stateMachine?.Update();
        }
    }
}