using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Replay {
    public class CameraController : MonoBehaviour {



        /* 
        Made simple to use (drag and drop, done) for regular keyboard layout  
        wasd : basic movement
        shift : Makes camera accelerate
        space : Moves camera on X and Z axis only.  So camera doesn't gain any height*/

        public Camera Camera;
        public bool Move = false;
        public bool Enabled = false;

        public float mainSpeed = 10.0f; //regular speed
        public float shiftAdd = 25.0f; //multiplied by how long shift is held.  Basically running
        public float maxShift = 100.0f; //Maximum speed when holdin gshift
        public float camSens = 0.25f; //How sensitive it with mouse

        private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
        private float totalRun = 1.0f;

        void Start()
        {
            Camera = GetComponent<Camera>();
            Camera.enabled = Enabled;
            CameraManager.Instance.AllCameras.Add(Camera);
            
        }

        void Update() {

            if (!Camera.enabled)
            {
                return;
            }

            float f = 0.0f;


            if (Input.GetMouseButtonDown(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
                //Cursor.lockState = CursorLockMode.None;
            } else if (Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
            }


            Vector3 p = Vector3.zero;
            if (Input.GetMouseButton(1)) {
                // lastMouse = Input.mousePosition - lastMouse;
                lastMouse = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0);
                lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
                lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
                transform.eulerAngles = lastMouse;
                // lastMouse = Input.mousePosition;
            }

            if (Move)
            {
                p = GetBaseInput();
            }


            if (p.sqrMagnitude > 0) { // only move while a direction key is pressed
                if (Input.GetKey(KeyCode.LeftShift)) {
                    totalRun += Time.deltaTime;
                    p = p * totalRun * shiftAdd;
                    p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                    p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                    p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
                }
                else {
                    totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                    p = p * mainSpeed;
                }

                p = p * Time.deltaTime;
                Vector3 newPosition = transform.position;
                if (Input.GetKey(KeyCode.Space)) { //If player wants to move on X and Z axis only
                    transform.Translate(p);
                    newPosition.x = transform.position.x;
                    newPosition.z = transform.position.z;
                    transform.position = newPosition;
                }
                else {
                    transform.Translate(p);
                }
            }

        }

        private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
            Vector3 p_Velocity = new Vector3();
            if (Input.GetKey(KeyCode.W)) {
                p_Velocity += new Vector3(0, 0, 1f);
            }
            if (Input.GetKey(KeyCode.S)) {
                p_Velocity += new Vector3(0, 0, -1f);
            }
            if (Input.GetKey(KeyCode.A)) {
                p_Velocity += new Vector3(-1f, 0, 0);
            }
            if (Input.GetKey(KeyCode.D)) {
                p_Velocity += new Vector3(1f, 0, 0);
            }
            p_Velocity += new Vector3(0, 0, 10 * Input.mouseScrollDelta.y);

            return p_Velocity;
        }
    } 
}
