using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTabletView : MonoBehaviour
{
    private GameObject[] views;
    public GameObject minimap;
    public GameObject Browser;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchToMinimap()
    {
        Browser.SetActive(false);
        minimap.SetActive(true);
    }

    public void switchToBrowser()
    {
        minimap.SetActive(false);
        Browser.SetActive(true);
    }
}
