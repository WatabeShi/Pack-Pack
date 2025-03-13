using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Header("�ǂݍ��ރV�[����"), SerializeField]
    private string sceneName;

    public void LoadSomeScene(bool isStopMusic)
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(sceneName);
        if(isStopMusic) BGMSource.instance.StopBGM();   // �t���O���L���Ȃ�BGM���~�߂�
    }

    public void BackTitle(bool isFPS)
    {
        DOTween.Clear(true);
        SceneManager.LoadScene("Title");
        
        BGMSource.instance.StopBGM();   // �t���O���L���Ȃ�BGM���~�߂�

        if (isFPS) ScoreRanking.instance.RankInFPS(ScoreManager.scoreNow);
        else ScoreRanking.instance.RankInTPS(ScoreManager.scoreNow);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;   //�Q�[���v���C�I��
#else
    Application.Quit();   //�Q�[���v���C�I��
#endif
    }
}
