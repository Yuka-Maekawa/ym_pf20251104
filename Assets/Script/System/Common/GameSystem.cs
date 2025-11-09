using Cysharp.Threading.Tasks;
using MyProject.Systems.Resource;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    private static readonly string _eventSystemFilePath = "Common/MyProjectEventSystem";

    private GameObject _eventSystemObj = null;

    /// <summary>
    /// 初期化(非同期)
    /// </summary>
    public async UniTask InitializeAsync()
    {
        DontDestroyOnLoad(this.gameObject);
        
        await ResourceManager.CreateInstance(this.gameObject);
        await ResourceManager.Global.LoadAssetAsync<GameObject>(_eventSystemFilePath);
        _eventSystemObj = ResourceManager.Global.GetAsset<GameObject>(_eventSystemFilePath, this.gameObject.transform);
    }

    /// <summary>
    /// 解放
    /// </summary>
    public void Release()
    {
        Destroy(_eventSystemObj);
        ResourceManager.DestroyInstance();
    }
}
