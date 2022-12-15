using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Replay;
using TMPro;


namespace Replay
{

    public class EventController : MonoBehaviour
    {
        // Start is called before the first frame update

        public GameObject EventLine;

        public GameObject EventPoint;

        public GameObject SpeechBubble;

        private StreamReader reader;

        public Material material1;

        public Material material2;

        public Material material3;

        private int participantCounter = 1;

        public List<GameObject> ParticipantEventList = new List<GameObject>();


        public ModelController modelController;


        public JToken DataPoint;


        public static EventController Instance;

        public double SecondsBetweenPoints = 1f;

        //public double[] offsetArr = { 0, 2445.6584587, -74141.8629084 };
        public double[] offsetArr = { 0, 244.56584587, -74141.8629084 };

        //only first value important
        public double[] startArr = { 1662208381.8918169, 1662210979.0403559, 1662134426.0296292 };

        public float carHeight = 2f;

        public float startDistance = 2f;
        public float distance = 2f;

        public GameObject SingleParticipantPrefab;


        public Camera MainCamera;

        public Camera SubCamera;

        public List<Sprite> EventIcons;

        public Sprite FallBackSprite;

        public GameObject selectedEvent;

        public List<GameObject> allAvatars = new List<GameObject>();

        private bool DoneLoading = false;

        private int EventNumber;

        public GameObject passengerModel;

        public double selectedStartTime = 0;

        public double selectedEndTime = 0;

        public bool loop = false;


        //< Participantid < EventNumber < Passenger/participant  <Text>>>>
        public List<List<List<List<string>>>> voiceText = new List<List<List<List<string>>>>();




        void Awake()
        {
            Instance = this;
            fillVoiceText();
        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            UpdateSpeechBubble();

        }

        public void LoadEventFromDisk(string path, int participantId)
        {
            reader = new StreamReader(path, Encoding.UTF8, false, 65536);

            string content = reader.ReadToEnd();
            //Debug.Log(content);
            SetupEvents(content, participantId);
        }

        public void SetupEvents(string json, int participantId)
        {
            JArray main = JArray.Parse(json);
            // GameObject participant = new GameObject("ParticipantEventline" + participantCounter);
            GameObject participant = Instantiate(SingleParticipantPrefab);
            participant.name = "ParticipantEventline" + participantCounter;
            ParticipantEventList.Add(participant);
            SingleParticipantEventHandler singlePart = participant.GetComponent<SingleParticipantEventHandler>();
            singlePart.ParticipantEventId = participantId;
            SetupMaterialOfParticipant(singlePart, participantId);
            GameObject mainEventLine = new GameObject("MainEventLine" + participantCounter);
            GameObject copyEventLine = new GameObject("Copy" + participantCounter);
            mainEventLine.transform.parent = participant.transform;
            mainEventLine.tag = "MainEventLine";
            copyEventLine.transform.parent = participant.transform;
            copyEventLine.tag = "SubEventLine";
            copyEventLine.SetActive(false);
            foreach (JToken t in main)    //foreach recorded frame
            {


                int counterForEventsInside = 0;
                foreach (JToken values in t["data"])
                {

                    if (values["valueY"].ToString() != "Nothing")
                    {
                        GameObject category = GetChildifExistsWithName(mainEventLine, t["dataType"].ToString());
                        if (category != null)
                        {
                            GameObject eventline = Instantiate(EventLine, category.transform);
                            eventline.name = t["dataType"].ToString();
                            SingleEventData eventdata = eventline.GetComponent<SingleEventData>();
                            eventdata.ParentEventType = t["dataType"].ToString();
                            eventdata.EventType = values["valueY"].ToString();
                            eventdata.EventTime = (double)values["valueX"] - offsetArr[participantId];
                            eventdata.ParticipantID = participantId;
                            if (t["data"].Count<JToken>() > counterForEventsInside + 1)
                            {
                                eventdata.EventEndTime = (double)t["data"][counterForEventsInside + 1]["valueX"] - offsetArr[participantId];
                            }
                            GameObject copyCategory = GetChildifExistsWithName(copyEventLine, t["dataType"].ToString());
                            GameObject copy = Instantiate(eventline, copyCategory.transform);
                            SetIcon(eventline, eventdata.EventType);
                            SetIcon(copy, eventdata.EventType);
                            SetupMaterial(eventline, participantCounter);
                            SetupMaterial(copy, participantCounter);
                        }
                        else
                        {
                            GameObject Category = new GameObject(t["dataType"].ToString());
                            GameObject CopyCategory = new GameObject(t["dataType"].ToString());
                            Category.transform.parent = mainEventLine.transform;
                            CopyCategory.transform.parent = copyEventLine.transform;
                            GameObject eventline = Instantiate(EventLine, Category.transform);
                            eventline.name = t["dataType"].ToString();
                            SingleEventData eventdata = eventline.GetComponent<SingleEventData>();
                            eventdata.ParentEventType = t["dataType"].ToString();
                            eventdata.EventType = values["valueY"].ToString();
                            eventdata.EventTime = (double)values["valueX"];
                            eventdata.ParticipantID = participantId;
                            if (t["data"].Count<JToken>() > counterForEventsInside + 1)
                            {
                                eventdata.EventEndTime = (double)t["data"][counterForEventsInside + 1]["valueX"];
                            }
                            GameObject copy = Instantiate(eventline, CopyCategory.transform);
                            SetIcon(eventline, eventdata.EventType);
                            SetIcon(copy, eventdata.EventType);
                            SetupMaterial(eventline, participantCounter);
                            SetupMaterial(copy, participantCounter);
                        }
                        //counterForEventsInside++;


                    }
                    counterForEventsInside++;

                }
                // for (int i = 0; i < counterForEventsInside; i++)
                // {

                // }

            }


            participantCounter++;
        }

        void SetupMaterialOfParticipant(SingleParticipantEventHandler singlePart, int participantCount)
        {
            switch (participantCount)
            {
                case 0:
                    singlePart.ParticipantMaterial = material3;
                    break;
                case 1:
                    singlePart.ParticipantMaterial = material1;
                    break;
                case 2:
                    singlePart.ParticipantMaterial = material2;
                    break;
            }
        }

        public void SetupPointsOfEventline()
        {

            for (int i = 0; i < ParticipantEventList.Count; i++)
            {
                for (int k = 0; k < ParticipantEventList[i].transform.childCount; k++)
                {
                    for (int m = 0; m < ParticipantEventList[i].transform.GetChild(k).childCount; m++)
                    {
                        for (int j = 0; j < ParticipantEventList[i].transform.GetChild(k).GetChild(m).childCount; j++)
                        {
                            SingleEventData singleEventData = ParticipantEventList[i].transform.GetChild(k).GetChild(m).GetChild(j).GetComponent<SingleEventData>();
                            double pointStart = returnTimeStamp(singleEventData.EventTime);
                            double pointEndTime = returnTimeStamp(singleEventData.EventEndTime);
                            double currentPointTime = pointStart;
                            while (currentPointTime < pointEndTime)
                            {
                                currentPointTime = returnTimeStamp(currentPointTime);
                                if (modelController.FrameData.TryGetValue(currentPointTime, out DataPoint))
                                {
                                    GameObject pointOfEvent = Instantiate(EventPoint, ParticipantEventList[i].transform.GetChild(k).GetChild(m).GetChild(j));
                                    pointOfEvent.transform.position = new Vector3(((Vector3)DataPoint["position"].ToObject(typeof(Vector3))).x, ((Vector3)DataPoint["position"].ToObject(typeof(Vector3))).y + carHeight + distance, ((Vector3)DataPoint["position"].ToObject(typeof(Vector3))).z);
                                }
                                currentPointTime += SecondsBetweenPoints;

                            }
                        }
                    }

                }
                distance += startDistance;
            }
            SetupEventLineRenderer();
        }

        public void SetupEventLineRenderer()
        {
            distance = startDistance;
            for (int i = 0; i < ParticipantEventList.Count; i++)
            {
                for (int w = 0; w < ParticipantEventList[i].transform.childCount; w++)
                {
                    for (int m = 0; m < ParticipantEventList[i].transform.GetChild(w).childCount; m++)
                    {
                        for (int j = 0; j < ParticipantEventList[i].transform.GetChild(w).GetChild(m).childCount; j++)
                        {
                            if (ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).tag != "EventIcon")
                            {

                                SingleEventData singleEventData = ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).GetComponent<SingleEventData>();
                                LineRenderer line = ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).GetComponent<LineRenderer>();
                                double pointStart = returnTimeStamp(singleEventData.EventTime);
                                double pointEndTime = returnTimeStamp(singleEventData.EventEndTime);
                                if (singleEventData.EventEndTime != 0)
                                {


                                    if (ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).childCount > 0)
                                    {

                                        line.positionCount = 2 + ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).transform.childCount - 1;
                                        Vector3[] addArr = new Vector3[2 + ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).transform.childCount];
                                        if (modelController.FrameData.TryGetValue(pointStart, out DataPoint))
                                        {
                                            Vector3 data = (Vector3)DataPoint["position"].ToObject(typeof(Vector3));
                                            data.y += carHeight + distance;
                                            addArr[0] = data;
                                        }
                                        for (int k = 1; k < ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).transform.childCount; k++)
                                        {
                                            Vector3 data = ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).GetChild(k).position;
                                            addArr[k] = data;

                                        }
                                        if (modelController.FrameData.TryGetValue(pointEndTime, out DataPoint))
                                        {
                                            Vector3 data = (Vector3)DataPoint["position"].ToObject(typeof(Vector3));
                                            data.y += carHeight + distance;
                                            addArr[ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).transform.childCount] = data;
                                        }
                                        line.SetPositions(addArr);
                                    }
                                    else
                                    {
                                        line.positionCount = 2;
                                        Vector3[] addArr = new Vector3[2];
                                        if (modelController.FrameData.TryGetValue(pointStart, out DataPoint))
                                        {
                                            Vector3 data = (Vector3)DataPoint["position"].ToObject(typeof(Vector3));
                                            data.y += carHeight + distance;
                                            addArr[0] = data;
                                        }
                                        if (modelController.FrameData.TryGetValue(pointEndTime, out DataPoint))
                                        {
                                            Vector3 data = (Vector3)DataPoint["position"].ToObject(typeof(Vector3));
                                            data.y += carHeight + distance;
                                            addArr[1] = data;
                                        }
                                        line.SetPositions(addArr);
                                    }
                                }

                            }
                        }
                    }
                }
                distance += startDistance;
            }
            distance = startDistance;
            SetupCapsuleCollider();
        }


        public void SetupCapsuleCollider()
        {
            for (int i = 0; i < ParticipantEventList.Count; i++)
            {
                for (int w = 0; w < ParticipantEventList[i].transform.childCount; w++)
                {
                    for (int m = 0; m < ParticipantEventList[i].transform.GetChild(w).childCount; m++)
                    {
                        for (int j = 0; j < ParticipantEventList[i].transform.GetChild(w).GetChild(m).childCount; j++)
                        {
                            if (ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).childCount > 0)
                            {
                                for (int k = 0; k < ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).childCount; k++)
                                {
                                    if (ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).GetChild(k).tag != "EventIcon")
                                    {
                                        GameObject eventpoint = ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).GetChild(k).gameObject;
                                        //CapsuleCollider singleEventData = eventpoint.GetComponent<CapsuleCollider>();
                                        BoxCollider singleEventData = eventpoint.GetComponent<BoxCollider>();
                                        GameObject transformsave = new GameObject();
                                        transformsave.transform.position = eventpoint.transform.parent.GetComponent<LineRenderer>().GetPosition(k + 1);
                                        Transform target = transformsave.transform;
                                        eventpoint.transform.LookAt(target, Vector3.left);
                                        float distanceOfPointToNext = Vector3.Distance(transformsave.transform.position, eventpoint.transform.position);
                                        singleEventData.size = new Vector3(singleEventData.size.x, singleEventData.size.y, distanceOfPointToNext);
                                        singleEventData.center = new Vector3(singleEventData.center.x, singleEventData.center.y, (distanceOfPointToNext) / 2);
                                        Destroy(transformsave);
                                    }
                                }

                            }

                        }
                    }
                }
            }
            PostProcessAudioLines();
        }

        public double returnTimeStamp(double timeStamp)
        {
            double time = ReplayManager.Instance.TimeStamps.ClosestTo(timeStamp);
            //int CurrentFrame = Array.IndexOf(ReplayManager.Instance.TimeStamps, time);
            return time;
        }


        private void SetupMaterial(GameObject eventline, int participantCount)
        {
            switch (participantCount)
            {
                case 1:
                    eventline.GetComponent<LineRenderer>().material = material3;
                    break;
                case 2:
                    eventline.GetComponent<LineRenderer>().material = material1;
                    break;
                case 3:
                    eventline.GetComponent<LineRenderer>().material = material2;
                    break;
            }
        }
        GameObject GetChildifExistsWithName(GameObject obj, string name)
        {
            Transform trans = obj.transform;
            Transform childTrans = trans.Find(name);
            if (childTrans != null)
            {
                return childTrans.gameObject;
            }
            else
            {
                return null;
            }

        }
        public void DisableAllParticipantEvents()
        {
            for (int k = 0; k < ParticipantEventList.Count; k++)
            {
                ParticipantEventList[k].SetActive(false);
            }
        }

        public void EnableParticipantEvent(int id)
        {
            ParticipantEventList[id].SetActive(true);
        }


        public void EnableAllParticipant()
        {
            for (int i = 0; i < ParticipantEventList.Count; i++)
            {
                EnableParticipantEvent(i);
            }

        }


        public void SwitchCamera(bool main)
        {
            if (main)
            {
                MainCamera.enabled = true;
                SubCamera.enabled = false;
                MainCamera.gameObject.transform.position = SubCamera.gameObject.transform.position;
                MainCamera.gameObject.transform.rotation = SubCamera.gameObject.transform.rotation;
            }
            else
            {
                MainCamera.enabled = false;
                SubCamera.enabled = true;
                SubCamera.gameObject.transform.position = MainCamera.gameObject.transform.position;
                SubCamera.gameObject.transform.rotation = MainCamera.gameObject.transform.rotation;
            }

        }


        public void HandleOnHover(bool boolcamera, string Eventtype)
        {
            // Debug.Log("jo");
            // Debug.Log(Eventtype);
            //RemoveLayer();
            //AddLayer(Eventtype, 7);
            //SwitchCamera(boolcamera);
        }

        void RemoveLayer()
        {
            for (int i = 0; i < ParticipantEventList.Count; i++)
            {
                for (int w = 0; w < ParticipantEventList[i].transform.childCount; w++)
                {
                    for (int m = 0; m < ParticipantEventList[i].transform.GetChild(w).childCount; m++)
                    {
                        for (int j = 0; j < ParticipantEventList[i].transform.GetChild(w).GetChild(m).childCount; j++)
                        {
                            ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).gameObject.layer = 0;
                            // foreach (Transform child in ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j))
                            // {
                            //     child.gameObject.layer = 0;
                            // }

                        }
                    }
                }
            }
        }

        void AddLayer(string type, int layerNumber)
        {
            for (int i = 0; i < ParticipantEventList.Count; i++)
            {
                for (int w = 0; w < ParticipantEventList[i].transform.childCount; w++)
                {
                    for (int m = 0; m < ParticipantEventList[i].transform.GetChild(w).childCount; m++)
                    {
                        for (int j = 0; j < ParticipantEventList[i].transform.GetChild(w).GetChild(m).childCount; j++)
                        {
                            if (ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).GetComponent<SingleEventData>().EventType == type)
                            {
                                ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j).gameObject.layer = layerNumber;
                                foreach (Transform child in ParticipantEventList[i].transform.GetChild(w).GetChild(m).GetChild(j))
                                {
                                    child.gameObject.layer = layerNumber;
                                }
                            }

                        }
                    }
                }
            }

        }

        public void SetupAudioEvents()
        {

            //AddAudioEvents();
            //AudioEvent("Recordings/audio_Part1_old.json", 0);
            //AudioEvent("Recordings/audio_Part2.json", 1);
            //AudioEvent("Recordings/audio_Part3.json", 2);
        }

        void AddAudioEvents()
        {
            for (int i = 0; i < ParticipantEventList.Count; i++)
            {
                for (int w = 0; w < ParticipantEventList[i].transform.childCount; w++)
                {
                    GameObject audioline = new GameObject("Audio");
                    audioline.transform.parent = ParticipantEventList[i].transform.GetChild(w);

                }
            }

        }
        void AudioEvent(string path, int participantNumber)
        {
            reader = new StreamReader(path, Encoding.UTF8, false, 65536);

            string content = reader.ReadToEnd();

            JArray main = JArray.Parse(content);

            GameObject mainEventLine = ParticipantEventList[participantNumber].transform.GetChild(0).GetChild(ParticipantEventList[participantNumber].transform.GetChild(0).childCount - 1).gameObject;
            GameObject copyEventLine = ParticipantEventList[participantNumber].transform.GetChild(1).GetChild(ParticipantEventList[participantNumber].transform.GetChild(1).childCount - 1).gameObject;

            int counterForEventsInside = 0;
            foreach (JToken t in main)    //foreach recorded frame
            {



                if ((double)t["value"] > -50)
                {

                    GameObject eventline = Instantiate(EventLine, mainEventLine.transform);
                    eventline.name = "Audio";
                    SingleEventData eventdata = eventline.GetComponent<SingleEventData>();
                    eventdata.ParentEventType = "Audio";
                    eventdata.EventType = "Audio";
                    eventdata.EventTime = (double)t["timeStamp"] + startArr[0];

                    if (main.Count<JToken>() > counterForEventsInside + 1)
                    {
                        eventdata.EventEndTime = (double)main[counterForEventsInside + 1]["timeStamp"] + startArr[0];
                    }
                    GameObject copy = Instantiate(eventline, copyEventLine.transform);
                    SetIcon(eventline, eventdata.EventType);
                    SetIcon(copy, eventdata.EventType);
                    SetupMaterial(eventline, participantNumber + 1);
                    SetupMaterial(copy, participantNumber + 1);

                }
                counterForEventsInside++;
            }
        }


        public void SetIcon(GameObject obj, string type)
        {
            obj.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = FindSprite(type);
        }

        public void ImportEventIcons()
        {
            var loadGOs = Resources.LoadAll<Sprite>("EventIcons");
            EventIcons = new List<Sprite>(loadGOs);

        }

        public Sprite FindSprite(string type)
        {
            var model = EventIcons.FirstOrDefault(o => o.name.Equals(type));
            if (model != null)
                return model;

            return FallBackSprite;
        }




        public void PostProcessAudioLines()
        {
            for (int i = 0; i < ParticipantEventList.Count; i++)
            {
                for (int j = 0; j < ParticipantEventList[i].transform.GetChild(ParticipantEventList[i].transform.childCount - 1).childCount; j++)
                {

                }
            }
        }

        public void HandleEvent()
        {
            if (selectedEvent != null && DoneLoading)
            {
                Debug.Log("yes");
                SingleEventData singleEvent = selectedEvent.GetComponent<SingleEventData>();
                switch (singleEvent.EventType)
                {
                    case "Voice Command":
                        Debug.Log("da");
                        EventNumber = 0;
                        VoiceCommandEvent(singleEvent);
                        break;
                    case "Second Passenger Enters":
                        EventNumber = 1;
                        SecondPassengerEntersEvent(singleEvent);
                        break;
                    case "Emergency Break: Road Crossing":
                        EventNumber = 2;
                        EmergencyBreakEvent(singleEvent);
                        break;
                    case "Pointing & Voice":
                        EventNumber = 3;
                        PointingandVoice(singleEvent);
                        break;
                    default:
                        break;
                }
            }
        }


        private void VoiceCommandEvent(SingleEventData singleEvent)
        {
            EnableParticipantOutline(singleEvent.ParticipantID);
            DisplayVoiceLines(singleEvent);
            DisplayThaughtPortal(singleEvent);
            DisplayGazeLine(singleEvent);
        }


        private void SecondPassengerEntersEvent(SingleEventData singleEvent)
        {
            EnableParticipantOutline(singleEvent.ParticipantID);
            DisplayVoiceLines(singleEvent);
            DisplayGazeLine(singleEvent);
        }
        private void EmergencyBreakEvent(SingleEventData singleEvent)
        {
            EnableParticipantOutline(singleEvent.ParticipantID);
            DisplayVoiceLines(singleEvent);
            DisplayGazeLine(singleEvent);
            DisplayGazePortal(singleEvent);
        }

        private void PointingandVoice(SingleEventData singleEvent)
        {
            EnableParticipantOutline(singleEvent.ParticipantID);
            DisplayVoiceLines(singleEvent);
            DisplayGazeLine(singleEvent);
            DisplayPointingLine(singleEvent);
            DisplayPortal(singleEvent);
            DisplayTowerHeatmap(singleEvent);
        }




        private void DisplayVoiceLines(SingleEventData singleEvent)
        {
            Debug.Log("ok");
            Transform speechParent = JavaScriptManager.instanceJS.mainCar.transform;
            for (int i = 0; i < voiceText[singleEvent.ParticipantID][EventNumber].Count; i++)
            {
                int counterSpeechBubble = 0;
                float distanceSpeechbubble = 0.6f;
                for (int j = 0; j < voiceText[singleEvent.ParticipantID][EventNumber][i].Count; j++)
                {
                    Debug.Log("los");
                    if (i == 0)
                    {
                        Debug.Log(voiceText[singleEvent.ParticipantID][EventNumber][i][j]);
                        GameObject newSpeechBubble = Instantiate(SpeechBubble, speechParent);
                        newSpeechBubble.tag = "SpeechBubble";
                        newSpeechBubble.transform.position = allAvatars[singleEvent.ParticipantID].transform.position + new Vector3(0, 2.5f, 0) + new Vector3(0, distanceSpeechbubble * counterSpeechBubble, 0); ;
                        newSpeechBubble.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = voiceText[singleEvent.ParticipantID][EventNumber][i][j];
                        newSpeechBubble.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = allAvatars[singleEvent.ParticipantID].transform.GetChild(0).GetComponent<Outline>().OutlineColor;
                    }
                    else
                    {
                        GameObject newSpeechBubble = Instantiate(SpeechBubble, speechParent);
                        newSpeechBubble.tag = "SpeechBubble";
                        newSpeechBubble.transform.position = passengerModel.transform.position + new Vector3(0, 2.5f, 0) + new Vector3(0, distanceSpeechbubble * counterSpeechBubble, 0);
                        newSpeechBubble.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = voiceText[singleEvent.ParticipantID][EventNumber][i][j];
                        newSpeechBubble.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = allAvatars[singleEvent.ParticipantID].transform.GetChild(0).GetComponent<Outline>().OutlineColor;

                    }
                    counterSpeechBubble++;


                }


            }

        }

        private void DisplayGazeLine(SingleEventData singleEvent)
        {
            GazeLine gazeLine = allAvatars[singleEvent.ParticipantID].transform.GetChild(2).GetComponent<GazeLine>();
            gazeLine.brakeEvent = true;
            allAvatars[singleEvent.ParticipantID].transform.GetChild(2).gameObject.SetActive(true);
        }

        private void DisplayPointingLine(SingleEventData singleEvent)
        {
            PointingLine pointingLine = allAvatars[singleEvent.ParticipantID].transform.GetChild(2).GetComponent<PointingLine>();
            //pointingLine.brakeEvent = true;
            allAvatars[singleEvent.ParticipantID].transform.GetChild(3).gameObject.SetActive(true);
        }

        private void DisplayThaughtPortal(SingleEventData singleEvent)
        {
            allAvatars[singleEvent.ParticipantID].GetComponent<PortalHandler>().EnableThaughtPortal();
        }

        private void DisplayPortal(SingleEventData singleEvent)
        {
            allAvatars[singleEvent.ParticipantID].GetComponent<PortalHandler>().EnablePortal();
        }

        private void DisplayGazePortal(SingleEventData singleEvent)
        {
            allAvatars[singleEvent.ParticipantID].GetComponent<PortalHandler>().EnableGazePortal();
        }

        private void DisplayTowerHeatmap(SingleEventData singleEvent)
        {
            GameObject.Find("San Fran Tower").GetComponent<Outline>().enabled = true;
        }




        private void EnableParticipantOutline(int participantid)
        {

            if (!allAvatars[participantid].transform.GetChild(0).GetComponent<Outline>().enabled)
            {

                foreach (GameObject avatar in allAvatars)
                {
                    avatar.transform.GetChild(0).GetComponent<Outline>().enabled = false;
                }
                allAvatars[participantid].transform.GetChild(0).GetComponent<Outline>().enabled = true;

            }
        }


        public void SetAvatarList()
        {
            allAvatars = GameObject.FindGameObjectsWithTag("AvatarModell").ToList();
            DoneLoading = true;
        }






        private void UpdateSpeechBubble()
        {
            if (selectedStartTime != 0 && selectedEndTime != 0)
            {
                if(loop)
                {
                    if (ReplayManager.Instance.CurrentTimeStamp >= selectedEndTime)
                    {
                        ReplayManager.Instance.GoToNearestTimeStamp(selectedStartTime);
                    }
                } else
                {
                    //if (ReplayManager.Instance.CurrentTimeStamp < selectedStartTime || ReplayManager.Instance.CurrentTimeStamp > selectedEndTime)
                    //{
                    foreach (GameObject avatar in allAvatars)
                    {
                        avatar.transform.GetChild(2).gameObject.SetActive(false);
                        avatar.GetComponent<PortalHandler>().DisablePortal();
                        avatar.GetComponent<PortalHandler>().DisableGazePortal();
                        avatar.transform.GetChild(2).gameObject.SetActive(false);
                        GazeLine gazeLine = avatar.transform.GetChild(2).GetComponent<GazeLine>();
                        gazeLine.brakeEvent = false;
                        avatar.transform.GetChild(3).gameObject.SetActive(false);
                        avatar.GetComponent<PortalHandler>().DisableThaughtPortal();
                        GameObject.Find("San Fran Tower").GetComponent<Outline>().enabled = false;
                    }
                    List<GameObject> allSpeechbubbles = GameObject.FindGameObjectsWithTag("SpeechBubble").ToList();
                    foreach (GameObject g in allSpeechbubbles)
                    {
                        Destroy(g);
                    }
                    selectedStartTime = 0;
                    selectedEndTime = 0;
                    //}
                }
            }
        }












        private void fillVoiceText()
        {

            //Event 1 Lombard Event 2 Passenger enters Event 3 Cyclist Event 4 Pointing on san fran tower
            // voiceText
            List<List<List<string>>> eventNumber = new List<List<List<string>>>();
            List<List<string>> group = new List<List<string>>();


            //Event 1
            List<string> part1 = new List<string>();
            List<string> pass1 = new List<string>();
            part1.Add("Where are am? Where am i currently?");
            group.Add(part1);
            eventNumber.Add(group);
            //Event 2
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            pass1.Add("Hey! What's up?");
            part1.Add("Hey! What's up man?");
            pass1.Add("Where are you headed to?");
            part1.Add("I'm heading to Main Street and you?");
            pass1.Add("Wow me too");
            part1.Add("Nice!");
            pass1.Add("Nice!");
            group.Add(part1);
            group.Add(pass1);
            eventNumber.Add(group);
            //Event 3
            part1 = new List<string>();
            pass1 = new List<string>();
            part1.Add("Wow! Did you see that cyclist?");
            pass1.Add("Oh yes!");
            group.Add(part1);
            group.Add(pass1);
            eventNumber.Add(group);
            //Event 4
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            part1.Add("What is this building over there?");
            group.Add(part1);
            group.Add(pass1);
            eventNumber.Add(group);
            voiceText.Add(eventNumber);
            //Part 2
            //Event 1
            eventNumber = new List<List<List<string>>>();
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            part1.Add("Where are we?");
            group.Add(part1);
            eventNumber.Add(group);
            //Event 2
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            pass1.Add("Hello. What's up man?");
            part1.Add("Hey! What's up?");
            pass1.Add("Where are you heading to?");
            part1.Add("I'm going to Main Street and you ?");
            pass1.Add("Me too. Nice!");
            group.Add(part1);
            group.Add(pass1);
            eventNumber.Add(group);
            //Event 3
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            part1.Add("Oi! Did you see that cyclist?");
            pass1.Add("Yeah what was that ?");
            part1.Add("How rude!");
            pass1.Add("So rude");
            group.Add(part1);
            group.Add(pass1);
            eventNumber.Add(group);
            //Event 4
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            part1.Add("What's that building over there?");
            group.Add(part1);
            eventNumber.Add(group);
            voiceText.Add(eventNumber);

            //Part 3

            //Event 1
            eventNumber = new List<List<List<string>>>();
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            part1.Add("Where are we?");
            group.Add(part1);
            eventNumber.Add(group);
            //Event 2
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            pass1.Add("Hello. What's up man?");
            part1.Add("Hey! What's up?");
            pass1.Add("Where are you heading to?");
            part1.Add("I'm going to Main Street. How about you?");
            pass1.Add("Me too");
            group.Add(part1);
            group.Add(pass1);
            eventNumber.Add(group);
            //Event 3
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            part1.Add("Woah! Did you see that cyclist?");
            pass1.Add("Yeah what the heck ?");
            group.Add(part1);
            group.Add(pass1);
            eventNumber.Add(group);
            //Event 4
            part1 = new List<string>();
            pass1 = new List<string>();
            group = new List<List<string>>();
            part1.Add("What's that building over there called?");
            group.Add(part1);
            eventNumber.Add(group);
            voiceText.Add(eventNumber);


            // voiceText.Add()
        }

    }




}

