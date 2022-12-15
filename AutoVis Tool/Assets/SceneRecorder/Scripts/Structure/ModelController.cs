using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that takes care of modifying the model
/// </summary>
[Obsolete]
public class ModelController : MonoBehaviour
{
    /// <summary>
    /// Global Modifiers
    /// TODO: Theoretically, we could add a second array/ list that holds the modifiers that actually can be applied to this model
    /// </summary>
    public static string[] GLOBALMODIFIERS = { "COLOR", "SIZE" };

    /// <summary>
    /// The actual model
    /// </summary>
    public GameObject Model;

    /// <summary>
    /// This shows all the info of the Model
    /// </summary>
    public PropertiesBillboard Billboard;

    /// <summary>
    /// Model Type
    /// </summary>
    public string Type;

    /// <summary>
    /// The object record
    /// </summary>
    public ObjectRecord Record;

    /// <summary>
    /// pre-formatted billboard text
    /// </summary>
    public string BillBoardText { get => Billboard.BillboardText.text; }

    public void Initialize(ObjectRecord record)
    {
        string rs = "Name: " + record.name + "\nType: " + record.type + "\nID: " + record.id + "\n";
        JObject d = JObject.Parse(record.data);
       
        
        foreach (JProperty prop in d.Properties()) {
            rs += "\n"+prop.Name + ": " + prop.Value;

            if( Array.IndexOf(GLOBALMODIFIERS, prop.Name.ToUpper()) > -1) { //If property is a global modifier; Alternatively: If modifier is in in models modifier list
                ApplyModifiers(this, prop.Name, prop.Value);    //Apply the modifier
            }
        }
        

        Record = record;
        Billboard.SetText(rs);

        name = record.name;
        Type = record.type;
    }

    /// <summary>
    /// Applies modifiers tot he Model
    /// </summary>
    /// <param name="controller">The Model</param>
    /// <param name="modifier">The modifier</param>
    /// <param name="value">the value we should modify the property by</param>
    public void ApplyModifiers(ModelController controller, string modifier, JToken value) {

        switch (modifier.ToUpper()) {
            case "COLOR":                
                  Color tmp;
                if (ColorUtility.TryParseHtmlString("" + value, out tmp))
                {
                    controller.Model.GetComponent<Renderer>().material.color = tmp;
                }

                break;
            case "SIZE":
                Vector3 test = (Vector3) value.ToObject(typeof(Vector3));
                Model.transform.localScale = test;
                Model.transform.localPosition += new Vector3(0, test.y / 2, 0);
                break;
            default:
                Debug.Log("" + value);
                break;
        }
    }


}
