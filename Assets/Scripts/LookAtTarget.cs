using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    // remember to covert euler angles to radians since Math.f gets radians as input
    public Camera mainCamera = null;
    public void LookTowards()
    {
        transform.position = new Vector3(Mathf.Sin((mainCamera.transform.eulerAngles.y * Mathf.PI) / 180) * 20, 0, Mathf.Cos((mainCamera.transform.eulerAngles.y * Mathf.PI) / 180) * 20);
        transform.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
    }
}
