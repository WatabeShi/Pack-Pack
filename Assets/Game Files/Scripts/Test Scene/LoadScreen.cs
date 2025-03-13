using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [Header("ロード時に表示するUI"), SerializeField]
    private GameObject _loadingUI;

    [Header("ロードゲージ"), SerializeField]
    private Slider _slider;

    [Header("スタートボタンクリック時のSE"), SerializeField]
    private AudioClip clickSE;

    private LifeManager lifeManager;
    private TitleModeChange modeChanger;

    private void Awake()
    {
        lifeManager = FindAnyObjectByType<LifeManager>();
        modeChanger = FindAnyObjectByType<TitleModeChange>();
    }

    public void StartGame()
    {
        BGMSource.instance.StopBGM();
        BGMSource.instance.RemoveClip();
        SESource.instance.PlaySE(clickSE);

        if (modeChanger.IsFPS)
        {
            // 各種値のリセット
            lifeManager.InitLife();
            ScoreManager.scoreNow = 0;

            LoadGameScene("Game FPS");
        }
        else
        {
            // 各種値のリセット
            lifeManager.InitLife();
            GameManagerTPS.stageNum = 1;
            ScoreManager.scoreNow = 0;

            LoadGameScene("Game");
        }
    }

    public void LoadGameScene(string sceneName)
    {
        _loadingUI.SetActive(true);
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string _sceneName)
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(_sceneName);
        while (!async.isDone)
        {
            _slider.value = async.progress;
            yield return null;
        }
    }
}
