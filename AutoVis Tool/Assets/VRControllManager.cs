using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Replay;

public class VRControllManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject UIPrefab;

    private Color activated = Color.green;

    private Color disabled = Color.white;

    public GameObject selectedObject;

    public int selectedObjectindex;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateUIWindow(GameObject ClickedObject)
    {
        //instantiated later
        // selectedObject = ClickedObject;

        UIPrefab.SetActive(true);
        UIPrefab.transform.GetChild(0).GetChild(0).name = ClickedObject.name;
        int index = ClickedObject.transform.GetSiblingIndex();
        selectedObjectindex = index;
        disableAllButtons();
        UIPrefab.transform.GetChild(1).GetChild(index).gameObject.GetComponent<Image>().color = activated;


    }

    private void disableAllButtons()
    {
        for (int i = 0; i < UIPrefab.transform.GetChild(1).childCount; i++)
        {
            UIPrefab.transform.GetChild(1).GetChild(i).gameObject.GetComponent<Image>().color = disabled;
        }
    }

    public void disableSelectedHeatmap()
    {
        int index = selectedObjectindex;
        JavaScriptManager.instanceJS.toggleHeatmap(index);
    }

    public void ButtonSetOtherHeatmaps(Button b)
    {
        selectedObjectindex = b.transform.GetSiblingIndex();
        UIPrefab.transform.GetChild(0).GetChild(0).name = b.name;
        disableAllButtons();
        UIPrefab.transform.GetChild(1).GetChild(selectedObjectindex).gameObject.GetComponent<Image>().color = activated;
    }

}
