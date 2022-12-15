using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    //SyncList<string> conns = new SyncList<string>();
    public GameObject mainCar;
    public GameObject vrPlayer;
    public GameObject desktopPlayer;

    //[SyncVar(hook = nameof(setAvater))]
    [SyncVar]
    public bool vrUser;
    private void Start()
    {
        Debug.Log("PLAYER!");
        if (isLocalPlayer)
        {
            GameObject playerCam = GameObject.Find("Camera");
            gameObject.transform.parent = playerCam.transform;
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            gameObject.GetComponent<NetworkTransform>().target = playerCam.transform;
            gameObject.name = "Local Player";
            mainCar = GameObject.Find("MainCarSyncer");
            //addPlayer();
        }


        if (isLocalPlayer)
        {
            setIsVr();
        }
        setAvater(vrUser);
    }

    [Command]
    public void updateTimestamp(GameObject car, double ts)
    {
        car.GetComponent<updateCar>().timestamp = ts;
    }

    public void setPlayerTimestamp(double ts)
    {
        updateTimestamp(mainCar, ts);
    }

    [Command]
    void setIsVr()
    {
        if (GameObject.Find("VROrigin") == null)
        {
            gameObject.GetComponent<Player>().vrUser = false;
        }
        else
        {
            gameObject.GetComponent<Player>().vrUser = true;
        }
    }

    void setAvater(bool isVr)
    {
        GameObject avatar;
        if(isVr)
        {
            avatar = Instantiate(vrPlayer, transform);
        } else
        {
            avatar = Instantiate(desktopPlayer, transform);
        }

        if (isLocalPlayer)
        {
            Destroy(avatar.transform.GetChild(1).gameObject);
        }
    }

    //void setAvater(bool oldValue, bool newValue)
    //{
    //    if (newValue)
    //    {
    //        Instantiate(vrPlayer, transform);
    //    }
    //    else
    //    {
    //        Instantiate(desktopPlayer, transform);
    //    }
    //}

    //private void Update()
    //{

    //}
}
