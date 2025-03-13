using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("読み込むシーン名"), SerializeField]
    private string sceneName;

    public void LoadSomeScene(bool isStopMusic)
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(sceneName);
        if(isStopMusic) BGMSource.instance.StopBGM();   // フラグが有効ならBGMを止める
    }

    public void BackTitle(bool isFPS)
    {
        DOTween.Clear(true);
        SceneManager.LoadScene("Title");
        
        BGMSource.instance.StopBGM();   // フラグが有効ならBGMを止める

        if (isFPS) ScoreRanking.instance.RankInFPS(ScoreManager.scoreNow);
        else ScoreRanking.instance.RankInTPS(ScoreManager.scoreNow);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   //ゲームプレイ終了
#else
    Application.Quit();   //ゲームプレイ終了
#endif
    }
}
