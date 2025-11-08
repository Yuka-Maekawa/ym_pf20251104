using Cysharp.Threading.Tasks;
using MyProject.Systems.Resource;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    [SerializeField] private static GameObject _object= null;

    /// <summary>
    /// 初期化(非同期)
    /// </summary>
    public async UniTask InitializeAsync()
    {
        DontDestroyOnLoad(this.gameObject);
       await ResourceManager.CreateInstance(this.gameObject);
    }

    /// <summary>
    /// 解放
    /// </summary>
    public void Release()
    {
        ResourceManager.DestroyInstance();
    }
}
