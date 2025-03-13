using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
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
