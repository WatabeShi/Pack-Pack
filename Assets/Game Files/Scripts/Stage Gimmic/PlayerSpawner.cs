using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("スポーンさせるプレイヤー"), SerializeField]
    private GameObject player;

    public void Spawn()
    {
        Instantiate(player, this.transform.position, Quaternion.identity);
    }
}
