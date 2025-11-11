using Cysharp.Threading.Tasks;
using MyProject.Common;
using MyProject.Gacha.Lottery;
using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaResultController : MonoBehaviour
    {
        protected enum State
        {
            Idle,
            Lottery,
            ViewUI
        }

        [SerializeField] protected GachaResultUIController _uIController = null;
        [SerializeField] protected uint _useId = 1;
        [SerializeField] protected uint _playCount = 1;

        protected GachaLotteryController _gachaLottery = null;
        private GachaLotteryController.ItemInfo[] _items = null;

        protected StateMachine<State> _stateMachine = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _stateMachine = new StateMachine<State>(State.Idle);
            _gachaLottery = new GachaLotteryController();
            _items = new GachaLotteryController.ItemInfo[_playCount];

            _uIController.Initialize((int)_playCount);

            _stateMachine.AddState(State.Lottery, StateLottery);
            _stateMachine.AddState(State.ViewUI, StateViewUI);

            InitializeAsync().Forget();
        }

        /// <summary>
        /// 初期化（非同期）
        /// </summary>
        public async UniTask InitializeAsync()
        {
            await _gachaLottery.InitializeAsync((int)_useId);
            _stateMachine.MoveState(State.Lottery);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        {
            _uIController.ReleaseAsync().Forget();

            _gachaLottery?.Release();
            _gachaLottery = null;

            _stateMachine = null;
        }

        public void Update()
        {
            _stateMachine?.Update();
        }

        /// <summary>
        /// ステート：抽選
        /// </summary>
        private void StateLottery()
        {
            if (_stateMachine.FirstTime)
            {
                for (int i = 0; i < _playCount; ++i)
                {
                    _items[i] = _gachaLottery.GetLotteryResult();
                    Debug.Log($"{_items[i].Rarity}： {_items[i].Item.Name}");
                }

                StateLotteryAsync().Forget();
            }
        }

        /// <summary>
        /// 抽選ステートの非同期処理
        /// </summary>
        private async UniTask StateLotteryAsync()
        {
            for (int i = 0; i < _playCount; ++i)
            {
                await _uIController.SetupItemsAsync(_items[i], i);
            }

            _stateMachine.MoveState(State.ViewUI);
        }

        /// <summary>
        /// ステート：UIを表示
        /// </summary>
        private void StateViewUI()
        {
            if (_stateMachine.FirstTime)
            {
                _uIController.ViewAllItem();
            }
        }
    }
}