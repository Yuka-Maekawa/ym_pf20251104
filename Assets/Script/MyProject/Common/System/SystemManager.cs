using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class SystemManager
{
    private static readonly string _gameSystemPath = "Assets/_Res/System/Prefab/GameSystem.prefab";

    private static GameSystem _gameSystem = null;

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
        var  obj = Object.Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(_gameSystemPath));
        _gameSystem = obj.GetComponent<GameSystem>();

        await _gameSystem.InitializeAsync();

        _isInitialized = true;
    }
}
