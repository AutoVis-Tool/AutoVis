using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
[System.Serializable]
public class Snapshot 
{
    /// <summary>
    /// TimeStamp of recording
    /// </summary>
    public double timeStamp;

    /// <summary>
    /// All worldobjects that were recorded
    /// </summary>
    public List<ObjectRecord> worldObjects = new List<ObjectRecord>();

    /// <summary>
    /// Other data, e.g. input from measuring heart rate to make it easier to track
    /// </summary>
    public List<ScriptRecord> otherData = new List<ScriptRecord>();

}
