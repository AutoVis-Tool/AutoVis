using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class spawnSettingsPopup : MonoBehaviour, IPointerDownHandler
{
    public GameObject settingsPopup;
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        //Debug.Log(name + " Game Object Clicked!");
        //GameObject newPopup = Instantiate(settingsPopup);
        //Vector3 clickedPosition = pointerEventData.pointerCurrentRaycast.worldPosition;
        //newPopup.transform.position = clickedPosition;
    }
}
