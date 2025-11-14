using Cysharp.Threading.Tasks;
using MyProject.Common.UI;
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
            Simulation,
            WriteFile
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            InitializeAsync().Forget();
        }

        /// <summary>
        /// 初期化(非同期)
        /// </summary>
        public async UniTask InitializeAsync()
        {
            await FadeManager.Fade.PlayFadeInAsync();
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release()
        { 
}

    }
}