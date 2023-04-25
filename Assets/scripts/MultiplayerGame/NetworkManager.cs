using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;

public class NetworkManager : Photon.PunBehaviour
{
    string GameVersion = "1.0";
    private const string MODE = "Mode";
    private const int MAX_PLAYER = 2;
    private LevelMode PlayerLevel;

    private void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }
    public void connect()
    {
        PlayerLevel = ((LevelMode)1);
        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master. Loading to Select Game Mode");

    }
    public void JoinRandomRoom()
    {
        
        Debug.Log("Looking For Random Room With Game Mode " + PlayerLevel); 
        PhotonNetwork.JoinRandomRoom( new ExitGames.Client.Photon.Hashtable() { { MODE , PlayerLevel } } , MAX_PLAYER);
        // new ExitGames.Client.Photon.Hashtable() { { MODE, PlayerLevel } }
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Join Random Room Fieled .Creating New One");
        PhotonNetwork.CreateRoom(null , new RoomOptions 
        {
            
            CustomRoomPropertiesForLobby = new string[] {MODE} ,
            MaxPlayers = MAX_PLAYER,
            CustomRoomProperties =  new ExitGames.Client.Photon.Hashtable() { { MODE , PlayerLevel } } ,

        });
        
    }
    

    public override void OnCreatedRoom()
    {
        // Creating Room Properties
        PhotonNetwork.room.MaxPlayers= MAX_PLAYER;
        PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { MODE, PlayerLevel } });

    }
    public override void OnJoinedRoom()
    {
            Debug.Log($"Player {PhotonNetwork.player.ID} Joined the room With {(LevelMode)PhotonNetwork.room.CustomProperties[MODE]} MaxPlyaer = {PhotonNetwork.room.MaxPlayers}");
    }

    public void SetPlayerLevel(LevelMode levelMode)
    {
        PlayerLevel = levelMode;
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { MODE, levelMode } } );
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("New Player Connected The Toom");
    }
}
