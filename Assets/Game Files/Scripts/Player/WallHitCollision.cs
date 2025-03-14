using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHitCollision : MonoBehaviour
{
    private bool isHit = false;   // 壁に触れているかの判定

    private bool IsHitWall(Collider _other)   // 壁に触れたか判定
    {
        return _other.gameObject.CompareTag("Wall");
    }

    private void OnTriggerStay(Collider other)
    {
        // 壁に触れたら先の処理へ
        if (!IsHitWall(other)) return;

        isHit = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // 壁に離れたら先の処理へ
        if (!IsHitWall(other)) return;

        isHit = false;
    }

    public bool IsHit
    {
        get { return isHit; }
    }
}
