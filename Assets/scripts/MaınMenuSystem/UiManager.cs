using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{

    private MultiGameManagerUpdate MultiGameManagerUpdateSC;
    private GameManager TwoPlayerGameManagerSC;


    [Header("Managers")]
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject MultiGameManagerUpdate;
    [SerializeField] private GameObject TwoPlayerGameManager;
    [SerializeField] private GameObject AiGameMnager;

    [Header("Buttons")]
    [SerializeField] private GameObject GameMode;
    [SerializeField] private GameObject GameType;
    [SerializeField] private GameObject PlayFriends;
    [SerializeField] private GameObject FindRandomGame;
    [SerializeField] private GameObject WinerScreen;
    [SerializeField] private GameObject SingleWinScreen;
    [SerializeField] private GameObject LoserScreen;
    [SerializeField] private GameObject GameScreen;
    [SerializeField] private GameObject EnemyDisConnectedSceene;
    [SerializeField] private GameObject SingleModeSceene;
    [SerializeField] private GameObject SelectAiLevelSceene;
    [SerializeField] private GameObject SingleGameScene;
    [SerializeField] private GameObject OnlineGameScene;
    [SerializeField] private GameObject GamePousedSingle;
    [SerializeField] private GameObject GamePousedOnline;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI PlayAgainTextWin;
    [SerializeField] private TextMeshProUGUI PlayAgainTextLose;
    [SerializeField] private TextMeshProUGUI SingleWinText;
    
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
        SingleModeSceene.SetActive(false);
        SelectAiLevelSceene.SetActive(false);
        SingleWinScreen.SetActive(false);
        SingleGameScene.SetActive(false);
        GamePousedSingle.SetActive(false);
        GamePousedOnline.SetActive(false);
        OnlineGameScene.SetActive(false);
        PlayAgainTextLose.text = "" ;
        PlayAgainTextWin.text =  "" ;
    }
    private void Awake()
    {
        Application.targetFrameRate=  120;
        MultiGameManagerUpdateSC = MultiGameManagerUpdate.GetComponent<MultiGameManagerUpdate>();
        TwoPlayerGameManagerSC = TwoPlayerGameManager.GetComponent<GameManager>();
        MainMenuButton();
    }
    private void Start()
    {
        
    }
    public void OnSinglePlayerModeSelected()
    {
        DisableAllScreen();
        SingleModeSceene.SetActive(true);
    }
    public void OnTwoPlayerModeSelected()
    {
        DisableAllScreen();
        SingleGameScene.SetActive(true);
        TwoPlayerGameManager.SetActive(true);
        TwoPlayerGameManagerSC.CanPlay = true;


    }
    public void PouseGameSingle()
    {
            DisableAllScreen();
            GamePousedSingle.SetActive(true);
            TwoPlayerGameManagerSC.CanPlay = false;
    }
    public void PouseGameOnline()
    {
        DisableAllScreen();
        GamePousedOnline.SetActive(true);
        MultiGameManagerUpdateSC.CanPlay = false ;


    }
    public void ContinueSingleGame()
    {
        DisableAllScreen();
        SingleGameScene.SetActive(true);
        TwoPlayerGameManagerSC.CanPlay = true;
    }
    public void ContinueOnlineGame()
    {
        DisableAllScreen();
        OnlineGameScene.SetActive(true);
        MultiGameManagerUpdateSC.CanPlay = true;
    }

    public void OnAiModeSelected()
    {
        DisableAllScreen();
        SelectAiLevelSceene.SetActive(true);
    }

    public void OnOnlineModeSelected()
    {
        networkManager.DisConnectFun();
        DisableAllScreen();
        GameType.SetActive(true);
        Debug.Log("OnlineMode Sellected");
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
        MultiGameManagerUpdateSC.EndGame();
        
    }

    public void WinForSingle(int Player)
    {
        DisableAllScreen();
        SingleWinText.text = "Player " + Player.ToString() +" Win";
        SingleWinScreen.SetActive(true);
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
        MultiGameManagerUpdateSC.IWanaPlayAgain = true;
        MultiGameManagerUpdateSC.ReStartRoom();
        MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("ReStartRoom", PhotonNetwork.player.GetNext());

        

        


        
    }
    public void NewGameButton()
    {

    }
    public void MainMenuFromOnlineGame()
    {

        //MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("ClearBoard", PhotonNetwork.player);
        if(MultiGameManagerUpdate.GetActive())
        {
            MultiGameManagerUpdateSC.ClearBoard();
        }
        

        networkManager.LeaveTheRoomFun();

        networkManager.DisConnectFun();

        DisableAllScreen();
        MultiGameManagerUpdateSC.Player1Ghost.SetActive(false);
        MultiGameManagerUpdateSC.Player2Ghost.SetActive(false);
        GameMode.SetActive(true);
        //Debug.LogError("MainMenuFromOnlineGame");
    }
    public void MainMenuButton()
    {
        
        //MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("ClearBoard", PhotonNetwork.player);
        
        networkManager.LeaveTheRoomFun();

        networkManager.DisConnectFun();

        DisableAllScreen();
        MultiGameManagerUpdateSC.Player1Ghost.SetActive(false);
        MultiGameManagerUpdateSC.Player2Ghost.SetActive(false);
        GameMode.SetActive(true);
    }

    public void MainMenuForSingle()
    {
        MultiGameManagerUpdateSC.Player1Ghost.SetActive(false);
        MultiGameManagerUpdateSC.Player2Ghost.SetActive(false);
        TwoPlayerGameManagerSC.CanPlay =false;
        DisableAllScreen();
        GameMode.SetActive(true);
        TwoPlayerGameManager.SetActive(false);
        // Debug.LogError("MainMenu For Single ");
        TwoPlayerGameManagerSC.ClearSingleBoard();
        TwoPlayerGameManagerSC.ClearSinglePecies();
    }

    public void GameScreenActive(string Text)
    {
        DisableAllScreen();
        OnlineGameScene.SetActive(true);
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

    public void PlayAgainSingle()
    {
        DisableAllScreen();
        SingleGameScene.SetActive(true) ;


        TwoPlayerGameManagerSC.CanPlay = true;
        TwoPlayerGameManagerSC.SingleTurnText.text = "Player 1 Turn";

        TwoPlayerGameManagerSC.ClearSingleBoard();
        TwoPlayerGameManagerSC.ClearSinglePecies();


    }
}
