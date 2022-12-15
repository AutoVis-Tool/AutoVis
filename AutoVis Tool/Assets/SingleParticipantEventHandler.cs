using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Replay;

public class SingleParticipantEventHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public Material ParticipantMaterial;
    public bool isExpanded = false;

    public int ParticipantEventId;

    public Material ExpandedMaterial;

    public float defaultBetweenSingleEvents = 0.75f;

    public float distanceBetweenSingleEvents = 0.75f;




    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void handleOnClick()
    {
        if (!isExpanded)
        {
            transform.GetChild(1).gameObject.SetActive(true);
            //updateAllPositions();
            ExpandEvents();
            isExpanded = true;
        }
        else
        {
            CondenseEvents();

            isExpanded = false;
        }
    }

    void ExpandEvents()
    {
        Debug.Log("EXPAND");
        // float distance = 2f * transform.GetChild(1).childCount;
        // float singleChildDistance = distance / transform.GetChild(1).childCount;
        distanceBetweenSingleEvents = defaultBetweenSingleEvents;
        float completionTime = 2f;
        //transform.GetChild(0).position = new Vector3(0f, (distanceBetweenSingleEvents * transform.GetChild(1).childCount), 0f);
        //transform.GetChild(1).position = transform.GetChild(0).position;
        Vector3 targetPosition = new Vector3(0f, (distanceBetweenSingleEvents * (transform.GetChild(1).childCount + 1)), 0f);
        iTween.MoveTo(transform.GetChild(0).gameObject, targetPosition, completionTime);
        iTween.MoveTo(transform.GetChild(1).gameObject, targetPosition, completionTime);
        Invoke("ExpandChilds", completionTime);


        setMainLineMaterial(ExpandedMaterial);


        // int index = transform.GetSiblingIndex();
        // GameObject nextBrotherNode = transform.parent.GetChild(index + 1).gameObject;
    }

    void ExpandChilds()
    {
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            iTween.MoveTo(transform.GetChild(1).GetChild(i).gameObject, new Vector3(0f, ((distanceBetweenSingleEvents * i) + distanceBetweenSingleEvents), 0f), 4f);
            //iTween.MoveTo(GameObject target, Vector3 position, float time)
            //transform.GetChild(1).GetChild(i).position = new Vector3(transform.GetChild(1).GetChild(i).position.x, transform.GetChild(1).GetChild(i).position.y - ((distanceBetweenSingleEvents * i) + distanceBetweenSingleEvents), transform.GetChild(1).GetChild(i).position.z);
            // for (int j = 0; j < transform.GetChild(1).GetChild(i).childCount; j++)
            // {
            //     Debug.Log("JO" + transform.GetChild(1).GetChild(i).GetChild(j).GetComponent<LineRenderer>().GetPosition(0));
            //     transform.GetChild(1).GetChild(i).GetChild(j).GetChild(0).position = transform.GetChild(1).GetChild(i).GetChild(j).GetComponent<LineRenderer>().GetPosition(0) - new Vector3(0, singleChildDistance, 0);
            // }

        }
        ToggleEventIconsOnMainLine();
    }

    void CondenseEvents()
    {
        Debug.Log("CONDENSE");
        //transform.GetChild(0).position = Vector3.zero;


        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            // transform.GetChild(1).GetChild(i).position = Vector3.zero;
            iTween.MoveTo(transform.GetChild(1).GetChild(i).gameObject, Vector3.zero, 2f);
        }

        //iTween.MoveTo(transform.GetChild(1).gameObject, Vector3.zero, completionTime);
        Invoke("CondenseChilds", 2f);



    }

    void CondenseChilds()
    {
        float completionTime = 4f;
        iTween.MoveTo(transform.GetChild(0).gameObject, Vector3.zero, completionTime);
        Invoke("DisableCopyLine", 4f);
    }

    void DisableCopyLine()
    {
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(1).position = Vector3.zero;
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            transform.GetChild(1).GetChild(i).position = Vector3.zero;
        }
        setMainLineMaterial(ParticipantMaterial);
        ToggleEventIconsOnMainLine();
        EventController.Instance.EnableAllParticipant();
    }

    public void SetCurrentlySelectedType(string type)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(i).childCount; j++)
            {
                for (int k = 0; k < transform.GetChild(i).GetChild(j).childCount; k++)
                {
                    transform.GetChild(i).GetChild(j).GetChild(k).GetComponent<SingleEventData>().CurrentlySelectedType = type;

                }
            }
        }
        // foreach (Transform g in transform)
        // {
        //     foreach (Transform s in g)
        //     {
        //         s.GetComponent<SingleEventData>().CurrentlySelectedType = type;
        //     }
        // }
    }


    void ToggleEventIconsOnMainLine()
    {

        for (int j = 0; j < transform.GetChild(0).childCount; j++)
        {
            for (int k = 0; k < transform.GetChild(0).GetChild(j).childCount; k++)
            {
                transform.GetChild(0).GetChild(j).GetChild(k).GetChild(0).gameObject.SetActive(!transform.GetChild(0).GetChild(j).GetChild(k).GetChild(0).gameObject.activeInHierarchy);

            }
        }

    }


    void setMainLineMaterial(Material mat)
    {
        for (int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            for (int j = 0; j < transform.GetChild(0).GetChild(i).childCount; j++)
            {
                transform.GetChild(0).GetChild(i).GetChild(j).GetComponent<LineRenderer>().material = mat;
            }

        }

    }

    // void updateAllPositions()
    // {
    //     foreach (Transform g in transform.GetChild(1))
    //     {
    //         Debug.Log("g" + g.gameObject.name);
    //         foreach (Transform s in g)
    //         {
    //             Debug.Log(s.gameObject.name);
    //             s.GetChild(0).gameObject.GetComponent<SingleEventIcon>().ChangePos();
    //         }
    //     }

    // }
}
