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
    private AiGameManager AiGameManagerSC;

    [SerializeField] private TMP_InputField NickNameText;
    

    [Header("Managers")]
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private GameObject MultiGameManagerUpdate;
    [SerializeField] private GameObject TwoPlayerGameManager;
    [SerializeField] private GameObject AiGameMnager;

    [Header("Buttons")]
    [SerializeField] private GameObject CreateNickNameSeeneOBJ;
    [SerializeField] private GameObject GameMode;
    [SerializeField] private GameObject SettingsScenee;
    [SerializeField] private GameObject GameType;
    [SerializeField] private GameObject PlayFriends;
    [SerializeField] private GameObject FindRandomGame;
    [SerializeField] private GameObject WinerScreen;
    [SerializeField] private GameObject SingleWinScreen;
    [SerializeField] private GameObject LoserScreen;
    [SerializeField] private GameObject OnlineDrawScreen;
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
    [SerializeField] private TextMeshProUGUI PlayAgainTextDraw;
    [SerializeField] private TextMeshProUGUI SingleWinText;
    [SerializeField] private TextMeshProUGUI MenuNickName;
    [SerializeField] private TextMeshProUGUI NickNameErrorText;

    public TextMeshProUGUI InfoText;

    [Header("DropDown")]
    [SerializeField] private TMP_Dropdown FpsLimitDD;







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
        OnlineDrawScreen.SetActive(false);
        SettingsScenee.SetActive(false);
        CreateNickNameSeeneOBJ.SetActive(false);
        PlayAgainTextLose.text = "" ;
        PlayAgainTextWin.text =  "" ;
        PlayAgainTextDraw.text = "" ;
    }
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();
        MenuNickName.text = "";
        NickNameErrorText.text = "";
        MultiGameManagerUpdateSC = MultiGameManagerUpdate.GetComponent<MultiGameManagerUpdate>();
        TwoPlayerGameManagerSC = TwoPlayerGameManager.GetComponent<GameManager>();
        AiGameManagerSC = AiGameMnager.GetComponent<AiGameManager>();
        if (PlayerPrefs.HasKey("QualityLevel"))
        {
            FpsLimitDD.SetValueWithoutNotify(PlayerPrefs.GetInt("QualityLevel")-1);
            GraphicLevelChange(PlayerPrefs.GetInt("QualityLevel"));
            Debug.Log("Quality Level = " + PlayerPrefs.GetInt("QualityLevel"));
        }
        else
        {
            Debug.Log("It Is First Game");
            PlayerPrefs.SetInt("QualityLevel", 2);
        }
        if(PlayerPrefs.HasKey("NickName"))
        {
            MenuNickName.text = PlayerPrefs.GetString("NickName");
            MainMenuButton();
        }
        else
        {
            CreateNickNameSceene();
        }
        
        
    }
    public void CreateNickNameSceene()
    {
        DisableAllScreen();
        CreateNickNameSeeneOBJ.SetActive(true);

    }
    public void NickNameOkeyButton()
    {

        if(NickNameText.text != null && NickNameText.text != ""  && NickNameText.text != "Enter Nick Name..." && NickNameText.text.Length < 15 && NickNameText.text != " ")
        {
            char[] NickNameChar  = NickNameText.text.ToCharArray();
            char[] EmptyChar = "                 ".ToCharArray();
            bool CanNickName = false;
            for (int i = 0; i < NickNameText.text.Length; i ++)
            {
                if (NickNameChar[i] == EmptyChar[i])
                {
                    //Debug.LogError(" Using Space");
                    NickNameErrorText.text = "Naver Use Space";
                    CanNickName = false;
                    break;
                }
                else
                {
                    //Debug.LogError("Naver Use Space");
                    CanNickName = true;
                    continue;
                }
            }
            if(CanNickName)
            {
                PlayerPrefs.SetString("NickName", NickNameText.text);
                //Debug.Log("Created Nick Name = " + PlayerPrefs.GetString("NickName"));
                MenuNickName.text = PlayerPrefs.GetString("NickName");
                NickNameErrorText.text = "";
                MainMenuButton();
            }
                
            
            
            
        }
        else if(NickNameText.text.Length >= 15)
        {
            NickNameErrorText.text = "Enter less than 15 characters"; 
            //Debug.LogError("Invalid Character");
        }
        else
            NickNameErrorText.text = "Enter a valid Nick Name";

    }

    public void SellectedPlayerVSAi()
    {
        AiGameMnager.SetActive(true);
        DisableAllScreen();
        SingleGameScene.SetActive(true);
        AiGameManagerSC.Player1Turn = true;
        AiGameManagerSC.CanPlay = true;
        AiGameManagerSC.SingleTurnText.text = "Your Turn";
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
        TwoPlayerGameManagerSC.OnWtoPlayerModeSellected();

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
        TwoPlayerGameManagerSC.CanPlay = true;
        SelectAiLevelSceene.SetActive(true);
        AiGameManagerSC.PlayAiGameOnSellected();
        
    }

    public void OnOnlineModeSelected()
    {
        networkManager.DisConnectFun();
        DisableAllScreen();
        GameType.SetActive(true);
        //Debug.Log("OnlineMode Sellected");
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
        MultiGameManagerUpdateSC.EndGame();
        networkManager.GetComponent<PhotonView>().RPC("OnLoseRPC" , PhotonNetwork.player.GetNext());
        MultiGameManagerUpdate.GetComponent<PhotonView>().RPC("EndGame" , PhotonNetwork.player.GetNext());
        
        
    }

    public void WinForSingle(int Player)
    {
        if(Player == 1 || Player == 2)
        {
            DisableAllScreen();
            SingleWinText.text = "Player " + Player.ToString() + " Win";
            SingleWinScreen.SetActive(true);
        }
        if (Player == 3)
        {
            DisableAllScreen();
            SingleWinText.text = "Ai Win";
            SingleWinScreen.SetActive(true);
        }
        if (Player == 4)
        {
            DisableAllScreen();
            SingleWinText.text = "Draw";
            SingleWinScreen.SetActive(true);
        }

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
        MultiGameManagerUpdate.SetActive(false);
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
        if(MultiGameManagerUpdate.activeSelf)
        {
            MultiGameManagerUpdateSC.ClearBoard();
            MultiGameManagerUpdate.SetActive(false);
        }

    }

    public void MainMenuForSingle()
    {
        if(TwoPlayerGameManager.activeSelf)
        {
            TwoPlayerGameManagerSC.Player1Ghost.SetActive(false);
            TwoPlayerGameManagerSC.Player2Ghost.SetActive(false);
            TwoPlayerGameManagerSC.CanPlay = false;
            DisableAllScreen();
            GameMode.SetActive(true);
            TwoPlayerGameManager.SetActive(false);
            // Debug.LogError("MainMenu For Single ");
            TwoPlayerGameManagerSC.ClearSingleBoard();
            TwoPlayerGameManagerSC.ClearSinglePecies();
            AiGameMnager.SetActive(false);
        }
        else if(AiGameMnager.activeSelf)
        {
            AiGameManagerSC.Player1Ghost.SetActive(false);
            AiGameManagerSC.Player2Ghost.SetActive(false);
            AiGameManagerSC.CanPlay = false;
            DisableAllScreen();
            GameMode.SetActive(true);
            TwoPlayerGameManager.SetActive(false);
            // Debug.LogError("MainMenu For Single ");
            AiGameManagerSC.ClearSingleBoard();
            AiGameManagerSC.ClearSinglePecies();
            AiGameMnager.SetActive(false);
        }
        
        
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
        PlayAgainTextDraw.text = "Enemy Wanna  Play Again";
    }
    public void EnemyDisConnectedUi()
    {
        DisableAllScreen();
        EnemyDisConnectedSceene.SetActive(true);
        
    }

    public void PlayAgainSingle()
    {
        if(TwoPlayerGameManager.activeSelf)
        {
            DisableAllScreen();
            SingleGameScene.SetActive(true);


            TwoPlayerGameManagerSC.CanPlay = true;
            TwoPlayerGameManagerSC.SingleTurnText.text = "Player 1 Turn";

            TwoPlayerGameManagerSC.ClearSingleBoard();
            TwoPlayerGameManagerSC.ClearSinglePecies();
        }
        else if(AiGameMnager.activeSelf)
        {
            DisableAllScreen();
            SingleGameScene.SetActive(true);


            AiGameManagerSC.CanPlay = true;
            AiGameManagerSC.Player1Turn = true;
            AiGameManagerSC.SingleTurnText.text = "Player 1 Turn";

            AiGameManagerSC.ClearSingleBoard();
            AiGameManagerSC.ClearSinglePecies();
        }
        


    }
    public void OnlineDraw()
    {
        //Debug.Log("DwarScenee Loading");
        DisableAllScreen();
        OnlineDrawScreen.SetActive(true);
    }
    public void OnSettingsMenuSellected()
    {
        DisableAllScreen();
        SettingsScenee.SetActive(true);
        FpsLimitDD.SetValueWithoutNotify(PlayerPrefs.GetInt("QualityLevel") - 1);
        FpsLimitDD.RefreshShownValue();
    }
    public void GraphicLevelChange(int QualityLevel)
    {
        QualityLevel = FpsLimitDD.value;
        if (QualityLevel == 0)
        {
            Application.targetFrameRate = 30;
            QualitySettings.SetQualityLevel(1);
            PlayerPrefs.SetInt("QualityLevel", 1);
        }
        else if (QualityLevel == 1)
        {
            Application.targetFrameRate = 45;
            QualitySettings.SetQualityLevel(2);
            PlayerPrefs.SetInt("QualityLevel", 2);
            
        }
        else if (QualityLevel == 2)
        {
            Application.targetFrameRate = 60;
            QualitySettings.SetQualityLevel(2);
            PlayerPrefs.SetInt("QualityLevel", 3);
        }
        Debug.Log("New Quality Level = " + PlayerPrefs.GetInt("QualityLevel"));
    }
}
