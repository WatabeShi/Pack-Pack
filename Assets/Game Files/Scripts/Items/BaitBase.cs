using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaitBase : MonoBehaviour
{
    [Header("������X�R�A"), SerializeField]
    protected int score = 10;

    [Header("�H�ׂ�����SE"), SerializeField]
    private AudioClip eatSE;

    protected GameManager gameManager;
    protected ScoreManager scoreManager;

    protected bool isFPS = false;

    protected void Awake()
    {
        gameManager = /*isFPS ? FindAnyObjectByType<GameManagerFPS>() : */FindAnyObjectByType<GameManager>();
        isFPS = gameManager.IsFPS;

        scoreManager = FindAnyObjectByType<ScoreManager>();

        if (isFPS) GetComponent<SphereCollider>().radius = 0.02f;   // FPS���[�h���ɓ����蔻��̃T�C�Y����
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            BaseProcess(true);
        }
    }

    protected void BaseProcess(bool countTrigger)   // �v���C���[�ƐG�ꂽ���̊�{����
    {
        if (scoreManager != null) scoreManager.AddScore(score);   // �X�R�A��ǉ�
        if(countTrigger) gameManager.IncreaseEatCount();          // �g���K�[���L���̏ꍇ�H�ׂ����𑝂₷

        SESource.instance.PlaySE(eatSE);

        Destroy(this.gameObject);   // �������폜
    }
}
