using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallHitCollision : MonoBehaviour
{
    private bool isHit = false;   // •Ç‚ÉG‚ê‚Ä‚¢‚é‚©‚Ì”»’è

    private bool IsHitWall(Collider _other)   // •Ç‚ÉG‚ê‚½‚©”»’è
    {
        return _other.gameObject.CompareTag("Wall");
    }

    private void OnTriggerStay(Collider other)
    {
        // •Ç‚ÉG‚ê‚½‚çæ‚Ìˆ—‚Ö
        if (!IsHitWall(other)) return;

        isHit = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // •Ç‚É—£‚ê‚½‚çæ‚Ìˆ—‚Ö
        if (!IsHitWall(other)) return;

        isHit = false;
    }

    public bool IsHit
    {
        get { return isHit; }
    }
}
