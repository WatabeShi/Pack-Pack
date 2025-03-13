using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private bool isOpen = false;   // メニューの開閉状態

    [Header("メニューオブジェクト"), SerializeField]
    private GameObject menuObj;
    private CanvasGroup menuCG;

    private PlayerBase player;

    [Header("オプションスクリプト"), SerializeField]
    private Options option;

    [Header("FPSモードか"), SerializeField]
    private bool isFPS = false;

    private void Awake()
    {
        player = isFPS ? FindAnyObjectByType<PlayerBaseFPS>() : FindAnyObjectByType<PlayerBase>();   // FPSモードかどうかで取得するものを変更

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

            ChangeOpenState();   // 開閉状態の反転
        }

        if (!isFPS) return;

        Cursor.visible = isOpen;
        Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;   // メニューを開いているかどうかでカーソルの状態を変更
    }

    public void Open()   // メニューボタンを押した時の処理
    {
        if (player.IsDead) return;   // プレイヤーが死んでいたら終了

        ChangeOpenState(true);
    }

    public void Resume()   // ゲームへ戻るボタンを押した時の処理
    {
        ChangeOpenState(false);
    }

    private void MenuActiveChanger()   // メニューの表示切り替え
    {
        menuObj.SetActive(isOpen);
    }

    private void DispMenu()   // メニュー表示と時間の切り替え
    {
        if (isOpen) Time.timeScale = 0;
        else Time.timeScale = 1;

        MenuActiveChanger();
    }

    private void ChangeOpenState()   // メニューを開いたり閉じたり
    {
        isOpen = !isOpen;
        DispMenu();

        ControlBGM();
    }
    private void ChangeOpenState(bool _isOpen)   // メニューを開くか閉じるか指定して処理を行う
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
