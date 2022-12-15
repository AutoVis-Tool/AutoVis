using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Replay
{
    public class ModelController : MonoBehaviour
    {
        /// <summary>
        /// Identifier
        /// </summary>
        public string Key;

        /// <summary>
        /// Type of the Model
        /// </summary>
        public string Type;

        /// <summary>
        /// ID of the Model
        /// </summary>
        public int ID;

        /// <summary>
        /// Name of the Model
        /// We use the same as the GameObject's name as this makes identifying it easier
        /// </summary>
        public string Name { get => gameObject.name; set => gameObject.name = value; }

        /// <summary>
        /// The Model
        /// </summary>
        public GameObject Model;


        public bool ScaleModel = true;

        /// <summary>
        /// Dictionary that stores the data of an object as JToken with the timeStamp of the JToken as key
        /// </summary>
        public Dictionary<double, JToken> FrameData = new Dictionary<double, JToken>();

        /// <summary>
        /// Whether or not the Model is Active in the Current Frame
        /// </summary>
        public bool Active;

        public JToken CurrentData;

        private Renderer[] mRenderers;

        private Outline outline;

        private PathHighlighter pathHighlighter;

        void Awake()
        {
            if (!gameObject.TryGetComponent(out outline))
            {


                outline = gameObject.AddComponent<Outline>();


            }
            outline.enabled = false;

            if (!gameObject.TryGetComponent(out pathHighlighter))
            {
                pathHighlighter = gameObject.AddComponent<PathHighlighter>();
            }


        }

        /// <summary>
        /// Called upon first creation of the object
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeStamp"></param>
        public void Setup(JToken data, double timeStamp)
        {
            //TODO: Add initial setup here
            Name = data.Value<string>("name");
            Type = data.Value<string>("type");
            ID = data.Value<int>("id");
            Key = ID.ToString() + "_" + Type + "_" + Name;
            Active = false;

            mRenderers = GetComponentsInChildren<Renderer>();
            CurrentData = data;
            AddDataTimeStamp(data, timeStamp);
            UpdateModel(timeStamp);

            Model.SetActive(false);
            Active = false;
            if (this.Type == "Tesla")
            {
                Destroy(gameObject.GetComponent<Outline>());
                Outline outline = transform.GetChild(0).gameObject.AddComponent<Outline>();
                outline.enabled = false;
                Destroy(gameObject.GetComponent<PathHighlighter>());
                PathHighlighter pathHighlighter = transform.GetChild(0).gameObject.AddComponent<PathHighlighter>();

            }
        }

        /// <summary>
        /// After all data is there, this is called to finalize the setup
        /// </summary>
        public void FinalizeSetup()
        {
            pathHighlighter.CreateLine();
        }

        /// <summary>
        /// Adds a new recorded timestamp to the model
        /// </summary>
        /// <param name="data"></param>
        /// <param name="timeStamp"></param>
        public void AddDataTimeStamp(JToken data, double timeStamp)
        {
            FrameData.Add(timeStamp, data);

            pathHighlighter.LinePositions.Add((Vector3)data["position"].ToObject(typeof(Vector3)) + new Vector3(0, 0.5f, 0));
        }

        /// <summary>
        /// Update Model to show data represented at the given timeStamp
        /// </summary>
        /// <param name="timeStamp"></param>
        public void UpdateModel(double timeStamp)
        {
            if (!Active)
            {
                Active = true;
                //SetRendererAlphas(1f);
                Model.SetActive(true);
            }

            if (FrameData.TryGetValue(timeStamp, out CurrentData))  //Frame Data exists
            {
                transform.position = (Vector3)CurrentData["position"].ToObject(typeof(Vector3));

                if (ScaleModel)  //If it's a model that's scalable scale it, also adjust postion according to height to make sure it's not in the ground
                {
                    //Model.transform.localScale = (Vector3)CurrentData["size"].ToObject(typeof(Vector3));
                    Model.transform.localScale = new Vector3(1f, 1f, 1f);
                    Model.transform.localPosition = new Vector3(0, Model.transform.localScale.y / 2, 0);

                }
                transform.rotation = Quaternion.Euler((Vector3)CurrentData["rotation"].ToObject(typeof(Vector3)));    //TODO: RE-enable this once the Recording Mesh issue has been resolved
                if (transform.GetChild(0).eulerAngles.x > 270 && transform.GetChild(0).eulerAngles.x < 290)
                {
                    transform.GetChild(0).localEulerAngles += new Vector3(90, 0, 0);
                }

            }
            else //This means that there is no recorded data for this frame. Thus, the object should not be visible
            {
                //TODO Hide the model. Maybe?
            }


        }

        public void Deactivate()
        {
            Active = false;

            if (CameraManager.Instance.Follow && CameraManager.Instance.FollowModel.Key == Key)
            {
                CameraManager.Instance.StopFollow();
            }

            //TODO: Whatever happens once we hide the model
            Model.SetActive(false);
            // SetRendererAlphas(0.3f);
        }

        /// <summary>
        /// If the object is highlighted
        /// </summary>
        public void Highlight()
        {
            if (Type == "Tesla")
            {
                transform.GetChild(0).GetComponent<Outline>().enabled = true;
            }
            else
            {
                outline.enabled = true;
            }

            // pathHighlighter.DrawLine();
            pathHighlighter.PathRenderer.enabled = true;
        }

        /// <summary>
        /// If the object is no longer highlighted
        /// </summary>
        public void DeHighlight()
        {
            if (Type == "Tesla")
            {
                transform.GetChild(0).GetComponent<Outline>().enabled = false;
            }
            else
            {
                outline.enabled = false;

            }
            pathHighlighter.PathRenderer.enabled = false;
        }



        private void SetRendererAlphas(float alpha)
        {
            for (int i = 0; i < mRenderers.Length; i++)
            {
                for (int j = 0; j < mRenderers[i].materials.Length; j++)
                {
                    Color matColor = mRenderers[i].materials[j].color;
                    matColor.a = alpha;
                    mRenderers[i].materials[j].color = matColor;
                }
            }
        }

    }
}
