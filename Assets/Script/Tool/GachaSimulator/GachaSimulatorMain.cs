using Cysharp.Threading.Tasks;
using MyProject.Common;
using MyProject.Common.UI;
using MyProject.Gacha.Lottery;
using System.Collections.Generic;
using UnityEngine;

namespace MyProject.Tool.GachaSimulator
{
    public class GachaSimulatorMain : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Home,
            InputPlayCount,
            InputGachaType,
            DefaultSimulation,
            TenTimesSimulation,
            WriteFile
        }

        public enum LotteryType
        {
            Default,
            SROrHigher, // SR以上
        }

        // 今は定数ですが、今後UIから情報を取得する予定
        private static readonly int _id = 1;
        private static readonly int _playCount = 1000;
        private static LotteryType _lotteryType = LotteryType.Default;
        //

        private GachaLotteryControllerBase _gachaOnceLottery = null;
        private GachaLotteryMultipleTimeController _gachaTenTimesLottery = null;

        private GachaLotteryControllerBase.ItemInfo[] _items = null;

        private Dictionary<string, int> _history = new Dictionary<string, int>();

        private StateMachine<State> _stateMachine = null;

        private int _simulationCount = 0;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _gachaOnceLottery = new GachaLotteryControllerBase();

            _stateMachine = new StateMachine<State>(State.Idle);
            _stateMachine.AddState(State.Home, StateHome);
            _stateMachine.AddState(State.DefaultSimulation, StateDefaultSimulation);
            _stateMachine.AddState(State.TenTimesSimulation, StateTenTimeSimulation);

            InitializeAsync().Forget();
        }

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        public async UniTask InitializeAsync()
        {
            await _gachaOnceLottery.InitializeAsync(_id);

            await FadeManager.Fade.PlayFadeInAsync();
            _stateMachine.MoveState(State.Home);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        {
            ResetSimulation();

            _stateMachine = null;

            _gachaOnceLottery.Release();
            _gachaOnceLottery = null;
        }

        public void Update()
        {
            _stateMachine?.Update();
        }

        /// <summary>
        /// ステート：ホーム
        /// </summary>
        private void StateHome()
        {

        }

        /// <summary>
        /// ステート：シミュレーション(通常)
        /// </summary>
        private void StateDefaultSimulation()
        {
            if(_stateMachine.FirstTime)
            {
                ResetSimulation();
            }

            var item = _gachaOnceLottery.GetDefaultLotteryResult();

            CountupGachaHistory(item);
            _items[_simulationCount] = item;

            ++_simulationCount;

            if (_simulationCount >= _playCount)
            {
                Debug.Log("シミュレーションが完了しました");
                _stateMachine.MoveState(State.Home);
            }
        }

        /// <summary>
        /// ステート：シミュレーション(SR以上確定)
        /// </summary>
        private void StateTenTimeSimulation()
        {
            if (_stateMachine.FirstTime)
            {
                ResetSimulation();
            }

            var item = _gachaTenTimesLottery.GetDefaultLotteryResult();
            
            CountupGachaHistory(item);
            _items[_simulationCount] = item;

            ++_simulationCount;

            if (_simulationCount >= _playCount)
            {
                Debug.Log("シミュレーションが完了しました");
                _stateMachine.MoveState(State.Home);
            }
        }

        /// <summary>
        /// シミュレーションをリセット
        /// </summary>
        private void ResetSimulation()
        {
            _simulationCount = 0;

            _history?.Clear();
            _history = null;
            _history = new Dictionary<string, int>(_playCount);

            _items = null;
            _items = new GachaLotteryControllerBase.ItemInfo[_playCount];
        }

        /// <summary>
        /// ガチャの抽選結果を記録
        /// </summary>
        /// <param name="itemInfo">抽選結果</param>
        private void CountupGachaHistory(GachaLotteryControllerBase.ItemInfo itemInfo)
        {
            string name = itemInfo.Item.Name;
            if(_history != null && _history.ContainsKey(name))
            {
                ++_history[name];
                return;
            }

            _history.Add(name, 1);
        }

        /// <summary>
        /// シミュレーション開始ボタンを押下
        /// </summary>
        public void PushStartButton()
        {
            if(_stateMachine.Current == State.Home)
            {
                _stateMachine.MoveState(_lotteryType == LotteryType.Default ? State.DefaultSimulation : State.TenTimesSimulation);
            }
        }
    }
}