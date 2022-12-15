using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

/// <summary>
/// See <see cref="Replay.ModelManager"/> & <see cref="Replay.ModelSelect"/>
/// </summary>
[Obsolete]
public class Objectproperties : MonoBehaviour
{

    private UnityManager unitymanager;


    private void Awake()
    {
        unitymanager = GameObject.Find("ScenePlayer").GetComponent<UnityManager>();
    }

    void OnMouseDown()
    {
        GameObject replaycontainer = GameObject.Find("ReplayContainer");

        for (int i = 0; i < replaycontainer.transform.childCount; i++)
        {
            GameObject selected = replaycontainer.transform.GetChild(i).gameObject;
            selected.GetComponent<Outline>().enabled = false;
        }

        //Debug.Log(this.gameObject.transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text);
        // string s = this.gameObject.transform.parent.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text;
        // unitymanager.sendData(s);
        // if (!transform.parent.GetComponent<Outline>().enabled)
        // {
        //     transform.parent.GetComponent<Outline>().enabled = true;
        // }
        // else
        // {
        //     transform.parent.GetComponent<Outline>().enabled = false;
        // }
        transform.parent.GetComponent<Outline>().enabled = true;
        string s = JsonUtility.ToJson(this.gameObject.transform.parent.GetComponent<ModelController>().Record);
        Debug.Log(s);
#if UNITY_WEBGL && !UNITY_EDITOR
        unitymanager.sendData(s);
#endif
    }
}
