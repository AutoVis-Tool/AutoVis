using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PropertiesBillboard : MonoBehaviour
{

    public Text BillboardText;
    
    void LateUpdate()
    {
       transform.forward = Camera.main.transform.forward;
    }

    public void SetText(string s) {
        BillboardText.text = s;
    }
}
