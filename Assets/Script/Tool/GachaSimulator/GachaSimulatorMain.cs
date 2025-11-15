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

        public class SimulationInfo
        {
            public GachaRarityLottery.Rarity Rarity = GachaRarityLottery.Rarity.Rare;
            public int Count = 0;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="rarity">レアリティ</param>
            public SimulationInfo(GachaRarityLottery.Rarity rarity)
            {
                Rarity = rarity;
                Count = 1;
            }
        }

        // 今は定数ですが、今後UIから情報を取得する予定
        private static readonly int _id = 1;
        private static readonly int _playCount = 1000;
        private static LotteryType _lotteryType = LotteryType.Default;
        //

        private GachaLotteryControllerBase _gachaOnceLottery = null;
        private GachaLotteryMultipleTimeController _gachaTenTimesLottery = null;

        private GachaLotteryControllerBase.ItemInfo[] _items = null;

        private Dictionary<string, SimulationInfo> _itemHistory = null;
        private Dictionary<GachaRarityLottery.Rarity, int> _rarityHistory = null;

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

        /// <summary>
        /// シミュレーションをリセット
        /// </summary>
        private void ResetSimulation()
        {
            _simulationCount = 0;

            _itemHistory?.Clear();
            _itemHistory = null;

            _rarityHistory?.Clear();
            _rarityHistory = null;

            _items = null;
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
                StartupSimulation();
            }

            var resultItem = _gachaOnceLottery.GetDefaultLotteryResult();

            CountupGachaItemHistory(resultItem);
            CountupGachaItemHistory(resultItem.Rarity);
            _items[_simulationCount] = resultItem;

            ++_simulationCount;

            if (_simulationCount >= _playCount)
            {
                ResultLottery();
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
                StartupSimulation();
            }

            var item = _gachaTenTimesLottery.GetDefaultLotteryResult();
            
            CountupGachaItemHistory(item);
            CountupGachaItemHistory(item.Rarity);
            _items[_simulationCount] = item;

            ++_simulationCount;

            if (_simulationCount >= _playCount)
            {
                Debug.Log("シミュレーションが完了しました");
                ResultLottery();
                _stateMachine.MoveState(State.Home);
            }
        }

        /// <summary>
        /// シミュレーションの開始準備
        /// </summary>
        private void StartupSimulation()
        {
            ResetSimulation();

            _rarityHistory = new Dictionary<GachaRarityLottery.Rarity, int>(_playCount);
            _itemHistory = new Dictionary<string, SimulationInfo>(_playCount);
            _items = new GachaLotteryControllerBase.ItemInfo[_playCount];
        }

        /// <summary>
        /// アイテムの抽選結果を記録
        /// </summary>
        /// <param name="itemInfo">抽選結果</param>
        private void CountupGachaItemHistory(GachaLotteryControllerBase.ItemInfo itemInfo)
        {
            string name = itemInfo.Item.Name;

            if (_itemHistory != null && _itemHistory.ContainsKey(name))
            {
                ++_itemHistory[name].Count;
                return;
            }

            _itemHistory.Add(name, new SimulationInfo(itemInfo.Rarity));
        }

        /// <summary>
        /// レアリティの抽選結果を記録
        /// </summary>
        /// <param name="itemInfo">抽選結果</param>
        private void CountupGachaItemHistory(GachaRarityLottery.Rarity rarity)
        {
            if (_rarityHistory != null && _rarityHistory.ContainsKey(rarity))
            {
                ++_rarityHistory[rarity];
                return;
            }

            _rarityHistory.Add(rarity, 1);
        }

        /// <summary>
        ///  抽選結果
        /// </summary>
        private void ResultLottery()
        {
            Debug.Log("シミュレーションが完了しました");
            Debug.Log("-- シミュレーション結果 --");

            foreach (var item in _rarityHistory)
            {
                var lotteryCount = item.Value;
                Debug.Log($"{item.Key}：{(float)(lotteryCount / (float)_playCount) * 100}");
            }

            foreach (var item in _itemHistory)
            {
                var lotteryCount = item.Value.Count;
                Debug.Log($"{item.Key}：{(float)(lotteryCount / (float)_rarityHistory[item.Value.Rarity]) * 100}");
            }

            Debug.Log("---------------------------------");
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