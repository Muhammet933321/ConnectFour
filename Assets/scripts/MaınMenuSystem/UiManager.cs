using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    [Header("MainMenu")]
    [SerializeField] private GameObject GameModeSelectionScreen;
    [SerializeField] private GameObject GameSellectSreen;
    [SerializeField] private GameObject CreateJoin;
    [SerializeField] private GameObject FindRandomGame;
    [SerializeField] private GameObject ConnectToGameScane;
    [SerializeField] private GameObject GameOverScreen;
    
    

    private void DisableAllScreen()
    {
        GameModeSelectionScreen.SetActive(false);
        GameSellectSreen.SetActive(false);
        ConnectToGameScane.SetActive(false);
        GameOverScreen.SetActive(false);
        CreateJoin.SetActive(false);
        FindRandomGame.SetActive(false);
    }
    private void Awake()
    {
        DisableAllScreen();
        GameModeSelectionScreen.SetActive(true);
    }
    public void OnSinglePlayerModeSelected()
    {
        DisableAllScreen();
    }

    public void OnOnlineModeSelected()
    {
        DisableAllScreen();
        GameSellectSreen.SetActive(true);
    }

    public void OnRandomGameSelected()
    {
        DisableAllScreen();
        FindRandomGame.SetActive(true);
        networkManager.SetPlayerLevel((LevelMode)1);
        networkManager.connectRandom();
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
        DisableAllScreen();
        CreateJoin.SetActive(true);
        networkManager.SetPlayerLevel((LevelMode)0);
        networkManager.connectFriends();
    }

}
