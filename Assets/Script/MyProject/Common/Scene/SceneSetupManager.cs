using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SceneSetupManager : MonoBehaviour
{
    [SerializeField] private UnityEvent OnInitialize = null;
    [SerializeField] private UnityEvent OnRelease = null;

    private void Start()
    {
        InitializeAsync().Forget();
    }

    /// <summary>
    /// 初期化(非同期)
    /// </summary>
    private async UniTask InitializeAsync()
    {
        await UniTask.WaitUntil(() => SystemManager.IsInitialized);
        OnInitialize?.Invoke();
    }

    private void OnDestroy()
    {
        Release();
    }

    /// <summary>
    /// 解放処理
    /// </summary>
    public void Release()
    {
        OnRelease?.Invoke();
    }
}
