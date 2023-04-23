using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class NetworkManager : Photon.PunBehaviour
{
    string GameVersion = "1.0";
    
    public void connect()
    {
        
        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master. Loading to Select Game Mode");

    }

}
