using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Demo Script attached to the DemoCube to demonstrate what the implementation of the <see cref="IRecordable"/> interface could look like.
/// </summary>
public class DemoCube : RecordableMonoBehavior
{
   
    

    //Additional variables that come from the script can be added here
    [SerializeField]
    private string sampleInfo = "";

    [SerializeField]
    private int numberField = 0;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
