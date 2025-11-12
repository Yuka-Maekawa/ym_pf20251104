using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace MyProject.Systems.Resource
{
    public partial class SceneLoader
    {
        public class Loader
        {
            public async UniTask LoadSceneAsync(string nextScenePath)
            {
                var defaultSceneSetupObj = GameObject.Find("DefaultSceneSetup");
                if (defaultSceneSetupObj != null)
                {
                    var script = defaultSceneSetupObj.GetComponent<SceneSetupManager>();
                    script.Release();
                }

                await Addressables.LoadSceneAsync(nextScenePath, LoadSceneMode.Single);
            }
        }
    }
}