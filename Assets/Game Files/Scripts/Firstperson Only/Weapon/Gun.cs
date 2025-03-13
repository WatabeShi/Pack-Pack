using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("�_���[�W"), SerializeField]
    protected int damage = 0;

    [Header("�A�ˊԊu�i���ԁj"), SerializeField]
    protected float shotDelay = 0;
    protected float shotDelayTimer = 0;

    [Header("�˒�����"), SerializeField]
    protected float shotDistance = 0;

    [Header("�ˌ��{�^��"), SerializeField]
    KeyCode shotKey = KeyCode.None;

    [Header("�I�[�g�}�`�b�N�ɂ���t���O"), SerializeField]
    private bool isAuto = false;

    [Header("�ˌ�SE"), SerializeField]
    private AudioClip shotSE;

    [Header("�����G�t�F�N�g"), SerializeField]
    protected GameObject hitEffect;
    [Header("�ˌ��G�t�F�N�g"), SerializeField]
    private GameObject shotEffect;
    [Header("�ˌ��G�t�F�N�g���o���ꏊ"), SerializeField]
    private Transform muzzle;
    private GameObject muzzleSave;   // �ˌ��G�t�F�N�g�ۑ��p

    // Start is called before the first frame update
    protected void Start()
    {
        shotDelayTimer = shotDelay;   // �^�C�}�[������
    }

    protected void Shot()   // �ˌ��^�C�v�ɉ����ď����̎d����ύX
    {
        if (Time.timeScale == 0) return;

        DelayCount();

        if (!CanShot()) return;   // �ˌ��ł��Ȃ���Ԃ̏ꍇ�I��

        if (isAuto)
        {
            if(Input.GetKey(shotKey)) LaunchAmmo();
        }
        else
        {
            if (Input.GetKeyDown(shotKey)) LaunchAmmo();
        }
    }

    protected virtual void LaunchAmmo()   // �e�̔��ˋ@�\
    {
        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Ray ray = Camera.main.ViewportPointToRay(screenCenter);   // ���C����ʒ��������΂�
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shotDistance))
        {
            float displayTime = 3f;   // �f�o�b�O���C�L���X�g�̕\������
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, displayTime);

            GenerateHitEffect(hit);   // ���e���̃G�t�F�N�g

            if (hit.collider.CompareTag("Enemy")) hit.collider.gameObject.GetComponent<EnemyBaseFPS>().Damage(damage);   // �_���[�W��^����
        }

        ShotEffecter();

        // �^�C�}�[�����Z�b�g
        shotDelayTimer = 0;
    }

    protected void ShotEffecter()   // �ˌ�����SE��G�t�F�N�g������
    {
        // SE�Đ�
        SESource.instance.PlaySE(shotSE);

        // �G�t�F�N�g�쐬
        muzzleSave = Instantiate(shotEffect, muzzle.position, muzzle.rotation);
        muzzleSave.transform.SetParent(muzzle);
    }

    protected void GenerateHitEffect(RaycastHit _hit)
    {
        float hitRayPosition = 0.5f;   // �����n�_�ɂ߂荞�܂Ȃ����߂̕ϐ�
        Instantiate(hitEffect, _hit.point + (_hit.normal * hitRayPosition), Quaternion.identity);   // �G�t�F�N�g����
    }

    private void DelayCount()   // �ˌ��Ԋu�̃J�E���g
    {
        if (!CanShot()) shotDelayTimer += Time.deltaTime;
    }

    private bool CanShot()   // �ˌ��ł����Ԃ�����
    {
        if (shotDelayTimer >= shotDelay) return true;   // �^�C�}�[���ݒ莞�Ԃ��z���Ă����� true
        else return false;
    }

    protected void DestroyMuzzleFlash()
    {
        Destroy(muzzleSave);
    }

    private void OnDisable()
    {
        DestroyMuzzleFlash();
    }
}
