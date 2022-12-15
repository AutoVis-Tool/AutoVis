using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DemoAlwaysRecord : MonoBehaviour, IRecordable
{
    
    public int Id { get => 4; set => throw new System.NotImplementedException(); }
    public string Name { get => "SampleData"; set => throw new System.NotImplementedException(); }
    public string Type { get => "Graph"; set => throw new System.NotImplementedException(); }


    [SerializeField]
    public float fps;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        fps = 1 / Time.deltaTime;
    }
}
