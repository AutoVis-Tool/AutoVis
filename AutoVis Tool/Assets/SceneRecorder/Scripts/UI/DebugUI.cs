using Replay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour
{
    public ReplayManager Player;

    /// <summary>
    /// File Path Input field
    /// </summary>
    public InputField FilePath;

    /// <summary>
    /// Canvas component
    /// </summary>
    public Canvas c;

    public InputField type;


    public static DebugUI Instance;

    void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// Load File from UI Path
    /// </summary>
    /// <param name="online"></param>
    public void LoadFile(bool online)
    {
        if (FilePath.text != "")
        {
            if (online)
            {
                Player.LoadWebFile(FilePath.text);
            }
            else
            {
                Debug.Log(FilePath.text);
                Player.LoadFileFromDisk(FilePath.text);
            }
        }


    }

    public void DebugHighlight()
    {
        ModelManager.Instance.ToggleHighlightModelType(type.text);
    }


    private void Start()
    {
        c = GetComponent<Canvas>();
        //LoadFile(false);
#if UNITY_WEBGL && !UNITY_EDITOR
                 LoadFile(true);
#endif
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.H)) //Toggle UI visibility
        //{

        //    c.enabled = !c.enabled;

        //}
    }




}
