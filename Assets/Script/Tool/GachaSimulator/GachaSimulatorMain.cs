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
            SetupSimulation,
            DefaultSimulation,
            TenTimesSimulation,
            WriteCsv,
        }

        public enum LotteryType
        {
            Default,
            SROrHigher, // SR以上
        }

        public class SimulationInfo
        {
            public GachaRarityLottery.Rarity Rarity = GachaRarityLottery.Rarity.Rare;
            public int Id;
            public int Count = 0;

            /// <summary>
            /// コンストラクタ
            /// </summary>
            /// <param name="rarity">レアリティ</param>
            /// <param name="id">Id</param>
            public SimulationInfo(GachaRarityLottery.Rarity rarity, int id)
            {
                Rarity = rarity;
                Id = id;
                Count = 1;
            }
        }

        [SerializeField] private GachaSimulatorMenuController _menuController = null;
        [SerializeField] private GachaSimulatiorProgressGage _progressGage = null;

        private GachaLotteryControllerBase _gachaOnceLottery = null;
        private GachaLotteryMultipleTimeController _gachaTenTimesLottery = null;
        private GachaSimulatiorFileController _fileController = null;

        private GachaLotteryControllerBase.ItemInfo[] _items = null;

        private Dictionary<string, SimulationInfo> _itemHistory = null;
        private Dictionary<GachaRarityLottery.Rarity, int> _rarityHistory = null;

        private StateMachine<State> _stateMachine = null;

        private LotteryType _simulationLotteryType = LotteryType.Default;

        private int _gachaInfoId = 0;
        private int _simulationNum = 0;
        private int _simulationCount = 0;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _menuController.Initialize();
            _progressGage.Initialize();

            _gachaOnceLottery = new GachaLotteryControllerBase();
            _gachaTenTimesLottery = new GachaLotteryMultipleTimeController();
            _fileController = new GachaSimulatiorFileController();

            _stateMachine = new StateMachine<State>(State.Idle);
            _stateMachine.AddState(State.SetupSimulation, StateSetupSimulation);
            _stateMachine.AddState(State.DefaultSimulation, StateDefaultSimulation);
            _stateMachine.AddState(State.TenTimesSimulation, StateTenTimeSimulation);
            _stateMachine.AddState(State.WriteCsv, StateWriteCsv);

            InitializeAsync().Forget();
        }

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        public async UniTask InitializeAsync()
        {
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

            _gachaTenTimesLottery.Release();
            _gachaTenTimesLottery = null;

            _gachaOnceLottery.Release();
            _gachaOnceLottery = null;

            _fileController = null;

            _menuController.Release();
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
        /// ステート：シミュレーションの準備
        /// </summary>
        private void StateSetupSimulation()
        {
            if(_stateMachine.FirstTime)
            {
                _simulationNum = _menuController.GetSimulationCount();
                if(_simulationNum <= 0)
                {
                    Debug.LogError("回数は1以上を登録してください");
                    _stateMachine.MoveState(State.Home);
                    return;
                }

                _progressGage.View(_simulationNum);
                StateSetupSimulationAsync().Forget();
            }
        }

        /// <summary>
        /// シミュレーションの準備の非同期処理
        /// </summary>
        private async UniTask StateSetupSimulationAsync()
        {
            _gachaInfoId = _menuController.GetSimulationId();

            if(_gachaInfoId < 0)
            {
                Debug.LogError("正しい数値が入力されていません");
                _stateMachine.MoveState(State.Home);
                return;
            }

            _simulationLotteryType = _menuController.GetSelectLotteryType();

            if (_simulationLotteryType == LotteryType.Default)
            {
                await _gachaOnceLottery.InitializeAsync(_gachaInfoId);
                _stateMachine.MoveState(State.DefaultSimulation);
                return;
            }

            await _gachaTenTimesLottery.InitializeAsync(_gachaInfoId);
            _stateMachine.MoveState(State.TenTimesSimulation);
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

            Simulation(_gachaOnceLottery.GetDefaultLotteryResult());
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

            Simulation(_gachaTenTimesLottery.GetLastLotteryResult());
        }

        /// <summary>
        /// シミュレーション
        /// </summary>
        /// <param name="itemInfo">抽選結果</param>
        private void Simulation(GachaLotteryControllerBase.ItemInfo itemInfo)
        {
            CountupGachaItemHistory(itemInfo);
            CountupGachaItemHistory(itemInfo.Rarity);
            _items[_simulationCount] = itemInfo;

            ++_simulationCount;

            // ゲージ更新
            _progressGage.UpdateProgress(_simulationCount);

            if (_simulationCount >= _simulationNum)
            {

                if(_simulationLotteryType == LotteryType.Default)
                {
                    _gachaOnceLottery.Release();
                }
                else
                {
                    _gachaTenTimesLottery.Release();
                }

                _stateMachine.MoveState(State.WriteCsv);
            }
        }

        /// <summary>
        /// ステート：シミュレーション結果をCSV出力
        /// </summary>
        private void StateWriteCsv()
        {
            if(_stateMachine.FirstTime)
            {
                _fileController.WriteSimulationResult(_rarityHistory, _itemHistory, _simulationLotteryType, _simulationNum, _gachaInfoId);
                _progressGage.Hide();

                _stateMachine.MoveState(State.Home);

                Debug.Log("シミュレーションが完了しました");
            }
        }

        /// <summary>
        /// シミュレーションの開始準備
        /// </summary>
        private void StartupSimulation()
        {
            ResetSimulation();

            _rarityHistory = new Dictionary<GachaRarityLottery.Rarity, int>(_simulationNum);
            int num = System.Enum.GetValues(typeof(GachaRarityLottery.Rarity)).Length;
            for (int i = 0; i < num; ++i)
            {
                _rarityHistory.Add((GachaRarityLottery.Rarity)i, 0);
            }

            _itemHistory = new Dictionary<string, SimulationInfo>(_simulationNum);
            _items = new GachaLotteryControllerBase.ItemInfo[_simulationNum];
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

            _itemHistory.Add(name, new SimulationInfo(itemInfo.Rarity, itemInfo.Item.Id));
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
        /// シミュレーション開始ボタンを押下
        /// </summary>
        public void PushStartButton()
        {
            if(_stateMachine.Current == State.Home)
            {
                _stateMachine.MoveState(State.SetupSimulation);
            }
        }

        /// <summary>
        /// 抽選の種類を選択するボタンを押下(左)
        /// </summary>
        public void PushLotteryTypeSelectLeftButton()
        {
            if (_stateMachine.Current == State.Home)
            {
                _menuController.PushSimulationTypeLeftButton();
            }
        }

        /// <summary>
        /// 抽選の種類を選択するボタンを押下(右)
        /// </summary>
        public void PushLotteryTypeSelectRightButton()
        {
            if (_stateMachine.Current == State.Home)
            {
                _menuController.PushSimulationTypeRightButton();
            }
        }
    }
}