using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject MultiGameManagerUpdate;

    [Header("Buttons")]
    [SerializeField] private GameObject GameMode;
    [SerializeField] private GameObject GameType;
    [SerializeField] private GameObject PlayFriends;
    [SerializeField] private GameObject FindRandomGame;
    [SerializeField] private GameObject WinerScreen;
    [SerializeField] private GameObject LoserScreen;
    [SerializeField] private GameObject GameScreen;
    [SerializeField] private GameObject EnemyDisConnectedSceene;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI PlayAgainTextWin;
    [SerializeField] private TextMeshProUGUI PlayAgainTextLose;
    public TextMeshProUGUI InfoText;
    




    public void DisableAllScreen()
    {

        GameMode.SetActive(false);
        GameType.SetActive(false);
        PlayFriends.SetActive(false);
        FindRandomGame.SetActive(false);
        WinerScreen.SetActive(false);
        LoserScreen.SetActive(false);
        GameScreen.SetActive(false);
        EnemyDisConnectedSceene.SetActive(false);
        PlayAgainTextLose.text = "" +
            "";
        PlayAgainTextWin.text = "" +
            "";
    }
    private void Awake()
    {
        MainMenuButton();
    }
    public void OnSinglePlayerModeSelected()
    {
        DisableAllScreen();
    }

    public void OnOnlineModeSelected()
    {
        if(networkManager.IsConnectedFun())
        {
            PhotonNetwork.Disconnect();
        }
        DisableAllScreen();
        GameType.SetActive(true);
    }

    public void OnRandomGameSelected()
    {
        
        networkManager.SetPlayerLevel((LevelMode)1);
        networkManager.connectRandom();
    }
    public void RandomGameMenuLoad()
    {
        DisableAllScreen();
        FindRandomGame.SetActive(true);
    }
    public void MultiGameMenuLoad()
    {
        DisableAllScreen();
        PlayFriends.SetActive(true);
    }
    public void OnFindRandomGameSelected()
    {
        DisableAllScreen();

    }
    /*
    public void OnConnectRandom()
    {
        networkManager.SetPlayerLevel((LevelMode)1);
        networkManager.connectRandom();  
    }
    */
    public void OnConnectFriends()
    {
        
        networkManager.SetPlayerLevel((LevelMode)0);
        networkManager.connectFriends();
    }

    public void OnWin()
    {
        DisableAllScreen();
        WinerScreen.SetActive(true);
        networkManager.GetComponent<PhotonView>().RPC("OnLoseRPC" , PhotonNetwork.player.GetNext());
        MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("EndGame" , PhotonNetwork.player.GetNext());
        MultiGameManagerUpdate.GetComponent<MultiGameManagerUpdate>().EndGame();
        
    }
    public void OnLose()
    {
        DisableAllScreen();
        LoserScreen.SetActive(true);
    }
    public void PlayAgainButton()
    {
        // Debug.LogError("Cliced Play Again Button");
        MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("EnemyWannaPlayAgain", PhotonNetwork.player.GetNext());
        MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("ClearBoard", PhotonNetwork.player.GetNext());
        MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("ClearBoard", PhotonNetwork.player);
        MultiGameManagerUpdate.GetComponent<MultiGameManagerUpdate>().IWanaPlayAgain = true;
        MultiGameManagerUpdate.GetComponent<MultiGameManagerUpdate>().ReStartRoom();
        MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("ReStartRoom", PhotonNetwork.player.GetNext());

        

        


        
    }
    public void NewGameButton()
    {

    }

    public void MainMenuButton()
    {
        networkManager.LeaveTheRoomFun();

        networkManager.DisConnectFun();

        DisableAllScreen();
        MultiGameManagerUpdate.GetComponent<MultiGameManagerUpdate>().Player1Ghost.SetActive(false);
        MultiGameManagerUpdate.GetComponent<MultiGameManagerUpdate>().Player2Ghost.SetActive(false);
        GameMode.SetActive(true);
    }

    public void GameScreenActive(string Text)
    {
        DisableAllScreen();
        GameScreen.SetActive(true);
        InfoText.text = Text;
    }    
    public void EnemyWannaPlayAgainUi()
    {
        PlayAgainTextLose.text = "Enemy Wanna  Play Again";
        PlayAgainTextWin.text = "Enemy Wanna  Play Again";
    }
    public void EnemyDisConnectedUi()
    {
        DisableAllScreen();
        EnemyDisConnectedSceene.SetActive(true);
        
    }
}
