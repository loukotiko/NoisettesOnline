using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class HostMenuScript : MonoBehaviour
{
    void Start()
    {
        if (NetworkManager.Singleton.IsClient || NetworkManager.Singleton.IsServer)
            gameObject.SetActive(false);
    }

    public void OnClickPlay()
    {
        StartClient();
        gameObject.SetActive(false);
    }

    public void OnClickHost()
    {
        StartHost();
        gameObject.SetActive(false);
    }

    void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }

    void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    void StartServer()
    {
        NetworkManager.Singleton.StartServer();
    }
}
