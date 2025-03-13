using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsultShotGun : ShotGun
{
    // Update is called once per frame
    void Update()
    {
        Shot();
    }

    protected override void LaunchAmmo()
    {
        for (int i = 0; i < ammoToFirst; i++)
        {
            float rangeX = Random.Range(0.4f, 0.6f);
            float rangeY = Random.Range(0.4f, 0.6f);
            Vector2 screenCenter = new Vector2(rangeX, rangeY);
            Ray ray = Camera.main.ViewportPointToRay(screenCenter);   // ���C����ʒ��������΂�

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, shotDistance))
            {
                float displayTime = shotDelayTimer;   // �f�o�b�O���C�L���X�g�̕\������
                Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, displayTime);

                GenerateHitEffect(hit);

                if (hit.collider.CompareTag("Enemy")) hit.collider.gameObject.GetComponent<EnemyBaseFPS>().Damage(damage);   // �_���[�W��^����
            }
        }

        ShotEffecter();

        // �^�C�}�[�����Z�b�g
        shotDelayTimer = 0;
    }

    private void OnDisable()
    {
        DestroyMuzzleFlash();
    }
}
