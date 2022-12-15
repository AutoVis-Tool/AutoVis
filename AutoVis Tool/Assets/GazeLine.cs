using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Replay;

public class GazeLine : MonoBehaviour
{
    public GameObject eyes;
    public Material[] materials;
    public GameObject portal;
    public bool brakeEvent;
    JavaScriptManager jsManager = JavaScriptManager.instanceJS;
    ReplayManager replayManager = ReplayManager.Instance;
    private int partIndex = 0;
    private LineRenderer thisLineRenderer;
    private int[] offset = { 0, 84, 282 };
    // Start is called before the first frame update
    void Start()
    {
        int maxParts = 0;
        foreach (Transform currentChild in transform.parent.parent)
        {
            if (currentChild.name == "Prefab Humanoid 1(Clone)")
            {
                if (currentChild.GetChild(2) == transform)
                {
                    partIndex = maxParts;
                }
                maxParts++;
            }
        }

        if (partIndex == maxParts - 1)
        {
            Destroy(gameObject);
        } else
        {
            thisLineRenderer = gameObject.GetComponent<LineRenderer>();
            thisLineRenderer.material = materials[partIndex];
        }
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        thisLineRenderer.SetPosition(0, eyes.transform.position);
        thisLineRenderer.SetPosition(1, portal.transform.position);
        //if (brakeEvent)
        //{
        //    thisLineRenderer.SetPosition(1, portal.transform.position);
        //} else
        //{
        //    thisLineRenderer.SetPosition(1, jsManager.gazePoints[partIndex][replayManager.CurrentFrame]);
        //}
    }
}
