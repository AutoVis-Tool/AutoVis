using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

namespace Replay
{
    public class SliceManager : MonoBehaviour
    {
        public static SliceManager Instance;
        public ModelController Model;
        public List<GameObject> sliceableParts;

        public GameObject slicedObject;

        public Material intersectionMat;
        public Vector3 planePos = new Vector3(0, 1f, 0);
        public Vector3 planeNormal = new Vector3(0, 1, 0);


        bool sliced;
        bool toggleSlice;
        // Start is called before the first frame update
        void Awake()
        {
            Instance = this;
        }

 

        /// <summary>
        /// Cut the model <see cref="sliceableParts"></see> into two parts, keeping the lower and adding it to a new <see cref="GameObject"/>
        /// Model is cut along the <see cref="planePos"/> & <see cref="planeNormal"/>
        /// </summary>
        void CutModel()
        {

            var t = Model.GetComponentsInChildren<MeshFilter>();

            foreach (var mf in t)
            {
                sliceableParts.Add(mf.gameObject);
            }

            planePos = Model.transform.rotation * planePos;
            planePos += Model.transform.position;
            planeNormal = Model.transform.rotation * planeNormal;
            
            slicedObject.transform.position = Model.transform.position;
            slicedObject.transform.rotation = Model.transform.rotation;


            foreach (var part in sliceableParts)
            {
                SlicedHull hull = part.Slice(planePos, planeNormal, intersectionMat);
                GameObject go;
                if (hull == null)
                {
                    go = Instantiate(part, part.transform.position, part.transform.rotation);
                    go.name = part.name;
                }
                else
                {
                    go = hull.CreateLowerHull(part, intersectionMat);
                    go.transform.position = part.transform.position;
                    go.transform.rotation = part.transform.rotation;
                }


                go.transform.parent = slicedObject.transform;
            }
           
            slicedObject.transform.parent = Model.transform;
            slicedObject.AddComponent<ModelSelect>();
            sliced = true;
        }

        /// <summary>
        /// Toggle between Sliced & unsliced vehicle model
        /// Can be used by the Web Interface
        /// </summary>
        public void ToggleSlice()
        {
            if (slicedObject == null) return;   //If the sliced model doesn't exist yet
                      
            toggleSlice = !toggleSlice;
            if (toggleSlice)                    
            {
                if (!sliced)    //If model isn't sliced yet, slice it
                {
                    CutModel();
                }
                Model.Model.SetActive(false);
                slicedObject.SetActive(true);
            }
            else
            {
                Model.Model.SetActive(true);
                slicedObject.SetActive(false);
            }
        }
    }
}