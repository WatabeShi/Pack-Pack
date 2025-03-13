using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static event Action clearEvent;   // �N���A�C�x���g
    private bool clearEventTrigger = true;   // �N���A�C�x���g�L���t���O

    //public static event Action gameoverEvent;   // �Q�[���I�[�o�[�C�x���g
    //private bool gameoverEventTrigger = true;   // �Q�[���I�[�o�[�C�x���g�L���t���O

    private GameManagerTPS gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManagerTPS>();
    }

    // Update is called once per frame
    void Update()
    {
        ClearEventExecuter();
        //GameoverEventExecuter();
    }

    private void ClearEventExecuter()   // �N���A�C�x���g���s��
    {
        if (!clearEventTrigger) return;     // �C�x���g�t���O�������̏ꍇ
        if (!gameManager.IsClear) return;   // �N���A�t���O�������̏ꍇ

        clearEvent?.Invoke();        // �C�x���g���s
        clearEventTrigger = false;   // �t���O������
    }

    //private void GameoverEventExecuter()   // �Q�[���I�[�o�[�C�x���g���s��
    //{
    //    if (!gameoverEventTrigger) return;     // �C�x���g�t���O�������̏ꍇ
    //    if (!gameManager.IsGameOver) return;   // �Q�[���I�[�o�[�t���O�������̏ꍇ

    //    gameoverEvent?.Invoke();        // �C�x���g���s
    //    gameoverEventTrigger = false;   // �t���O������
    //}
}
