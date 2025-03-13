using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : Gun
{
    [Header("一度の射撃で発射する弾数"), Range(1, 99), SerializeField]
    protected int ammoToFirst = 1;

    [Header("ポンプアクションSE"), SerializeField]
    private AudioClip pumpactionSE;

    protected override void LaunchAmmo()
    {
        for(int i = 0; i < ammoToFirst; i++) 
        {
            float rangeX = Random.Range(0.4f, 0.6f);
            float rangeY = Random.Range(0.4f, 0.6f);
            Vector2 screenCenter = new Vector2(rangeX, rangeY);
            Ray ray = Camera.main.ViewportPointToRay(screenCenter);   // レイを画面中央から飛ばす

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, shotDistance))
            {
                float displayTime = shotDelayTimer;   // デバッグレイキャストの表示時間
                Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, displayTime);

                GenerateHitEffect(hit);   // ヒット地点にエフェクト生成

                if (hit.collider.CompareTag("Enemy")) hit.collider.gameObject.GetComponent<EnemyBaseFPS>().Damage(damage);   // ダメージを与える
            }
        }

        ShotEffecter();   // マズルフラッシュ生成

        Invoke("PlayPumpActionSE", shotDelay - 0.5f);

        // タイマーをリセット
        shotDelayTimer = 0;
    }

    private void PlayPumpActionSE()
    {
        SESource.instance.PlaySE(pumpactionSE);
    }

    // Update is called once per frame
    void Update()
    {
        Shot();
    }

    private void OnDisable()
    {
        DestroyMuzzleFlash();
    }
}
