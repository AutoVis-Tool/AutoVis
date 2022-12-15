using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using Replay;

public class UnityManager : MonoBehaviour
{
    /// <summary>
    /// Static Instance assigned on Start for easier access
    /// </summary>
    public static UnityManager INSTANCE;

    // #if UNITY_WEBGL && !UNITY_EDITOR



    [DllImport("__Internal")]
    private static extern void Hello();

    [DllImport("__Internal")]
    private static extern void SendClickedObjectData(string s);

    [DllImport("__Internal")]
    private static extern void SendModelsAll(string s);

    [DllImport("__Internal")]
    private static extern void SendTypesAll(string s);  //TODO: Write the javascript method for this

    [DllImport("__Internal")]
    private static extern void LoadingDone();

    [DllImport("__Internal")]
    private static extern void ProgressLoadingBar();

    void Start()
    {
        INSTANCE = this;

        //Hello();

        // HelloString("This is a string.");

        // float[] myArray = new float[10];
        // PrintFloatArray(myArray, myArray.Length);

        // int result = AddNumbers(5, 7);
        // Debug.Log(result);

        // Debug.Log(StringReturnValueFunction());

        // var texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
        // BindWebGLTexture(texture.GetNativeTexturePtr());
    }

    public void sendData(string s)
    {
        SendClickedObjectData(s);
    }

    public void SendModels(JArray arr)
    {
        // string modelsString = JsonUtility.ToJson(arr);
        //Debug.Log(arr.ToString());
#if UNITY_WEBGL && !UNITY_EDITOR
        SendModelsAll(arr.ToString());
#endif
    }

    public void SendTypes(JArray arr)
    {
        // string modelsString = JsonUtility.ToJson(arr);
        // Debug.Log(arr.ToString());
        // #if UNITY_WEBGL && !UNITY_EDITOR
        //         SendTypesAll(arr.ToString());
        // #endif
    }

    public void sendDoneLoading()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        LoadingDone();
#endif
    }

    public void ProgressBar()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ProgressLoadingBar();
#endif
    }

    // #endif
}