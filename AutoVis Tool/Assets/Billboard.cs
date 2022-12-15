using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private GameObject vrCam;
    private void Awake()
    {
        vrCam = GameObject.Find("Camera");
    }
    void Update()
    {
        transform.LookAt(vrCam.transform.position, -Vector3.up);
        transform.LookAt(2 * transform.position - vrCam.transform.position);
    }
}