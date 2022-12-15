using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Replay;

public class SingleEventPoint : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    // Start is called before the first frame update
    private Material saveMaterial;


    private bool isClicked = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        GameObject rig = GameObject.Find("VROrigin");
        GameObject parent = rig.transform.parent.gameObject;
        rig.transform.parent = null;
        if (transform.parent.parent.parent.tag == "MainEventLine")
        {
            EventController.Instance.DisableAllParticipantEvents();
            SingleParticipantEventHandler single = transform.parent.parent.parent.parent.GetComponent<SingleParticipantEventHandler>();
            EventController.Instance.EnableParticipantEvent(single.ParticipantEventId);
            ReplayManager.Instance.GoToNearestTimeStamp(transform.parent.GetComponent<SingleEventData>().EventTime);
            transform.parent.parent.parent.parent.GetComponent<SingleParticipantEventHandler>().handleOnClick();
            isClicked = !isClicked;
            if (!isClicked)
            {
                EventController.Instance.loop = false;
            }
        } else
        {
            SingleEventData single = transform.parent.GetComponent<SingleEventData>();
            EventController.Instance.selectedEvent = transform.parent.gameObject;
            EventController.Instance.selectedStartTime = single.EventTime;
            EventController.Instance.selectedEndTime = single.EventEndTime;
            EventController.Instance.loop = true;
            EventController.Instance.HandleEvent();
        }
        rig.transform.parent = parent.transform;


        //Output to console the clicked GameObject's name and the following message. You can replace this with your own actions for when clicking the GameObject.
        // Debug.Log(name + " Game Object Clicked!");
        // Debug.Log(transform.parent.name);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        Material mat = transform.parent.GetComponent<LineRenderer>().materials[0];
        saveMaterial = mat;
        Material newMat = new Material(mat);
        if (mat.color.r > 50f)
        {
            Color32 c = mat.color;
            Color c2 = new Color32((byte)(c.r - 50), c.g, c.b, c.a);
            newMat.color = c2;
        }
        else
        {
            Color32 c = mat.color;
            Color c2 = new Color32((byte)(c.r + 50), c.g, c.b, c.a);
            newMat.color = c2;
        }
        transform.parent.GetComponent<LineRenderer>().material = newMat;
        if (transform.parent.parent.parent.tag != "MainEventLine")
        {

            SingleEventData single = transform.parent.GetComponent<SingleEventData>();
            string type = single.EventType;
            //single.CurrentlySelectedType = type;
            transform.parent.parent.parent.parent.GetComponent<SingleParticipantEventHandler>().SetCurrentlySelectedType(type);
            EventController.Instance.HandleOnHover(false, type);

        }



        //EventController.Instance.SwitchCamera(false);
        // if (transform.parent.parent.parent.tag == "MainEventLine")
        // {

        //     transform.parent.parent.parent.parent.GetComponent<SingleParticipantEventHandler>().handleOnClick();
        // }
        // else
        // {

        // }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {


        if (isClicked)
        {

        }
        else
        {
            transform.parent.GetComponent<LineRenderer>().material = saveMaterial;
        }



        SingleEventData single = transform.parent.GetComponent<SingleEventData>();
        string type = single.EventType;
        transform.parent.parent.parent.parent.GetComponent<SingleParticipantEventHandler>().SetCurrentlySelectedType(null);
        //single.CurrentlySelectedType = null;
        EventController.Instance.HandleOnHover(true, type);
        isClicked = false;
        //EventController.Instance.SwitchCamera(true);
    }
}
