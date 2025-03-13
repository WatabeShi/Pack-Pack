using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScreen : MonoBehaviour
{
    [Header("���[�h���ɕ\������UI"), SerializeField]
    private GameObject _loadingUI;

    [Header("���[�h�Q�[�W"), SerializeField]
    private Slider _slider;

    [Header("�X�^�[�g�{�^���N���b�N����SE"), SerializeField]
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
            // �e��l�̃��Z�b�g
            lifeManager.InitLife();
            ScoreManager.scoreNow = 0;

            LoadGameScene("Game FPS");
        }
        else
        {
            // �e��l�̃��Z�b�g
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
