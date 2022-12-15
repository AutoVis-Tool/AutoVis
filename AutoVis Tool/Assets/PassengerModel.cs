using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Replay;

public class PassengerModel : MonoBehaviour
{
    // Start is called before the first frame update

    void Awake()
    {
        EventController.Instance.passengerModel = this.gameObject;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
