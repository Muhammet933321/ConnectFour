using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using Unity.VisualScripting;
using System;

public class NetworkManager : Photon.PunBehaviour
{
    string GameVersion = "1.0";
    private const string MODE = "Mode";
    private const int MAX_PLAYER = 2;
    private LevelMode PlayerLevel;
    [Header("InputFields")]
    [SerializeField] private TMP_InputField RoomName;
    [SerializeField] private GameObject GameManagerOBJ;
    [SerializeField] private GameObject BoardInput;
    [SerializeField] private GameObject BoardInput0;
    [SerializeField] private GameObject BoardInput1;
    [SerializeField] private GameObject BoardInput2;
    [SerializeField] private GameObject BoardInput3;
    [SerializeField] private GameObject BoardInput4;
    [SerializeField] private GameObject BoardInput5;
    [SerializeField] private GameObject BoardInput6;
    [SerializeField] private GameObject UiManagerOBJ;
    private PhotonView photonViewOBJ ;
    private MultiGameManagerUpdate GameManager;
    private UiManager UiManagerSC;


    private void Awake()
    {
        photonViewOBJ = GetComponent<PhotonView>();
        GameManager = GameManagerOBJ.GetComponent<MultiGameManagerUpdate>();
        UiManagerSC = UiManagerOBJ.GetComponent<UiManager>();
        GameManagerOBJ.SetActive(false);
        BoardInput.SetActive(false);
        BoardInput0.GetComponent<MultiInputFileds>().GameMode = 0;
        BoardInput1.GetComponent<MultiInputFileds>().GameMode = 0;
        BoardInput2.GetComponent<MultiInputFileds>().GameMode = 0;
        BoardInput3.GetComponent<MultiInputFileds>().GameMode = 0;
        BoardInput4.GetComponent<MultiInputFileds>().GameMode = 0;
        BoardInput5.GetComponent<MultiInputFileds>().GameMode = 0;
        BoardInput6.GetComponent<MultiInputFileds>().GameMode = 0;
        
    }
    public void connectRandom()
    {
        PhotonNetwork.player.NickName =  PlayerPrefs.GetString("NickName");
        PlayerLevel = ((LevelMode)1);
        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }
    public void connectFriends()
    {
        PhotonNetwork.player.NickName = PlayerPrefs.GetString("NickName");
        PlayerLevel = ((LevelMode)0);
        PhotonNetwork.ConnectUsingSettings(GameVersion);
    }


    public override void OnConnectedToMaster()
    {
        // Debug.LogError($"Connected to master. Loading to Select Game Mode");
        Debug.Log("Connected To Master His Name Is = " + PhotonNetwork.player.NickName);
        if(((int)PlayerLevel) == 1)
        {
            UiManagerOBJ.GetComponent<UiManager>().RandomGameMenuLoad();
        }
        else
        {
            UiManagerOBJ.GetComponent<UiManager>().MultiGameMenuLoad();
        }
        

    }
    public void JoinRandomRoom()
    {
        
      //  Debug.LogError("Looking For Random Room With Game Mode " + PlayerLevel);

        Hashtable RANDOM_HASH = new Hashtable { { MODE , PlayerLevel } };
        
        PhotonNetwork.JoinRandomRoom(RANDOM_HASH, MAX_PLAYER );
        //PhotonNetwork.JoinRoom( null );
        // new ExitGames.Client.Photon.Hashtable() { { MODE, PlayerLevel } }
    }
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        //Debug.LogError("Join Random Room Fieled .Creating New One");
        RoomOptions RANDOM_ROOM_OPTIONS = new RoomOptions();
        RANDOM_ROOM_OPTIONS.CustomRoomPropertiesForLobby = new string[] { MODE };
        RANDOM_ROOM_OPTIONS.MaxPlayers= MAX_PLAYER;
        RANDOM_ROOM_OPTIONS.CustomRoomProperties = new Hashtable { { MODE , PlayerLevel } };
        
        PhotonNetwork.CreateRoom(null, RANDOM_ROOM_OPTIONS );

        PhotonNetwork.CreateRoom(RoomName.text, null);
        UiManagerSC.DisableAllScreen();
        GameManager.AmIPlayer1 = true;
        GameManager.IsMyTurn = true;
        GameManager.CanPlay = false;

    }
    

    public override void OnCreatedRoom()
    {
        // Creating Room Properties
        PhotonNetwork.room.SetPropertiesListedInLobby(new string[] {MODE});
        PhotonNetwork.room.MaxPlayers= MAX_PLAYER;
        PhotonNetwork.room.SetCustomProperties(new Hashtable() { { MODE, PlayerLevel } });

    }
    public override void OnJoinedRoom()
    {
           // Debug.LogError($"Player {PhotonNetwork.player.ID} Joined the room With  " +
             //   $"{(LevelMode)PhotonNetwork.room.CustomProperties[MODE]} " +
           //     $"MaxPlyaer = {PhotonNetwork.room.MaxPlayers}");
        UiManagerSC.GameScreenActive("Waiting For Connect A Player");
        
    }

    public void SetPlayerLevel(LevelMode levelMode)
    {
        PlayerLevel = levelMode;
        PhotonNetwork.player.SetCustomProperties(new Hashtable { { MODE, levelMode } } );
    }
    // On Connected Another Player To Room
    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        
      //  Debug.LogError($"Player {newPlayer.ID} Connected The Toom");
        RoomIsReady(newPlayer);
    }

    public void OnJoinButton()
    {
        
        PhotonNetwork.JoinRoom(RoomName.text);

    }
    public void OnCreateButton()
    {
        PhotonNetwork.CreateRoom(RoomName.text,null);
        UiManagerSC.DisableAllScreen();
        GameManager.AmIPlayer1 = true;
        GameManager.IsMyTurn = true;
        GameManager.CanPlay = false;
    }
    public void RoomIsReady(PhotonPlayer newPlayer)
    {
        GameManagerOBJ.SetActive(true);
        BoardInput.SetActive(true);
        GameManager.CanPlay = true;
        photonViewOBJ.RPC("RPC_GameStart", newPlayer , null);
      //  Debug.LogError("Game Is Starting");
        UiManagerSC.GameScreenActive("Your Turn");


    }

    [PunRPC]
    public void RPC_GameStart()
    {
        GameManagerOBJ.SetActive(true);
        BoardInput.SetActive(true);
        GameManager.CanPlay = true;
        GameManager.AmIPlayer1 = false;
        GameManager.IsMyTurn = false;
        GameManager.CanPlay = true;
        UiManagerSC.GameScreenActive("Enemy's Turn");
       // Debug.LogError("RPC GAME IS STARTING");
    }

    [PunRPC]
    public void OnLoseRPC()
    {
       // Debug.LogError("I am Loser");
        UiManagerSC.OnLose();

    }
    public void DisConnectFun()
    {
        if(PhotonNetwork.connected)
        {
            //Debug.Log("Player Is Online .DisConnecting Now");
            PhotonNetwork.Disconnect();
            if (PhotonNetwork.inRoom)
            {
                PhotonNetwork.LeaveRoom();
                //Debug.Log("Player Is In A Room .Leaving Now");
            }
            else
            {
                //Debug.Log("Player Is Not In A Room");
            }
            
        }
        else
        {
            //Debug.Log("Player Already Offline");
        }
    }
    public bool IsConnectedFun()
    {
        if (PhotonNetwork.connected)
        {
         //   Debug.LogError("Player Is Online");
            return true;
        }
        else
        {
          //  Debug.LogError("Player Is Offline");
            return false;
        }

    }
    public void LeaveTheRoomFun()
    {
        if (PhotonNetwork.inRoom)
        {
           // Debug.LogError("Player Is In Room Leaving Now");
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            //Debug.LogError("Player Already Not In Room");
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        if(GameManagerOBJ.GetActive())
        {
            GameManager.ClearBoard();
        }
        
    }
    public override void OnConnectionFail(DisconnectCause cause)
    {
       // Debug.LogError("Internet Error");
    }
    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
       // Debug.LogError("Enemy DisConnected To Game");
        GameManager.CanPlay = false;
        GameManager.ClearBoard();
        PhotonNetwork.LeaveRoom();
        UiManagerSC.EnemyDisConnectedUi();
    }


}
