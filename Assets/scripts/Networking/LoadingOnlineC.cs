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
    RoomOptions roomOP;
    void Awake()
    {

        PhotonNetwork.ConnectUsingSettings(GameVersion);

        roomOP = new RoomOptions();
        roomOP.MaxPlayers = 2;

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
    private void TryCreateRoom()
    {
        Debug.Log("Try Create Room");
        PhotonNetwork.CreateRoom("Room" + TryRandomRoom.ToString());
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Created Room" + TryRandomRoom.ToString());
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
                TryCreateRoom();
            }    
                
        }
        else if(TryRandomRoom < 10 && TryToJoin == false)
        {
            
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
    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        
        Debug.Log("Created Room" + TryRandomRoom.ToString());
        TryRandomRoom++;
        PhotonNetwork.CreateRoom("Room" + TryRandomRoom.ToString(), roomOP, null);
        
    }
    */


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
