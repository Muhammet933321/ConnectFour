using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private NetworkManager networkManager;

    [Header("Buttons")]
    [SerializeField] private GameObject GameMode;
    [SerializeField] private GameObject GameType;
    [SerializeField] private GameObject PlayFriends;
    [SerializeField] private GameObject FindRandomGame;




    private void DisableAllScreen()
    {

        GameMode.SetActive(false);
        GameType.SetActive(false);
        PlayFriends.SetActive(false);
        FindRandomGame.SetActive(false);
        
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

}
