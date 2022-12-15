using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Replay;


namespace Replay
{
    public class ArmatureController : MonoBehaviour
    {
        public List<GameObject> Armature;
        // Start is called before the first frame update
        void Awake()
        {
            List<GameObject> avatar = GameObject.Find("ReplayManager").GetComponent<JavaScriptManager>().Avatar;
            for (int i = 0; i < Armature.Count; i++)
            {
                avatar.Add(Armature[i]);
            }
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
