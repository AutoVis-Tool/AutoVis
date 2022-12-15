using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace Replay
{

    /// <summary>
    /// Script sitting on top of a model to make it clickable
    /// </summary>
    public class ModelSelect : MonoBehaviour, IPointerClickHandler
    {
        /// <summary>
        /// Reference to the Models model controller
        /// </summary>
        private ModelController controller;

        void Start()
        {
            // if (gameObject.name == "SimplifiedCar")
            // {
            //     controller = transform.GetComponent<ModelController>();
            // }
            // else
            // {
            controller = transform.parent.GetComponent<ModelController>();
            //}

        }

        void OnMouseDown()
        {
            // ModelManager.Instance.HighlightModel(controller); //If model is clicked, focus on it
        }

        void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(0))
            {

                ModelManager.Instance.HighlightModel(controller); //If model is clicked, focus on it
            }

            if (Input.GetMouseButtonDown(2))
            {
                ModelManager.Instance.DeHighlightModel(controller);
            }
        }

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            GameObject worldSpaceCanvas = GameObject.Find("World Space Canvas(Clone)");
            worldSpaceCanvas.transform.GetChild(0).gameObject.SetActive(false);
            worldSpaceCanvas.transform.GetChild(1).gameObject.SetActive(false);
            GameObject context = worldSpaceCanvas.transform.GetChild(2).gameObject;
            context.SetActive(true);
            context.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = controller.Key;
        }
    }
}