using Cysharp.Threading.Tasks;
using MyProject.Gacha.Lottery;
using UnityEngine;

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
                    _items[i] = _gachaLottery.GetDefaultLotteryResult();
                    Debug.Log($"{_items[i].Rarity}： {_items[i].Item.Name}");
                }

                var item = _gachaLottery.GetLastLotteryResult();
                _items[_playCount - 1] = item;
                Debug.Log($"{item.Rarity}： {item.Item.Name}");

                _uIController.Open();

                StateLotteryAsync().Forget();
            }

            if (_uIController.IsMenuItemsSetting() && _uIController.IsEndOpenAnimation())
            {
                _stateMachine.MoveState(State.ViewUI);
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
        /// ステート：UIを表示
        /// </summary>
        private void StateViewUI()
        {
            if (_stateMachine.FirstTime)
            {
                StateViewUIAsync().Forget();
            }
        }

        private async UniTask StateViewUIAsync()
        {
            await UniTask.Yield();
            _uIController.ViewAllItem();
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