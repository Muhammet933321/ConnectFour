using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectToNetwork : Photon.PunBehaviour
{
    string GameVersion = "1.0";
    private float ConnectToDelay = 2f;
    void Awake()
    {

        PhotonNetwork.ConnectUsingSettings(GameVersion);


    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null);  

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(null);
    }

    public override void OnJoinedLobby()
    {
        SceneManager.LoadScene("Lobby");
    }
}
