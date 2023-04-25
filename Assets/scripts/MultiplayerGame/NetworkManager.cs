using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;

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
        Debug.Log($"Connected to master. Loading to Select Game Mode");

    }
    public void JoinRandomRoom()
    {
        
        Debug.Log("Looking For Random Room With Game Mode " + PlayerLevel);

        Hashtable RANDOM_HASH = new Hashtable { { MODE , 1 } };
        
        PhotonNetwork.JoinRandomRoom(RANDOM_HASH, MAX_PLAYER );
        //PhotonNetwork.JoinRoom( null );
        // new ExitGames.Client.Photon.Hashtable() { { MODE, PlayerLevel } }
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Join Random Room Fieled .Creating New One");
        RoomOptions RANDOM_ROOM_OPTIONS = new RoomOptions();
        RANDOM_ROOM_OPTIONS.CustomRoomPropertiesForLobby = new string[] { MODE };
        RANDOM_ROOM_OPTIONS.MaxPlayers= MAX_PLAYER;
        RANDOM_ROOM_OPTIONS.CustomRoomProperties = new Hashtable { { MODE , 1 } };
        
        PhotonNetwork.CreateRoom(null, RANDOM_ROOM_OPTIONS );
        
    }
    

    public override void OnCreatedRoom()
    {
        // Creating Room Properties
        PhotonNetwork.room.SetPropertiesListedInLobby(new string[] {MODE});
        PhotonNetwork.room.MaxPlayers= MAX_PLAYER;
        PhotonNetwork.room.SetCustomProperties(new Hashtable() { { MODE, 1 } });

    }
    public override void OnJoinedRoom()
    {
            Debug.Log($"Player {PhotonNetwork.player.ID} Joined the room With  {(LevelMode)PhotonNetwork.room.CustomProperties[MODE]} MaxPlyaer = {PhotonNetwork.room.MaxPlayers}");
    }

    public void SetPlayerLevel(LevelMode levelMode)
    {
        PlayerLevel = levelMode;
        PhotonNetwork.player.SetCustomProperties(new Hashtable { { MODE, 1 } } );
    }
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        Debug.Log("New Player Connected The Toom");
    }
}
