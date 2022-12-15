using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchHeatmaps : MonoBehaviour
{


    public List<(string, Vector4)> TouchPoints = new List<(string, Vector4)>();

    private List<List<Vector4>> InteriorTouchDataContainer = new List<List<Vector4>>();



    public List<Heatmap> InteriorTouchHeatmapContainer = new List<Heatmap>();

    private List<Vector4> InteriorTouchList = new List<Vector4>();
    private List<Vector4> InteriorTouchDisplayList = new List<Vector4>();
    private List<Vector4> InteriorTouchSteeringWheelList = new List<Vector4>();
    private List<Vector4> InteriorTouchWindowsList = new List<Vector4>();


    private List<Vector4> PropertiesList = new List<Vector4>();

    public Shader shader;

    public List<Texture> partTexturesTouch = new List<Texture>();


    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        InteriorTouchDataContainer.Add(InteriorTouchList);
        InteriorTouchDataContainer.Add(InteriorTouchDisplayList);
        InteriorTouchDataContainer.Add(InteriorTouchSteeringWheelList);
        InteriorTouchDataContainer.Add(InteriorTouchWindowsList);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetupTextures(int index, List<(string, Vector4)> points)
    {

        TouchPoints = points;
        for (int i = 0; i < InteriorTouchHeatmapContainer.Count; i++)
        {

            Material mat = new Material(shader);
            mat.SetTexture("_HeatTex", partTexturesTouch[index]);
            InteriorTouchHeatmapContainer[i].material = mat;
            InteriorTouchHeatmapContainer[i].gameObject.GetComponent<Renderer>().material = mat;
            // if (i == 2)
            // {
            //     //Material mat2 = new Material(shader);
            //     //Material mat3 = new Material(shader);
            //     //mat2.SetTexture("_HeatTex", partTextures[index]);
            //     //mat3.SetTexture("_HeatTex", partTextures[index]);
            //     Material[] arr = { mat, mat, mat };
            //     InteriorHeatmapContainer[i].gameObject.GetComponent<MeshRenderer>().materials = arr;
            //     //InteriorHeatmapContainer[i].gameObject.GetComponent<MeshRenderer>().materials[1] = mat;
            // }
            SetupDefaultShaderSize(InteriorTouchHeatmapContainer[i]);
        }

    }

    private void SetupDefaultShaderSize(Heatmap heatmap)
    {
        Vector4[] arr = new Vector4[100];
        heatmap.material.SetInt("_Points_Length", 100);
        heatmap.material.SetVectorArray("_Points", arr);
        heatmap.material.SetVectorArray("_Properties", arr);
    }

    public void DrawTouchInteriorHeatmap(int index)
    {
        int NumberOfHeatmaps = 100;
        InteriorTouchList.Clear();
        InteriorTouchSteeringWheelList.Clear();
        InteriorTouchWindowsList.Clear();
        InteriorTouchDisplayList.Clear();

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
            Heatmap changeHeatmapGameobject = InteriorTouchHeatmapContainer[returnCorrectHeatmap(TouchPoints[i].Item1)];

            Vector3 coords = changeHeatmapGameobject.gameObject.transform.position;
            Quaternion rot = changeHeatmapGameobject.gameObject.transform.rotation;
            // Vector4 newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles - new Vector3(359.98699951171875f, 1.8998982906341553f, 0f));
            Vector3 positionSinglePoint = TouchPoints[i].Item2;
            Vector3 newPos;
            if (changeHeatmapGameobject.gameObject.name == "InteriorSteeringWheel")
            {
                newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords - new Vector3(-0.373954058f, 1.04961121f, 0.474320441f), coords, rot.eulerAngles - new Vector3(23.3007126f, 0, 0));
            }
            else
            {
                newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles);
            }
            returnCorrectList(TouchPoints[i].Item1).Add(newPos);
            PropertiesList.Add(new Vector4(0.05f, 1f));
        }


        for (int i = 0; i < InteriorTouchDataContainer.Count; i++)
        {
            if (InteriorTouchDataContainer[i].Count > 0)
            {
                //              Debug.Log(InteriorDataContainer[i][0]);
                Material material = InteriorTouchHeatmapContainer[i].material;
                material.SetInt("_Points_Length", InteriorTouchDataContainer[i].ToArray().Length);
                material.SetVectorArray("_Points", InteriorTouchDataContainer[i].ToArray());
                material.SetVectorArray("_Properties", PropertiesList.ToArray());
            }
            else
            {
                if (InteriorTouchHeatmapContainer[i].material != null)
                {
                    Material material = InteriorTouchHeatmapContainer[i].material;
                    // Vector4[] arr = { new Vector4(0, 0, 0, 0) };
                    material.SetInt("_Points_Length", 0);
                }

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
                return InteriorTouchDisplayList;

            case "Interior":
                return InteriorTouchList;

            case "InteriorWindows":
                return InteriorTouchWindowsList;

            case "InteriorSteeringWheel":
                return InteriorTouchSteeringWheelList;

            default:
                return InteriorTouchList;
        }
    }



}
