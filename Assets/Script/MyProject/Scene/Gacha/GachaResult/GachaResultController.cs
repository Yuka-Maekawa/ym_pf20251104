using Cysharp.Threading.Tasks;
using MyProject.Gacha.Lottery;
using UnityEngine;

namespace MyProject.Gacha.Result
{
    public class GachaResultController : GachaResultControllerBase
    {
        private GachaLotteryControllerBase _gachaLottery = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            _gachaLottery = new GachaLotteryControllerBase();

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
                for (int i = 0; i < _playCount; ++i)
                {
                    _items[i] = _gachaLottery.GetDefaultLotteryResult();
                    Debug.Log($"{_items[i].Rarity}： {_items[i].Item.Name}");
                }

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

            if (!_uIController.IsMenuItemsSetting() && _uIController.IsEndOpenAnimation())
            {
                _stateMachine.MoveState(State.ViewUI);
            }
        }

        /// <summary>
        /// メニューに戻る
        /// </summary>
        public void PushBackSceneButton()
        {
            PushBackSceneButtonAsync().Forget();
        }
    }
}