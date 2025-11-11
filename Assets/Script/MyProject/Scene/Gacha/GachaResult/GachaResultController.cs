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
                _uIController.Open();
                _uIController.ViewAllItem();
            }
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
            _uIController.Close();
            await UniTask.Yield();
            await _uIController.ReleaseAsync();
            await NextSceneAsync();
        }
    }
}