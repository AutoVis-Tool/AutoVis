using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Mirror;
using Replay;

public class GhostCar : NetworkBehaviour, IPointerClickHandler
{
    public GameObject player;
    public GameObject mainCar;
    public GameObject ghostCarPrefab;
    public GameObject carSyncerPrefab;
    private ReplayManager replayManager;

    private void Start()
    {
        replayManager = GameObject.Find("ReplayManager").GetComponent<ReplayManager>();
        mainCar = GameObject.Find("Main Vehicle");
    }

    //private void Update()
    //{
    //    if(Vector3.Distance(gameObject.transform.position, mainCar.transform.position) < 5)
    //    {
    //        gameObject.SetActive(false);
    //    } else
    //    {
    //        gameObject.SetActive(true);
    //    }
    //}
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        GameObject myOldGhostCar = GameObject.Find("myGhostCar");
        if(myOldGhostCar != null)
        {
            Destroy(myOldGhostCar);
        }
        Debug.Log("Ghost car Clicked!");
        GameObject carSyncer = Instantiate(carSyncerPrefab);
        carSyncer.name = "myGhostCar";
        carSyncer.transform.position = mainCar.transform.position;
        carSyncer.transform.rotation = mainCar.transform.rotation;
        carSyncer.GetComponent<updateCar>().timestamp = GameObject.Find("MainCarSyncer").GetComponent<updateCar>().timestamp;

        //GameObject ghostCar = Instantiate(ghostCarPrefab);
        //ghostCar.name = "myGhostCar";
        //ghostCar.transform.parent = carSyncer.transform;
        //ghostCar.transform.position = new Vector3(0, 0, 0);
        //ghostCar.transform.rotation = new Quaternion(0, 0, 0, 0);

        GameObject thisCarSyncer = gameObject.transform.parent.parent.gameObject;
        double timestamp = thisCarSyncer.GetComponent<updateCar>().timestamp;
        replayManager.LoadTimeStamp(timestamp);
    }
}
