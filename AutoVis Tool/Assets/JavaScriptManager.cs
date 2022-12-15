using System.Collections;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Replay;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine.Networking;



namespace Replay
{
    public class JavaScriptManager : MonoBehaviour
    {

        public static JavaScriptManager instanceJS;
        public GameObject Line;

        private bool lineDisplayed = false;

        private Color HexintoColor;
        ReplayManager compScenePlayer;

        private int fastForwardMultiplier;

        public GameObject avatarmodel;

        public List<(bool, GameObject)> AvatarList = new List<(bool, GameObject)>();

        public GameObject teslaModel;

        public GameObject PassengerModel;

        public GameObject PassengerController;

        public RigControl rigcontrol;


        public MapBuilderCustom mapBuilderCustom;

        public List<GameObject> bonesTest;

        public List<Material> AvatarMaterials;

        public Heatmap heatmapExterior;

        public int focusedId = 0;

        private StreamReader reader;

        public List<List<Vector3>> headArmatureList = new List<List<Vector3>>();

        public List<List<Vector3>> leftHandArmatureList = new List<List<Vector3>>();
        public List<List<Vector3>> rightHandArmatureList = new List<List<Vector3>>();

        private List<InteriorHeatmaps> interiorHeatmaps = new List<InteriorHeatmaps>();

        private List<TouchHeatmaps> interiorTouchHeatmaps = new List<TouchHeatmaps>();

        private List<Trajectories> TrajectoriesAll = new List<Trajectories>();


        public List<GameObject> exteriorBuildings;

        public List<GameObject> participants;

        // public Material transmat;

        public List<Material> materialofAvatars;

        public List<Vector4> rotations;

        public List<Vector3> positions;

        public GameObject nodesCube;

        //public List<List<Vector3>> AvatarPositionsList = new List<List<Vector3>>();

        public List<List<List<Vector3>>> AvatarBonePositionParticipant = new List<List<List<Vector3>>>();
        public List<List<(bool, List<Vector3>)>> PassengerBonePositionParticipant = new List<List<(bool, List<Vector3>)>>();

        public List<List<List<Vector3>>> FinalPassengerBonePositionParticipant = new List<List<List<Vector3>>>();


        public Heatmap radiusHeatmap;

        public Heatmap radiusHeatmap1;

        public Heatmap radiusHeatmap2;

        private List<Heatmap> radiuses = new List<Heatmap>();

        public GameObject mainCar;

        public List<Vector3> mainCarPositions;

        public List<Color32> participantColors;

        public GameObject HeatmapInterior;

        public GameObject HeatmapTouch;


        public GameObject Trajectory;



        public List<List<List<Vector4>>> radiusCar = new List<List<List<Vector4>>>();

        public List<Vector4> propertiesList;



        private int counterforavatar = 0;

        public Vector3 startRotation;

        public int selectedParticipant;

        public GameObject ColorPicker;

        public List<List<Vector3>> gazePoints = new List<List<Vector3>>();


        public List<GameObject> Lines = new List<GameObject>();

        float zOffset = 1.78400159f;

        private void Awake()
        {
            compScenePlayer = this.GetComponent<ReplayManager>();
            instanceJS = this;
        }


        public List<GameObject> Avatar;
        // Start is called before the first frame update
        void Start()
        {
            //readHeatmapJsonandProcess();


            // Color32 c = new Color32(85, 85, 255, 255);
            // Color32 c1 = new Color32(85, 85, 200, 255);
            // Color32 c2 = new Color32(85, 85, 150, 255);
            // Color32 c3 = new Color32(255, 85, 255, 255);
            // Color32 c4 = new Color32(200, 85, 200, 255);
            // Color32 c5 = new Color32(150, 85, 150, 255);
            // participantColors.Add(c);
            // participantColors.Add(c1);
            // participantColors.Add(c2);
            // participantColors.Add(c3);
            // participantColors.Add(c4);
            // participantColors.Add(c5);


        }

        // Update is called once per frame
        void Update()
        {
            // if (lineDisplayed)
            // {
            //     animateLine();
            // }

            // if (GameObject.Find("Left Arm") != null)
            // {
            //     animateAvatar();
            // }


            //jumptoFrame("1646783526.9056569");
        }





        void hexToRgb(string s)
        {
            ColorUtility.TryParseHtmlString(s, out HexintoColor);
        }



        public void buildCustomAvatar()
        {
            readJsonandProcess();
            for (int i = 0; i < positions.Count; i++)
            {
                GameObject cube = Instantiate(nodesCube);
                cube.name = "bone " + i;
                bonesTest.Add(cube);
                setNewBonePosition(cube, positions[i]);
            }
            rotationtest();

        }


        public void rotationtest()
        {

            for (int i = 0; i < bonesTest.Count; i++)
            {
                if (i != bonesTest.Count - 1)
                {
                    // Quaternion rotation = new Quaternion();
                    // rotation.SetLookRotation(bonesTest[i].transform.position, bonesTest[i + 1].transform.position);
                    // transform.localRotation = rotation;
                    //bonesTest[i].transform.localRotation = rotation;
                    bonesTest[i].transform.LookAt(bonesTest[i + 1].transform);
                }


            }
        }


        public void rotationtest2()
        {

            for (int i = 0; i < Avatar.Count; i++)
            {
                if (i != Avatar.Count - 1)
                {
                    Debug.Log("test");
                    // Quaternion rotation = new Quaternion();
                    // rotation.SetLookRotation(bonesTest[i].transform.position, bonesTest[i + 1].transform.position);
                    // transform.localRotation = rotation;
                    //bonesTest[i].transform.localRotation = rotation;
                    //Avatar[i].transform.LookAt(Avatar[i + 1].transform);
                    Avatar[i].transform.Rotate(0, -180f, 0);
                }


            }
        }

        public void rotateBone()
        {
            Debug.Log("done");
            Quaternion rotation = new Quaternion();
            rotation = Avatar[14].transform.rotation;
            rotation.SetLookRotation(Avatar[15].transform.position, Avatar[15].transform.position);
            //transform.localRotation = rotation;

            Avatar[14].transform.rotation = rotation;
        }
        public void jumptoFrame(string stamp)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";
            //Destroy(compScenePlayer.ReplayContainer); 
            // compScenePlayer.AllWorldObjectsInFrame.Clear();
            double time = Convert.ToDouble(stamp, provider);
            //Debug.Log(time);

            compScenePlayer.GoToNearestTimeStamp(time);


            //compScenePlayer.RefocusOnObject();
        }




        [Obsolete("See ModelManager.ToggleHighlightModelType(string key)")]
        public void OutlineAllType(string type)
        {
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            for (int i = 0; i < rootObjects.Count; i++)
            {
                if (rootObjects[i].GetComponent<ModelController>() != null)
                {
                    rootObjects[i].transform.GetComponent<Outline>().enabled = false;
                }
            }
            for (int i = 0; i < rootObjects.Count; i++)
            {
                if (rootObjects[i].GetComponent<ModelController>() != null)
                {
                    if (type == rootObjects[i].GetComponent<ModelController>().Type)
                    {
                        Debug.Log("test");
                        rootObjects[i].transform.GetComponent<Outline>().enabled = true;
                    }
                }
                //Debug.Log(rootObjects[i]);
            }

        }

        [Obsolete("See ModelManager.HighlightModel(string key)")]
        public void Outlinesingle(string key)
        {
            List<GameObject> rootObjects = new List<GameObject>();
            Scene scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            for (int i = 0; i < rootObjects.Count; i++)
            {
                if (rootObjects[i].GetComponent<ModelController>() != null)
                {
                    rootObjects[i].transform.GetComponent<Outline>().enabled = false;
                }
            }
            for (int i = 0; i < rootObjects.Count; i++)
            {
                if (rootObjects[i].GetComponent<ModelController>() != null)
                {

                    if (key == rootObjects[i].GetComponent<ModelController>().Key)
                    {

                        rootObjects[i].transform.GetComponent<Outline>().enabled = true;
                    }
                }
                //Debug.Log(rootObjects[i]);
            }

        }







        //Just for Demo purposes


        public void UpdateAvatar()
        {
            //TODO edit with data inside array
            float currentTimestamp = 1;
            float listobjecTimestamp = 1;
            //TODO read data
            //UnityEditor.TransformWorldPlacementJSON:{ "position":{ "x":5.849999904632568,"y":-0.11959359794855118,"z":24.770000457763673},"rotation":{ "x":-7.843471649948697e-9,"y":0.7109220623970032,"z":-1.5537693798250986e-8,"w":0.7032707929611206},"scale":{ "x":1.0,"y":1.0,"z":1.0} }
            // Vector3 pos = new Vector3(5.849999904632568f, -0.11959359794855118f, 24.770000457763673f);
            // Vector3 rot = new Vector3(-7.84347164994869f, 0.7109220623970032f, -1.5537693798250986f);
            // TODO list of Positions in the same Order as bones of Avatar List

            //fixAvatar();
            if (currentTimestamp == listobjecTimestamp)
            {


                //rebuildAvatar();
                // for (int i = 0; i < Avatar.Count; i++)
                // {
                //     setNewBonePosition(Avatar[i], positions[i]);
                //     //setNewBoneRotation(Avatar[i], rotations[i]);
                // }
                // for (int i = 0; i < Avatar.Count; i++)
                // {
                //     //setNewBonePosition(Avatar[i], positions[i]);
                //     //setNewBoneRotation(Avatar[i], rotations[i]);
                // }
                for (int i = 0; i < positions.Count; i++)
                {
                    Debug.Log("DONE");
                    rigcontrol.positionsBones.Add(positions[i]);
                }

                rigcontrol.setbonesAll();
                //rotationtest2();

            }

        }

        public void rebuildAvatar()
        {
            int[] hips = { 0, 1, 2, 3 };
            for (int i = 1; i < hips.Length; i++)
            {
                if (i == 1)
                {
                    Avatar[hips[i]].transform.parent = Avatar[hips[i - 1]].transform.GetChild(0);
                }
                else if (i == 2)
                {
                    Avatar[hips[i]].transform.parent = Avatar[hips[i - 1]].transform.GetChild(0);
                }
                else
                {

                    Avatar[hips[i]].transform.parent = Avatar[hips[i - 1]].transform;

                }
            }
            int[] leftarm = { 0, 1, 4, 5, 6 };
            for (int i = 1; i < leftarm.Length; i++)
            {
                if (i == 1)
                {
                    Avatar[leftarm[i]].transform.parent = Avatar[leftarm[i - 1]].transform.GetChild(0);
                }
                else if (i == 2)
                {
                    Avatar[leftarm[i]].transform.parent = Avatar[leftarm[i - 1]].transform.GetChild(0);
                }
                else
                {
                    Avatar[leftarm[i]].transform.parent = Avatar[leftarm[i - 1]].transform;
                }
            }
            int[] rightarm = { 0, 1, 7, 8, 9 };
            for (int i = 1; i < rightarm.Length; i++)
            {
                if (i == 1)
                {
                    Avatar[rightarm[i]].transform.parent = Avatar[rightarm[i - 1]].transform.GetChild(0);
                }
                else if (i == 2)
                {
                    Avatar[rightarm[i]].transform.parent = Avatar[rightarm[i - 1]].transform.GetChild(0);
                }
                else
                {
                    Avatar[rightarm[i]].transform.parent = Avatar[rightarm[i - 1]].transform;
                }
            }
            int[] leftFoot = { 0, 10, 11, 12, 13 };
            for (int i = 1; i < leftFoot.Length; i++)
            {
                Avatar[leftFoot[i]].transform.parent = Avatar[leftFoot[i - 1]].transform;
            }
            int[] rightFoot = { 0, 14, 15, 16, 17 };
            for (int i = 1; i < rightFoot.Length; i++)
            {
                Avatar[rightFoot[i]].transform.parent = Avatar[rightFoot[i - 1]].transform;
            }
        }

        public void fixAvatar()
        {
            Quaternion offset = new Quaternion(0f, 1f, 0f, 0f);
            for (int i = 0; i < Avatar.Count; i++)
            {
                Avatar[i].transform.localRotation = offset;
            }
        }


        public void handleTimeStampString(string json)
        {
            JObject main = JObject.Parse(json);
            handleGameObjectString(main);

        }

        public void readWholeJson(string json)
        {
            JObject main = JObject.Parse(json);
        }

        public void handleGameObjectString(JObject main)
        {
            JEnumerable<JToken> tokens = main["objects"].Children();
            foreach (JToken t in tokens)    //foreach recorded frame
            {
                GameObject thisObject = GameObject.Find((string)t["id"]);
                if (thisObject == null)
                {
                    if ((string)t["id"] == "98")
                    {
                        GameObject tesla = Instantiate(teslaModel);
                        tesla.name = (string)t["id"];
                        tesla.transform.position = new Vector3((float)t["position"]["x"], (float)t["position"]["y"], (float)t["position"]["z"]);
                        tesla.transform.rotation = Quaternion.Euler(new Vector3((float)t["rotation"]["x"], (float)t["rotation"]["y"], (float)t["rotation"]["z"]));
                    }
                    else
                    {
                        GameObject thisOne = new GameObject((string)t["id"]);
                        thisOne.name = (string)t["id"];
                        thisOne.transform.position = new Vector3((float)t["position"]["x"], (float)t["position"]["y"], (float)t["position"]["z"]);
                        thisOne.transform.rotation = Quaternion.Euler(new Vector3((float)t["rotation"]["x"], (float)t["rotation"]["y"], (float)t["rotation"]["z"]));

                    }

                }
                else
                {
                    thisObject.transform.position = new Vector3((float)t["position"]["x"], (float)t["position"]["y"], (float)t["position"]["z"]);
                    thisObject.transform.rotation = Quaternion.Euler(new Vector3((float)t["rotation"]["x"], (float)t["rotation"]["y"], (float)t["rotation"]["z"]));
                }
            }
        }


        public void readJsonandProcess()
        {
            reader = new StreamReader("./Recordings/Pascal - All.json", Encoding.UTF8, false, 65536);

            string content = reader.ReadToEnd();
            StartCoroutine(coroutinePlayTest(content));
            //Debug.Log(positions);

        }



        IEnumerator coroutinePlayTest(string content)
        {
            // while (true)
            // {

            JObject main = JObject.Parse(content);
            JToken snapshots = main["snapshots"].ElementAt(counterforavatar)["other"];
            JEnumerable<JToken> obj = main["snapshots"].ElementAt(3)["other"]["Kinect"]["Bodies"].Children();
            int[] arr = { 0, 1, 2, 3, 4, 5, 7, 8, 9, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            //int[] arr = { 0, 1, 2, 3, 4, 5, 7, 8, 9, 11, 12, 13, 14, 15 };
            //int[] arr = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };
            //Debug.Log(arr.Length);
            for (int i = 0; i < arr.Length; i++)
            {
                //positions.Add(new Vector3((float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["position"]["x"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["position"]["y"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["position"]["z"]));
                positions.Add(new Vector3((float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["position"]["x"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["position"]["y"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["position"]["z"] - zOffset));
                rotations.Add(new Vector4((float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["rotation"]["x"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["rotation"]["y"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["rotation"]["z"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["rotation"]["w"]));
                var orientation = new Vector4((float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["rotation"]["x"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["rotation"]["y"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["rotation"]["z"], (float)obj.ElementAt(0)["joints"].ElementAt(arr[i])["rotation"]["w"]);
                // var rotationX = Pitch(orientation);
                // var rotationY = Yaw(orientation);
                // var rotationZ = Roll(orientation);
                // rotations.Add(new Vector4((float)rotationX, (float)rotationY, (float)rotationZ, 1f));
                //                Debug.Log("position an stelle" + i + " mit dem vector" + positions[i]);
            }

            // positions.Add(positions[0] - new Vector3(0, 0.1220733f, -0.01573658f));
            // rotations.Add(rotations[0]);
            UpdateAvatar();
            counterforavatar++;
            //Debug.Log(counterforavatar);

            yield return new WaitForSeconds(0.1f);

            // }
        }


        public void InitInteriorHeatmap(List<(string, Vector4)> points, int participantId)
        {
            GameObject Heatmaps = Instantiate(HeatmapInterior);
            Heatmaps.transform.position = mainCar.transform.position;
            Heatmaps.transform.rotation = mainCar.transform.rotation;
            Heatmaps.transform.parent = mainCar.transform;
            InteriorHeatmaps thisHeatmap = Heatmaps.GetComponent<InteriorHeatmaps>();
            thisHeatmap.SetupTextures(participantId, points);
            interiorHeatmaps.Add(thisHeatmap);
        }

        public void DrawHeatmaps(int index)
        {
            //heatmaps[0].drawHeatmap(index);
            //heatmaps[1].drawHeatmap(index);
            //heatmaps[2].drawHeatmap(index);

            for (int i = 0; i < interiorHeatmaps.Count; i++)
            {
                interiorHeatmaps[i].DrawInteriorHeatmap(index);
            }
            mapBuilderCustom.DrawPointingHeatmap(index);
        }

        public void InitTouchInteriorHeatmap(List<(string, Vector4)> points, int participantId)
        {
            GameObject Heatmaps = Instantiate(HeatmapTouch);
            Heatmaps.transform.position = mainCar.transform.position;
            Heatmaps.transform.rotation = mainCar.transform.rotation;
            Heatmaps.transform.parent = mainCar.transform;
            TouchHeatmaps thisHeatmap = Heatmaps.GetComponent<TouchHeatmaps>();
            thisHeatmap.SetupTextures(participantId, points);
            interiorTouchHeatmaps.Add(thisHeatmap);
        }

        public void DrawTouchHeatmaps(int index)
        {
            //heatmaps[0].drawHeatmap(index);
            //heatmaps[1].drawHeatmap(index);
            //heatmaps[2].drawHeatmap(index);
            for (int i = 0; i < interiorTouchHeatmaps.Count; i++)
            {
                //                Debug.Log(i + "HIER");
                interiorTouchHeatmaps[i].DrawTouchInteriorHeatmap(index);
            }
            // mapBuilderCustom.DrawPointingHeatmap(index);
        }


        public void InitExteriorBuildingHeatmap(List<Vector4> points, int participantId)
        {
            mapBuilderCustom.SetupTextures(points);
            // GameObject Heatmaps = Instantiate(HeatmapInterior);
            // Heatmaps.transform.position = mainCar.transform.position;
            // Heatmaps.transform.rotation = mainCar.transform.rotation;
            // Heatmaps.transform.parent = mainCar.transform;
            // InteriorHeatmaps thisHeatmap = Heatmaps.GetComponent<InteriorHeatmaps>();
            // thisHeatmap.SetupTextures(participantId, points);
            // interiorHeatmaps.Add(thisHeatmap);


        }

        public void InitAvatar(List<List<Vector3>> ParticipantAvatarList, int partIndex)
        {
            GameObject avatar = Instantiate(avatarmodel);
            Vector3 posAvatar = avatar.transform.localPosition;
            avatar.transform.parent = mainCar.transform;
            avatar.transform.rotation = mainCar.transform.rotation;
            avatar.transform.localPosition = posAvatar;


            AvatarBonePositionParticipant.Add(ParticipantAvatarList);
            avatar.GetComponent<RigControl>().mainCar = mainCar;
            avatar.GetComponent<RigControl>().AvatarModel = Avatar;
            avatar.GetComponent<PortalHandler>().SetOutlineColor(AvatarMaterials[partIndex].color);
            //Debug.Log("DONE" + Avatar);

            avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = AvatarMaterials[partIndex];
            AvatarList.Add((true, avatar));
        }



        public void InitMergedAvatar()
        {
            GameObject avatar = Instantiate(avatarmodel);
            Vector3 posAvatar = avatar.transform.localPosition;
            avatar.transform.parent = mainCar.transform;
            avatar.transform.rotation = mainCar.transform.rotation;
            avatar.transform.localPosition = posAvatar;

            AvatarList.Add((true, avatar));

            avatar.GetComponent<RigControl>().mainCar = mainCar;
            avatar.GetComponent<RigControl>().AvatarModel = Avatar;

            avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = AvatarMaterials[3];
            List<List<Vector3>> MergedPositions = new List<List<Vector3>>();
            int savetest = Math.Min(AvatarBonePositionParticipant[0].Count, AvatarBonePositionParticipant[1].Count);
            int numberOfBones = Math.Min(savetest, AvatarBonePositionParticipant[2].Count);
            for (int i = 0; i < numberOfBones; i++)
            {

                List<Vector3> bones = new List<Vector3>();
                for (int k = 0; k < AvatarBonePositionParticipant[0][0].Count; k++)
                {
                    Vector3 thisVec = new Vector3();

                    for (int j = 0; j < AvatarBonePositionParticipant.Count; j++)
                    {
                        thisVec += AvatarBonePositionParticipant[j][i][k];
                    }
                    Vector3 mergedVec = thisVec / AvatarBonePositionParticipant.Count;
                    bones.Add(mergedVec);

                }
                MergedPositions.Add(bones);

            }
            AvatarBonePositionParticipant.Add(MergedPositions);

        }

        public void DrawAvatarMovement(int index)
        {
            // Debug.Log(index);
            // RigControl avatar = AvatarList[0].GetComponent<RigControl>();
            // avatar.positionsBones = AvatarBonePositionParticipant[0][index];
            // avatar.setbonesAll();
            for (int i = 0; i < AvatarBonePositionParticipant.Count; i++)
            {
                if (AvatarList[i].Item1)
                {
                    RigControl avatar = AvatarList[i].Item2.GetComponent<RigControl>();
                    avatar.positionsBones = AvatarBonePositionParticipant[i][index];
                    avatar.setbonesAll();
                }

            }
        }

        public void InitMergedPassenger()
        {
            GameObject avatar = PassengerModel;
            Vector3 posAvatar = avatar.transform.localPosition;
            avatar.transform.parent = mainCar.transform;
            avatar.transform.rotation = mainCar.transform.rotation;
            avatar.transform.localPosition = posAvatar;


            avatar.GetComponent<RigControl>().mainCar = mainCar;

            avatar.GetComponent<RigControl>().AvatarModel = Avatar;
            PassengerController = avatar;

            //avatar.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material = AvatarMaterials[3];
            List<List<Vector3>> MergedPositions = new List<List<Vector3>>();
            int savetest = Math.Min(PassengerBonePositionParticipant[0].Count, PassengerBonePositionParticipant[1].Count);
            int numberOfBones = Math.Min(savetest, PassengerBonePositionParticipant[2].Count);
            for (int i = 0; i < numberOfBones; i++)
            {


                List<Vector3> bones = new List<Vector3>();
                for (int k = 0; k < PassengerBonePositionParticipant[0][0].Item2.Count; k++)
                {
                    Vector3 thisVec = new Vector3();
                    int counterForPassengerTimestamp = 0;
                    for (int j = 0; j < PassengerBonePositionParticipant.Count; j++)
                    {
                        if (PassengerBonePositionParticipant[j][i].Item1)
                        {
                            counterForPassengerTimestamp++;
                        }
                        thisVec += PassengerBonePositionParticipant[j][i].Item2[k];
                    }
                    Vector3 mergedVec = new Vector3();
                    if (counterForPassengerTimestamp == 0)
                    {
                        mergedVec = new Vector3(-1000f, -1000f, -1000f);
                    }
                    else
                    {
                        mergedVec = thisVec / counterForPassengerTimestamp;
                    }

                    bones.Add(mergedVec);

                }
                MergedPositions.Add(bones);

            }
            FinalPassengerBonePositionParticipant.Add(MergedPositions);

        }

        public void DrawPassenger(int index)
        {

            RigControl avatar = PassengerModel.GetComponent<RigControl>();
            avatar.positionsBones = FinalPassengerBonePositionParticipant[0][index];
            avatar.setbonesAll();

        }

        public void InitTrajectories(int participantId, List<Vector3> head, List<Vector3> leftHand, List<Vector3> rightHand)
        {
            GameObject Trajectories = Instantiate(Trajectory);
            Trajectories.transform.position = mainCar.transform.position;
            Trajectories.transform.rotation = mainCar.transform.rotation;
            Trajectories.transform.parent = mainCar.transform;
            //Trajectories.transform.localPosition = new Vector3(0, 1f, 1.86399996f);
            Trajectories.transform.localPosition = new Vector3(0, 1.125f, 1.76999998f);
            Trajectories.transform.localRotation = new Quaternion(0f, 1f, 0f, 0f);
            Trajectories thisTrajectory = Trajectories.GetComponent<Trajectories>();
            thisTrajectory.SetupTrajectories(participantId, head, leftHand, rightHand);
            TrajectoriesAll.Add(thisTrajectory);
        }

        public void DrawTrajectories(int index)
        {
            for (int i = 0; i < interiorHeatmaps.Count; i++)
            {
                TrajectoriesAll[i].DrawTrajectories(index);
            }
        }

        public void InitRadiusus()
        {
            radiuses.Add(radiusHeatmap);
            radiuses.Add(radiusHeatmap1);
            radiuses.Add(radiusHeatmap2);
        }


        public void updateHeatmapCarRadius(int index)
        {
            // int NumberOfPoints = 100;
            List<Vector4> newList = new List<Vector4>();
            List<Vector4> propertiesForCar = new List<Vector4>();
            for (int i = 0; i < radiusCar[index][0].Count; i++)
            {
                Vector4 point = new Vector4(radiusCar[index][0][i].x, radiusHeatmap.gameObject.transform.position.y, radiusCar[index][0][i].z, radiusCar[index][0][i].w);
                //Vector4 newPos = (Vector4)RotatePointAroundPivot(point, radiusHeatmap.gameObject.transform.position, new Vector3(radiusHeatmap.gameObject.transform.rotation.eulerAngles.x, 0, radiusHeatmap.gameObject.transform.rotation.eulerAngles.z));
                newList.Add(point);

                propertiesForCar.Add(new Vector4(5f, 1f));
            }
            if (newList.Count > 0)
            {
                Material material = radiusHeatmap.material;
                material.SetInt("_Points_Length", newList.ToArray().Length);
                material.SetVectorArray("_Points", newList.ToArray());
                material.SetVectorArray("_Properties", propertiesForCar.ToArray());
            }
            else
            {
                Material material = radiusHeatmap.material;
                material.SetInt("_Points_Length", 0);

            }
            List<Vector4> newList1 = new List<Vector4>();
            List<Vector4> propertiesForCar1 = new List<Vector4>();
            for (int i = 0; i < radiusCar[index][1].Count; i++)
            {
                Vector4 point = new Vector4(radiusCar[index][1][i].x, radiusHeatmap.gameObject.transform.position.y, radiusCar[index][1][i].z, radiusCar[index][1][i].w);
                //Vector4 newPos = (Vector4)RotatePointAroundPivot(point, radiusHeatmap.gameObject.transform.position, new Vector3(radiusHeatmap.gameObject.transform.rotation.eulerAngles.x, 0, radiusHeatmap.gameObject.transform.rotation.eulerAngles.z));
                newList1.Add(point);

                propertiesForCar1.Add(new Vector4(10f, 1f));
            }
            if (newList1.Count > 0)
            {
                Material material = radiusHeatmap1.material;
                material.SetInt("_Points_Length", newList1.ToArray().Length);
                material.SetVectorArray("_Points", newList1.ToArray());
                material.SetVectorArray("_Properties", propertiesForCar1.ToArray());
            }
            else
            {
                Material material = radiusHeatmap1.material;
                material.SetInt("_Points_Length", 0);

            }
            List<Vector4> newList2 = new List<Vector4>();
            List<Vector4> propertiesForCar2 = new List<Vector4>();
            for (int i = 0; i < radiusCar[index][2].Count; i++)
            {
                Vector4 point = new Vector4(radiusCar[index][2][i].x, radiusHeatmap.gameObject.transform.position.y, radiusCar[index][2][i].z, radiusCar[index][2][i].w);
                //Vector4 newPos = (Vector4)RotatePointAroundPivot(point, radiusHeatmap.gameObject.transform.position, new Vector3(radiusHeatmap.gameObject.transform.rotation.eulerAngles.x, 0, radiusHeatmap.gameObject.transform.rotation.eulerAngles.z));
                newList2.Add(point);

                propertiesForCar2.Add(new Vector4(10f, 1f));
            }
            if (newList2.Count > 0)
            {
                Material material = radiusHeatmap2.material;
                material.SetInt("_Points_Length", newList2.ToArray().Length);
                material.SetVectorArray("_Points", newList2.ToArray());
                material.SetVectorArray("_Properties", propertiesForCar2.ToArray());
            }
            else
            {
                Material material = radiusHeatmap2.material;
                material.SetInt("_Points_Length", 0);

            }



        }




        public void ExteriorHeatmap()
        {

            Vector3 example = new Vector3(519.9404f, 94.63153f, 2823.5957f);
            Vector3 coords = heatmapExterior.gameObject.transform.position;
            Quaternion rot = heatmapExterior.gameObject.transform.rotation;
            // Vector4 newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles - new Vector3(359.98699951171875f, 1.8998982906341553f, 0f));
            Vector3 positionSinglePoint = example;
            // Vector4 newPos = (Vector4)RotatePointAroundPivot(positionSinglePoint + coords, coords, rot.eulerAngles);
            List<Vector4> thisOne = new List<Vector4>();
            List<Vector4> properties = new List<Vector4>();
            properties.Add(new Vector4(5f, 1f));
            thisOne.Add(positionSinglePoint);
            Debug.Log(thisOne.Count);
            if (thisOne.Count > 0)
            {
                Material material = heatmapExterior.material;
                material.SetInt("_Points_Length", thisOne.ToArray().Length);
                material.SetVectorArray("_Points", thisOne.ToArray());
                material.SetVectorArray("_Properties", properties.ToArray());
            }

        }


        public void LoadOtherParticipantData(string s, int part, bool webgl)
        {
            string content;
            if (webgl)
            {
                content = s;
            }
            else
            {
                reader = new StreamReader("./Recordings/" + s, Encoding.UTF8, false, 65536);

                content = reader.ReadToEnd();
            }

            //string content = s;
            JObject main = JObject.Parse(content);
            //ReplayManager.Instance.main = JObject.Parse(content);

            JEnumerable<JToken> tokens = main["snapshots"].Children();
            List<List<Vector4>> carPositions = new List<List<Vector4>>();
            List<Vector3> positionThisCar = new List<Vector3>();
            List<Vector3> headList = new List<Vector3>();
            List<Vector3> left = new List<Vector3>();
            List<Vector3> right = new List<Vector3>();
            List<(string, Vector4)> HeatmapPoints = new List<(string, Vector4)>();
            List<Vector4> ExteriorHeatmapPoints = new List<Vector4>();
            List<(string, Vector4)> TouchHeatmapPoints = new List<(string, Vector4)>();
            List<List<Vector3>> AvatarBonePosition = new List<List<Vector3>>();
            List<(bool, List<Vector3>)> PassengerBonePosition = new List<(bool, List<Vector3>)>();

            for (int i = 0; i < tokens.Count<JToken>(); i++)
            {


                JToken snapshots = tokens.ElementAt(i)["other"]["Gaze"]["interior"];
                string tag = (string)snapshots["tag"];
                JToken localCoords = snapshots["localCoords"];
                float x = (float)localCoords["x"];
                float y = (float)localCoords["y"];
                float z = (float)localCoords["z"];
                Vector3 addingvector = new Vector3(x, y, z);
                HeatmapPoints.Add((tag, addingvector));


                JToken snapshotsEx = tokens.ElementAt(i)["other"]["Gaze"]["exterior"];
                JToken localCoordsEx = snapshotsEx["worldCoords"];
                float xEx = (float)localCoordsEx["x"];
                float yEx = (float)localCoordsEx["y"];
                float zEx = (float)localCoordsEx["z"];
                Vector3 addingvectorEx = new Vector3(xEx, yEx, zEx);
                ExteriorHeatmapPoints.Add(addingvectorEx);


                JToken snapshotsTouch = tokens.ElementAt(i)["other"]["HandTracking"];
                string tagTouch = (string)snapshotsTouch["tag"];
                if (snapshotsTouch["localTouchLocation"] != null)
                {

                    JToken localCoordsTouch = snapshotsTouch["localTouchLocation"];
                    float xTouch = (float)localCoordsTouch["x"];
                    float yTouch = (float)localCoordsTouch["y"];
                    float zTouch = (float)localCoordsTouch["z"];
                    Vector3 addingvectorTouch = new Vector3(xTouch, yTouch, zTouch);
                    TouchHeatmapPoints.Add((tagTouch, addingvectorTouch));
                }
                else
                {
                    TouchHeatmapPoints.Add(("InteriorDisplay", new Vector4(1000, 1000, 1000, 1000)));
                }

                JEnumerable<JToken> armature = tokens.ElementAt(i)["other"]["Kinect"]["Bodies"].ElementAt(0)["joints"].Children();
                List<Vector3> AvatarBones = new List<Vector3>();
                foreach (JToken bone in armature)
                {
                    if ((string)bone["joint"] != "WristLeft" && (string)bone["joint"] != "WristRight" && (string)bone["joint"] != "SpineShoulder" && (string)bone["joint"] != "HandTipLeft" && (string)bone["joint"] != "ThumbLeft" && (string)bone["joint"] != "HandTipRight" && (string)bone["joint"] != "ThumbRight")
                    {
                        AvatarBones.Add(new Vector3((float)bone["position"]["x"], (float)bone["position"]["y"], (float)bone["position"]["z"]));
                    }
                    if ((string)bone["joint"] == "Head")
                    {
                        headList.Add(new Vector3((float)bone["position"]["x"], (float)bone["position"]["y"], (float)bone["position"]["z"]));
                    }
                    if ((string)bone["joint"] == "HandLeft")
                    {
                        left.Add(new Vector3((float)bone["position"]["x"], (float)bone["position"]["y"], (float)bone["position"]["z"]));
                    }
                    if ((string)bone["joint"] == "HandRight")
                    {
                        right.Add(new Vector3((float)bone["position"]["x"], (float)bone["position"]["y"], (float)bone["position"]["z"]));
                    }
                }
                AvatarBonePosition.Add(AvatarBones);

                List<Vector3> PassengerBones = new List<Vector3>();
                JEnumerable<JToken> passengerArmature = tokens.ElementAt(i)["other"]["Kinect"]["Bodies"].Children();
                if (passengerArmature.Count<JToken>() > 1)
                {
                    JEnumerable<JToken> passengerarmatureBigger = tokens.ElementAt(i)["other"]["Kinect"]["Bodies"].ElementAt(1)["joints"].Children();
                    foreach (JToken bone in passengerarmatureBigger)
                    {
                        if ((string)bone["joint"] != "WristLeft" && (string)bone["joint"] != "WristRight" && (string)bone["joint"] != "SpineShoulder" && (string)bone["joint"] != "HandTipLeft" && (string)bone["joint"] != "ThumbLeft" && (string)bone["joint"] != "HandTipRight" && (string)bone["joint"] != "ThumbRight")
                        {
                            PassengerBones.Add(new Vector3((float)bone["position"]["x"], (float)bone["position"]["y"], (float)bone["position"]["z"]));
                        }
                    }
                    PassengerBonePosition.Add((true, PassengerBones));
                }
                else
                {
                    for (int o = 0; o < 18; o++)
                    {
                        PassengerBones.Add(new Vector3(0, 0, 0));
                    }
                    PassengerBonePosition.Add((false, PassengerBones));
                }


                JEnumerable<JToken> objects = tokens.ElementAt(i)["objects"].Children();
                List<Vector4> cars = new List<Vector4>();
                foreach (JToken obj in objects) //foreach object
                {
                    //generate the key consisting of id type name (combination of those 3 should be unique)

                    if (obj["type"].ToString() == "Car" || obj["type"].ToString() == "Pickup" || obj["type"].ToString() == "SUV")
                    {
                        JToken position = obj["position"];
                        float xCar = (float)position["x"];
                        float yCar = (float)position["y"];
                        float zCar = (float)position["z"];
                        cars.Add(new Vector3(xCar, yCar, zCar));
                    }

                    if (obj["type"].ToString() == "Tesla")
                    {
                        JToken position = obj["position"];
                        float xCar = (float)position["x"];
                        float yCar = (float)position["y"];
                        float zCar = (float)position["z"];
                        positionThisCar.Add(new Vector3(xCar, yCar, zCar));
                    }
                }
                carPositions.Add(cars);
                // List<Vector4> oldlist = radiusCar[i];
                // oldlist.AddRange(cars);
                // radiusCar[i] = oldlist;
            }


            for (int j = 0; j < mainCarPositions.Count; j++)
            {
                float distanceold = 10000000000f;
                int index = 0;
                for (int k = 0; k < positionThisCar.Count; k++)
                {
                    float distancenew = Vector3.Distance(mainCarPositions[j], positionThisCar[k]);
                    if (distancenew < distanceold)
                    {
                        distanceold = distancenew;
                        index = k;
                    }
                    //float distance =
                }
                List<List<Vector4>> newList = new List<List<Vector4>>();
                newList.Add(carPositions[index]);
                List<List<Vector4>> oldlist = radiusCar[j];
                oldlist.AddRange(newList);
                // for (int i = 0; i < oldlist.Count; i++)
                // {
                //     Debug.Log("i = " + oldlist[i]);
                // }

                radiusCar[j] = oldlist;
            }

            List<Vector3> gazePointsPart = new List<Vector3>();
            for (int i = 0; i < tokens.Count<JToken>(); i++)
            {
                JToken gazeInt = tokens.ElementAt(i)["other"]["Gaze"]["interior"];
                JToken gazeEx = tokens.ElementAt(i)["other"]["Gaze"]["exterior"];
                Vector3 interriorGazepoint = new Vector3((float)gazeInt["localCoords"]["x"], (float)gazeInt["localCoords"]["y"], (float)gazeInt["localCoords"]["z"]);
                Vector3 exterriorGazepoint = new Vector3((float)gazeEx["worldCoords"]["x"], (float)gazeEx["worldCoords"]["y"], (float)gazeEx["worldCoords"]["z"]);

                if (exterriorGazepoint == new Vector3(0f, 0f, 0f))
                {
                    gazePointsPart.Add(interriorGazepoint + mainCarPositions[i]);
                }
                else
                {
                    gazePointsPart.Add(exterriorGazepoint);
                }
            }
            gazePoints.Add(gazePointsPart);

            PassengerBonePositionParticipant.Add(PassengerBonePosition);
            InitInteriorHeatmap(HeatmapPoints, part);
            InitTrajectories(part, headList, left, right);
            InitExteriorBuildingHeatmap(ExteriorHeatmapPoints, part);
            InitAvatar(AvatarBonePosition, part);
            InitTouchInteriorHeatmap(TouchHeatmapPoints, part);
            Debug.Log("End of Loading Participant" + part);
            //UnityManager.INSTANCE.ProgressBar();
            main = null;
            GC.Collect();
            if (part == 2)
            {
                Debug.Log("starting InitMergedAvatar");
                //                UnityManager.INSTANCE.ProgressBar();
                InitMergedAvatar();
                InitMergedPassenger();
                jumptoFrame("0");
                EventController.Instance.SetupPointsOfEventline();
                //                UnityManager.INSTANCE.sendDoneLoading();
            }
            else
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                ReplayManager.Instance.LoadWebFile2("p3-neu.json", 2);
#endif
            }
        }






        Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
        {
            Vector3 dir = point - pivot; // get point direction relative to pivot
            dir = Quaternion.Euler(angles) * dir; // rotate it
            point = dir + pivot; // calculate rotated point
            return point; // return it
        }

        private void setNewBonePosition(GameObject bone, Vector3 boneposition)
        {
            bone.transform.localPosition = boneposition;
            // GameObject cube = Instantiate(nodesCube);
            // Debug.Log(cube);
            // cube.transform.parent = GameObject.Find("Ego Vehicle").transform;
            // cube.transform.position = boneposition;

        }

        private void setNewBoneRotation(GameObject bone, Vector4 bonerotation)
        {
            //Quaternion t = Quaternion.Euler(bonerotation.x, bonerotation.y, bonerotation.z);
            // Quaternion t = new Quaternion(bonerotation.x, bonerotation.y, bonerotation.z, bonerotation.w);
            // bone.transform.rotation = t;

            Quaternion t = new Quaternion(bonerotation.x, bonerotation.y, bonerotation.z, bonerotation.w);
            Quaternion initial = bone.transform.rotation;
            bone.transform.rotation = t * bone.transform.rotation;
            bone.transform.rotation = initial * bone.transform.rotation;
            //bone.transform.Rotate(bonerotation.x, bonerotation.y, bonerotation.z);
        }
        // [Obsolete("See ArrayExtension.ClosestTo")]
        // /// <see cref="ArrayExtension.ClosestTo(IEnumerable{double}, double)"/>
        // int nearestFrame(double stamp)
        // {
        //     // Debug.Log("timestamp: " + compScenePlayer.snapshots[0].timeStamp);
        //     // Debug.Log("stamp: " + stamp);

        //     for (int i = 0; i < compScenePlayer.snapshots.Count; i++)
        //     {
        //         if (compScenePlayer.snapshots[0].timeStamp > stamp)
        //         {
        //             // Debug.Log("Ausgabe 0");
        //             return 0;
        //         }
        //         else if (compScenePlayer.snapshots[i].timeStamp == stamp)
        //         {
        //             // Debug.Log("Ausgabe i");
        //             return i;
        //         }
        //         else if (compScenePlayer.snapshots[i].timeStamp > stamp)
        //         {
        //             // Debug.Log("Ausgabe i-1");
        //             // Debug.Log(i - 1);
        //             return (i - 1);
        //         }

        //     }
        //     // Debug.Log("default");
        //     return (compScenePlayer.snapshots.Count - 1);
        // }

        public void toggleHeatmap(int index)
        {
            if (index < 4)
            {
                for (int i = 0; i < interiorHeatmaps.Count; i++)
                {
                    interiorHeatmaps[i].gameObject.transform.GetChild(index).gameObject.SetActive(!interiorHeatmaps[i].gameObject.transform.GetChild(index).gameObject.activeInHierarchy);
                    interiorTouchHeatmaps[i].gameObject.transform.GetChild(index).gameObject.SetActive(!interiorTouchHeatmaps[i].gameObject.transform.GetChild(index).gameObject.activeInHierarchy);
                }
            }
            else if (index == 4)
            {
                foreach (GameObject heatmap in exteriorBuildings)
                {
                    if (heatmap.name == "Heatmap")
                    {
                        heatmap.SetActive(!heatmap.activeInHierarchy);
                    }
                    else
                    {
                        heatmap.transform.GetChild(1).gameObject.SetActive(!heatmap.transform.GetChild(1).gameObject.activeInHierarchy);
                    }

                }
            }
            else
            {
                radiusHeatmap.gameObject.SetActive(!radiusHeatmap.gameObject.activeInHierarchy);
                radiusHeatmap1.gameObject.SetActive(!radiusHeatmap1.gameObject.activeInHierarchy);
                radiusHeatmap2.gameObject.SetActive(!radiusHeatmap2.gameObject.activeInHierarchy);
            }
        }

        public void toggleTrajectoy(int index)
        {
            for (int i = 0; i < TrajectoriesAll.Count; i++)
            {
                TrajectoriesAll[i].transform.GetChild(index).gameObject.SetActive(!TrajectoriesAll[i].transform.GetChild(index).gameObject.activeInHierarchy);
            }
        }

        public void togglePassenger()
        {
            PassengerController.SetActive(!PassengerController.activeInHierarchy);
        }

        public void toggleDriver()
        {
            for (int i = 0; i < 4; i++)
            {
                AvatarList[i].Item2.SetActive(!AvatarList[i].Item2.activeInHierarchy);
                AvatarList[i] = (!AvatarList[i].Item1, AvatarList[i].Item2);
            }

        }

        public void disableParticipant(int index)
        {
            TrajectoriesAll[index].gameObject.SetActive(false);
            radiuses[index].gameObject.SetActive(false);
            AvatarList[index].Item2.SetActive(false);
            AvatarList[index] = (false, AvatarList[index].Item2);

            interiorHeatmaps[index].gameObject.SetActive(false);
        }
        public void enableParticipant(int index)
        {
            TrajectoriesAll[index].gameObject.SetActive(true);
            radiuses[index].gameObject.SetActive(true);
            AvatarList[index].Item2.SetActive(true);
            AvatarList[index] = (true, AvatarList[index].Item2);
            interiorHeatmaps[index].gameObject.SetActive(true);
        }

        public void changeTrajectoyColor(string s)
        {
            string[] arr;
            arr = s.Split(';');
            Color colorOfString;
            ColorUtility.TryParseHtmlString(arr[1], out colorOfString);
            Color colorAvatar;
            colorAvatar = new Color(colorOfString.r, colorOfString.g, colorOfString.b, 0.4f);
            Color trajectory;
            trajectory = new Color(colorOfString.r, colorOfString.g, colorOfString.b, 1f);
            Trajectories selectedTrajectory = TrajectoriesAll[Int32.Parse(arr[0])].GetComponent<Trajectories>();
            for (int i = 0; i < 3; i++)
            {
                selectedTrajectory.TrajectoriesList[i].startColor = trajectory;
                selectedTrajectory.TrajectoriesList[i].endColor = trajectory;
            }
            AvatarList[Int32.Parse(arr[0])].Item2.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>().material.SetColor("_Color", colorAvatar);
            Debug.Log(arr[1]);
            WebBrowserHandling.Instance.webViewPrefab.WebView.PostMessage("{\"type\": \"Color\",\"participant\": \"" + selectedParticipant + "\", \"message\": \"" + (string)arr[1] + "\"}");
        }

        public void handleChangedColor(InputField input)
        {
            Debug.Log(input.text);
            changeTrajectoyColor(selectedParticipant + ";" + input.text);

        }

        public void toggleColorPicker()
        {
            ColorPicker.SetActive(!ColorPicker.activeInHierarchy);
        }
    }
}
