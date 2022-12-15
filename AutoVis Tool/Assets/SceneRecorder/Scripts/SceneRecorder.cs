using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;


/// <summary>
/// Script used to record Game Data.
/// </summary>
public class SceneRecorder : MonoBehaviour
{
    [Tooltip("File name of the recording. Consider assigning this automatically")]
    public string RecordingFileName = "Demo_Recording";


    [Tooltip("Path where the Recordings are stored")]
    public string RecordingPath = "./";


    [Tooltip("Add the current date and time at the end of the file (to prevent override)")]
    public bool AddDateToRecording = true;


    [Tooltip("Formatting for the Date string that's added to the file-name")]
    public string DateFormatting = "yyyy-MM-dd_HH-mm";


    [Tooltip("The radius of the sphere created around the target object.")]
    public float DetectionRadius = 20f;

    [Tooltip("How many times per second should the Recorder record the game state")]
    public int TickRate = 20;

    [Tooltip("Interval at which the Recorder writes stored gamelogs to disk")]
    public int BackUpInterval = 100;    //For performance reasons we probably shouldn't save gamestates to disk every frame, thus this counter exists.

    [Tooltip("Whether or not Recording should record from the beginning")]
    public bool RecordOnStart = true;

    [Tooltip("Layers that are recorded")]
    public UnityLayer[] RecordingLayers;

    [Tooltip("Put GameObjects you always want to record here. These scripts should still implement the IRecordable interface.")]
    public GameObject[] AlwaysRecordGameObjects;

    [Tooltip("Put the GameObject that holds scripts that should always be recorded here. Can be multiple scripts per GameObject. These scripts should implement the IRecordable interface. Doesn't store GameObject position.")]
    public GameObject[] ScriptRecordings;

    /// <summary>
    /// Layermask calculated based on <see cref="RecordingLayers"/>
    /// </summary>
    [HideInInspector]
    public LayerMask RecordingMask;

    [Tooltip("The logging target, e.g. the player: Everything in a defined radius around the player is recorded")]
    public Transform RecordingSphereCenter;

    [Tooltip("If we want to fake the starting date for debugging purposes (to sync it with other data), set this to true")]
    public bool UseFakeStartingDate = false;

    [Tooltip("Set the Fake Starting Unix Time here. In Seconds.Milliseconds")]
    public double FakeStartingUnixTime;

    /// <summary>
    /// Components of game objects that shall always be recorded, see <see cref="AlwaysRecordGameObjects"/>
    /// </summary>
    private List<Component> alwaysRecordComponents = new List<Component>();


    /// <summary>
    /// Script components of <see cref="ScriptRecordings"/>
    /// </summary>
    private List<Component> scriptRecordComponents = new List<Component>();

    /// <summary>
    /// Tick interval in seconds
    /// </summary>
    private float tickInterval = 1;

    /// <summary>
    /// +1 every time a Snapshot is saved. Reset once it's greater than <see cref="BackUpInterval"/>
    /// </summary>
    private int flushCounter = 0;


    /// <summary>
    /// Writes the logs to disk
    /// </summary>
    private StreamWriter streamWriter;

    /// <summary>
    /// Unix offset, is set if <see cref="UseFakeStartingDate"/> is true;
    /// </summary>
    private double unixOffset = 0;

    void Start()
    {

        if (RecordingLayers.Length == 0)
        {
            Debug.LogError("At least 1 Logging Layer is required for recording!");

        }

        //Calculate the layer mask based on layers to log
        foreach (var layer in RecordingLayers)
        {
            RecordingMask = RecordingMask | layer.Mask;
        }

        //Add components to always record list
        foreach (GameObject o in AlwaysRecordGameObjects)
        {
            Component rec;

            if (o.TryGetComponent(typeof(RecordableMonoBehavior), out rec))
            {
                alwaysRecordComponents.Add(rec);
            }
        }


        foreach (GameObject o in ScriptRecordings)
        {

           // scriptRecordComponents.AddRange(o.GetComponents(typeof(IRecordable)));
        }


        if (RecordOnStart)
        {
            StartRecording();

        }

    }

    public void StartRecording()
    {
        StartCoroutine(Record());
    }

    /// <summary>
    /// This recording coroutine records the scene
    /// </summary>
    /// <returns></returns>
    IEnumerator Record()
    {

        yield return new WaitForSeconds(0.05f);

        if (UseFakeStartingDate)
        {
            unixOffset = GetUnixTimeStamp() - FakeStartingUnixTime;
        }

        tickInterval = 1f / TickRate;

        streamWriter = new StreamWriter(RecordingPath + RecordingFileName + "_" + (AddDateToRecording ? DateTime.Now.ToString(DateFormatting) : "") + ".json", true, Encoding.UTF8, 65536);

        streamWriter.Write("{ \"name\": \"" + RecordingFileName + "\", \"tickRate\":" + TickRate + ", \"snapshots\": [");



        while (true)
        {
            JObject currentFrame = new JObject();
            currentFrame["timeStamp"] = GetUnixTimeStamp();

            Collider[] hitColliders = Physics.OverlapSphere(RecordingSphereCenter.position, DetectionRadius, RecordingMask);


            //Log game objects
            JArray gameObjects = new JArray();

            foreach (var comp in alwaysRecordComponents)
            {
                string json = JsonUtility.ToJson(comp);
                gameObjects.Add(JObject.Parse(json));
            }
                        
            foreach (var col in hitColliders)
            {
                Component rec;

                if (col.transform.TryGetComponent(typeof(RecordableMonoBehavior), out rec))
                {
                    string json = JsonUtility.ToJson(rec);
                    gameObjects.Add(JObject.Parse(json));
                }
            }
            currentFrame["objects"] = gameObjects;


            //Log additonal scripts
            //TODO: Add this back later
            /*   foreach (var comp in scriptRecordComponents)
               {

                   snapshot.otherData.Add(new ScriptRecord(comp));
               }

            */

            string toWrite = currentFrame.ToString();

            streamWriter.WriteLine(toWrite + ",");

            flushCounter += 1;

            if (flushCounter >= BackUpInterval)
            {
                StartCoroutine(Write());
            }
            yield return new WaitForSeconds(tickInterval);
        }
    }


    IEnumerator Write()
    {
        streamWriter.Flush();
        flushCounter = 0;

        yield return null;
    }



    public void StopRecording()
    {

        streamWriter.WriteLine("]}");
        streamWriter.Close();


        StopAllCoroutines();

    }

    private void OnApplicationQuit()
    {
        StopRecording();
    }


    private double GetUnixTimeStamp()
    {
        return DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds - unixOffset;
        //return DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - unixOffset;
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(RecordingSphereCenter.position, DetectionRadius);
    }

}
