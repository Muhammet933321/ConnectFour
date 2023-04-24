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
        PhotonNetwork.JoinRandomRoom(new ExitGames.Client.Photon.Hashtable() { { MODE, PlayerLevel } } , MAX_PLAYER );
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Join Random Room Fieled .Creating New One");
        PhotonNetwork.CreateRoom(null , new RoomOptions
        {
            CustomRoomPropertiesForLobby = new string[] { MODE } ,
            MaxPlayers = MAX_PLAYER ,
            CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { MODE, 1 } } ,


        });
        PhotonNetwork.room.CustomProperties.Add(MODE , 1);
        // PhotonNetwork.room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { MODE, 1 } });
        
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.room.CustomProperties[MODE] == null)
        {
            Debug.Log($" Room Properties is null . Player Level =  { PlayerLevel }");
            Debug.Log(PhotonNetwork.room.CustomProperties);
            Debug.Log(PhotonNetwork.room.CustomProperties[MODE]);
        }
        else
            Debug.Log($"Player {PhotonNetwork.player.ID} Joined the room With {PhotonNetwork.room.CustomProperties[MODE]}");
    }

    public void SetPlayerLevel(LevelMode levelMode)
    {
        PlayerLevel = levelMode;
        PhotonNetwork.player.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { MODE, levelMode } } );
    }
}
