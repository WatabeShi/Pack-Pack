using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    [SerializeField]
    private bool isOpen = false;   // 表示切り替えフラグ

    [Header("メイン画面"), SerializeField]
    private GameObject mainScreen;
    [Header("オプション画面"), SerializeField]
    private GameObject optionsScreen;   // オプション画面

    [Header("ボタンクリック時のSE"), SerializeField]
    private AudioClip clickSE;

    //[Header("オプションUIの親オブジェクト"), SerializeField]
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
