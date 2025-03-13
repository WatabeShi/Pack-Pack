using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowManager : MonoBehaviour
{
    //private FullScreenMode screenMode;
    private static bool isFullscreen;

    private int screenWidth;
    private int screenHeight;

    [SerializeField]
    private Dropdown dropdown;

    [Header("ウィンドウモード表示テキスト"), SerializeField]
    private Text screenModeText;

    [Header("ボタンクリック時のSE"), SerializeField]
    private AudioClip clickSE;

    private void Awake()
    {
        isFullscreen = Screen.fullScreen;

        screenWidth = Screen.width;
        screenHeight = Screen.height;

        ChangeScreenModeText();
    }

    private void Start()
    {
        //ChangeScreenModeText();
        //ChangeDropDown();
    }

    private void Update()
    {
        print(isFullscreen);
    }

    public void ChangeWindowMode()   // ウィンドウモードの切り替え
    {
        SESource.instance.PlaySE(clickSE);

        //if (screenMode == FullScreenMode.FullScreenWindow) screenMode = FullScreenMode.Windowed;
        //else screenMode = FullScreenMode.FullScreenWindow;

        isFullscreen = !isFullscreen;

        ChangeWindow();
        ChangeScreenModeText();
    }

    private void ChangeScreenModeText()
    {
        //if (screenMode == FullScreenMode.FullScreenWindow) screenModeText.text = "Full Screen";
        //else screenModeText.text = "Window";

        if (isFullscreen) screenModeText.text = "Full Screen";
        else screenModeText.text = "Window";
    }

    public void ChangeResolution()
    {
        SESource.instance.PlaySE(clickSE);

        // 選択されたオプションに応じて解像度変更
        if (dropdown.value == 0)
        {
            screenWidth = 3840;
            screenHeight = 2160;
        }
        else if (dropdown.value == 1)
        {
            screenWidth = 1920;
            screenHeight = 1080;
        }
        else
        {
            screenWidth = 1280;
            screenHeight = 720;
        }

        ChangeWindow();
    }

    private void ChangeDropDown()   // 解像度ドロップダウンの値の変更
    {
        int changedNum = 0;

        if (screenWidth == 1920 && screenHeight == 1080) changedNum = 1;
        else if(screenWidth == 1280 && screenHeight == 720) changedNum = 2;

        dropdown.value = changedNum;
    }

    public void ChangeHD()   // 画質をHDへ
    {
        screenWidth = 1280;
        screenHeight = 720;

        ChangeWindow();
    }

    public void ChangeFullHD()   // 画質をフルHDへ
    {
        screenWidth = 1920;
        screenHeight = 1080;

        ChangeWindow();
    }

    public void Change4K()   // 画質を4Kへ
    {
        screenWidth = 3840;
        screenHeight = 2160;

        ChangeWindow();
    }

    private void ChangeWindow()   // 画面状態の変更
    {
        Screen.SetResolution(screenWidth, screenHeight, isFullscreen);
    }
}
