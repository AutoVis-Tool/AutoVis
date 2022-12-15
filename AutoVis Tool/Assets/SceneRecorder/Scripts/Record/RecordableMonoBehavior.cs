using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Implement this class instead of MonoBehavior; This was we can ask for instance of this class and parse this entire class
/// as one string
/// </summary>
public abstract class RecordableMonoBehavior : MonoBehaviour, ISerializationCallbackReceiver
{
    public static int idCounter;

    public string name;
    public string type;
    public int id;

    /// <summary>
    /// position of the object
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// rotation of the object
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// size of the object
    /// </summary>
    public Vector3 size;

    public Renderer MeshRenderer;

    public virtual void Start()
    {
        id = idCounter;
        idCounter++;

        name = gameObject.name;
    }

    public virtual void OnAfterDeserialize()
    {
        //?
    }

    public virtual void OnBeforeSerialize()
    {
        position = transform.position;
        rotation = transform.rotation.eulerAngles;
        //size = MeshRenderer.bounds.size;  //We shouldnt do this because it will cause issues because size changes 

    }

}
