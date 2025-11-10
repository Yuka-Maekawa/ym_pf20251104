using System;

namespace MyProject.Common
{
    public class StateMachine<T>
    {
        public delegate void StateFunc();

        private StateFunc[] _stateFuncs = null;

        private int _current = 0;
        private T[] _stateList = null;

        public T Current => _stateList[_current];

        public bool FirstTime { private set; get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StateMachine()
        {
            if (typeof(T).IsEnum == false)
            {
                throw new ArgumentException("enum型以外登録できません");
            }

            _stateList = Enum.GetValues(typeof(T)) as T[];
            _stateFuncs = new StateFunc[_stateList.Length];

            FirstTime = true;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="firstState">最初のステート</param>
        public StateMachine(T firstState) : this()
        {
            int count = _stateList.Length;
            for (int i = 0; i < count; i++)
            {
                if (!firstState.Equals(_stateList[i])) { continue; }

                _current = i;
                break;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        public void Update()
        {
            var beforIdx = _current;
            _stateFuncs[_current]?.Invoke();

            FirstTime = beforIdx != _current;
        }

        /// <summary>
        /// ステートを加える
        /// </summary>
        /// <param name="state">ステート</param>
        /// <param name="func">ステートのメソッド</param>
        public void AddState(T state, StateFunc func)
        {
            var idx = Array.IndexOf(_stateList, state);
            _stateFuncs[idx] = func;
        }

        /// <summary>
        /// 指定のステートに移動
        /// </summary>
        /// <param name="state">ステート</param>
        public void MoveState(T state)
        {
            _current = Array.IndexOf(_stateList, state);

            FirstTime = true;
        }
    }
}