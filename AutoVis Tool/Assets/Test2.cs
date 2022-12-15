using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using System;


public class Test2 : MonoBehaviour, IPointerClickHandler
{
    public Material newMat;
    //Detect if a click occurs
    //public void OnPointerDown(PointerEventData pointerEventData)
    //{
    //    Debug.Log(name + " Game Object Clicked!");
    //    gameObject.GetComponent<MeshRenderer>().material = newMat;
    //}

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log(name + " Game Object Clicked!");
        gameObject.GetComponent<MeshRenderer>().material = newMat;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void testSync()
    {
        GameObject localPlayer = GameObject.Find("Local Player");
        localPlayer.GetComponent<Player>().setPlayerTimestamp(Random.Range(0f, 100f));
    }
}
