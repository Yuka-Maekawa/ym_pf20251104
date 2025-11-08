using Cysharp.Threading.Tasks;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    public static T Instance;

    protected virtual void Release() { }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            Debug.LogWarning("シングルトンが重複しています");
        }
    }

    private void OnDestroy()
    {
        DestroyInstance();
    }

    protected virtual UniTask Initialize() => UniTask.CompletedTask;

    /// <summary>
    /// インスタンスを作成
    /// </summary>
    /// <param name="parent">親オブジェクト</param>
    public static async UniTask<T> CreateInstance(GameObject parent = null)
    {
        Instance = new GameObject(typeof(T).Name).AddComponent<T>();

        if (parent != null)
        {
            Instance.transform.SetParent(parent.transform, false);
        }

        await Instance.Initialize();

        return Instance;
    }

    /// <summary>
    /// インスタンスを破棄
    /// </summary>
    public static void DestroyInstance()
    {
        if (Instance == null) { return; }

        Instance.Release();
        Instance = null;
    }
}
