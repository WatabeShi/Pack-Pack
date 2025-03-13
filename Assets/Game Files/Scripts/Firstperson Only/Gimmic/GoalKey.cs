using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKey : MonoBehaviour
{
    [Header("���肵������SE"), SerializeField]
    private AudioClip getSE;

    [Header("��]���x"), SerializeField]
    private float rotateSpeed = 0;

    private void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);   // ���̉�]
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        SESource.instance.PlaySE(getSE);
    }
}
