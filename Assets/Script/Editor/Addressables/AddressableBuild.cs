using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.Build.Pipeline.Utilities;

public class AddressableBuild
{
    /// <summary>
    /// クリーンビルド
    /// </summary>
    [MenuItem("MyProject/Build/Resource/Addressable(Clean)", priority = 1)]
    public static void BuildAddressablesWithClean()
    {
        AddressableAssetSettings.CleanPlayerContent();
        BuildCache.PurgeCache(false);
        BuildAddressables();
    }

    /// <summary>
    /// Addressablesをビルド
    /// </summary>
    [MenuItem("MyProject/Build/Resource/Addressable(Default)", priority = 2)]
    public static void BuildAddressables()
    {
        // デフォルトのビルド
        AddressableAssetSettings.BuildPlayerContent();
    }
}
