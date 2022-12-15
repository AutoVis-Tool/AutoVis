using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Replay;

public class PointingLine : MonoBehaviour
{
    public Material[] materials;
    public GameObject finger;
    public GameObject portal;

    private int partIndex = 0;
    private LineRenderer thisLineRenderer;
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
        }
        else
        {
            thisLineRenderer = gameObject.GetComponent<LineRenderer>();
            thisLineRenderer.material = materials[partIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(portal.activeInHierarchy)
        {
            thisLineRenderer.SetPosition(0, finger.transform.position);
            thisLineRenderer.SetPosition(1, portal.transform.position);
        }
    }
}
