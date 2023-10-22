using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCam : MonoBehaviour
{
    private Transform MainCam;
    private void Awake()
    {
        MainCam = Camera.main.transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(MainCam);
    }
}
