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
                await Addressables.LoadSceneAsync(nextScenePath, LoadSceneMode.Single);
            }
        }
    }
}