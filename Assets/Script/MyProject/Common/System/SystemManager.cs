using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class SystemManager
{
    private static bool _isInitialized = false;
    public static bool IsInitialized { get { return _isInitialized; } }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnRuntimeLoad()
    {
        _isInitialized = false;

        InitializeAsync().Forget();
    }

    /// <summary>
    /// 初期化(非同期)
    /// </summary>
    private static async UniTask InitializeAsync()
    {
        await GameSystem.CreateInstance();

        await GameSystem.Instance.InitializeAsync();

#if UNITY_EDITOR
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif

        _isInitialized = true;
    }

#if UNITY_EDITOR
    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        // 再生終了直前に呼ばれる
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            GameSystem.DestroyInstance();
        }
    }

#else

    /// <summary>
    /// ROM終了時の処理
    /// </summary>
    private void OnApplicationQuit()
    {
        GameSystem.DestroyInstance();
    }
#endif

}
