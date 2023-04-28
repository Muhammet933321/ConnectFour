using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [Header("NetworkManager")]
    [SerializeField] private NetworkManager networkManager;

    [Header("Buttons")]
    [SerializeField] private GameObject GameMode;
    [SerializeField] private GameObject GameType;
    [SerializeField] private GameObject PlayFriends;
    [SerializeField] private GameObject FindRandomGame;
    [SerializeField] private GameObject WinerScreen;
    [SerializeField] private GameObject LoserScreen;



    public void DisableAllScreen()
    {

        GameMode.SetActive(false);
        GameType.SetActive(false);
        PlayFriends.SetActive(false);
        FindRandomGame.SetActive(false);
        WinerScreen.SetActive(false);
        LoserScreen.SetActive(false);

    }
    private void Awake()
    {
        DisableAllScreen();
        GameMode.SetActive(true);
    }
    public void OnSinglePlayerModeSelected()
    {
        DisableAllScreen();
    }

    public void OnOnlineModeSelected()
    {
        DisableAllScreen();
        GameType.SetActive(true);
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
        PlayFriends.SetActive(true);
        networkManager.SetPlayerLevel((LevelMode)0);
        networkManager.connectFriends();
    }

    public void OnWin()
    {
        DisableAllScreen();
        WinerScreen.SetActive(true);
    }
    public void OnLose()
    {
        DisableAllScreen();
        LoserScreen.SetActive(false);
    }
    public void PlayAgainButton()
    {

    }
    public void NewGameButton()
    {

    }
}
