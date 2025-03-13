using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("ダメージ"), SerializeField]
    protected int damage = 0;

    [Header("連射間隔（時間）"), SerializeField]
    protected float shotDelay = 0;
    protected float shotDelayTimer = 0;

    [Header("射程距離"), SerializeField]
    protected float shotDistance = 0;

    [Header("射撃ボタン"), SerializeField]
    KeyCode shotKey = KeyCode.None;

    [Header("オートマチックにするフラグ"), SerializeField]
    private bool isAuto = false;

    [Header("射撃SE"), SerializeField]
    private AudioClip shotSE;

    [Header("命中エフェクト"), SerializeField]
    protected GameObject hitEffect;
    [Header("射撃エフェクト"), SerializeField]
    private GameObject shotEffect;
    [Header("射撃エフェクトを出す場所"), SerializeField]
    private Transform muzzle;
    private GameObject muzzleSave;   // 射撃エフェクト保存用

    // Start is called before the first frame update
    protected void Start()
    {
        shotDelayTimer = shotDelay;   // タイマー初期化
    }

    protected void Shot()   // 射撃タイプに応じて処理の仕方を変更
    {
        if (Time.timeScale == 0) return;

        DelayCount();

        if (!CanShot()) return;   // 射撃できない状態の場合終了

        if (isAuto)
        {
            if(Input.GetKey(shotKey)) LaunchAmmo();
        }
        else
        {
            if (Input.GetKeyDown(shotKey)) LaunchAmmo();
        }
    }

    protected virtual void LaunchAmmo()   // 弾の発射機構
    {
        Vector2 screenCenter = new Vector2(0.5f, 0.5f);
        Ray ray = Camera.main.ViewportPointToRay(screenCenter);   // レイを画面中央から飛ばす
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, shotDistance))
        {
            float displayTime = 3f;   // デバッグレイキャストの表示時間
            Debug.DrawRay(ray.origin, ray.direction * shotDistance, Color.red, displayTime);

            GenerateHitEffect(hit);   // 着弾時のエフェクト

            if (hit.collider.CompareTag("Enemy")) hit.collider.gameObject.GetComponent<EnemyBaseFPS>().Damage(damage);   // ダメージを与える
        }

        ShotEffecter();

        // タイマーをリセット
        shotDelayTimer = 0;
    }

    protected void ShotEffecter()   // 射撃時のSEやエフェクト発生等
    {
        // SE再生
        SESource.instance.PlaySE(shotSE);

        // エフェクト作成
        muzzleSave = Instantiate(shotEffect, muzzle.position, muzzle.rotation);
        muzzleSave.transform.SetParent(muzzle);
    }

    protected void GenerateHitEffect(RaycastHit _hit)
    {
        float hitRayPosition = 0.5f;   // 命中地点にめり込まないための変数
        Instantiate(hitEffect, _hit.point + (_hit.normal * hitRayPosition), Quaternion.identity);   // エフェクト生成
    }

    private void DelayCount()   // 射撃間隔のカウント
    {
        if (!CanShot()) shotDelayTimer += Time.deltaTime;
    }

    private bool CanShot()   // 射撃できる状態か判定
    {
        if (shotDelayTimer >= shotDelay) return true;   // タイマーが設定時間を越えていたら true
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
