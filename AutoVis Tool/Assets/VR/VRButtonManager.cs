using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleWebBrowser;
using Replay;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class VRButtonManager : MonoBehaviour, IPointerClickHandler
{
    public WebBrowser2D web;
    public GameObject colorPicker;

    private void Update()
    {
        //Debug.Log(UnityEngine.Input.GetJoystickNames()[0]);
        //if(UnityEngine.Input.GetJoystickNames() == Left Controller Trackpad (2))

    }
    public void test()
    {
        string s = "test();";
        //string s = "document.getElementsByTagName('body')[0].style.backgroundColor = 'green';";
        web.RunJavaScript(s);
    }

    public void OpenColorPicker()
    {
        colorPicker.SetActive(true);
    }
    public void ChangeColor(InputField input)
    {
        GameObject replayManager = GameObject.Find("ReplayManager");
        replayManager.GetComponent<JavaScriptManager>().handleChangedColor(input);
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log("Tablet Clicked!");
    }
}