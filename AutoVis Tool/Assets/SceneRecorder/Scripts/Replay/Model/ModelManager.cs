using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Replay;

namespace Replay
{
    /// <summary>
    /// Class that takes care of interacting with models
    /// </summary>
    public class ModelManager : MonoBehaviour
    {
        /// <summary>
        /// Static Instance of Manager. There is only one, this makes accessing it easier
        /// </summary>
        public static ModelManager Instance;

        /// <summary>
        /// Model that is used in case a Model Type is selected that does not have a special model
        /// </summary>
        public GameObject FallbackModel;

        /// <summary>
        /// A list of all available models
        /// </summary>
        public List<GameObject> ReplayModels;

        /// <summary>
        /// List of all Types
        /// </summary>
        public List<string> Types = new List<string>();

        /// <summary>
        /// Dictionary with unique string as key and Model as value
        /// Holds all models
        /// </summary>
        public Dictionary<string, ModelController> Models = new Dictionary<string, ModelController>();

        /// <summary>
        /// Dictionary that holds frames as keys and list of model reference. This makes updating the correct models super easy
        /// </summary>
        public Dictionary<double, List<ModelController>> timeStampModels = new Dictionary<double, List<ModelController>>();

        /// <summary>
        /// Model we are currently focusing on (aka has an outline)
        /// </summary>
        public ModelController FocusedModel;

        /// <summary>
        /// Holds all currently highlighted models
        /// </summary>
        private List<string> _highlightedTypes = new List<string>();


        void Start()
        {
            Instance = this;
            ImportModels("ReplayModels");
            EventController.Instance.ImportEventIcons();
            EventController.Instance.LoadEventFromDisk("Recordings/Events.json", 0);
            //EventController.Instance.LoadEventFromDisk("Recordings/Events_Part1.json", 0);
            //EventController.Instance.LoadEventFromDisk("Recordings/Events_Part2.json", 1);
            //EventController.Instance.LoadEventFromDisk("Recordings/Events_Part3.json", 2);
            EventController.Instance.SetupAudioEvents();
            //ReplayManager.Instance.LoadFileFromDisk("Recordings/p1-neu.json");
            DebugUI.Instance.LoadFile(false);
            EventController.Instance.SetAvatarList();
            //ReplayManager.Instance.LoadFileFromDisk("../ASFI 3D View/Recordings/Part1 - All.json");
        }



        /// <summary>
        /// Loads models from a specified path
        /// </summary>
        /// <param name="path"></param>
        public void ImportModels(string path)
        {
            var loadGOs = Resources.LoadAll<GameObject>(path);
            ReplayModels = new List<GameObject>(loadGOs);
        }

        /// <summary>
        /// Find model in our model list based on name
        /// </summary>
        /// <param name="type"></param>
        /// <returns>the correct model or fallback</returns>
        public GameObject FindModel(string type)
        {

            var model = ReplayModels.FirstOrDefault(o => o.name.Equals(type));
            if (model != null)
                return model;

            return FallbackModel;
        }





        /// <summary>
        /// Fired by <see cref="ModelSelect"/> to highlight a model
        /// </summary>
        /// <param name="model"></param>
        public void HighlightModel(ModelController model)
        {
            if (FocusedModel != null)
            {

                if (FocusedModel.Key == model.Key)
                {
                    // if (!CameraManager.Instance.Follow)
                    // {
                    CameraManager.Instance.FocusOnModel(model);
                    // }
                    // else {
                    //     DeHighlightModel(model);
                    // }

                }
                else
                {
                    FocusedModel.DeHighlight();
                    CameraManager.Instance.StopFollow();
                    model.Highlight();
                    FocusedModel = model;

                }
            }
            else
            {
                model.Highlight();
                FocusedModel = model;
            }

#if UNITY_WEBGL && !UNITY_EDITOR
        UnityManager.INSTANCE.sendData(FocusedModel.CurrentData.ToString());
#endif

        }


        /// <summary>
        /// Highlight a model based on it's unique string
        /// Can be called by Web Interface
        /// </summary>
        /// <param name="key">Unique key of model</param>
        public void HighlightModel(string key)
        {
            ModelController model;
            Models.TryGetValue(key, out model);

            HighlightModel(model);
        }

        public void DeHighlightModel(ModelController model)
        {
            if (FocusedModel == null) return;
            FocusedModel.DeHighlight();
            CameraManager.Instance.StopFollow();
            FocusedModel = null;

        }



        /// <summary>
        /// Highlight/Dehighlight models based on type
        /// Can be called by Web Interface
        /// </summary>
        /// <param name="type"></param>
        public void ToggleHighlightModelType(string type)
        {
            if (_highlightedTypes.Contains(type))   //If model type is already highlighted
            {
                _highlightedTypes.Remove(type);
                var models = Models.Where(o => o.Value.Type.Equals(type));
                foreach (var model in models)
                {
                    model.Value.DeHighlight();  //Dehighlight those
                }

                if (FocusedModel == null) return;
                if (FocusedModel.Type.Equals(type))
                {
                    FocusedModel.Highlight();
                }
            }
            else //if not highlighted yet, highlight them
            {
                _highlightedTypes.Add(type);
                var models = Models.Where(o => o.Value.Type.Equals(type));
                foreach (var model in models)
                {
                    model.Value.Highlight();
                }
            }



        }


        /// <summary>
        /// Forward all models to UnityManager 
        /// </summary>
        public void ForwardData()
        {
            JArray array = new JArray(Models.Keys);
            //          UnityManager.INSTANCE.SendModels(array);

            JArray tmp = new JArray(Types);
            //        UnityManager.INSTANCE.SendTypes(tmp);
        }

    }
}
