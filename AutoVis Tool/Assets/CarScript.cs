using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Replay;

namespace Replay
{

    public class CarScript : MonoBehaviour
    {

        public GameObject mainCar;
        public Heatmap interiorheatmap;
        public Heatmap interiorsteeringwheelheatmap;
        public Heatmap interiorwindowsheatmap;
        public Heatmap interiordisplayheatmap;

        public Heatmap radiusHeatmap;

        public Heatmap radiusHeatmap1;

        public Heatmap radiusHeatmap2;

        public GameObject passengerModel;


        // Start is called before the first frame update

        void Awake()
        {
            // JavaScriptManager.instanceJS.interiorheatmap = interiorheatmap;
            // JavaScriptManager.instanceJS.interiorsteeringwheelheatmap = interiorsteeringwheelheatmap;
            // JavaScriptManager.instanceJS.interiorwindowsheatmap = interiorwindowsheatmap;
            // JavaScriptManager.instanceJS.interiordisplayheatmap = interiordisplayheatmap;
            JavaScriptManager.instanceJS.radiusHeatmap = radiusHeatmap;
            JavaScriptManager.instanceJS.radiusHeatmap1 = radiusHeatmap1;
            JavaScriptManager.instanceJS.radiusHeatmap2 = radiusHeatmap2;
            JavaScriptManager.instanceJS.mainCar = mainCar;
            JavaScriptManager.instanceJS.PassengerModel = passengerModel;
            Vector4[] arr = new Vector4[100];
            radiusHeatmap.GetComponent<Heatmap>().material.SetInt("_Points_Length", 100);
            radiusHeatmap.GetComponent<Heatmap>().material.SetVectorArray("_Points", arr);
            radiusHeatmap.GetComponent<Heatmap>().material.SetVectorArray("_Properties", arr);
            radiusHeatmap1.GetComponent<Heatmap>().material.SetInt("_Points_Length", 100);
            radiusHeatmap1.GetComponent<Heatmap>().material.SetVectorArray("_Points", arr);
            radiusHeatmap1.GetComponent<Heatmap>().material.SetVectorArray("_Properties", arr);
            radiusHeatmap2.GetComponent<Heatmap>().material.SetInt("_Points_Length", 100);
            radiusHeatmap2.GetComponent<Heatmap>().material.SetVectorArray("_Points", arr);
            radiusHeatmap2.GetComponent<Heatmap>().material.SetVectorArray("_Properties", arr);
            EventController.Instance.modelController = GetComponent<ModelController>();
            EventController.Instance.passengerModel = passengerModel;
        }
        void Start()
        {
            JavaScriptManager.instanceJS.InitRadiusus();

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
