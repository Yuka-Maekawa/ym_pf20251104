using Cysharp.Threading.Tasks;
using MyProject.Systems.Resource;
using UnityEngine;

namespace MyProject.Launcher
{
    public class LauncherMain : MonoBehaviour
    {
        private static readonly string _backScenePath = "Scene/Gacha/GachaMenu";

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            SceneLoader.SceneLoad.LoadSceneAsync(_backScenePath).Forget();
        }

        /// <summary>
        /// 解放
        /// </summary>
        public void Release() { }
    }
}

