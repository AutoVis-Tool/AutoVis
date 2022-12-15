using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
public class MyActions : MonoBehaviour
{
    // a reference to the action
    public SteamVR_Action_Boolean SpawnAnnotation;
    public SteamVR_Action_Boolean MoveForward;
    public SteamVR_Action_Boolean MoveBackwards;
    public SteamVR_Action_Boolean MoveRight;
    public SteamVR_Action_Boolean MoveLeft;
    // a reference to the hand
    public SteamVR_Input_Sources handType;
    //reference to the sphere
    public GameObject customAnnotationPopup;
    public GameObject VROrigin;
    private IEnumerator coroutine;

    private void Start()
    {
        SpawnAnnotation.AddOnStateDownListener(TriggerDown, handType);
        SpawnAnnotation.AddOnStateUpListener(TriggerUp, handType);

        MoveForward.AddOnStateDownListener(moveForward, handType);
        MoveBackwards.AddOnStateDownListener(moveBackwards, handType);
        MoveRight.AddOnStateDownListener(moveRight, handType);
        MoveLeft.AddOnStateDownListener(moveLeft, handType);

        MoveForward.AddOnStateUpListener(stopMovement, handType);
        MoveBackwards.AddOnStateUpListener(stopMovement, handType);
        MoveRight.AddOnStateUpListener(stopMovement, handType);
        MoveLeft.AddOnStateUpListener(stopMovement, handType);
    }

    private void Update()
    {
        if (Input.GetKeyDown("o"))
        {
            StopCoroutine(coroutine);
        }
    }

    public void TriggerUp(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        //GameObject.Instantiate(customAnnotationPopup);
        Debug.Log("Trigger is up");
        //customAnnotationPopup.GetComponent<MeshRenderer>().enabled = false;
    }
    public void TriggerDown(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Destroy(GameObject.Find(customAnnotationPopup.name + "(Clone)"));
        GameObject popup = GameObject.Instantiate(customAnnotationPopup);
        Debug.Log(fromAction.activeDevice.ToString());
        if(fromAction.activeDevice.ToString() == "LeftHand")
        {
            popup.transform.position = GameObject.Find("LeftHand").transform.position;
        } else
        {
            popup.transform.position = GameObject.Find("RightHand").transform.position;
        }
        //Debug.Log(SteamVR_Input_Sources.LeftHand);
        //popup.transform.position = fromAction.activeDevice.transform.position;
        //Debug.Log("Trigger is down");
        //customAnnotationPopup.GetComponent<MeshRenderer>().enabled = true;
    }

    public void moveForward(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        StopAllCoroutines();
        coroutine = moveOrigin("f");
        StartCoroutine(coroutine);
    }

    public void moveBackwards(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        StopAllCoroutines();
        coroutine = moveOrigin("b");
        StartCoroutine(coroutine);
    }

    public void moveRight(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        StopAllCoroutines();
        coroutine = moveOrigin("r");
        StartCoroutine(coroutine);
    }

    public void moveLeft(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        StopAllCoroutines();
        coroutine = moveOrigin("l");
        StartCoroutine(coroutine);
    }

    public void stopMovement(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        Debug.Log("Stop");
        StopAllCoroutines();
    }

    IEnumerator moveOrigin(String d)
    {
        GameObject mainCamera = Camera.main.gameObject;
        Vector3 moveDirection = new Vector3(0, 0, 0);
        while (true)
        {
            if(d == "f")
            {
                moveDirection = mainCamera.transform.forward;
            } else if(d == "b")
            {
                moveDirection = -mainCamera.transform.forward;
            } else if(d == "r")
            {
                moveDirection = mainCamera.transform.right;
            } else if(d == "l")
            {
                moveDirection = -mainCamera.transform.right;
            }
            VROrigin.transform.position += 0.2f * moveDirection;
            yield return null;
        }
    }
}