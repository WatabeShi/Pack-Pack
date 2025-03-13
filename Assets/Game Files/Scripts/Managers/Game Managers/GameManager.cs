using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("�Q�[�����[�h�؂�ւ�"), SerializeField]
    protected bool isFPS = false;

    protected int baitCount = 0;      // �X�e�[�W���̃G�T�̑���
    protected int eatBaitCount = 0;   // �H�ׂ��G�T�̐�

    //[Header("�c�@�\���摜"), SerializeField]
    //private GameObject[] lifeImgs;

    protected LifeManager lifeManager;

    protected PowerUpBait[] puBaits;
    protected EnemyBase[] enemies;

    protected void Awake()
    {
        lifeManager = FindAnyObjectByType<LifeManager>();

        puBaits = FindObjectsByType<PowerUpBait>(FindObjectsSortMode.None);
        enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
    }

    // Start is called before the first frame update
    protected void Start()
    {
        Time.timeScale = 1;

        baitCount = FindObjectsByType<BaitBase>(FindObjectsSortMode.None).Length;   // �G�T�̑������L�^
    }

    public void IncreaseEatCount()   // �H�ׂ��G�T�̐��̉��Z
    {
        eatBaitCount++;
    }

    protected void DispLife()   // ���C�t�̕\��
    {
        lifeManager.DispLife();
    }

    public void GameOver()   // �Q�[���I�[�o�[����
    {
        SceneManager.LoadScene("Gameover");
    }

    public int EatBaitCount
    {
        get { return eatBaitCount; }
    }

    public bool IsFPS
    {
        get { return isFPS; }
    }
}
