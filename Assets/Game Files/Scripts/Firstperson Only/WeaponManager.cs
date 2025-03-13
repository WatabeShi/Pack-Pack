using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("武器使用可能時間"), SerializeField]
    private float useTime = 5;
    private float usingTimer = 0;
    private Coroutine usingCoroutin;

    [Header("使用できる武器"), SerializeField]
    private Gun[] weapons;
    private GameObject[] equipedWeapons;   // 使用中の武器の配列

    [Header("武器を持たせる場所"), SerializeField]
    private Transform weaponSocket;

    [Header("武器使用中のBGM"), SerializeField]
    private AudioClip weaponTimeBGM;
    private bool isPlayWeaponTimeBGM = false;   // BGMを再生中か

    [Header("通常時のBGM"), SerializeField]
    private AudioClip normalBGM;

    // チート用(武器自由生成)変数
    private int selectNum = 10;
    private bool nowUseCheat = false;

    private void Awake()
    {
        equipedWeapons = new GameObject[weapons.Length];
    }

    // Start is called before the first frame update
    void Start()
    {
        InitWeapon();
    }

    // Update is called once per frame
//    void Update()
//    {
//        print(isPlayBGM);

//#if UNITY_EDITOR
//        // チートモードの切り替え
//        if (Input.GetMouseButton(2)) nowUseCheat = true;
//        else nowUseCheat = false;

//        Cheat();
//#endif
//    }

    private void InitWeapon()   // 武器の初期化
    {
        var equipPoint = weaponSocket.position;

        for (int i = 0; i < weapons.Length; i++)
        {
            equipedWeapons[i] = Instantiate(weapons[i].gameObject, equipPoint, weapons[i].transform.rotation);
            equipedWeapons[i].transform.parent = weaponSocket.transform;
        }

        DisableWeapons();   //　武器の非表示
    }

    public void EquipWeapon()   // 武器の装備
    {
        bool isActive;
        int weaponNum = SelectWeapon();

        for (int i = 0; i < weapons.Length; i++)
        {
            // 選択された武器のみ表示
            isActive = (i == weaponNum) ? true : false;
            equipedWeapons[i].gameObject.SetActive(isActive);
        }

        if(!isPlayWeaponTimeBGM)   // 武器タイムBGMがかかっていない場合
        {
            BGMSource.instance.StopBGM(0.5f);

            Invoke("PlayUseBGM", 0.5f);
        }

        RenderSettings.fog = false;
    }

    private void EquipWeapon(int _weaponNum)   // 指定した武器を装備（チート用）
    {
        bool isActive;
        int weaponNum = _weaponNum;

        for (int i = 0; i < weapons.Length; i++)
        {
            // 選択された武器のみ表示
            isActive = (i == weaponNum) ? true : false;
            equipedWeapons[i].gameObject.SetActive(isActive);
        }

        PlayUseBGM();

        RenderSettings.fog = false;
    }

    public void DisableWeapons()   // 武器の非表示
    {
        for (int i = 0; i < equipedWeapons.Length; i++) equipedWeapons[i].gameObject.SetActive(false);

        BGMSource.instance.StopBGM(0.5f);
        Invoke("ResetBGM", 0.65f);
        isPlayWeaponTimeBGM = false;

        RenderSettings.fog = true;
    }

    private void PlayUseBGM()   // 武器使用中のBGM再生
    {
        if (isPlayWeaponTimeBGM) return;   //BGMが既に再生中の場合終了

        var startPos = 58;   // 再生開始位置
        var fadeTime = 1;    // フェード時間

        BGMSource.instance.PlayBGM(weaponTimeBGM, startPos, fadeTime);   // BGM再生

        isPlayWeaponTimeBGM = true;   // 再生中フラグの有効化
    }

    private void ResetBGM()
    {
        BGMSource.instance.PlayBGM(normalBGM);
    }

    public void StartPowerUp()   // コルーチンの起動
    {
        if (usingCoroutin != null) StopCoroutine(usingCoroutin);   // コルーチンが既に起動中の場合停止

        usingCoroutin = StartCoroutine(UseWeaponTime());

        //if (usingTimer > 0) usingTimer = useTime;   // タイマーセット
        //else StartCoroutine(UseWeaponTime());
    }

    private IEnumerator UseWeaponTime()   // 武器の使用開始から終了までの処理
    {
        EquipWeapon();
        usingTimer = useTime;   // タイマーセット

        while(usingTimer > 0)
        {
            usingTimer -= Time.deltaTime;   // カウントダウン

            yield return null;
        }

        DisableWeapons();
        usingCoroutin = null;
    }

    private int SelectWeapon()   // 装備する武器を決める
    {
        return Random.Range(0, weapons.Length);
    }

#if UNITY_EDITOR
    private void Cheat()   // チートモード（指定した番号の武器を生成）
    {
        if (!nowUseCheat) return;

        if (Input.GetKey(KeyCode.Alpha1)) selectNum = 0;
        else if (Input.GetKey(KeyCode.Alpha2)) selectNum = 1;
        else if (Input.GetKey(KeyCode.Alpha3)) selectNum = 2;
        else if (Input.GetKey(KeyCode.Alpha4)) selectNum = 3;
        else if (Input.GetKey(KeyCode.Alpha5)) selectNum = 4;
        else if (Input.GetKey(KeyCode.Alpha6)) selectNum = 5;
        else if (Input.GetKey(KeyCode.Alpha7)) selectNum = 6;

        selectNum = (selectNum > equipedWeapons.Length) ? Mathf.Min(selectNum, equipedWeapons.Length) : selectNum;
        if (selectNum < equipedWeapons.Length) EquipWeapon(selectNum);
        else DisableWeapons();
    }
#endif

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
