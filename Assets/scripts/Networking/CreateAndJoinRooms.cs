using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;

public class CreateAndJoinRooms : Photon.PunBehaviour
{
    public InputField CreateInput;
    public InputField JoinInput;
    //string GameVersion = "1.0";
    void Awake()
    {

       // PhotonNetwork.ConnectUsingSettings(GameVersion);


    }
    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers= 2;
        
        PhotonNetwork.CreateRoom(CreateInput.text ,roomOptions, null);

    }
    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(JoinInput.text);
    }
    public override void OnJoinedRoom()
    {


        PhotonNetwork.LoadLevel("PlayMultiplayerGame");
    }
}
