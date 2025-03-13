using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKey : MonoBehaviour
{
    [Header("“üŽè‚µ‚½Žž‚ÌSE"), SerializeField]
    private AudioClip getSE;

    [Header("‰ñ“]‘¬“x"), SerializeField]
    private float rotateSpeed = 0;

    private void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);   // Œ®‚Ì‰ñ“]
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        SESource.instance.PlaySE(getSE);
    }
}
