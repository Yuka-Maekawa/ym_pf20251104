using Cysharp.Threading.Tasks;
using MyProject.Common;
using MyProject.Common.UI;
using UnityEngine;

namespace MyProject.Gacha.Menu
{
    public class GachaMenuController : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Home,
            Lineup
        }

        [SerializeField] private GachaMenuUIController _menuUIController = null;
        [SerializeField] private GachaMenuLineupUIController _lineupUIController = null;

        private StateMachine<State> _stateMachine = null;

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            _stateMachine = new StateMachine<State>(State.Idle);

            _menuUIController.Initialize();

            InitializeAsync().Forget();
        }

        /// <summary>
        /// 初期化（非同期）
        /// </summary>
        public async UniTask InitializeAsync()
        {
            await _lineupUIController.InitializeAsync();
            _menuUIController.Open();

            await FadeManager.Fade.PlayFadeInAsync();

            _stateMachine.MoveState(State.Home);
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        {
            _stateMachine = null;
        }

        public void Update()
        {
            _stateMachine?.Update();
        }

        /// <summary>
        /// ラインナップを開く
        /// </summary>
        public void PushLineupOpenButton()
        {
            if (_stateMachine.Current == State.Home)
            {
                PushLineupOpenButtonAsync().Forget();
            }
        }

        /// <summary>
        /// ラインナップを開く動作の非同期処理
        /// </summary>
        private async UniTask PushLineupOpenButtonAsync()
        {
            _stateMachine.MoveState(State.Idle);
            await _lineupUIController.OpenAsync();
            _stateMachine.MoveState(State.Lineup);
        }

        /// <summary>
        /// ラインナップを閉じる
        /// </summary>
        public void PushLineupCloseButton()
        {
            if (_stateMachine.Current == State.Lineup)
            {
                PushLineupCloseButtonAsync().Forget();
            }
        }

        /// <summary>
        /// ラインナップを閉じる動作の非同期処理
        /// </summary>
        private async UniTask PushLineupCloseButtonAsync()
        {
            _stateMachine.MoveState(State.Idle);
            await _lineupUIController.CloseAsync();
            _stateMachine.MoveState(State.Home);
        }

        /// <summary>
        /// 単発ボタンを押下
        /// </summary>
        public void PushOneGachaButton()
        {
            if (_stateMachine.Current == State.Home)
            {
                PushOneGachaButtonAsync().Forget();
            }
        }

        /// <summary>
        /// 単発ボタンを押下の非同期処理
        /// </summary>
        public async UniTask PushOneGachaButtonAsync()
        {
            _stateMachine.MoveState(State.Idle);
            await _lineupUIController.ReleaseAsync();
            _menuUIController.PushOneGachaButton();
        }

        /// <summary>
        /// 10回ボタンを押下
        /// </summary>
        public void PushTenTimeGachaButton()
        {
            if (_stateMachine.Current == State.Home)
            {
                PushTenTimeGachaButtonAsync().Forget();
            }
        }

        /// <summary>
        /// 10回ボタンを押下の非同期処理
        /// </summary>
        public async UniTask PushTenTimeGachaButtonAsync()
        {
            _stateMachine.MoveState(State.Idle);
            await _lineupUIController.ReleaseAsync();
            _menuUIController.PushTenTimeGachaButton();
        }
    }
}