using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AvatarMerger : MonoBehaviour
{
    public GameObject Avatar1;
    public GameObject Avatar2;
    public GameObject Avatar3;

    public GameObject MergedAvatar;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotations(MergedAvatar.transform, Avatar1.transform, Avatar2.transform, Avatar3.transform);
    }


    public void UpdateRotations(Transform merge, Transform a1, Transform a2, Transform a3)
    {
        int children = merge.childCount;
        merge.localRotation = calcAvg(a1.localRotation, a2.localRotation, a3.localRotation);

        for (int i = 0; i < children; ++i) {
            UpdateRotations(merge.GetChild(i), a1.GetChild(i), a2.GetChild(i), a3.GetChild(i));
        }
    }



    private Quaternion calcAvg(Quaternion a1, Quaternion a2, Quaternion a3)
    {
        

        float x, y, z, w;

        x = a1.x + a2.x + a3.x;
        y = a1.y + a2.y + a3.y;
        z = a1.z + a2.z + a3.z;
        w = a1.w + a2.w + a3.w;

        float k = 1.0f / Mathf.Sqrt(x * x + y * y + z * z + w * w);
        return new Quaternion(x * k, y * k, z * k, w * k);
    }
}
