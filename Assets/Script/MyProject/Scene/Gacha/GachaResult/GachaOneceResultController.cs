using Cysharp.Threading.Tasks;
using MyProject.Common;
using MyProject.Gacha.Lottery;
using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaOneceResultController : MonoBehaviour
    {
        protected enum State
        {
            Idle,
            Lottery,
            SetupUI,
            ViewUI
        }

        private static readonly int _useId = 1;

        private GachaLotteryController _gachaLottery = null;
        private GachaLotteryController.ItemInfo _item = null;

        private StateMachine<State> _stateMachine = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _stateMachine = new StateMachine<State>(State.Idle);
            _stateMachine.AddState(State.Lottery, StateLottery);
            _stateMachine.AddState(State.SetupUI, StateSetupUI);
            _stateMachine.AddState(State.ViewUI, StateViewUI);

            _gachaLottery = new GachaLotteryController();

            InitializeAsync().Forget();
        }

        /// <summary>
        /// 初期化（非同期）
        /// </summary>
        public async UniTask InitializeAsync()
        {
            await _gachaLottery.InitializeAsync(_useId);
            _stateMachine.MoveState(State.Lottery);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        {
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
                _item = _gachaLottery.GetLotteryResult();
                Debug.Log($"{_item.Rarity}： {_item.Item.Name}");
                _stateMachine.MoveState(State.SetupUI);
            }
        }

        /// <summary>
        /// ステート：UIの設定
        /// </summary>
        private void StateSetupUI()
        {

        }

        /// <summary>
        /// ステート：UIを表示
        /// </summary>
        private void StateViewUI()
        {

        }
    }
}