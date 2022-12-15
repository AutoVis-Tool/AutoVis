using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SinglePortal : MonoBehaviour, IPointerClickHandler
{

    public PortalHandler portalHandler;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventdata)
    {
        portalHandler.OnBoneClick();

    }
}
