using UnityEngine;

public class TestGacha : MonoBehaviour
{
    /// <summary>
    /// 初期化の終了確認
    /// </summary>
    public void TestInitialize()
    {
        Debug.Log("シーンの初期化が終わったよ！");
    }

    /// <summary>
    /// 解放の終了確認
    /// </summary>
    public void TestRelease()
    {
        Debug.Log("シーンの開放が終わったよ！");
    }
}
