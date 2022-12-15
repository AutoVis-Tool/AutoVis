using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Wrapper for Transform
/// By default Unity can't store <see cref="Transform"/> directly. Thus, this wrapper exists
/// </summary>
[System.Serializable]
[Obsolete]
public class SerializedTransform {

    public float[] position = new float[3];
    public float[] rotation = new float[3];
    public float[] scale = new float[3];


    public SerializedTransform() {} //This empty constructor exists to stop the JToken.ToObject<Snapshot>() from throwing nullpointers because it doesn't have a "transform"

    public SerializedTransform(Transform transform) {

        position[0] = transform.position.x;
        position[1] = transform.position.y;
        position[2] = transform.position.z;


        rotation[0] = transform.localRotation.x;
        rotation[1] = transform.localRotation.y;
        rotation[2] = transform.localRotation.z;


        scale[0] = transform.localScale.x;
        scale[1] = transform.localScale.y;
        scale[2] = transform.localScale.z;
    }

}
