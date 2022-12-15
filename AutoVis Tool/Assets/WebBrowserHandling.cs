using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuplex.WebView;
using Newtonsoft.Json.Linq;
using Replay;

public class WebBrowserHandling : MonoBehaviour
{
    // Start is called before the first frame update

    public static WebBrowserHandling Instance;
    private CameraManager cameraManager;

    public RawImage rawImage;

    public CanvasWebViewPrefab webViewPrefab;

    public GameObject CanvasOf3DView;

    void Awake()
    {
        Instance = this;
    }
    async void Start()
    {
        cameraManager = GetComponent<CameraManager>();
        webViewPrefab = GameObject.Find("CanvasWebViewPrefab").GetComponent<CanvasWebViewPrefab>();
        // Wait for the WebViewPrefab to initialize, because the WebViewPrefab.WebView property
        // is null until the prefab has initialized.
        await webViewPrefab.WaitUntilInitialized();
        webViewPrefab.WebView.MessageEmitted += (sender, eventArgs) =>
        {
            Debug.Log("JSON received: " + eventArgs.Value);
            JObject main = JObject.Parse(eventArgs.Value);
            handleIncomingJSON(main);
        };
        // Send a message after the page has loaded.
        await webViewPrefab.WebView.WaitForNextPageLoadToFinish();

    }

    // Update is called once per frame
    void Update()
    {

    }


    void handleIncomingJSON(JObject json)
    {
        switch (json["type"].ToString())
        {
            case "Camera":
                Debug.Log((int)json["message"]);
                cameraManager.SwitchToCamera((int)json["message"]);
                break;
            case "Timestamp":
                ReplayManager.Instance.GoToNearestTimeStamp((double)json["message"]);
                // code block
                break;
            case "Option":
                handleOptionJSON(json);
                // code block
                break;

            case "Color":
                JavaScriptManager.instanceJS.selectedParticipant = (int)json["message"];
                JavaScriptManager.instanceJS.toggleColorPicker();
                // code block
                break;

            case "Window":
                CanvasOf3DView.SetActive(true);

                // code block
                break;
            default:
                // code block
                break;
        }
    }


    void handleOptionJSON(JObject json)
    {
        switch (json["optiontype"].ToString())
        {
            case "Avatar":
                JavaScriptManager.instanceJS.toggleDriver();
                break;
            case "Passenger":
                JavaScriptManager.instanceJS.togglePassenger();
                break;
            case "Trajectory":
                JavaScriptManager.instanceJS.toggleTrajectoy((int)json["number"]);
                // code block
                break;
            case "Heatmaps":
                JavaScriptManager.instanceJS.toggleHeatmap((int)json["number"]);
                // code block
                break;
            case "enableParticipant":
                JavaScriptManager.instanceJS.enableParticipant((int)json["number"]);
                // code block
                break;
            case "disableParticipant":
                JavaScriptManager.instanceJS.disableParticipant((int)json["number"]);
                // code block
                break;
            default:
                // code block
                break;
        }
    }


    // Not needed ATM maybe when more complex json handling is required

    // void handleAvatarJson(JObject json)
    // {
    //     switch (json["message"].ToString())
    //     {
    //         case "Avatar":

    //             break;
    //         case "Passenger":

    //             // code block
    //             break;
    //         default:
    //             // code block
    //             break;
    //     }
    // }


    // Not needed ATM maybe when more complex json handling is required

    // void handleTrajectoryJson(JObject json)
    // {
    //     switch (json["message"].ToString())
    //     {
    //         case "Head":
    //             Debug.Log((int)json["message"]);
    //             cameraManager.SwitchToCamera((int)json["message"]);
    //             break;
    //         case "Left":

    //             break;
    //         case "Right":

    //             break;
    //         default:
    //             // code block
    //             break;
    //     }
    // }


    // Not needed ATM maybe when more complex json handling is required

    // void handleHeatmapJson(JObject json)
    // {
    //     switch (json["message"].ToString())
    //     {
    //         case "Interior":

    //             break;
    //         case "Interior SteeringWheel":

    //             break;
    //         case "Interior Display":

    //             break;
    //         case "Interior Windows":

    //             break;
    //         case "Exterior Buildings":

    //             break;

    //         case "Exterior Street":

    //             break;

    //         default:
    //             // code block
    //             break;
    //     }
    //}




}
