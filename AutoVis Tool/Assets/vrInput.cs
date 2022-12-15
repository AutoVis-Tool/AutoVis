using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuplex.WebView;
using HTC.UnityPlugin.Pointer3D;
using UnityEngine.EventSystems;

public class vrInput : MonoBehaviour, IPointerClickHandler, IPointerDownHandler
{
    // Start is called before the first frame update
    void Start()
    {
        //var customInputDetector = gameObject.GetComponent<WebViewPrefab>().Collider.gameObject.AddComponent<YourCustomInputDetector>();
        //gameObject.GetComponent<WebViewPrefab>().SetPointerInputDetector(customInputDetector);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log(gameObject.name + " Clicked!");
    }

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        Debug.Log(gameObject.name + " Downed!");
    }

    //Pointer3DRaycaster
}
