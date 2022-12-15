using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface that makes a GameObject "recordable"
/// 
/// Add this to an existing script if you need to record values from that script.
/// If you need to reference variables from various scripts or modifying an existing script is not a possibility, implement this interface
/// with a new custom script.
/// </summary>
[Obsolete]
public interface IRecordable {
    
    /// <summary>
    /// ID of the object
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the object you want to record. Can be the object's actual name
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// Type of the object. Could be it's tag in unity or a custom value
    /// </summary>
    public string Type { get; set; }

}
