//using Replay;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//[CustomEditor(typeof(VRViewLauncher))]
//public class VRViewLauncherEditor : Editor
//{

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector();

//        VRViewLauncher myScript = (VRViewLauncher)target;

//        if (GUILayout.Button("Toggle Heatmaps"))
//        {
//            JavaScriptManager.instanceJS.toggleHeatmap(0);
//            JavaScriptManager.instanceJS.toggleHeatmap(1);
//            JavaScriptManager.instanceJS.toggleHeatmap(2);
//            JavaScriptManager.instanceJS.toggleHeatmap(3);
//            JavaScriptManager.instanceJS.toggleHeatmap(4);
//            JavaScriptManager.instanceJS.toggleHeatmap(5);
//        }

//        if (GUILayout.Button("Toggle Trajectories"))
//        {
//            JavaScriptManager.instanceJS.toggleTrajectoy(0);
//            JavaScriptManager.instanceJS.toggleTrajectoy(1);
//            JavaScriptManager.instanceJS.toggleTrajectoy(2);
//        }

//        if (GUILayout.Button("Detach from Car"))
//        {
//            TimelineSyncerScript.Instance.Detach();
//        }


//        if (GUILayout.Button("Hide GUI"))
//        {
//            myScript.ACTUALBROWSER.SetActive(false);
//        }



//        if (GUILayout.Button("show GUI"))
//        {
//            myScript.ACTUALBROWSER.SetActive(true);
//        }

//    }
//}
