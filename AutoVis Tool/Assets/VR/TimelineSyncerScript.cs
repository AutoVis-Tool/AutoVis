using Replay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class TimelineSyncerScript : MonoBehaviour
{

    public static TimelineSyncerScript Instance;
    public Slider Slider;

    bool sliderActive = false;

    public GameObject viveRig;
    void Start()
    {
        Instance = this;
        Slider.minValue = 0;
        Slider.maxValue = ReplayManager.Instance.TimeStamps.Length - 1;
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    Slider.minValue = 0;
        //    Slider.maxValue = ReplayManager.Instance.TimeStamps.Length - 1;
        //    Debug.Log("Sync successful");
        //}

        //if (Input.GetKeyDown(KeyCode.P))
        //{
        //    sliderActive = !sliderActive;
        //    Debug.Log("Slider active? " + sliderActive);
        //}

        if (ReplayManager.Instance.playing && !sliderActive)
        {
            Slider.value = (float) ReplayManager.Instance.CurrentFrame;
        }
        else
        {
            //if (ReplayManager.Instance.CurrentFrame != Slider.value && sliderActive)
            //{
            //    ReplayManager.Instance.SkipFrames((int)Slider.value - ReplayManager.Instance.CurrentFrame);
            //}
        }
    }


    public void Play()
    {

        ReplayManager.Instance.Play(1);
    }


    public void Pause()
    {
        ReplayManager.Instance.Pause();
    }


    int CamPointer = 0;

    public void Cam(int i)
    {
        CamPointer += i;
        if (CamPointer < 0)
        {
            CamPointer = CameraManager.Instance.AllCameras.Count - 1;
        }
        CamPointer = CamPointer % CameraManager.Instance.AllCameras.Count;
        viveRig = GameObject.Find("VROrigin");

        viveRig.transform.parent = null;
        viveRig.transform.rotation = CameraManager.Instance.AllCameras[CamPointer].transform.rotation;
        // viveRig.transform.position =
        //  CameraManager.Instance.AllCameras[CamPointer].transform.position - new Vector3(0, viveRig.transform.Find("Camera").transform.localPosition.y, 0);

        viveRig.transform.position = CameraManager.Instance.AllCameras[CamPointer].transform.position - new Vector3(0, viveRig.transform.GetChild(0).GetChild(0).transform.localPosition.y, 0);

        viveRig.transform.parent = CameraManager.Instance.AllCameras[CamPointer].transform;
    }


    public void Detach()
    {
        viveRig.transform.parent = null;
    }

    public void syncSlider()
    {
        int frame = (int)Slider.value;
        ReplayManager.Instance.GoToNearestTimeStamp(ReplayManager.Instance.TimeStamps[frame]);
    }
}
