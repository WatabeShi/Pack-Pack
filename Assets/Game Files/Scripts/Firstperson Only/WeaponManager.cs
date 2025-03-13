using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("����g�p�\����"), SerializeField]
    private float useTime = 5;
    private float usingTimer = 0;
    private Coroutine usingCoroutin;

    [Header("�g�p�ł��镐��"), SerializeField]
    private Gun[] weapons;
    private GameObject[] equipedWeapons;   // �g�p���̕���̔z��

    [Header("�������������ꏊ"), SerializeField]
    private Transform weaponSocket;

    [Header("����g�p����BGM"), SerializeField]
    private AudioClip weaponTimeBGM;
    private bool isPlayWeaponTimeBGM = false;   // BGM���Đ�����

    [Header("�ʏ펞��BGM"), SerializeField]
    private AudioClip normalBGM;

    // �`�[�g�p(���펩�R����)�ϐ�
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
//        // �`�[�g���[�h�̐؂�ւ�
//        if (Input.GetMouseButton(2)) nowUseCheat = true;
//        else nowUseCheat = false;

//        Cheat();
//#endif
//    }

    private void InitWeapon()   // ����̏�����
    {
        var equipPoint = weaponSocket.position;

        for (int i = 0; i < weapons.Length; i++)
        {
            equipedWeapons[i] = Instantiate(weapons[i].gameObject, equipPoint, weapons[i].transform.rotation);
            equipedWeapons[i].transform.parent = weaponSocket.transform;
        }

        DisableWeapons();   //�@����̔�\��
    }

    public void EquipWeapon()   // ����̑���
    {
        bool isActive;
        int weaponNum = SelectWeapon();

        for (int i = 0; i < weapons.Length; i++)
        {
            // �I�����ꂽ����̂ݕ\��
            isActive = (i == weaponNum) ? true : false;
            equipedWeapons[i].gameObject.SetActive(isActive);
        }

        if(!isPlayWeaponTimeBGM)   // ����^�C��BGM���������Ă��Ȃ��ꍇ
        {
            BGMSource.instance.StopBGM(0.5f);

            Invoke("PlayUseBGM", 0.5f);
        }

        RenderSettings.fog = false;
    }

    private void EquipWeapon(int _weaponNum)   // �w�肵������𑕔��i�`�[�g�p�j
    {
        bool isActive;
        int weaponNum = _weaponNum;

        for (int i = 0; i < weapons.Length; i++)
        {
            // �I�����ꂽ����̂ݕ\��
            isActive = (i == weaponNum) ? true : false;
            equipedWeapons[i].gameObject.SetActive(isActive);
        }

        PlayUseBGM();

        RenderSettings.fog = false;
    }

    public void DisableWeapons()   // ����̔�\��
    {
        for (int i = 0; i < equipedWeapons.Length; i++) equipedWeapons[i].gameObject.SetActive(false);

        BGMSource.instance.StopBGM(0.5f);
        Invoke("ResetBGM", 0.65f);
        isPlayWeaponTimeBGM = false;

        RenderSettings.fog = true;
    }

    private void PlayUseBGM()   // ����g�p����BGM�Đ�
    {
        if (isPlayWeaponTimeBGM) return;   //BGM�����ɍĐ����̏ꍇ�I��

        var startPos = 58;   // �Đ��J�n�ʒu
        var fadeTime = 1;    // �t�F�[�h����

        BGMSource.instance.PlayBGM(weaponTimeBGM, startPos, fadeTime);   // BGM�Đ�

        isPlayWeaponTimeBGM = true;   // �Đ����t���O�̗L����
    }

    private void ResetBGM()
    {
        BGMSource.instance.PlayBGM(normalBGM);
    }

    public void StartPowerUp()   // �R���[�`���̋N��
    {
        if (usingCoroutin != null) StopCoroutine(usingCoroutin);   // �R���[�`�������ɋN�����̏ꍇ��~

        usingCoroutin = StartCoroutine(UseWeaponTime());

        //if (usingTimer > 0) usingTimer = useTime;   // �^�C�}�[�Z�b�g
        //else StartCoroutine(UseWeaponTime());
    }

    private IEnumerator UseWeaponTime()   // ����̎g�p�J�n����I���܂ł̏���
    {
        EquipWeapon();
        usingTimer = useTime;   // �^�C�}�[�Z�b�g

        while(usingTimer > 0)
        {
            usingTimer -= Time.deltaTime;   // �J�E���g�_�E��

            yield return null;
        }

        DisableWeapons();
        usingCoroutin = null;
    }

    private int SelectWeapon()   // �������镐������߂�
    {
        return Random.Range(0, weapons.Length);
    }

#if UNITY_EDITOR
    private void Cheat()   // �`�[�g���[�h�i�w�肵���ԍ��̕���𐶐��j
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
