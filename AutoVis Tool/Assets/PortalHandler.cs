using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using Replay;

public class PortalHandler : MonoBehaviour
{
    public bool pointingOnObject = false;
    public Vector3 PositionOfPoiting = new Vector3();

    public GameObject rightHand;

    public GameObject head;

    public GameObject Portal;

    public GameObject ThaughtPortal;

    public Camera portalCam;

    public Image Border;

    public RenderTexture portalTex;
    // Start is called before the first frame update
    void Start()
    {
        RenderTexture newPortalTex = new RenderTexture(portalTex);
        portalCam.targetTexture = newPortalTex;
        Portal.transform.GetChild(0).GetComponent<RawImage>().texture = newPortalTex;
        //List<GameObject> all = transform.parent.gameObject.FindGameObjectsWithTag("AvatarBody").ToList();

    }

    // Update is called once per frame
    void Update()
    {

        //UpdateCameraRotation();
        //PlaceCameraInFrontOfPointing();
        if (pointingOnObject)
        {
            //TODO 

        }
        else
        {
            //TODO: disable Portal here
        }
    }

    public void OnBoneClick()
    {
        // Debug.Log("DONE Body from " + gameObject.name);
        List<GameObject> allAvatars = GameObject.FindGameObjectsWithTag("AvatarModell").ToList();
        foreach (GameObject avatar in allAvatars)
        {
            avatar.transform.GetChild(0).GetComponent<Outline>().enabled = false;
            avatar.transform.GetComponent<PortalHandler>().pointingOnObject = false;
        }
        transform.GetChild(0).GetComponent<Outline>().enabled = true;
        pointingOnObject = true;
        Border.gameObject.SetActive(true);


    }


    public void EnablePortal()
    {
        //Portal.transform.parent.parent = rightHand.transform;
        //Portal.transform.parent.localPosition = new Vector3(0f, 0.365f, 0f);
        //Portal.transform.parent.localEulerAngles = new Vector3(90f, -90f, -90f);
        Portal.transform.parent.gameObject.SetActive(true);
    }

    public void DisablePortal()
    {
        Portal.transform.parent.gameObject.SetActive(false);
    }

    public void EnableGazePortal()
    {
        //Portal.transform.parent.parent = head.transform;
        //Portal.transform.parent.localPosition = new Vector3(0, 0.172f, 0.62f);
        //Portal.transform.parent.localEulerAngles = new Vector3(0f, 180f, 0);
        Portal.transform.parent.gameObject.SetActive(true);
    }

    public void DisableGazePortal()
    {
        Portal.transform.parent.gameObject.SetActive(false);
    }

    public void EnableThaughtPortal()
    {
        ThaughtPortal.SetActive(true);
    }

    public void DisableThaughtPortal()
    {
        ThaughtPortal.SetActive(false);
    }

    public void activateOutlineandPortal()
    {

        //enable Portal here if 
    }

    public void SetOutlineColor(Color c)
    {
        transform.GetChild(0).GetComponent<Outline>().OutlineColor = c;
        Border.color = c;
    }


    public void UpdateCameraRotation()
    {
        portalCam.transform.LookAt(PositionOfPoiting);
        //Portal.transform.parent.eulerAngles = new Vector3(0, 0, 0);
        //portalCam.transform.rotation = Quaternion.Euler(portalCam.transform.eulerAngles.x, portalCam.transform.eulerAngles.y, 180f);
        //Debug.Log("CamRot " + portalCam.transform.eulerAngles);
    }

    public void PlaceCameraInFrontOfPointing(bool isGaze)
    {
        Vector3 direction = Vector3.Normalize(Portal.transform.position - PositionOfPoiting);
        if(isGaze)
        {
            portalCam.transform.position = PositionOfPoiting + (direction * 30);
        } else
        {
            portalCam.transform.position = PositionOfPoiting + (direction * 3);
        }
        //+ z - x
        //portalCam.transform.position = PositionOfPoiting - new Vector3(-2f, PositionOfPoiting.y,+2f);
    }


    //     public static List<GameObject> FindGameObjectInChildWithTag(GameObject parent, string tag)
    //     {
    //         Transform t = parent.transform;
    //         List<GameObject> full = new List<GameObject>();
    //         for (int i = 0; i < t.childCount; i++)
    //         {
    //             if (t.GetChild(i).gameObject.tag == tag)
    //             {
    //                 full.Add(t.GetChild(i).gameObject);
    //             }

    //         }

    //         return full;
    //     }


}
