using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;   // �\���؂�ւ��t���O

    [Header("���C�����"), SerializeField]
    private GameObject mainScreen;
    [Header("�I�v�V�������"), SerializeField]
    private GameObject optionsScreen;   // �I�v�V�������

    [Header("�{�^���N���b�N����SE"), SerializeField]
    private AudioClip clickSE;

    //[Header("�I�v�V����UI�̐e�I�u�W�F�N�g"), SerializeField]
    //private RectTransform contents;

    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;

        mainScreen.SetActive(!isOpen);
        optionsScreen.SetActive(isOpen);

        //contents.position = new Vector2(contents.position.x, 0);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Title") return;

        if (Input.GetKeyDown(KeyCode.Escape)) ControlOption();
    }

    public void ControlOption()
    {
        isOpen = !isOpen;

        SESource.instance.PlaySE(clickSE);

        mainScreen.SetActive(!isOpen);
        optionsScreen.SetActive(isOpen);
    }

    public bool IsOpen
    {
        get { return isOpen; }
    }
}
