using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpBait : BaitBase
{
    private PlayerBase player;
    private EnemyBase[] enemies;
    private WeaponManager weaponManager;

    protected new void Awake()
    {
        base.Awake();

        if (!isFPS) enemies = FindObjectsByType<EnemyBase>(FindObjectsSortMode.None);
        weaponManager = isFPS ? FindAnyObjectByType<WeaponManager>() : null;
    }

    // Start is called before the first frame update
    new void Start()
    {
        //base.Start();
        UpdateTarget();
    }

    public void UpdateTarget()
    {
        player = isFPS ? FindAnyObjectByType<PlayerBaseFPS>() : FindAnyObjectByType<PlayerBase>();
        player = FindAnyObjectByType<PlayerBase>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;   // プレイヤーが触れた場合先の処理を実行

        base.BaseProcess(true);

        player.StartPowerUp();

        if (isFPS)
        {
            weaponManager.StartPowerUp();
        }
        else
        {
            foreach (EnemyBase enemy in enemies)
            {
                enemy.StartIzike();
            }
        }
    }
}
