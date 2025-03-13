using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotationer : MonoBehaviour
{
    [Header("‰ñ“]‘¬“x"), SerializeField]
    private Vector3 rotateSpeed = Vector3.zero;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(rotateSpeed * Time.deltaTime);
    }
}
