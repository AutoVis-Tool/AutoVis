using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteriorHeatmaps : MonoBehaviour
{


    public List<(string, Vector4)> Points = new List<(string, Vector4)>();

    private List<List<Vector4>> InteriorDataContainer = new List<List<Vector4>>();



    public List<Heatmap> InteriorHeatmapContainer = new List<Heatmap>();

    private List<Vector4> InteriorList = new List<Vector4>();
    private List<Vector4> InteriorDisplayList = new List<Vector4>();
    private List<Vector4> InteriorSteeringWheelList = new List<Vector4>();
    private List<Vector4> InteriorWindowsList = new List<Vector4>();


    private List<Vector4> PropertiesList = new List<Vector4>();

    public Shader shader;

    public List<Texture> partTextures = new List<Texture>();


    void Awake()
    {
        InteriorDataContainer.Add(InteriorList);
        InteriorDataContainer.Add(InteriorDisplayList);
        InteriorDataContainer.Add(InteriorSteeringWheelList);
        InteriorDataContainer.Add(InteriorWindowsList);
    }

    // Start is called before the first frame update
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupTextures(int index, List<(string, Vector4)> points)
    {

        Points = points;
        for (int i = 0; i < InteriorHeatmapContainer.Count; i++)
        {

            Material mat = new Material(shader);
            mat.SetTexture("_HeatTex", partTextures[index]);
            InteriorHeatmapContainer[i].material = mat;
            InteriorHeatmapContainer[i].gameObject.GetComponent<Renderer>().material = mat;
            if (i == 2)
            {
                Material[] arr = { mat, mat, mat };
                //Material mat2 = new Material(shader);
                //Material mat3 = new Material(shader);
                //mat2.SetTexture("_HeatTex", partTextures[index]);
                //mat3.SetTexture("_HeatTex", partTextures[index]);
                InteriorHeatmapContainer[i].gameObject.GetComponent<MeshRenderer>().materials = arr;
                //InteriorHeatmapContainer[i].gameObject.GetComponent<MeshRenderer>().materials[1] = mat3;
            }
            SetupDefaultShaderSize(InteriorHeatmapContainer[i]);

        }

    }

    private void SetupDefaultShaderSize(Heatmap heatmap)
    {
        Vector4[] arr = new Vector4[100];
        Debug.Log(heatmap.material.GetTexture("_HeatTex"));
        heatmap.material.SetInt("_Points_Length", 100);
        heatmap.material.SetVectorArray("_Points", arr);
        heatmap.material.SetVectorArray("_Properties", arr);
    }

    public void DrawInteriorHeatmap(int index)
    {
        int NumberOfHeatmaps = 100;
        InteriorList.Clear();
        InteriorSteeringWheelList.Clear();
        InteriorWindowsList.Clear();
        InteriorDisplayList.Clear();


        PropertiesList.Clear();
        int loop = 0;
        if (index > NumberOfHeatmaps)
        {
            loop = index - NumberOfHeatmaps;
        }
        else
        {
            loop = 0;
        }
        for (int i = loop; i < index; i++)
        {
            //Debug.Log("Drawinterior loop" + loop + " index " + index + " points item1 " + Points[i].Item1 + " points item 2 " + Points[i].Item2);
            Heatmap changeHeatmapGameobject = InteriorHeatmapContainer[returnCorrectHeatmap(Points[i].Item1)];

            Vector3 coords = changeHeatmapGameobject.gameObject.transform.position;
            Quaternion rot = changeHeatmapGameobject.gameObject.transform.rotation;
            // Vector4 newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles - new Vector3(359.98699951171875f, 1.8998982906341553f, 0f));
            Vector3 positionSinglePoint = Points[i].Item2;
            Vector4 newPos;
            if (changeHeatmapGameobject.gameObject.name == "InteriorSteeringWheel")
            {
                newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords - new Vector3(-0.373954058f, 1.04961121f, 0.474320441f), coords, rot.eulerAngles - new Vector3(23.3007126f, 0, 0));
            }
            else
            {
                newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles);
            }

            returnCorrectList(Points[i].Item1).Add(newPos);
            PropertiesList.Add(new Vector4(0.05f, 1f));
        }

        if (InteriorSteeringWheelList.Count != 0)
        {
            // Debug.Log("HIER JETZT");
            // Debug.Log(InteriorSteeringWheelList[InteriorSteeringWheelList.Count - 1]);
        }


        // Debug.Log(InteriorDataContainer.Count);
        // Debug.Log(InteriorDataContainer[0].Count);
        for (int i = 0; i < InteriorDataContainer.Count; i++)
        {
            if (InteriorDataContainer[i].Count > 0)
            {

                Material material = InteriorHeatmapContainer[i].material;

                material.SetInt("_Points_Length", InteriorDataContainer[i].ToArray().Length);
                material.SetVectorArray("_Points", InteriorDataContainer[i].ToArray());
                material.SetVectorArray("_Properties", PropertiesList.ToArray());
            }
            else
            {
                Material material = InteriorHeatmapContainer[i].material;
                // Vector4[] arr = { new Vector4(0, 0, 0, 0) };
                material.SetInt("_Points_Length", 0);
                // material.SetVectorArray("_Points", arr);
                // material.SetVectorArray("_Properties", arr);
            }

        }



    }

    Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        Vector3 dir = point - pivot; // get point direction relative to pivot
        dir = Quaternion.Euler(angles) * dir; // rotate it
        point = dir + pivot; // calculate rotated point
        return point; // return it
    }


    int returnCorrectHeatmap(string name)
    {
        switch (name)
        {
            case "InteriorDisplay":
                return 1;

            case "Interior":
                return 0;

            case "InteriorWindows":
                return 3;

            case "InteriorSteeringWheel":
                return 2;

            default:
                return 0;
        }
    }

    List<Vector4> returnCorrectList(string name)
    {
        switch (name)
        {
            case "InteriorDisplay":
                return InteriorDisplayList;

            case "Interior":
                return InteriorList;

            case "InteriorWindows":
                return InteriorWindowsList;

            case "InteriorSteeringWheel":
                return InteriorSteeringWheelList;

            default:
                return InteriorList;
        }
    }

}
