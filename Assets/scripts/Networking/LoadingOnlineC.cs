using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LoadingOnlineC : Photon.PunBehaviour
{
    string GameVersion = "1.0";
    void Awake()
    {

        PhotonNetwork.ConnectUsingSettings(GameVersion);


    }

    
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(null);
    }

    public override void OnJoinedLobby()
    {

        PhotonNetwork.JoinRoom("a");
        
        
    }
    
    public override void OnJoinedRoom()
    {
        Debug.Log("Finded The Tandom game");
        SceneManager.LoadScene("PlayMultiplayerGame");
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        base.OnPhotonRandomJoinFailed(codeAndMsg);

        string roomName = "Room";

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        Debug.Log("Created Room");
        SceneManager.LoadScene("PlayMultiplayerGame");
    }
}
