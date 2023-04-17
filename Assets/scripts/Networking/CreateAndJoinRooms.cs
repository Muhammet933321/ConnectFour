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
        PhotonNetwork.CreateRoom(CreateInput.text);

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
