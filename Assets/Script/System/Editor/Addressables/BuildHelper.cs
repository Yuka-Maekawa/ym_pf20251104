using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class BuildHelper
{
    private static readonly string _scenePath = "Assets/_Res/Scene/Launcher.unity";
    private static readonly string _dateTimeFormat = "yyyyMMddHHmm";

    /// <summary>
    /// リリースROM作成
    /// </summary>
    [MenuItem("MyProject/Build/Rom/Release", priority = 10)]
    public static void BuildRelease()
    {
        Build(BuildOptions.None);

        SetBuildSettings(true, false, false, true, false);
        SetPlayerSettingLoggingAll(StackTraceLogType.ScriptOnly);
    }

    /// <summary>
    /// デベロップROM作成
    /// </summary>
    [MenuItem("MyProject/Build/Rom/Development(Default)", priority = 11)]
    public static void BuildDevelopment()
    {
        Build(BuildOptions.Development);
    }

    /// <summary>
    /// デベロップROM作成(DeepProfiling)
    /// </summary>
    [MenuItem("MyProject/Build/Rom/Development(Deep Profiling)", priority = 12)]
    public static void BuildDevelopmentDeepProfiling()
    {
        Build(BuildOptions.Development | BuildOptions.EnableDeepProfilingSupport);
    }

    /// <summary>
    /// デベロップROM作成(ScriptDebugging)
    /// </summary>
    [MenuItem("MyProject/Build/Rom/Development(Script Debugging)", priority = 13)]
    public static void BuildDevelopmentScriptDebugging()
    {
        Build(BuildOptions.Development | BuildOptions.AllowDebugging);
    }

    /// <summary>
    /// ビルド
    /// </summary>
    /// <param name="buildOption">Unityプロジェクトのビルドオプション</param>
    public static void Build(BuildOptions buildOption)
    {
        var isDevelopment = (buildOption & BuildOptions.Development) > 0;

        // ビルドの設定
        BuildPlayerOptions buildPlayerOption = new BuildPlayerOptions
        {
            locationPathName = CreateRomPath(isDevelopment),
            options = buildOption,
            scenes = new string[] { _scenePath },
            target = BuildTarget.StandaloneWindows64,
            targetGroup = BuildTargetGroup.Standalone
        };

        // ログの設定
        SetPlayerSettingLoggingAll(isDevelopment ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);

        // ユーザービルド設定
        SetBuildSettings(isDevelopment, false, buildOption.HasFlag(BuildOptions.AllowDebugging), true, false);

        var buildReport = BuildPipeline.BuildPlayer(buildPlayerOption);
        DeleteAutoGenerateFolder(buildReport);
    }

    /// <summary>
    /// ROMの出力パスの作成
    /// </summary>
    /// <param name="isDevelopment">true:Developmentビルド,  false:Releaseビルド</param>
    /// <returns>出力パス文字列</returns>
    public static string CreateRomPath(bool isDevelopment)
    {
        var buildType = isDevelopment ? "Development" : "Release";

        var romDir = $"Builds/{buildType}";

        if (System.IO.File.Exists(romDir) == false)
        {
            System.IO.Directory.CreateDirectory(romDir);
        }

        var dateTimeStr = System.DateTime.Now.ToString(_dateTimeFormat);
        return $"{romDir}/Portfolio_{dateTimeStr}_{buildType}.exe";
    }

    /// <summary>
    /// ビルド設定
    /// </summary>
    /// <param name="development">true: Development Build を有効, false: Development Build を無効</param>
    /// <param name="connectProfiler">true: プロファイラーに接続してプレイヤーを開始, false: なし</param>
    /// <param name="allowDebugging">true: 接続するソースレベルのデバッガーを有効, false: 接続するソースレベルのデバッガーを無効</param>
    /// <param name="explicitNullChecks">true:  null参照をチェック有効, false:  null参照をチェック無効</param>
    /// <param name="buildScriptsOnly">true: スクリプトのみをビルド, false: なし</param>
    public static void SetBuildSettings(bool development, bool connectProfiler, bool allowDebugging,
                                                       bool explicitNullChecks, bool buildScriptsOnly)
    {
        EditorUserBuildSettings.development = development;
        EditorUserBuildSettings.connectProfiler = connectProfiler;
        EditorUserBuildSettings.allowDebugging = allowDebugging;
        EditorUserBuildSettings.explicitNullChecks = explicitNullChecks;
        EditorUserBuildSettings.buildScriptsOnly = buildScriptsOnly;
    }

    /// <summary>
    /// 全てのログの記録方法を設定
    /// </summary>
    /// <param name="logType">StackTraceLogType</param>
    public static void SetPlayerSettingLoggingAll(StackTraceLogType logType)
    {
        PlayerSettings.SetStackTraceLogType(LogType.Error, logType);
        PlayerSettings.SetStackTraceLogType(LogType.Assert, logType);
        PlayerSettings.SetStackTraceLogType(LogType.Warning, logType);
        PlayerSettings.SetStackTraceLogType(LogType.Log, logType);
        PlayerSettings.SetStackTraceLogType(LogType.Exception, logType);
    }

    /// <summary>
    /// 自動生成されるビルドレポートを削除
    /// </summary>
    /// <param name="report">ビルドレポート</param>
    public static void DeleteAutoGenerateFolder(BuildReport report)
    {
        string outputPath = report.summary.outputPath;

        try
        {
            string applicationName = System.IO.Path.GetFileNameWithoutExtension(outputPath);
            string outputFolder = System.IO.Path.GetDirectoryName(outputPath);

            outputFolder = System.IO.Path.GetFullPath(outputFolder);

            string burstDebugInformationDirectoryPath = System.IO.Path.Combine(outputFolder, $"{applicationName}_BackUpThisFolder_ButDontShipItWithYourGame");

            if (System.IO.Directory.Exists(burstDebugInformationDirectoryPath))
            {
                Debug.Log($"'{burstDebugInformationDirectoryPath}' にあるデバッグ情報のフォルダを削除します。 ");
                System.IO.Directory.Delete(burstDebugInformationDirectoryPath, true);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"ビルドのクリーンアップ中に予期しない例外が発生しました: {e}");
        }
    }
}
