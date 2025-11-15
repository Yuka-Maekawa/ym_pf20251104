using UnityEngine;

public class ExitButton : MonoBehaviour
{
    /// <summary>
    /// ゲーム終了ボタンを押した
    /// </summary>
    public void PushExitButton()
    {
        //ゲーム終了
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
