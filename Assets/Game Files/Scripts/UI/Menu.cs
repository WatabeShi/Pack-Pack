using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private bool isOpen = false;   // ���j���[�̊J���

    [Header("���j���[�I�u�W�F�N�g"), SerializeField]
    private GameObject menuObj;
    private CanvasGroup menuCG;

    private PlayerBase player;

    [Header("�I�v�V�����X�N���v�g"), SerializeField]
    private Options option;

    [Header("FPS���[�h��"), SerializeField]
    private bool isFPS = false;

    private void Awake()
    {
        player = isFPS ? FindAnyObjectByType<PlayerBaseFPS>() : FindAnyObjectByType<PlayerBase>();   // FPS���[�h���ǂ����Ŏ擾������̂�ύX

        option = option.GetComponent<Options>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //menuCG = menuObj.GetComponent<CanvasGroup>();

        MenuActiveChanger();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (player.IsDead) return;
            if (option.IsOpen) return;

            ChangeOpenState();   // �J��Ԃ̔��]
        }

        if (!isFPS) return;

        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;   // ���j���[���J���Ă��邩�ǂ����ŃJ�[�\���̏�Ԃ�ύX
    }

    public void Open()   // ���j���[�{�^�������������̏���
    {
        if (player.IsDead) return;   // �v���C���[������ł�����I��

        ChangeOpenState(true);
    }

    public void Resume()   // �Q�[���֖߂�{�^�������������̏���
    {
        ChangeOpenState(false);
    }

    private void MenuActiveChanger()   // ���j���[�̕\���؂�ւ�
    {
        menuObj.SetActive(isOpen);
    }

    private void DispMenu()   // ���j���[�\���Ǝ��Ԃ̐؂�ւ�
    {
        if (isOpen) Time.timeScale = 0;
        else Time.timeScale = 1;

        MenuActiveChanger();
    }

    private void ChangeOpenState()   // ���j���[���J�����������
    {
        isOpen = !isOpen;
        DispMenu();

        ControlBGM();
    }
    private void ChangeOpenState(bool _isOpen)   // ���j���[���J�������邩�w�肵�ď������s��
    {
        isOpen = _isOpen;
        DispMenu();

        ControlBGM();
    }

    private void ControlBGM()
    {
        if (isOpen) BGMSource.instance.StopBGM();
        else BGMSource.instance.PlayBGM();
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
