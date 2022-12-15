// Alan Zucconi
// www.alanzucconi.com
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

using UnityEngine.UI;
using Replay;

public class Heatmap : MonoBehaviour
{

    public Vector4[] positions;
    public float[] radiuses;
    public float[] intensities;

    public Material material;

    public List<Vector3> localpositions = new List<Vector3>();
    public List<Vector3> rotationpositions = new List<Vector3>();


    private int count = 100;

    void Awake()
    {
        // positions = new Vector4[count];
        // radiuses = new float[count];
        // intensities = new float[count];
        // Vector4[] arr = new Vector4[100];
        // material.SetInt("_Points_Length", 100);
        // material.SetVectorArray("_Points", arr);
        // material.SetVectorArray("_Properties", arr);


    }

    void Start()
    {

        // TODO Disabled for now
        // Vector4[] arr = new Vector4[100];
        // material.SetInt("_Points_Length", 100);
        // material.SetVectorArray("_Points", arr);
        // material.SetVectorArray("_Properties", arr);





        // for (int i = 0; i < positions.Length; i++)
        // {

        //positions[i] = new Vector3(Random.Range(-0.4f, +0.4f), Random.Range(-0.4f, +0.4f), 1);
        // Vector3 coords = GameObject.Find("Quad").transform.position;
        //Vector3 coords = this.transform.position;
        //positions[i] = new Vector3(0.6f, Random.Range(+1.0f, +1.3f), Random.Range(-1.0f, +1.0f));
        //positions[i] = new Vector3(0.6f, 1f, 0f);
        // radiuses[i] = Random.Range(0f, 0.25f);
        // intensities[i] = Random.Range(-0.25f, 1f);
        //localpositions.Add(positions[i]);
        // rotationpositions.Add(coords.)
        //if (i < 30)
        //{
        //    positions[i] = new Vector3(Random.Range(-0.4f, +0.4f), Random.Range(-0.4f, +0.4f), 1);
        //    radiuses[i] = Random.Range(0f, 0.25f);
        //    intensities[i] = Random.Range(-0.25f, 1f);
        //} else if(i < 40)
        //{
        //    positions[i] = new Vector3(Random.Range(-0.4f, +0.4f), 1, Random.Range(-0.4f, +0.4f));
        //    radiuses[i] = Random.Range(0f, 0.25f);
        //    intensities[i] = Random.Range(-0.25f, 1f);
        //} else
        //{
        //    positions[i] = new Vector3(Random.Range(-0.4f, +0.4f), 1, Random.Range(-0.4f, +0.4f));
        //    radiuses[i] = Random.Range(0f, 0.25f);
        //    intensities[i] = Random.Range(-0.25f, 1f);
        //}

        //radiuses[i] = 1f;
        //intensities[i] = 1f;
        // }
        //Debug.Log(positions[0] + ";  " + radiuses[0] + ";  " + intensities[0]);
    }

    void Update()
    {
        // material.SetInt("_Points_Length", positions.Length);
        // Vector4[] properties = new Vector4[count];
        // // Vector3 coords = GameObject.Find("Quad").transform.position;
        // Vector3 coords = this.transform.position;
        // Quaternion rot = this.transform.rotation;
        // for (int i = 0; i < positions.Length; i++)
        // {
        //     //positions[i] += new Vector4(Random.Range(-0.5f, +0.5f) - coords.x, Random.Range(-0.5f, +0.5f) - coords.y) * Time.deltaTime;
        //     //positions[i] = new Vector3(coords.x, Random.Range(-0.4f, +0.4f) + coords.y, Random.Range(-0.4f, +0.4f) + coords.z);
        //     //positions[i] += new Vector4(0, Random.Range(-0.5f, +0.5f), Random.Range(-0.5f, +0.5f)) * Time.deltaTime;
        //     //positions[i] += new Vector4(0, positions[i].y, positions[i].z);
        //     //            Debug.Log(rot.eulerAngles);
        //     //positions[i] = (Vector4) ((localpositions[i] + coords));
        //     positions[i] = (Vector4)RotatePointAroundPivot(localpositions[i] + coords, coords, rot.eulerAngles - new Vector3(0f, +90.6f, 0f));

        //     properties[i] = new Vector4(radiuses[i], intensities[i]);
        // }
        // material.SetVectorArray("_Points", positions);
        // material.SetVectorArray("_Properties", properties);
        //Debug.Log(positions[positions.Length - 1]);
    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }
}