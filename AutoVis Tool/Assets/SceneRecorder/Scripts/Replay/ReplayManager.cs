using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Replay
{
    public class ReplayManager : MonoBehaviour
    {
        /// <summary>
        /// Array of all TimeStamps
        /// Used for finding the nearest
        /// </summary>
        [SerializeField]
        public double[] TimeStamps;

        /// <summary>
        /// A list of all available models
        /// </summary>
        public List<GameObject> ReplayModels
        {
            get => ModelManager.ReplayModels;
        }

        /// <summary>
        /// List of all Types
        /// </summary>
        public List<string> Types
        {
            get => ModelManager.Types;
        }

        /// <summary>
        /// Dictionary with unique string as key and Model as value
        /// Holds all models
        /// </summary>
        public Dictionary<string, ModelController> Models
        {
            get => ModelManager.Models;
        }

        /// <summary>
        /// Dictionary that holds frames as keys and list of model reference. This makes updating the correct models super easy
        /// </summary>
        public Dictionary<double, List<ModelController>> timeStampModels
        {
            get => ModelManager.timeStampModels;
        }


        /// <summary>
        /// Used for reading the json file
        /// </summary>
        private StreamReader reader;

        /// <summary>
        /// Time between frames, dictates playback Speed
        /// </summary>
        private float frameTime;

        /// <summary>
        /// If the scene is currently playing or not
        /// </summary>
        public bool playing;

        /// <summary>
        /// The frame we're currently at
        /// </summary>
        public int CurrentFrame = 0;

        /// <summary>
        /// The TimeStamp we're currently at
        /// </summary>
        public double CurrentTimeStamp;

        public static ReplayManager Instance;

        public ModelManager ModelManager;

        public GameObject timeline;

        // public JObject main;

        void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {



        }

        private void Update()
        {
            if(Input.GetKeyDown("space"))
            {
                if(playing)
                {
                    Pause();
                } else
                {
                    Play(1);
                }
            }

            if (Input.GetKeyDown("y"))
            {
                GoToNearestTimeStamp(CurrentTimeStamp - 0.2);
            }

            if (Input.GetKeyDown("x"))
            {
                GoToNearestTimeStamp(CurrentTimeStamp + 0.2);
            }

            if (Input.GetKeyDown("1"))  // Pointing
            {
                GoToNearestTimeStamp(1662208929.9406);
            }

            if(Input.GetKeyDown("2"))   // Lombard Street
            {
                GoToNearestTimeStamp(1662208561.01305);
            }

            if(Input.GetKeyDown("3"))   // Cyclist
            {
                GoToNearestTimeStamp(1662208870.95433);
            }

            if (Input.GetKeyDown("c"))
            {
                SliceManager.Instance.ToggleSlice();
            }
        }

        /// <summary>
        /// Initializes a recording from a json string.
        /// </summary>
        /// <param name="json">The recording json string</param>
        public void InitRecording(string json)
        {
            //  UnityManager.INSTANCE.ProgressBar();
            // Participant participant = new Participant();

            // //string jsonString = File.ReadAllText("./Recordings/Demo_Recording_2022-09-03_14-33.json");
            // participant = JsonUtility.FromJson<Participant>(json);
            JObject main = JObject.Parse(json); //Parse json as main object

            frameTime = 1f / main.Value<float>("tickRate"); //Default playback speed


            JEnumerable<JToken> tokens = main["snapshots"].Children();  //get all recorded frames

            List<double> ts = new List<double>();   //list of timestamps
            List<Vector3> headList = new List<Vector3>();
            List<Vector3> left = new List<Vector3>();
            List<Vector3> right = new List<Vector3>();
            List<(string, Vector4)> HeatmapPoints = new List<(string, Vector4)>();
            List<(string, Vector4)> TouchHeatmapPoints = new List<(string, Vector4)>();
            List<Vector4> ExteriorHeatmapPoints = new List<Vector4>();

            List<List<Vector3>> AvatarBonePosition = new List<List<Vector3>>();
            List<bool> CurrentlyPointing = new List<bool>();
            List<(bool, List<Vector3>)> PassengerBonePosition = new List<(bool, List<Vector3>)>();

            foreach (JToken t in tokens)    //foreach recorded frame
            {


                double timeStamp = t.Value<double>("timeStamp");    //add timestamp to array for easier access
                ts.Add(timeStamp);
                if (t["other"]["Gaze"]["interior"] != null)
                {

                JToken snapshots = t["other"]["Gaze"]["interior"];
                string tag = (string)snapshots["tag"];
                JToken localCoords = snapshots["localCoords"];
                float x = (float)localCoords["x"];
                float y = (float)localCoords["y"];
                float z = (float)localCoords["z"];
                Vector3 addingvector = new Vector3(x, y, z);
                HeatmapPoints.Add((tag, addingvector));

                }

                if (t["other"]["Gaze"]["exterior"] != null)
                {
                    JToken snapshotsEx = t["other"]["Gaze"]["exterior"];
                    JToken localCoordsEx = snapshotsEx["worldCoords"];
                    float xEx = (float)localCoordsEx["x"];
                    float yEx = (float)localCoordsEx["y"];
                    float zEx = (float)localCoordsEx["z"];
                    Vector3 addingvectorEx = new Vector3(xEx, yEx, zEx);
                    ExteriorHeatmapPoints.Add(addingvectorEx);
                }
                // JToken snapshotsTouch = t["other"]["HandTracking"];
                // string tagTouch = (string)snapshotsTouch["tag"];
                // JToken localCoordsTouch = snapshotsTouch["localTouchLocation"];
                // float xTouch = (float)localCoordsTouch["x"];
                // float yTouch = (float)localCoordsTouch["y"];
                // float zTouch = (float)localCoordsTouch["z"];
                // Vector3 addingvectorTouch = new Vector3(xTouch, yTouch, zTouch);
                // TouchHeatmapPoints.Add((tag, addingvectorTouch));
                TouchHeatmapPoints.Add(("InteriorDisplay", new Vector4(1000, 1000, 1000, 1000)));
                if (t["other"]["HandTracking"]["exterior"] != null)
                {
                    JToken snapshotsPointing = t["other"]["HandTracking"]["exterior"];
                    CurrentlyPointing.Add((bool)t["other"]["HandTracking"]["CurrentlyPointing"]);
                }


                // JEnumerable<JToken> armature = t["other"]["Kinect"]["Bodies"].Children();

                List<Vector3> AvatarBones = new List<Vector3>();
                JEnumerable<JToken> armature = t["other"]["Kinect"]["Bodies"].ElementAt(0)["joints"].Children();
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
                JEnumerable<JToken> passengerArmature = t["other"]["Kinect"]["Bodies"].Children();
                if (passengerArmature.Count<JToken>() > 1)
                {
                    JEnumerable<JToken> passengerarmatureBigger = t["other"]["Kinect"]["Bodies"].ElementAt(1)["joints"].Children();
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





                JEnumerable<JToken> objects = t["objects"].Children();  //List of all objects in one frame

                List<ModelController> modelsInFrame = new List<ModelController>();
                List<Vector4> cars = new List<Vector4>();
                bool firstvalue = true;
                foreach (JToken obj in objects) //foreach object
                {
                    string key = obj["id"].ToString() + "_" + obj["type"].ToString() + "_" + obj["name"].ToString();    //generate the key consisting of id type name (combination of those 3 should be unique)

                    ModelController controller;
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
                        if (firstvalue)
                        {
                            JavaScriptManager.instanceJS.startRotation = new Vector3((float)obj["rotation"]["x"], (float)obj["rotation"]["y"], (float)obj["rotation"]["z"]);
                            firstvalue = false;
                        }
                        JToken position = obj["position"];
                        float xCar = (float)position["x"];
                        float yCar = (float)position["y"];
                        float zCar = (float)position["z"];
                        JavaScriptManager.instanceJS.mainCarPositions.Add(new Vector3(xCar, yCar, zCar));
                    }
                    //Get the Model that represents the gameobject based on the key
                    if (Models.TryGetValue(key, out controller)) //If the Model was already created
                    {
                        controller.AddDataTimeStamp(obj, timeStamp);    //Add the recorded data as a timestamp
                    }
                    else //otherwise instantiate a new model and add it to the Models list
                    {

                        var go = Instantiate(ModelManager.FindModel(obj["type"].ToString()));    //Find the fitting model

                        //Initial model setup
                        controller = go.GetComponent<ModelController>();
                        controller.Setup(obj, timeStamp);
                        Models.Add(key, controller);    //add model controller to Model Dictionary

                        if (!Types.Contains(obj.Value<string>("type")))
                        {
                            Types.Add(obj.Value<string>("type"));
                        }

                    }

                    modelsInFrame.Add(controller);  //Add to models in frame

                }

                List<List<Vector4>> newList = new List<List<Vector4>>();
                newList.Add(cars);
                JavaScriptManager.instanceJS.radiusCar.Add(newList);
                timeStampModels.Add(timeStamp, modelsInFrame);    //Add timestamp


            }

            List<Vector3> gazePointsPart = new List<Vector3>();

            if(tokens.ElementAt(0)["other"]["Gaze"]["interior"]!= null)
            {
                for (int i = 0; i < tokens.Count<JToken>(); i++)
                {
                    JToken gazeInt = tokens.ElementAt(i)["other"]["Gaze"]["interior"];
                    JToken gazeEx = tokens.ElementAt(i)["other"]["Gaze"]["exterior"];
                    Vector3 interriorGazepoint = new Vector3((float)gazeInt["localCoords"]["x"], (float)gazeInt["localCoords"]["y"], (float)gazeInt["localCoords"]["z"]);
                    Vector3 exterriorGazepoint = new Vector3((float)gazeEx["worldCoords"]["x"], (float)gazeEx["worldCoords"]["y"], (float)gazeEx["worldCoords"]["z"]);

                    if (exterriorGazepoint == new Vector3(0f, 0f, 0f))
                    {
                        gazePointsPart.Add(interriorGazepoint + JavaScriptManager.instanceJS.mainCarPositions[i]);
                    }
                    else
                    {
                        gazePointsPart.Add(exterriorGazepoint);
                    }
                }
                JavaScriptManager.instanceJS.gazePoints.Add(gazePointsPart);
            }



            List<Vector3> timelinePositions = new List<Vector3>();
            foreach(Vector3 carPosition in JavaScriptManager.instanceJS.mainCarPositions)
            {
                timelinePositions.Add(carPosition + new Vector3(0f, 2.2f, 0f));
            }
            timeline.GetComponent<LineRenderer>().positionCount = timelinePositions.Count;
            timeline.GetComponent<LineRenderer>().SetPositions(timelinePositions.ToArray());


            TimeStamps = ts.ToArray();

            //We assign previous here once with the very last frame, this is done to avoid potential nullpointers
            //timeStampModels.TryGetValue(TimeStamps[CurrentFrame == 0 ? TimeStamps.Length - 1 : CurrentFrame - 1], out previous);    //Assign previous 
            previous = new List<ModelController>();


            foreach (var model in Models.Values)
            {

                model.FinalizeSetup();
            }

            ModelManager.ForwardData();
            //LoadFrame(0);
            // JavaScriptManager.instanceJS.headArmatureList.Add(headList);
            // JavaScriptManager.instanceJS.leftHandArmatureList.Add(left);
            // JavaScriptManager.instanceJS.rightHandArmatureList.Add(right);

            // GameObject headTrajectory = Instantiate(JavaScriptManager.instanceJS.Line);
            // GameObject lefthandTrajectory = Instantiate(JavaScriptManager.instanceJS.Line);
            // GameObject righthandTrajectory = Instantiate(JavaScriptManager.instanceJS.Line);
            // Color32 c = new Color32(0, 200, 200, 255);
            // Color32 c1 = new Color32(0, 150, 150, 255);
            // Color32 c2 = new Color32(0, 255, 255, 255);
            // headTrajectory.GetComponent<LineRenderer>().startColor = c2;
            // headTrajectory.GetComponent<LineRenderer>().endColor = c2;
            // lefthandTrajectory.GetComponent<LineRenderer>().startColor = c;
            // lefthandTrajectory.GetComponent<LineRenderer>().endColor = c;
            // righthandTrajectory.GetComponent<LineRenderer>().startColor = c1;
            // righthandTrajectory.GetComponent<LineRenderer>().endColor = c1;
            // JavaScriptManager.instanceJS.Lines.Add(headTrajectory);
            // JavaScriptManager.instanceJS.Lines.Add(lefthandTrajectory);
            // JavaScriptManager.instanceJS.Lines.Add(righthandTrajectory);

            JavaScriptManager.instanceJS.PassengerBonePositionParticipant.Add(PassengerBonePosition);
            JavaScriptManager.instanceJS.InitInteriorHeatmap(HeatmapPoints, 0);
            JavaScriptManager.instanceJS.InitTrajectories(0, headList, left, right);
            JavaScriptManager.instanceJS.InitExteriorBuildingHeatmap(ExteriorHeatmapPoints, 0);
            JavaScriptManager.instanceJS.InitAvatar(AvatarBonePosition, 0);
            JavaScriptManager.instanceJS.InitTouchInteriorHeatmap(TouchHeatmapPoints, 0);
            JavaScriptManager.instanceJS.mapBuilderCustom.CurrentlyPointingList = CurrentlyPointing;

            // disabled for now until new Map
            JavaScriptManager.instanceJS.mapBuilderCustom.startCreatingMaterial();



            //UnityManager.INSTANCE.ProgressBar();
            // JavaScriptManager.instanceJS.LoadOtherParticipantData("Mark - All.json", 1);
            // JavaScriptManager.instanceJS.LoadOtherParticipantData("Omid - All.json", 2);
            Debug.Log("starting loading other participants");
            LoadTimeStamp(TimeStamps[0]);
            main = null;
            GC.Collect();
#if UNITY_WEBGL && !UNITY_EDITOR
                                                LoadWebFile2("p2-neu.json", 1);

                                                
#else
            JavaScriptManager.instanceJS.LoadOtherParticipantData("p2-neu.json", 1, false);
            JavaScriptManager.instanceJS.LoadOtherParticipantData("p3-neu.json", 2, false);



#endif
            // LoadWebFile2("Mark - All.json", 1);
            // LoadWebFile2("Omid - All.json", 2);







            //LoadTimeStamp(TimeStamps[0]);
            //GoToNearestTimeStamp(1662208558.0955784);
            //JavaScriptManager.instanceJS.createOtherParticipantModel();

        }


        /// <summary>
        /// Start playing the scene
        /// </summary>
        /// <param name="playBackMultiplier">The playback speed multiplier, 1x, 1.25x, 2x etc.</param>
        public void Play(float playBackMultiplier)
        {
            StopAllCoroutines();
            playing = true;
            StartCoroutine(PlayScene(frameTime / playBackMultiplier));

        }

        /// <summary>
        /// Pauses the Replay
        /// </summary>
        public void Pause()
        {
            Debug.Log("Pause");
            playing = false;
            StopAllCoroutines();
        }

        List<ModelController> current;
        List<ModelController> previous;

        IEnumerator PlayScene(float playBackSpeed)
        {
            Debug.Log("Play " + playing);
            while (playing)
            {
                CurrentTimeStamp = TimeStamps[CurrentFrame];
                Debug.Log(CurrentTimeStamp + " " + CurrentFrame);
                GoToNearestTimeStamp(CurrentTimeStamp);

                //TODO: Alert Website about current frame
                //

                yield return new WaitForSeconds(playBackSpeed);

                CurrentFrame++;

                if (CurrentFrame >= TimeStamps.Length)
                {
                    CurrentFrame = 0;
                }


            }

            yield return null;

        }

        /// <summary>
        /// Load the timestamp frame closest to the given timeStamp
        /// </summary>
        /// <param name="timeStamp"></param>
        public void GoToNearestTimeStamp(double timeStamp)
        {
            double time = TimeStamps.ClosestTo(timeStamp);
            LoadTimeStamp(time);
            CurrentTimeStamp = time;
            CurrentFrame = Array.IndexOf(TimeStamps, time);
            //Debug.Log("cf  " + CurrentFrame);
            DrawAllFunctions(CurrentFrame);

            //JavaScriptManager.instanceJS.ExteriorHeatmap();
        }

        public void LoadFrameHere(int frame)
        {
            CurrentFrame = frame;
            double time = TimeStamps[frame];
            // Debug.Log(CurrentFrame);
            LoadTimeStamp(time);
            DrawAllFunctions(CurrentFrame);

        }

        public void DrawAllFunctions(int frame)
        {
            JavaScriptManager.instanceJS.DrawHeatmaps(frame);
            //   Debug.Log("Fertig 1");
            JavaScriptManager.instanceJS.DrawTouchHeatmaps(frame);
            //  Debug.Log("Fertig 2");
            JavaScriptManager.instanceJS.updateHeatmapCarRadius(frame);
            //  Debug.Log("Fertig 3");
            JavaScriptManager.instanceJS.DrawTrajectories(frame);
            // Debug.Log("Fertig 4");
            JavaScriptManager.instanceJS.DrawAvatarMovement(frame);
            //  Debug.Log("Fertig 5");
            JavaScriptManager.instanceJS.DrawPassenger(frame);
            //  Debug.Log("Fertig 6");
            JavaScriptManager.instanceJS.mapBuilderCustom.DrawExteriorBuildingHeatmap(frame);
            //   Debug.Log("Fertig 7");
            JavaScriptManager.instanceJS.mapBuilderCustom.DrawCyclistHeatmap(frame);
        }

        /// <summary>
        /// Load Models at given TimeStamp
        /// </summary>
        /// <param name="timeStamp">Should be an existing timestamp. Otherwise nothing will happen</param>
        public void LoadTimeStamp(double timeStamp)
        {
            if (timeStampModels.TryGetValue(timeStamp, out current))  //get list of objects in current frame
            {
                GameObject localPlayer = GameObject.Find("Local Player");
                if (localPlayer != null)
                {
                    localPlayer.GetComponent<Player>().setPlayerTimestamp(timeStamp);
                }
                foreach (var model in current)
                {
                    model.UpdateModel(timeStamp);    //foreach in current frame update them according to frame
                }

                foreach (var inActive in previous.Except(current))   //foreach that were seen in previous frame but are no longer here: set them to inactive
                {
                    inActive.Deactivate();
                }

                previous = current; //Set Current to Previous so whatever frame/timestamp we go to next, we will disable stuff we don't want to see
            }
        }





        public void SkipFrames(int i)
        {
            CurrentFrame += i;

            LoadFrameHere(CurrentFrame);
            // CurrentFrame += i;

            // if (CurrentFrame >= TimeStamps.Length)
            // {
            //     CurrentFrame = 0;
            // }
            // else if (CurrentFrame < 0)
            // {
            //     CurrentFrame = TimeStamps.Length - 1;
            // }

            // CurrentTimeStamp = TimeStamps[CurrentFrame];

            // LoadTimeStamp(CurrentTimeStamp);

        }



        /// <summary>
        /// Load file from disk with path
        /// </summary>
        public void LoadFileFromDisk(string path)
        {
            reader = new StreamReader(path, Encoding.UTF8, false, 65536);

            string content = reader.ReadToEnd();
            Debug.Log(content);
            InitRecording(content);
        }

        /// <summary>
        /// Load a web file via path. 
        /// Because this is an local server, the path will be localhost:port/path
        /// </summary>
        /// <param name="path"></param>
        public void LoadWebFile(string path)
        {
            StartCoroutine(DownloadFile(path));
        }

        public void LoadWebFile2(string path, int part)
        {
            StartCoroutine(DownloadFile2(path, part));
        }


        /// <summary>
        /// Download file from a server
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        IEnumerator DownloadFile(string url)
        {

            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);

            string path = Path.Combine(Application.persistentDataPath, "3dlog.json");

            uwr.downloadHandler = new DownloadHandlerFile(path);

            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                Debug.Log("File successfully downloaded and saved to " + path);
                reader = new StreamReader(path, Encoding.UTF8, false, 65536);

                string content = reader.ReadToEnd();
                InitRecording(content);
            }
        }


        IEnumerator DownloadFile2(string url, int part)
        {

            var uwr = new UnityWebRequest("./Recordings/" + url, UnityWebRequest.kHttpVerbGET);

            string path = Path.Combine(Application.persistentDataPath, "3dlog.json");

            uwr.downloadHandler = new DownloadHandlerFile(path);

            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(uwr.error);
            }
            else
            {
                Debug.Log("File successfully downloaded and saved to " + path);
                reader = new StreamReader(path, Encoding.UTF8, false, 65536);

                string content = reader.ReadToEnd();
                JavaScriptManager.instanceJS.LoadOtherParticipantData(content, part, true);

            }
        }
    }
}