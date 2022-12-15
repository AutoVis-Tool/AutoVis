using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using Replay;

public class MapBuilderCustom : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> Map;

    public GameObject buildCube;

    public List<string> Buildingid;

    public Material defaultMat;

    public List<List<Vector4>> PointsList = new List<List<Vector4>>();

    public List<Vector4> PropertiesList = new List<Vector4>();

    public Shader shader;

    public Texture texture;

    public List<GameObject> childrenOfGameobject = new List<GameObject>();

    public List<GameObject> childrenWithHeatmap = new List<GameObject>();

    public List<List<Vector4>> PointsOnHeatmap = new List<List<Vector4>>();

    public List<bool> CurrentlyPointingList = new List<bool>();
    public List<Vector3> PointsCustom = new List<Vector3>();

    public List<bool> pointing = new List<bool>();

    public Heatmap SanFranTower;

    public GameObject SanFranTowerGameobject;

    public GameObject CyclistGameobject;

    public List<Vector3> PointsCyclist = new List<Vector3>();


    //public JavascriptManager jsManager

    //public List<GameObject> 

    void Awake()
    {
        // disabled for now until new Map
        // foreach (Transform landscape in transform)
        // {
        //     foreach (Transform building in landscape.GetChild(5))
        //     {
        //         childrenOfGameobject.Add(building.gameObject);
        //     }
        // }



        foreach (Transform child in transform)
        {
            childrenOfGameobject.Add(child.gameObject);
        }

        childrenOfGameobject.Add(SanFranTowerGameobject);
        //asdadas();
        setupCustomList();
        SetupCyclistPoints();

    }
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    public void asdadas(GameObject map)
    {
        // for (int i = 0; i < childrenOfGameobject.Count; i++)

        // for (int i = 0; i < childrenOfGameobject.Count; i++)
        // {
        //     if (childrenOfGameobject[i].transform.childCount >= 1)
        //     {


        // GameObject heatmap = Instantiate(landscape.transform.GetChild(0).GetChild(0).gameObject, landscape.transform.GetChild(0));
        // heatmap.name = "Heatmap";
        // Heatmap heatmapofG = heatmap.AddComponent<Heatmap>() as Heatmap;

        for (int j = 0; j < map.transform.childCount; j++)
        {
            for (int k = 0; k < map.transform.GetChild(j).GetChild(5).childCount; k++)
            {
                GameObject heatmap = Instantiate(map.transform.GetChild(j).GetChild(5).GetChild(k).GetChild(0).gameObject, map.transform.GetChild(j).GetChild(5).GetChild(k));
                heatmap.name = "Heatmap";
                Heatmap heatmapofG = heatmap.AddComponent<Heatmap>() as Heatmap;
            }

            // heatmapofG.material = 

            // MeshFilter meshf = heatmap.AddComponent<MeshFilter>() as MeshFilter;
            // MeshRenderer meshr = heatmap.AddComponent<MeshRenderer>() as MeshRenderer;
            // meshf.mesh = childrenOfGameobject[i].transform.GetChild(j).gameObject.GetComponent<MeshFilter>().mesh;

        }


        // transform.GetChild(i).GetChild(1).GetComponent<MeshRenderer>().material = defaultMat;
        // transform.GetChild(i).GetChild(1).GetComponent<Heatmap>().material = defaultMat;
        //     }
        // }
    }


    //public void BuildCubes()
    //{
    //    Material m = new Material(defaultMat);
    //    AssetDatabase.CreateAsset(m, "Assets/SceneRecorder/Prefabs/Meshes/Materials/materialDefault.asset");
    //    m = AssetDatabase.LoadAssetAtPath("Assets/SceneRecorder/Prefabs/Meshes/Materials/materialDefault.asset", typeof(Material)) as Material;
    //    for (int i = 0; i < Map.Count; i++)
    //    {
    //        for (int j = 0; j < Map[i].transform.childCount; j++)
    //        {
    //            GameObject building = Map[i].transform.GetChild(j).GetChild(1).gameObject;
    //            if (building.GetComponent<MeshFilter>() != null)
    //            {
    //                Mesh meshCustom = building.GetComponent<MeshFilter>().mesh;
    //                //Mesh meshCustom = AssetDatabase.CreateAsset(buildingMesh, "./SceneRecorder/Prefabs/Meshes");

    //                GameObject buildingCube = Instantiate(buildCube);
    //                buildingCube.name = building.transform.parent.name;
    //                buildingCube.transform.parent = transform;
    //                if (Buildingid.Contains(building.transform.parent.name))
    //                {
    //                    AssetDatabase.CreateAsset(meshCustom, "Assets/SceneRecorder/Prefabs/Meshes/mesh" + j + ".asset");
    //                    meshCustom = AssetDatabase.LoadAssetAtPath("Assets/SceneRecorder/Prefabs/Meshes/mesh" + j + ".asset", typeof(Mesh)) as Mesh;
    //                    buildingCube.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh = meshCustom;
    //                    buildingCube.transform.GetChild(1).gameObject.GetComponent<MeshFilter>().mesh = meshCustom;


    //                    // for (int k = 0; k < meshCustom.subMeshCount; k++)
    //                    // {
    //                    //     buildingCube.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh.subMeshCount += 1;
    //                    //     buildingCube.transform.GetChild(0).gameObject.GetComponent<MeshFilter>().mesh.SetSubMesh(k, meshCustom.GetSubMesh(k));
    //                    //     buildingCube.transform.GetChild(1).gameObject.GetComponent<MeshFilter>().mesh.SetSubMesh(k, meshCustom.GetSubMesh(k));
    //                    //Material basic = buildingCube.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0];
    //                    // }
    //                    Material[] materialsMesh = new Material[meshCustom.subMeshCount];
    //                    for (int k = 0; k < meshCustom.subMeshCount; k++)
    //                    {
    //                        materialsMesh[k] = m;
    //                    }
    //                    buildingCube.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials = materialsMesh;
    //                    buildingCube.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().materials = materialsMesh;
    //                    buildingCube.transform.position = new Vector3(building.transform.parent.position.x, building.transform.parent.position.y, building.transform.parent.position.z);

    //                }
    //                else
    //                {
    //                    Vector3 oldPosition = meshCustom.bounds.center;
    //                    buildingCube.transform.position = new Vector3(building.transform.parent.position.x + oldPosition.x, building.transform.parent.position.y + oldPosition.y, building.transform.parent.position.z + oldPosition.z);
    //                    Vector3 oldSize = meshCustom.bounds.size;
    //                    buildingCube.transform.localScale = new Vector3(oldSize.x - 2, oldSize.y, oldSize.z - 2);
    //                    buildingCube.transform.rotation = new Quaternion(0, -0.0801989138f, 0, 0.996778905f);
    //                }



    //            }


    //        }
    //    }
    //}


    public void SetupTextures(List<Vector4> points)
    {
        for (int i = 0; i < points.Count; i++)
        {

            if (PointsList.Count < i + 1)
            {
                List<Vector4> newList = new List<Vector4>();
                newList.Add(points[i]);
                PointsList.Add(newList);
            }
            else
            {
                PointsList[i].Add(points[i]);
            }
        }



    }

    public void startCreatingMaterial()
    {
        for (int k = 0; k < PointsList.Count; k++)
        {

            for (int i = 0; i < PointsList[k].Count; i++)
            {
                if (!(PointsList[k][i].Equals(Vector4.zero)))
                {

                    Material mat = new Material(shader);
                    mat.SetTexture("_HeatTex", texture);

                    GameObject child = GetClosest(PointsList[k][i], childrenOfGameobject);

                    if (!childrenWithHeatmap.Contains(child))
                    {
                        childrenWithHeatmap.Add(child);
                    }


                    // if (child.name != "San Fran Tower")
                    // {
                    //   Debug.Log(child);
                    if (child.transform.childCount > 0)
                    {


                        GameObject heatmap = child.transform.GetChild(0).gameObject;
                        //JavaScriptManager.instanceJS.exteriorBuildings.Add(heatmap);
                        Heatmap heatmapofThis = heatmap.GetComponent<Heatmap>();
                        heatmapofThis.material = mat;
                        heatmap.GetComponent<MeshRenderer>().material = mat;

                        Vector4[] arr = new Vector4[100];
                        heatmapofThis.material.SetInt("_Points_Length", 100);
                        heatmapofThis.material.SetVectorArray("_Points", arr);
                        heatmapofThis.material.SetVectorArray("_Properties", arr);
                    }
                    // }



                }

            }
        }
        for (int i = 0; i < 100; i++)
        {
            PropertiesList.Add(new Vector4(0.3f, 1f));
        }

    }

    public void DrawExteriorBuildingHeatmap(int index)
    {
        int NumberOfHeatmaps = 100;
        PointsOnHeatmap.Clear();
        JavaScriptManager.instanceJS.exteriorBuildings = childrenWithHeatmap;
        for (int i = 0; i < childrenWithHeatmap.Count; i++)
        {
            List<Vector4> pointlist = new List<Vector4>();
            PointsOnHeatmap.Add(pointlist);
        }
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
            for (int j = 0; j < PointsList[i].Count; j++)
            {

                if (!(PointsList[i][j].Equals(Vector4.zero)))
                {

                    GameObject child = GetClosest(PointsList[i][j], childrenWithHeatmap);
                    // if (!childrenWithHeatmap.Contains(child))
                    // {
                    //     //childrenWithHeatmap.Add(child);
                    //     List<Vector4> pointlist = new List<Vector4>();
                    //     pointlist.Add(PointsList[i][j]);
                    //     PointsOnHeatmap.Add(pointlist);
                    // }
                    // else
                    // {

                    if(childrenWithHeatmap.IndexOf(child) >= 0)
                    {
                        if(child.name == "San Fran Tower")
                        {
                            PointsOnHeatmap[childrenWithHeatmap.IndexOf(child)].Add(PointsList[i][j] + new Vector4(18.29993f, 0f, -0.76001f, 0));
                        } else
                        {
                            PointsOnHeatmap[childrenWithHeatmap.IndexOf(child)].Add(PointsList[i][j]);
                        }
                    }

                    // }

                }

            }


        }

        for (int i = 0; i < childrenWithHeatmap.Count; i++)
        {
            if (childrenWithHeatmap[i].transform.childCount > 0)
            {
                Heatmap thisHeatmap = childrenWithHeatmap[i].transform.GetChild(0).GetComponent<Heatmap>();
                Material material = thisHeatmap.material;
                material.SetInt("_Points_Length", 0);
            }

        }



        for (int i = 0; i < PointsOnHeatmap.Count; i++)
        {
            if (PointsOnHeatmap[i].Count > 100)
            {

                PointsOnHeatmap[i].RemoveRange(0, PointsOnHeatmap[i].Count - 100);


            }
            if (PointsOnHeatmap[i].Count > 0)
            {
                Heatmap heatmap = childrenWithHeatmap[i].transform.GetChild(0).gameObject.GetComponent<Heatmap>();
                Material material = heatmap.material;
                material.SetInt("_Points_Length", PointsOnHeatmap[i].ToArray().Length);
                material.SetVectorArray("_Points", PointsOnHeatmap[i].ToArray());
                material.SetVectorArray("_Properties", PropertiesList.ToArray());
            }



        }
    }

    public GameObject GetClosest(Vector3 startPosition, List<GameObject> pickups)
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;

        foreach (GameObject potentialTarget in pickups)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - startPosition;

            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }

    public void setupCustomList()
    {
        for (int i = 0; i < 123; i++)
        {
            PointsCustom.Add(new Vector3(0f, 0f, 0f));
        }

        PointsCustom.Add(new Vector3(1913.53003f, 98.8000031f, 2052.71997f));
        PointsCustom.Add(new Vector3(1914.90002f, 98.8000031f, 2053.20996f));
        PointsCustom.Add(new Vector3(1916.27002f, 98.8000031f, 2053.3999f));
        PointsCustom.Add(new Vector3(1916.27002f, 98.3199997f, 2053.3999f));
        PointsCustom.Add(new Vector3(1918.18005f, 98.3199997f, 2053.5f));
        PointsCustom.Add(new Vector3(1919.31995f, 98.3199997f, 2053.55005f));
        PointsCustom.Add(new Vector3(1919.31995f, 99.6100006f, 2053.67993f));
        PointsCustom.Add(new Vector3(1918.51001f, 99.6100006f, 2053.67993f));
        PointsCustom.Add(new Vector3(1918.51001f, 100.419998f, 2053.67993f));
        PointsCustom.Add(new Vector3(1917.58997f, 101.019997f, 2053.67993f));
        PointsCustom.Add(new Vector3(1918.52002f, 101.690002f, 2053.22998f));
        PointsCustom.Add(new Vector3(1918.52002f, 102.440002f, 2053.22998f));
        PointsCustom.Add(new Vector3(1919.15002f, 102.93f, 2053.22998f));
        PointsCustom.Add(new Vector3(1918.37f, 103.580002f, 2053.22998f));
        PointsCustom.Add(new Vector3(1917.35999f, 103.580002f, 2053.09009f));
        PointsCustom.Add(new Vector3(1916.55005f, 103.809998f, 2053.09009f));
        PointsCustom.Add(new Vector3(1915.81006f, 103.230003f, 2053.09009f));
        PointsCustom.Add(new Vector3(1915.25f, 102.739998f, 2053.09009f));
        PointsCustom.Add(new Vector3(1915.25f, 101.970001f, 2053f));
        PointsCustom.Add(new Vector3(1915.25f, 101.290001f, 2053f));
        PointsCustom.Add(new Vector3(1914.90002f, 100.739998f, 2053f));
        PointsCustom.Add(new Vector3(1914.90002f, 99.9100037f, 2053f));
        //PointsCustom.Add(new Vector3(1933.31995f, 96.1999969f, 2052.28003f));
        //PointsCustom.Add(new Vector3(1937.93005f, 96.1999969f, 2053.03003f));
        //PointsCustom.Add(new Vector3(1937.93005f, 101.330002f, 2052.59009f));
        //PointsCustom.Add(new Vector3(1937.93005f, 107.5f, 2051.96997f));
        //PointsCustom.Add(new Vector3(1934.07996f, 107.5f, 2051.75f));
        //PointsCustom.Add(new Vector3(1932.90002f, 107.5f, 2048.55005f));
        //PointsCustom.Add(new Vector3(1932.29004f, 102.879997f, 2048.55005f));
        //PointsCustom.Add(new Vector3(1932f, 98.4000015f, 2048.55005f));
        //PointsCustom.Add(new Vector3(1931.90002f, 95f, 2049.30005f));
        //PointsCustom.Add(new Vector3(1931.69995f, 91.0999985f, 2048.1001f));
        //PointsCustom.Add(new Vector3(1931.19995f, 87.5f, 2048.8999f));
        //PointsCustom.Add(new Vector3(1930.90002f, 87.5f, 2051.1001f));

        for (int i = 135; i < 138; i++)
        {
            PointsCustom.Add(new Vector3(0f, 0f, 0f));
        }
    }


    public void DrawPointingHeatmap(int index)
    {
        //List<bool> PointingList = new List<bool>();
        List<Vector4> PointsHeatmap = new List<Vector4>();
        List<Vector4> PropertieslistCustom = new List<Vector4>();
        int didhappen = 0;

        for (int i = 0; i < index; i++)
        {
            if (CurrentlyPointingList[i])
            {

                // if (didhappen > 12)
                // {
                //     break;
                // }
                Debug.Log(PointsCustom[didhappen]);
                PointsHeatmap.Add(PointsCustom[didhappen]);
                PropertieslistCustom.Add(new Vector4(5f, 1f));
                didhappen++;
            }
            if (CurrentlyPointingList[index] && i == index - 1)
            {
                PortalHandler portalOfParticipant = EventController.Instance.allAvatars[0].GetComponent<PortalHandler>();
                portalOfParticipant.pointingOnObject = true;
                portalOfParticipant.PositionOfPoiting = PointsCustom[didhappen];
                portalOfParticipant.PlaceCameraInFrontOfPointing(false);
                portalOfParticipant.UpdateCameraRotation();
            }
            //if (i == index - 1)
            //{
            //    PortalHandler portalOfParticipant = EventController.Instance.allAvatars[0].GetComponent<PortalHandler>();
            //    portalOfParticipant.pointingOnObject = true;
            //    portalOfParticipant.PositionOfPoiting = PointsCustom[didhappen];
            //    portalOfParticipant.PlaceCameraInFrontOfPointing();
            //}
        }
        Material mat = SanFranTower.material;
        mat.SetInt("_Points_Length", 0);

        if (didhappen > 0)
        {
            Vector4[] sliced = PointsHeatmap.ToArray();
            if (PointsHeatmap.ToArray().Length > 100)
            {
                sliced = PointsHeatmap.ToArray().Skip(PointsHeatmap.ToArray().Length - 100).ToArray();
            }


            mat.SetInt("_Points_Length", sliced.Count());
            mat.SetVectorArray("_Points", sliced);
            mat.SetVectorArray("_Properties", PropertieslistCustom.ToArray());
        }



    }

    public void DisableBuildings()
    {
        for(int i = 0; i< transform.childCount; i++)
        {
            Debug.Log(transform.GetChild(i).GetChild(1).GetComponent<MeshRenderer>().materials[0].name);
            if (transform.GetChild(i).GetChild(1).GetComponent<MeshRenderer>().materials[0].name == "BuildingMaterial (Instance)")
            {
                Destroy(transform.GetChild(i).gameObject);

            }
            else
            {
                Destroy(transform.GetChild(i).GetChild(0).gameObject);

            }
            //transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
        }
    }

    public void SetupCyclistPoints()
    {
        PointsCyclist.Add(new Vector3(-0.5f, 0.224000007f, 1f));
        PointsCyclist.Add(new Vector3(-0.5f, 0.486999989f, 0.610000014f));
        PointsCyclist.Add(new Vector3(-0.5f, 0.540000021f, 0.84799999f));
        PointsCyclist.Add(new Vector3(-0.492000014f, 0.148000002f, 0.256999999f));
        PointsCyclist.Add(new Vector3(-0.458999991f, -0.00200000009f, 0.377999991f));
        PointsCyclist.Add(new Vector3(-0.458999991f, -0.00200000009f, 0.377999991f));
        PointsCyclist.Add(new Vector3(-0.495000005f, 0.444000006f, 0.474999994f));
        PointsCyclist.Add(new Vector3(-0.495000005f, 0.444000006f, 0.474999994f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.296999991f, 0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.296999991f, 0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.481999993f, 0.0930000022f, 0.216999993f));
        PointsCyclist.Add(new Vector3(-0.481999993f, 0.0930000022f, 0.216999993f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.870000005f, 0.412999988f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.870000005f, 0.412999988f));
        PointsCyclist.Add(new Vector3(-0.472000003f, 0.851000011f, 0.0280000009f));
        PointsCyclist.Add(new Vector3(-0.472000003f, 0.851000011f, 0.0280000009f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.986000001f, 0.430999994f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.986000001f, 0.430999994f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.976000011f, 0.261000007f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.976000011f, 0.261000007f));
        PointsCyclist.Add(new Vector3(-0.372000009f, 1.01100004f, 0.382999986f));
        PointsCyclist.Add(new Vector3(-0.372000009f, 1.01100004f, 0.382999986f));
        PointsCyclist.Add(new Vector3(-0.375f, 1, 0.0410000011f));
        PointsCyclist.Add(new Vector3(-0.375f, 1, 0.0410000011f));
        PointsCyclist.Add(new Vector3(-0.112999998f, 0.994000018f, 0.465999991f));
        PointsCyclist.Add(new Vector3(-0.112999998f, 0.994000018f, 0.465999991f));
        PointsCyclist.Add(new Vector3(-0.114f, 0.975000024f, 0.192000002f));
        PointsCyclist.Add(new Vector3(-0.114f, 0.975000024f, 0.192000002f));
        PointsCyclist.Add(new Vector3(-0.0460000001f, 0.975000024f, 0.00400000019f));
        PointsCyclist.Add(new Vector3(-0.0460000001f, 0.975000024f, 0.00400000019f));
        PointsCyclist.Add(new Vector3(0.180000007f, 0.980000019f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.180000007f, 0.980000019f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.377999991f, 0.966000021f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.377999991f, 0.966000021f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.379999995f, 0.972000003f, 0.43599999f));
        PointsCyclist.Add(new Vector3(0.379999995f, 0.972000003f, 0.43599999f));
        PointsCyclist.Add(new Vector3(0.372000009f, 0.972000003f, -0.0189999994f));
        PointsCyclist.Add(new Vector3(0.372000009f, 0.972000003f, -0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.00999999978f, 0.968999982f, -0.123000003f));

        PointsCyclist.Add(new Vector3(-0.5f, 0.224000007f, 1f));
        PointsCyclist.Add(new Vector3(-0.5f, 0.486999989f, 0.610000014f));
        PointsCyclist.Add(new Vector3(-0.5f, 0.540000021f, 0.84799999f));
        PointsCyclist.Add(new Vector3(-0.492000014f, 0.148000002f, 0.256999999f));
        PointsCyclist.Add(new Vector3(-0.458999991f, -0.00200000009f, 0.377999991f));
        PointsCyclist.Add(new Vector3(-0.458999991f, -0.00200000009f, 0.377999991f));
        PointsCyclist.Add(new Vector3(-0.495000005f, 0.444000006f, 0.474999994f));
        PointsCyclist.Add(new Vector3(-0.495000005f, 0.444000006f, 0.474999994f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.296999991f, 0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.296999991f, 0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.481999993f, 0.0930000022f, 0.216999993f));
        PointsCyclist.Add(new Vector3(-0.481999993f, 0.0930000022f, 0.216999993f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.870000005f, 0.412999988f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.870000005f, 0.412999988f));
        PointsCyclist.Add(new Vector3(-0.472000003f, 0.851000011f, 0.0280000009f));
        PointsCyclist.Add(new Vector3(-0.472000003f, 0.851000011f, 0.0280000009f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.986000001f, 0.430999994f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.986000001f, 0.430999994f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.976000011f, 0.261000007f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.976000011f, 0.261000007f));
        PointsCyclist.Add(new Vector3(-0.372000009f, 1.01100004f, 0.382999986f));
        PointsCyclist.Add(new Vector3(-0.372000009f, 1.01100004f, 0.382999986f));
        PointsCyclist.Add(new Vector3(-0.375f, 1, 0.0410000011f));
        PointsCyclist.Add(new Vector3(-0.375f, 1, 0.0410000011f));
        PointsCyclist.Add(new Vector3(-0.112999998f, 0.994000018f, 0.465999991f));
        PointsCyclist.Add(new Vector3(-0.112999998f, 0.994000018f, 0.465999991f));
        PointsCyclist.Add(new Vector3(-0.114f, 0.975000024f, 0.192000002f));
        PointsCyclist.Add(new Vector3(-0.114f, 0.975000024f, 0.192000002f));
        PointsCyclist.Add(new Vector3(-0.0460000001f, 0.975000024f, 0.00400000019f));
        PointsCyclist.Add(new Vector3(-0.0460000001f, 0.975000024f, 0.00400000019f));
        PointsCyclist.Add(new Vector3(0.180000007f, 0.980000019f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.180000007f, 0.980000019f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.377999991f, 0.966000021f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.377999991f, 0.966000021f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.379999995f, 0.972000003f, 0.43599999f));
        PointsCyclist.Add(new Vector3(0.379999995f, 0.972000003f, 0.43599999f));
        PointsCyclist.Add(new Vector3(0.372000009f, 0.972000003f, -0.0189999994f));
        PointsCyclist.Add(new Vector3(0.372000009f, 0.972000003f, -0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.00999999978f, 0.968999982f, -0.123000003f));

        PointsCyclist.Add(new Vector3(-0.5f, 0.224000007f, 1f));
        PointsCyclist.Add(new Vector3(-0.5f, 0.486999989f, 0.610000014f));
        PointsCyclist.Add(new Vector3(-0.5f, 0.540000021f, 0.84799999f));
        PointsCyclist.Add(new Vector3(-0.492000014f, 0.148000002f, 0.256999999f));
        PointsCyclist.Add(new Vector3(-0.458999991f, -0.00200000009f, 0.377999991f));
        PointsCyclist.Add(new Vector3(-0.458999991f, -0.00200000009f, 0.377999991f));
        PointsCyclist.Add(new Vector3(-0.495000005f, 0.444000006f, 0.474999994f));
        PointsCyclist.Add(new Vector3(-0.495000005f, 0.444000006f, 0.474999994f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.296999991f, 0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.296999991f, 0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.481999993f, 0.0930000022f, 0.216999993f));
        PointsCyclist.Add(new Vector3(-0.481999993f, 0.0930000022f, 0.216999993f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.870000005f, 0.412999988f));
        PointsCyclist.Add(new Vector3(-0.49000001f, 0.870000005f, 0.412999988f));
        PointsCyclist.Add(new Vector3(-0.472000003f, 0.851000011f, 0.0280000009f));
        PointsCyclist.Add(new Vector3(-0.472000003f, 0.851000011f, 0.0280000009f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.986000001f, 0.430999994f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.986000001f, 0.430999994f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.976000011f, 0.261000007f));
        PointsCyclist.Add(new Vector3(-0.469000012f, 0.976000011f, 0.261000007f));
        PointsCyclist.Add(new Vector3(-0.372000009f, 1.01100004f, 0.382999986f));
        PointsCyclist.Add(new Vector3(-0.372000009f, 1.01100004f, 0.382999986f));
        PointsCyclist.Add(new Vector3(-0.375f, 1, 0.0410000011f));
        PointsCyclist.Add(new Vector3(-0.375f, 1, 0.0410000011f));
        PointsCyclist.Add(new Vector3(-0.112999998f, 0.994000018f, 0.465999991f));
        PointsCyclist.Add(new Vector3(-0.112999998f, 0.994000018f, 0.465999991f));
        PointsCyclist.Add(new Vector3(-0.114f, 0.975000024f, 0.192000002f));
        PointsCyclist.Add(new Vector3(-0.114f, 0.975000024f, 0.192000002f));
        PointsCyclist.Add(new Vector3(-0.0460000001f, 0.975000024f, 0.00400000019f));
        PointsCyclist.Add(new Vector3(-0.0460000001f, 0.975000024f, 0.00400000019f));
        PointsCyclist.Add(new Vector3(0.180000007f, 0.980000019f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.180000007f, 0.980000019f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.377999991f, 0.966000021f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.377999991f, 0.966000021f, 0.238000005f));
        PointsCyclist.Add(new Vector3(0.379999995f, 0.972000003f, 0.43599999f));
        PointsCyclist.Add(new Vector3(0.379999995f, 0.972000003f, 0.43599999f));
        PointsCyclist.Add(new Vector3(0.372000009f, 0.972000003f, -0.0189999994f));
        PointsCyclist.Add(new Vector3(0.372000009f, 0.972000003f, -0.0189999994f));
        PointsCyclist.Add(new Vector3(-0.00999999978f, 0.968999982f, -0.123000003f));



        //PointsCyclist.Add(new Vector3(-0.492000014f, 0.148000002f, 0.256999999f));
        //PointsCyclist.Add(new Vector3(-0.458999991f, -0.00200000009f, 0.377999991f));
        //PointsCyclist.Add(new Vector3(-0.458999991f, -0.00200000009f, 0.377999991f));
        //PointsCyclist.Add(new Vector3(-0.495000005f, 0.444000006f, 0.474999994f));
        //PointsCyclist.Add(new Vector3(-0.495000005f, 0.444000006f, 0.474999994f));
        //PointsCyclist.Add(new Vector3(-0.49000001f, 0.296999991f, 0.0189999994f));
        //PointsCyclist.Add(new Vector3(-0.49000001f, 0.296999991f, 0.0189999994f));
        //PointsCyclist.Add(new Vector3(-0.481999993f, 0.0930000022f, 0.216999993f));
        //PointsCyclist.Add(new Vector3(-0.481999993f, 0.0930000022f, 0.216999993f));
        //PointsCyclist.Add(new Vector3(-0.49000001f, 0.870000005f, 0.412999988f));
        //PointsCyclist.Add(new Vector3(-0.49000001f, 0.870000005f, 0.412999988f));
        //PointsCyclist.Add(new Vector3(-0.472000003f, 0.851000011f, 0.0280000009f));
        //PointsCyclist.Add(new Vector3(-0.472000003f, 0.851000011f, 0.0280000009f));
        //PointsCyclist.Add(new Vector3(-0.469000012f, 0.986000001f, 0.430999994f));
        //PointsCyclist.Add(new Vector3(-0.469000012f, 0.986000001f, 0.430999994f));
        //PointsCyclist.Add(new Vector3(-0.469000012f, 0.976000011f, 0.261000007f));
        //PointsCyclist.Add(new Vector3(-0.469000012f, 0.976000011f, 0.261000007f));
        //PointsCyclist.Add(new Vector3(-0.372000009f, 1.01100004f, 0.382999986f));
        //PointsCyclist.Add(new Vector3(-0.372000009f, 1.01100004f, 0.382999986f));
        //PointsCyclist.Add(new Vector3(-0.375f, 1, 0.0410000011f));
        //PointsCyclist.Add(new Vector3(-0.375f, 1, 0.0410000011f));
        //PointsCyclist.Add(new Vector3(-0.112999998f, 0.994000018f, 0.465999991f));
        //PointsCyclist.Add(new Vector3(-0.112999998f, 0.994000018f, 0.465999991f));
        //PointsCyclist.Add(new Vector3(-0.114f, 0.975000024f, 0.192000002f));
        //PointsCyclist.Add(new Vector3(-0.114f, 0.975000024f, 0.192000002f));
        //PointsCyclist.Add(new Vector3(-0.0460000001f, 0.975000024f, 0.00400000019f));
        //PointsCyclist.Add(new Vector3(-0.0460000001f, 0.975000024f, 0.00400000019f));
        //PointsCyclist.Add(new Vector3(0.180000007f, 0.980000019f, 0.238000005f));
        //PointsCyclist.Add(new Vector3(0.180000007f, 0.980000019f, 0.238000005f));
        //PointsCyclist.Add(new Vector3(0.377999991f, 0.966000021f, 0.238000005f));
        //PointsCyclist.Add(new Vector3(0.377999991f, 0.966000021f, 0.238000005f));
        //PointsCyclist.Add(new Vector3(0.379999995f, 0.972000003f, 0.43599999f));
        //PointsCyclist.Add(new Vector3(0.379999995f, 0.972000003f, 0.43599999f));
        //PointsCyclist.Add(new Vector3(0.372000009f, 0.972000003f, -0.0189999994f));
        //PointsCyclist.Add(new Vector3(0.372000009f, 0.972000003f, -0.0189999994f));
        //PointsCyclist.Add(new Vector3(-0.00999999978f, 0.968999982f, -0.123000003f));


    }


    public void DrawCyclistHeatmap(int index)
    {
        //int start = 438;
        int start = 5580;
        //int end = 471;
        int end = 5643;
        Debug.Log(CyclistGameobject);
        Heatmap changeHeatmapGameobject = CyclistGameobject.transform.GetChild(1).GetComponent<Heatmap>();
        if (index > start && index < end)
        {

            List<Vector4> PointsOnCyclist = new List<Vector4>();
            List<Vector4> PropertiesOnCyclist = new List<Vector4>();
            for (int i = 0; i < index - start; i++)
            {


                Vector3 coords = changeHeatmapGameobject.transform.parent.gameObject.transform.position;
                Quaternion rot = changeHeatmapGameobject.transform.parent.gameObject.transform.rotation;
                // Vector4 newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles - new Vector3(359.98699951171875f, 1.8998982906341553f, 0f));
                Vector3 positionSinglePoint = PointsCyclist[i];
                Vector4 newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles);
                PointsOnCyclist.Add(newPos);
                PropertiesOnCyclist.Add(new Vector4(0.1f, 1f));
                //PropertiesOnCyclist.Add(new Vector4(1f, 1f));

                if (i == index - start - 1)
                {
                    PortalHandler portalOfParticipant = EventController.Instance.allAvatars[0].GetComponent<PortalHandler>();
                    portalOfParticipant.pointingOnObject = true;
                    portalOfParticipant.PositionOfPoiting = newPos;
                    portalOfParticipant.PlaceCameraInFrontOfPointing(true);
                    portalOfParticipant.UpdateCameraRotation();
                }
            }


            Material material = changeHeatmapGameobject.material;
            material.SetInt("_Points_Length", PointsOnCyclist.ToArray().Length);
            material.SetVectorArray("_Points", PointsOnCyclist.ToArray());
            material.SetVectorArray("_Properties", PropertiesOnCyclist.ToArray());

        }
        else
        {
            if (index < start || index > 514)
            {
                Material material = changeHeatmapGameobject.material;
                material.SetInt("_Points_Length", 0);
            }
            else
            {
                List<Vector4> PointsOnCyclist = new List<Vector4>();
                List<Vector4> PropertiesOnCyclist = new List<Vector4>();
                for (int i = 0; i < PointsCyclist.Count; i++)
                {


                    Vector3 coords = changeHeatmapGameobject.transform.parent.gameObject.transform.position;
                    Quaternion rot = changeHeatmapGameobject.transform.parent.gameObject.transform.rotation;
                    // Vector4 newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles - new Vector3(359.98699951171875f, 1.8998982906341553f, 0f));
                    Vector3 positionSinglePoint = PointsCyclist[i];
                    Vector4 newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles);
                    PointsOnCyclist.Add(newPos);
                    PropertiesOnCyclist.Add(new Vector4(0.1f, 1f));
                }
                Material material = changeHeatmapGameobject.material;
                material.SetInt("_Points_Length", PointsOnCyclist.ToArray().Length);
                material.SetVectorArray("_Points", PointsOnCyclist.ToArray());
                material.SetVectorArray("_Properties", PropertiesOnCyclist.ToArray());
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



}
