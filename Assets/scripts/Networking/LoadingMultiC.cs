using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LoadingMultiC : Photon.PunBehaviour
{
    string GameVersion = "1.0";
    void Awake()
    {

        PhotonNetwork.ConnectUsingSettings(GameVersion);


    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null , new RoomOptions());  

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
