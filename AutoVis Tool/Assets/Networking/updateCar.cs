using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


public class updateCar : NetworkBehaviour
{

    //public GameObject mainCarPrefab;
    public GameObject ghostCarPrefab;
    
    [SyncVar]
    public double timestamp;


    private void Start()
    {
        Debug.Log("CAR!");
        if (isOwned)
        {
            GameObject mainCar = GameObject.Find("Main Vehicle");
            transform.parent = mainCar.transform;
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            gameObject.GetComponent<NetworkTransform>().target = mainCar.transform;
            gameObject.name = "MainCarSyncer";
        }
        else
        {
            GameObject ghostCar = Instantiate(ghostCarPrefab);
            ghostCar.transform.parent = gameObject.transform;
            ghostCar.transform.localPosition = new Vector3(0, 0, 0);
            ghostCar.transform.localRotation = new Quaternion(0, 0, 0, 0);
            //ghostCar.transform.GetChild(0).GetComponent<GhostCar>().carSyncer = gameObject;
        }
    }

    //private void Update()
    //{

    //}

    //public void getTimestamp()
    //{
    //    Debug.Log("getTimestamp!!!!!!!!!!!!!!!!!!");
    //    getTimestamp2();
    //}
}
