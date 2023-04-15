using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class ConnectNetwork : MonoBehaviour
{
    private string GameVersion = "1.0";
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }

}
