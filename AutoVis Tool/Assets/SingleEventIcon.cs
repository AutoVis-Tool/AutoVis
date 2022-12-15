using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleEventIcon : MonoBehaviour
{



    private Camera SecondCamera;

    public float FixedSize = 0.001f;

    private SingleEventData single;

    // Start is called before the first frame update
    void Start()
    {
        //        SecondCamera = GameObject.Find("SubCamera").GetComponent<Camera>();
        single = transform.parent.GetComponent<SingleEventData>();
        UpdatePosition();
    }

    // Update is called once per frame
    void Update()
    {

        // if (SecondCamera.enabled)
        // {
        //     if (single.CurrentlySelectedType == single.EventType)
        //     {
        //         //float distance = Vector3.Distance(transform.position, SecondCamera.transform.position);
        //         var distance = (SecondCamera.transform.position - transform.position).magnitude;
        //         var size = distance * FixedSize * SecondCamera.fieldOfView;
        //         //            Debug.Log(size);
        //         transform.localScale = Vector3.one * size;
        //         //transform.forward = transform.position - SecondCamera.gameObject.transform.position;

        //     }
        // }
        // else
        // {
        //     transform.localScale = Vector3.one;
        // }
    }


    void UpdatePosition()
    {
        //Debug.Log(transform.parent.tag);
        if (transform.parent.parent.parent.tag != "SubEventLine")
        {
            transform.position = transform.parent.GetComponent<LineRenderer>().GetPosition(0) - new Vector3(transform.GetComponent<RectTransform>().rect.width / 2, transform.GetComponent<RectTransform>().rect.height / 2, 0);

        }
        else
        {
            transform.localPosition = transform.parent.GetComponent<LineRenderer>().GetPosition(0) - new Vector3(transform.GetComponent<RectTransform>().rect.width / 2, transform.GetComponent<RectTransform>().rect.height / 2, 0);

        }
    }


}
