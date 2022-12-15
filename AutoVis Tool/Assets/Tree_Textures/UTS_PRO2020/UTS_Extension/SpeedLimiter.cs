using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLimiter : MonoBehaviour
{
    public float SpeedLimit = 12f;

    BoxCollider collider;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
    }
    public void OnTriggerEnter(Collider other)
    {


        if (other.transform.parent.tag == "Car")
        {
            collider.enabled = false;
            CarAIController controller =
            other.transform.parent.GetComponent<CarAIController>();
            float newSpeed = SpeedLimit + Random.Range(SpeedLimit * -0.15f, SpeedLimit * 0.15f);


            // EventManager.Instance.AddEvent(TimelineEvent.Driving, newSpeed > controller.START_SPEED ? "Accelerating" : "Decelerating");
            Invoke("End", 2f);
            controller.MOVE_SPEED = newSpeed;

            controller.START_SPEED = controller.MOVE_SPEED;


        }
    }


    private void End()
    {
        //EventManager.Instance.EndEvent(TimelineEvent.Driving);
    }
}
