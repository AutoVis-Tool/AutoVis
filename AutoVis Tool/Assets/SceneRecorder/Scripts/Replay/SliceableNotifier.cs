using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Replay {
    
    /// <summary>
    /// Notifies the <see cref="SliceManager"/> upon initialization that it is the model that's supposed to get sliced
    /// </summary>
    public class SliceableNotifier : MonoBehaviour
    {
        void Start()
        {
            SliceManager.Instance.Model = GetComponent<ModelController>();
        }

        private void Update()
        {


        }
    }
}
