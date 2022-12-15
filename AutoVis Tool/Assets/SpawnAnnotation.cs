using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Replay;

public class SpawnAnnotation : MonoBehaviour
{
    //public GameObject annotationPrefab;
    // Start is called before the first frame update
    public GameObject annotationUI;
    public GameObject labelUI;
    public GameObject annotationButton;
    public GameObject labelButton;
    public GameObject customAnnotation;
    public GameObject customLabel;
    private Sprite selectedIcon;
    private Image lastButtonImage;
    private int startFrame;
    private int endFrame;
    private GameObject networkManager;
    void Start()
    {
        networkManager = GameObject.Find("CustomNetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void spawnAnnotation()
    {
        string annotationTitle = transform.GetChild(0).GetChild(2).GetComponent<TMP_InputField>().text;
        string annotationText = annotationUI.transform.GetChild(0).GetComponent<TMP_InputField>().text;
        Vector3 pos = transform.position;
        GameObject annotation = Instantiate(customAnnotation);
        annotation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = annotationTitle;
        annotation.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = annotationText;
        annotation.transform.GetChild(3).GetComponent<RawImage>().texture = selectedIcon.texture;
        annotation.transform.position = pos;
        //networkManager.GetComponent<MyNetworkManager>().spawnCustomAnnotation(annotationTitle, annotationText, selectedIcon, pos);
        Destroy(gameObject);
    }

    public void changeIcon(Image icon)
    {
        if (lastButtonImage != null)
        {
            lastButtonImage.color = new Color(0f, 0f, 0f);
        }
        lastButtonImage = icon;
        selectedIcon = icon.sprite;
        icon.color = new Color(1f, 1f, 1f);
    }

    public void changeToLabel()
    {
        annotationUI.SetActive(false);
        labelUI.SetActive(true);
        annotationButton.GetComponent<Image>().color = new Color32(178, 178, 178, 255);
        labelButton.GetComponent<Image>().color = new Color(1, 1, 1);
    }

    public void changeToAnnotation()
    {
        labelUI.SetActive(false);
        annotationUI.SetActive(true);
        labelButton.GetComponent<Image>().color = new Color32(178, 178, 178, 255);
        annotationButton.GetComponent<Image>().color = new Color(1, 1, 1);
    }

    public void addTimestampToLabel(GameObject button)
    {
        double timestamp = ReplayManager.Instance.CurrentTimeStamp;
        button.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = timestamp.ToString();
        if (button.name == "Start Button")
        {
            startFrame = ReplayManager.Instance.CurrentFrame;
        } else
        {
            endFrame = ReplayManager.Instance.CurrentFrame;
        }
    }

    public void spawnLabel()
    {
        string labelTitle = transform.GetChild(0).GetChild(2).GetComponent<TMP_InputField>().text;
        List<Vector3> points = new List<Vector3>();
        for (int i = startFrame; i < endFrame; i++)
        {
            points.Add(JavaScriptManager.instanceJS.mainCarPositions[i] + new Vector3(0, 3f, 0));
        }
        //networkManager.GetComponent<MyNetworkManager>().spawnCustomLabel(labelTitle, points.ToArray(), selectedIcon);
        GameObject label = Instantiate(customLabel);
        if(points.ToArray().Length > 0)
        {
            label.transform.position = points.ToArray()[0];
            label.GetComponent<LineRenderer>().positionCount = points.ToArray().Length;
            label.GetComponent<LineRenderer>().SetPositions(points.ToArray());
        }
        label.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = labelTitle;
        label.transform.GetChild(0).GetChild(1).GetComponent<RawImage>().texture = selectedIcon.texture;
        Destroy(gameObject);
    }
}
