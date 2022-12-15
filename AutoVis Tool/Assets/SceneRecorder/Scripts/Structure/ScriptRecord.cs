using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ScriptRecord
{
    /// <summary>
    /// ID of an object. Based on the <see cref="IRecordable"/> interface
    /// </summary>
    public int id;
    /// <summary>
    /// Name of an object. Based on the <see cref="IRecordable"/> interface
    /// </summary>
    public string name;

    /// <summary>
    /// Name of an object. Based on the <see cref="IRecordable"/> interface
    /// </summary>
    public string type;

    /// <summary>
    /// Additional data coming from the object. This can be deserialized using <see cref="JObject.Parse(string)"/>
    /// </summary>
    public string data;


    public ScriptRecord(Component comp) {

        var rec = comp as IRecordable;

        id = rec.Id;
        name = rec.Name;
        type = rec.Type;

        data = JsonUtility.ToJson(rec);

    }
}
