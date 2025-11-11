using System.IO;
using UnityEditor;
using UnityEngine;

namespace MyProject.Database
{
#if UNITY_EDITOR
    public class CsvConverterBase
    {
        protected static readonly string _databaseFilePath = "Assets/_Res/Database/";
        protected static readonly string _csvFilePath = "Assets/File/";

        protected static readonly string _csvExtension = ".csv";
        protected static readonly string _assetExtension = ".asset";

        /// <summary>
        /// ScriptableObjectを作成
        /// </summary>
        /// <typeparam name="T">作成するScriptableObjectの型</typeparam>
        /// <param name="path">保存先のファイルパス</param>
        /// <returns>作成されたアセットのインスタンス</returns>
        protected static void CreateScriptableObject<T>(ScriptableObject data, string path) where T : ScriptableObject
        {
            // ディレクトリが存在しない場合は作成
            string directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"ScriptableObject '{typeof(T).Name}' を作成しました: {path}");
        }
    }
#endif
}

