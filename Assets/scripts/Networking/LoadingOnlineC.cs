using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LoadingOnlineC : Photon.PunBehaviour
{
    string GameVersion = "1.0";
    int TryRandomRoom = 0;
    bool TryToJoin = true;
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

        TryJoinRoom();


    }
    
    private void TryJoinRoom()
    {
        Debug.Log("Try Join Room");
        PhotonNetwork.JoinRoom("Room" + TryRandomRoom.ToString());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Finded The Random Game");
        SceneManager.LoadScene("PlayMultiplayerGame");
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        if(TryToJoin)
        {
            if (TryRandomRoom < 10)
            {
                TryRandomRoom++;
                TryJoinRoom();
            }
            else
            {
                Debug.Log("Didn't Find Waiting Game");
                TryToJoin = false;
                TryRandomRoom = 0;
                TryJoinRoom();
            }    
                
        }
        else if(TryRandomRoom < 10 && !TryToJoin)
        {
            
            RoomOptions roomOP = new RoomOptions();
            roomOP.MaxPlayers = 2;
            Debug.Log("Created Room" + TryRandomRoom.ToString());
            PhotonNetwork.JoinOrCreateRoom("Room" + TryRandomRoom.ToString(), roomOP, null);
        }
        else
        {
            Debug.Log("Couldn't Find A Emnpty Room All Rooms Is Full");
            SceneManager.LoadScene("MainMenu");
        }
            

        

    }


    /*
    base.OnPhotonRandomJoinFailed(codeAndMsg);

    string roomName = "Room";

    RoomOptions roomOptions = new RoomOptions();
    roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(roomName, roomOptions, null);
        Debug.Log("Created Room");
        SceneManager.LoadScene("PlayMultiplayerGame");
        */



}
