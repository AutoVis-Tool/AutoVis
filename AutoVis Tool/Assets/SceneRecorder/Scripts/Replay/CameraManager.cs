using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Replay
{
    /// <summary>
    /// This script manages all cameras in the scene. It also handles additional movement option for the main camera.
    /// </summary>
    public class CameraManager : MonoBehaviour
    {
        /// <summary>
        /// Static instance of cam controller for easier access
        /// </summary>
        public static CameraManager Instance;


        /// <summary>
        /// Points to the current camera
        /// </summary>
        public int CamPointer = 0;

        /// <summary>
        /// The default cam. This always exists
        /// </summary>
        public Camera FreeCam;

        /// <summary>
        /// The current camera
        /// </summary>
        public Camera CurrentCamera { get => AllCameras[CamPointer]; }

        /// <summary>
        /// List of all cameras. If a new cam is created it should be added here
        /// </summary>
        public List<Camera> AllCameras;

        /// <summary>
        /// The minimum vertical distance from the ground
        /// </summary>
        public float CamMinHeight = 1;

        /// <summary>
        /// Whether or not the camera is currently following a model
        /// </summary>
        public bool Follow;

        /// <summary>
        /// The Model we're currently following
        /// Should be NULL if we're not following anything
        /// </summary>
        public ModelController FollowModel;


        /// <summary>
        /// rotation speed in follow mode
        /// </summary>
        public float turnSens = 25f;

        /// <summary>
        /// Regular speed
        /// </summary>
        public float mainSpeed = 10.0f;

        /// <summary>
        /// multiplied by how long shift is held.  Basically running
        /// </summary>
        public float shiftAdd = 25.0f;

        /// <summary>
        /// Maximum speed when holding shift
        /// </summary>
        public float maxShift = 100.0f;

        /// <summary>
        /// Mouse Sensitivity
        /// </summary>
        public float camSens = 1f;

        /// <summary>
        /// Orthographic mode or not
        /// </summary>
        private bool ortho = false;
        void Awake()
        {
            Instance = this;

        }

        // Update is called once per frame
        void Update()
        {

            if (!FreeCam.enabled)
            {
                return;
            }


            //Additional movement that only applies if we're currently following a model
            if (Follow)
            {
                if (Input.GetMouseButton(1))
                {
                    FreeCam.transform.RotateAround(FreeCam.transform.parent.position, FreeCam.transform.TransformDirection(Vector3.right), turnSens * Input.GetAxis("Mouse Y"));
                    FreeCam.transform.RotateAround(FreeCam.transform.parent.position, Vector3.up, turnSens * Input.GetAxis("Mouse X"));
                }
                FreeCam.transform.LookAt(FreeCam.transform.parent.position, Vector3.up);

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    StopFollow();
                }
            }

            if (ortho)
            {
                FreeCam.transform.position = new Vector3(FreeCam.transform.position.x, 25f, FreeCam.transform.position.z);

                FreeCam.orthographicSize = Mathf.Max(1, FreeCam.orthographicSize -= Input.mouseScrollDelta.y);

            }
            else if (FreeCam.transform.position.y <= CamMinHeight)
            {
                FreeCam.transform.position = new Vector3(FreeCam.transform.position.x, CamMinHeight, FreeCam.transform.position.z);
            }


        }

        /// <summary>
        /// Focuses the camera on a Model.
        /// If called while following this will disable follow mode
        /// </summary>
        /// <param name="model"></param>
        public void FocusOnModel(ModelController model)
        {

            FreeCam.transform.LookAt(model.transform.position, Vector3.up);
            FreeCam.transform.parent = model.transform;
            Follow = !Follow;

            if (Follow)
            {
                FollowModel = model;

            }
            else
            {
                StopFollow();
            }

        }

        /// <summary>
        /// Stop following a model
        /// </summary>
        public void StopFollow()
        {
            Follow = false;
            FreeCam.transform.parent = null;
        }


        /// <summary>
        /// Toggle between Orthographic & Perspective Camera projection
        /// Can be used by Web Interface
        /// </summary>
        public void ToggleProjection()
        {
            ortho = !ortho;

            if (ortho)
            {
                SwitchToOrtho();
            }
            else
            {
                SwitchToPerspective();
            }
        }

        /// <summary>
        /// Switch to Orthographic Projection
        /// </summary>
        public void SwitchToOrtho()
        {
            FreeCam.orthographicSize = 25;
            FreeCam.orthographic = true;
            FreeCam.transform.rotation = Quaternion.Euler(22.5f, FreeCam.transform.rotation.eulerAngles.y, 0);

        }

        /// <summary>
        /// Switch to Perspective Projection
        /// </summary>
        public void SwitchToPerspective()
        {
            FreeCam.orthographic = false;
            CamMinHeight = 1f;
        }

        /// <summary>
        /// Disables the current camera, switches to the next one and enables it
        /// </summary>
        /// <param name="i"></param>
        public void SwitchToCamera(int i)
        {
            //Debug.Log(Camera.allCamerasCount);
            CurrentCamera.enabled = false;
            CamPointer += i;
            if (CamPointer < 0)
            {
                CamPointer = AllCameras.Count - 1;
            }
            CamPointer = CamPointer % AllCameras.Count;

            CurrentCamera.enabled = true;
            WebBrowserHandling.Instance.rawImage.texture = CurrentCamera.targetTexture;
            Debug.Log(CurrentCamera.targetTexture);

        }

        /// <summary>
        /// Switch to the next/previous camera based on value. Ideally -1/+1
        /// Can be used by Web Interface
        /// </summary>
        /// <param name="number"></param>
        public void SwitchToCamera(string number)
        {
            int i = int.Parse(number);
            SwitchToCamera(i);
        }

    }
}