using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using TMPro;

public class MyNetworkManager : NetworkManager
{
    public GameObject mainCar;
    public GameObject customAnnotation;
    public GameObject customLabel;

    public override void OnStartServer()
    {
        base.OnStartServer();
        gameObject.GetComponent<NetworkManagerHUD>().enabled = false;
        Debug.Log("Server started!");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        gameObject.GetComponent<NetworkManagerHUD>().enabled = false;
    }

    public override void OnStopServer()
    {
        base.OnStopServer();
        Debug.Log("Server stopped!");
    }

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject go = Instantiate(mainCar);
        NetworkServer.Spawn(go, conn);
        Debug.Log("New Player added: " + conn);
        base.OnServerAddPlayer(conn);
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        Debug.Log("Player " + conn + " disconnected from Server!");
    }

    public override void OnClientConnect()
    {
        base.OnClientConnect();
        Debug.Log("Connected to Server!");
    }


    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();
        Debug.Log("Disconnected to server!");
    }

    public void spawnCustomAnnotation(string annotationTitle, string annotationText, Sprite icon, Vector3 pos)
    {
        GameObject annotation = Instantiate(customAnnotation);
        annotation.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = annotationTitle;
        annotation.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = annotationText;
        annotation.transform.GetChild(2).GetComponent<RawImage>().texture = icon.texture;
        annotation.transform.position = pos;
        NetworkServer.Spawn(annotation);
    }

    public void spawnCustomLabel(string annotationTitle, Vector3[] points, Sprite icon)
    {
        GameObject label = Instantiate(customLabel);
        label.transform.position = points[0];
        label.GetComponent<LineRenderer>().positionCount = points.Length;
        label.GetComponent<LineRenderer>().SetPositions(points);
        label.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = annotationTitle;
        label.transform.GetChild(0).GetChild(1).GetComponent<RawImage>().texture = icon.texture;
        NetworkServer.Spawn(label);
    }
}