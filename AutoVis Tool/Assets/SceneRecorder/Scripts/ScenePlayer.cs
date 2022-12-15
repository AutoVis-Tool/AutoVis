using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Replay;

[Obsolete]
public class ScenePlayer : MonoBehaviour
{
    [Tooltip("Path to the file")]
    public string FilePath;     //This path should probably come from somewhere else

    public int CurrentFrame = 0;

    /// <summary>
    /// List of all snapshots
    /// </summary>
    [HideInInspector]
    public List<Snapshot> snapshots;

    /// <summary>
    /// Read the file
    /// </summary>
    private StreamReader reader;

    /// <summary>
    /// Gameobject that holds all the demo prefabs
    /// </summary>
    public GameObject ReplayContainer;

    /// <summary>
    /// Fallback Model
    /// </summary>
    public GameObject FallbackModel;

    /// <summary>
    /// All Models available. Is filled
    /// </summary>
    public List<GameObject> AllModels;

    /// <summary>
    /// UI Text that shows the frame
    /// </summary>
    public Text FrameDisplay;

    /// <summary>
    /// UI Text that shows which object is currently focused
    /// </summary>
    public Text ObjDisplay;

    [HideInInspector]
    public List<GameObject> AllWorldObjectsInFrame;

    /// <summary>
    /// index of the world object pointer
    /// </summary>
    private int woPointer = 0;

    /// <summary>
    /// type of the currently focused object, used for identifying the object
    /// </summary>
    private string focusType;

    /// <summary>
    /// name of the currently focused object, used for identifying the object 
    /// </summary>
    private string focusName;

    /// <summary>
    /// the vector the camera has in regard to the focused game object
    /// </summary>
    private Vector3 focusVec;

    /// <summary>
    /// the time between frames: 1/Tickrate
    /// </summary>
    public float frameTime;

    /// <summary>
    /// Whether or not the scene is autoplaying
    /// </summary>
    public bool playing = true;

    /// <summary>
    /// Color of the Object
    /// </summary>
    public Color color = new Color32(128, 0, 255, 51);


    public Coroutine timeCoroutine;

    void Start()
    {
        LoadModels();
    }



    /// <summary>
    /// Load a web file via path. The default file is 
    /// "./Recordings/Demo_2.json"
    /// Because this is an local server, the path will be localhost:port/path
    /// </summary>
    /// <param name="path"></param>
    public void LoadWebFile(string path)
    {
        StartCoroutine(DownloadFile(path));
    }

    /// <summary>
    /// Load the file with the default path
    /// Consider using this for now as we haven't fully decided on the file loading process yet
    /// </summary>
    /// <param name="path"></param>
    public void LoadWebFile()
    {
        StartCoroutine(DownloadFile(FilePath));
    }

    /// <summary>
    /// Load file from disk.
    /// </summary>
    public void LoadFileFromDisk()
    {
        reader = new StreamReader(FilePath, Encoding.UTF8, false, 65536);

        string content = reader.ReadToEnd();
        LoadRecording(content);
    }

    /// <summary>
    /// Load file from disk with path
    /// </summary>
    public void LoadFileFromDisk(string path)
    {
        reader = new StreamReader(path, Encoding.UTF8, false, 65536);

        string content = reader.ReadToEnd();
        LoadRecording(content);
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
            LoadRecording(content);
        }
    }



    /// <summary>
    /// Load a recording
    /// </summary>
    /// <param name="json">Filepath</param>
    public void LoadRecording(string json)
    {

        //        Debug.Log(json);

        JObject main = JObject.Parse(json);

        //  Debug.Log(main.Value<string>("name"));  //access single value

        frameTime = 1f / main.Value<float>("tickRate");

        IEnumerable<Snapshot> s = main.Values<Snapshot>("snapshots");
        JEnumerable<JToken> t = main["snapshots"].Children();

        snapshots = new List<Snapshot>();

        AllWorldObjectsInFrame = new List<GameObject>();

        foreach (JToken snap in t)
        {
            snapshots.Add(snap.ToObject<Snapshot>());
        }

        LoadFrame(0);
    }


    /// <summary>
    /// Load a specific recorded frame
    /// </summary>
    /// <param name="frame">The frame that should be loaded</param>
    public void LoadFrame(int frame)
    {

        //Avoid out of bounds
        if (frame >= snapshots.Count) frame = 0;
        if (frame < 0) frame = snapshots.Count - 1;

        CurrentFrame = frame;
        ReplayContainer = new GameObject("ReplayContainer");

        FrameDisplay.text = CurrentFrame + "/" + (snapshots.Count - 1);

        Snapshot current = snapshots[CurrentFrame];

        foreach (var wo in current.worldObjects)
        {


            Vector3 pos = new Vector3(wo.transform.position[0], wo.transform.position[1], wo.transform.position[2]);

            Quaternion rot = Quaternion.Euler(wo.transform.rotation[0], wo.transform.rotation[1], wo.transform.rotation[2]);

            //TODO: Find correct object type, remove Demoprefab
            var obj = Instantiate(FindModel(wo.type), pos, rot);


            obj.transform.parent = ReplayContainer.transform;
            obj.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = color;

            obj.GetComponent<ModelController>().Initialize(wo);

            AllWorldObjectsInFrame.Add(obj);

        }

    }

    /// <summary>
    /// Go to next / previous frame
    /// </summary>
    public void NextFrame(int i)
    {
        CurrentFrame += i;

        ReplayManager.Instance.LoadFrameHere(CurrentFrame);

        // CurrentFrame += i;
        // Destroy(ReplayContainer);
        // AllWorldObjectsInFrame.Clear();
        // //woPointer = 0;
        // LoadFrame(CurrentFrame);

        // RefocusOnObject();


    }

    /// <summary>
    /// Attempt to focus on a game object in the current frame that was already focused in the previous frame
    /// </summary>
    public void RefocusOnObject()
    {
        for (int j = 0; j < AllWorldObjectsInFrame.Count; j++)
        {

            var cont = AllWorldObjectsInFrame[j].GetComponent<ModelController>();

            if (cont.name == focusName && cont.Type == focusType)
            {
                woPointer = j;
                FocusWorldObject(0);
                return;
            }

        }
        ObjDisplay.text = "None";
        woPointer = 0;


    }

    /// <summary>
    /// Focus on a recorded world object
    /// </summary>
    /// <param name="i">-1 = previous, 0 = current, +1 = next</param>
    public void FocusWorldObject(int i)
    {
        woPointer += i;

        if (woPointer < 0)
        {
            woPointer = AllWorldObjectsInFrame.Count - 1;
        }

        if (woPointer >= AllWorldObjectsInFrame.Count)
        {
            woPointer = 0;
        }

        if (i != 0)
        {
            focusVec = Camera.main.transform.position - AllWorldObjectsInFrame[woPointer].transform.position;

            focusVec.Normalize();
            focusVec = new Vector3(focusVec.x, 0.5f, focusVec.z);

            Camera.main.transform.position = AllWorldObjectsInFrame[woPointer].transform.position + focusVec * 10;
        }

        ObjDisplay.text = AllWorldObjectsInFrame[woPointer].name;

        focusName = AllWorldObjectsInFrame[woPointer].name;
        focusType = AllWorldObjectsInFrame[woPointer].GetComponent<ModelController>().Type;

        Camera.main.transform.LookAt(AllWorldObjectsInFrame[woPointer].transform);  //TODO: this might need changing if multiple cameras exist

    }

    /// <summary>
    /// Loads all objects of type <see cref="GameObject"/> in the Resources/Models directory
    /// </summary>
    public void LoadModels()
    {
        var loadGOs = Resources.LoadAll<GameObject>("Models");
        AllModels = new List<GameObject>(loadGOs);

    }

    /// <summary>
    /// Find a model to represent a recorded gameobject
    /// </summary>
    /// <param name="type">the type that matches the name of the model</param>
    /// <returns></returns>
    public GameObject FindModel(string type)
    {
        var model = AllModels.FirstOrDefault(o => o.name.Equals(type));
        if (model != null)
            return model;

        return FallbackModel;
    }

    /// <summary>
    /// Play the recording
    /// </summary>
    public void Play()
    {
        playing = true;
        timeCoroutine = StartCoroutine(PlayScene(frameTime));
    }

    /// <summary>
    /// Pause the recording
    /// </summary>
    public void Pause()
    {
        StopCoroutine(timeCoroutine);
        playing = false;
    }


    /// <summary>
    /// Replays the scene at a given time
    /// </summary>
    /// <param name="time">the time</param>
    /// <returns></returns>
    public IEnumerator PlayScene(float time)
    {

        while (playing)
        {

            NextFrame(1);

            yield return new WaitForSeconds(time);
        }

        yield return null;
    }

}
