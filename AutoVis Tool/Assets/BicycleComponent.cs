using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleComponent : MonoBehaviour
{

    private void Awake()
    {
        GameObject.Find("CustomBuildings 1").GetComponent<MapBuilderCustom>().CyclistGameobject = this.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
