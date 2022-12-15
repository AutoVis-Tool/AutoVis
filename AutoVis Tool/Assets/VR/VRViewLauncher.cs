using HTC.UnityPlugin.Vive;
using SimpleWebBrowser;
//using System;
using System.Collections;
//using System.Collections.Generic;
//using UnityEditor.SceneManagement;
//using UnityEditor.XR.Management;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.XR.Management;

/// <summary>
/// This script loads the main Replay scene and adds objects important for Vr context to it
/// This way the default 3D is not affected by this
/// </summary>
public class VRViewLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        LoadScene();
    }


    public string MainScene = "MainScene";

    public GameObject VRRig;

    public GameObject Browser;

    public GameObject ACTUALBROWSER;

    public WebBrowser2D WebBrowser;

    public GameObject rig;

    public Material vrMaterial;

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadScene()
    {

        //StartCoroutine(VrSetup());
        VrSetup();



    }


    private void VrSetup()
    {
        //Load the scene
        //var a = SceneManager.LoadSceneAsync(MainScene, LoadSceneMode.Additive);
        //yield return new WaitUntil(() => a.isDone);

        // Scene s = SceneManager.GetSceneByName(MainScene);

        //set scene to active scene so new stuff is added to it
        // SceneManager.SetActiveScene(s);
        //EditorSceneManager.SetActiveScene(s);

        //add Vr to the scene
        //GameObject rig = Instantiate(VRRig, new Vector3(472, 102, 2797), Quaternion.identity);


        //Make the grid teleportable
        /*  GameObject grid = GameObject.Find("Grid");
          grid.AddComponent<MeshCollider>();
          Teleportable t = grid.AddComponent<Teleportable>();
          t.target = rig.transform;
          t.pivot = rig.transform.Find("Camera");
         */
        //var leftHand = rig.transform.Find("ViveControllers").Find("Left").Find("Grabber").Find("DeviceTracker").Find("Caster");

        //Debug.Log(rig);
        //Debug.Log(rig.transform.Find("ViveCameraRig"));

        Transform leftHand = rig.transform.Find("ViveCameraRig").Find("LeftHand").Find("Model");
        //Transform rightHand = rig.transform.Find("ViveCameraRig").Find("RightHand").Find("Model").Find("Model");

        //foreach (Transform controllerPart in leftHand)
        //{
        //    controllerPart.gameObject.GetComponent<Renderer>().material = vrMaterial;
        //}
        //foreach (Transform controllerPart in rightHand)
        //{
        //    controllerPart.gameObject.GetComponent<Renderer>().material = vrMaterial;
        //}

        //////ACTUALBROWSER = Instantiate(Browser);
        ACTUALBROWSER = Instantiate(Browser, leftHand);

        WebBrowser = ACTUALBROWSER.GetComponentInChildren<WebBrowser2D>();

        //WebBrowser.RunJavaScript("applyGrid()");

    }
}
