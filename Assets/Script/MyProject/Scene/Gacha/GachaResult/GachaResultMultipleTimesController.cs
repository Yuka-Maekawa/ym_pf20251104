using Cysharp.Threading.Tasks;
using MyProject.Gacha.Lottery;

namespace MyProject.Gacha.Result
{
    public class GachaResultMultipleTimesController : GachaResultControllerBase
    {
        private GachaLotteryMultipleTimeController _gachaLottery = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _gachaLottery = new GachaLotteryMultipleTimeController();

            _stateMachine.AddState(State.Lottery, StateLottery);
            _stateMachine.AddState(State.OpenWindow, StateOpenWindow);
            _stateMachine.AddState(State.ViewUI, StateViewUI);

            InitializeAsync().Forget();
        }

        /// <summary>
        /// 初期化（非同期）
        /// </summary>
        public override async UniTask InitializeAsync()
        {
            await _gachaLottery.InitializeAsync((int)_defaultLotteryId);
            _stateMachine.MoveState(State.Lottery);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public override void Release()
        {
            _gachaLottery?.Release();
            _gachaLottery = null;

            base.Release();
        }

        public void Update()
        {
            base.UpdateSub();
        }

        /// <summary>
        /// ステート：抽選
        /// </summary>
        private void StateLottery()
        {
            if (_stateMachine.FirstTime)
            {
                int count = (int)_playCount - 1;
                for (int i = 0; i < count; ++i)
                {
                    LotteryItem(i);
                }

                LotteryItem(_playCount - 1);

                StateLotteryAsync().Forget();
                _stateMachine.MoveState(State.OpenWindow);
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
        }

        /// <summary>
        /// ステート：ウィンドウを開く
        /// </summary>
        private void StateOpenWindow()
        {
            if (_stateMachine.FirstTime)
            {
                _uIController.Open();
            }

            if (!_uIController.IsMenuItemsSetting() && !_uIController.IsPlayingOpenAnimation())
            {
                _stateMachine.MoveState(State.ViewUI);
            }
        }

        /// <summary>
        /// アイテム抽選
        /// </summary>
        /// <param name="index">インデックス値</param>
        private void LotteryItem(int index)
        {
            if (index >= _items.Length) { return; }

            var item = index < _playCount - 1 ? _gachaLottery.GetDefaultLotteryResult() : _gachaLottery.GetLastLotteryResult();

            _items[index] = item;
        }
    }
}