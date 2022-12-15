using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDataAllreadyLoaded : MonoBehaviour
{
    public bool AllreadyLoaded = false;
    private static SaveDataAllreadyLoaded instance;

    void Awake()
    {
        // Does another instance already exist?
        if (instance && instance != this)
        {
            // Destroy myself
            Destroy(gameObject);
            return;
        }

        // Otherwise store my reference and make me DontDestroyOnLoad
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        doStuff();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void doStuff()
    {
        if (AllreadyLoaded)
        {
            Debug.Log("IST LOADED");
        }
        else
        {
            AllreadyLoaded = true;
        }
    }
}
